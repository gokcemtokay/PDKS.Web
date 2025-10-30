import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
    Box, Paper, Typography, TextField, Button, Grid,
    FormControl, InputLabel, Select, MenuItem, Switch,
    FormControlLabel, Alert, Divider, Chip
} from '@mui/material';
import { Save, ArrowBack } from '@mui/icons-material';
import api from '../../services/api';
import { parseApiError, showError } from '../../utils/errorUtils';

interface Rol {
    id: number;
    rolAdi: string;
}

interface Personel {
    id: number;
    adSoyad: string;
}

interface Sirket {
    id: number;
    sirketAdi?: string;
    unvan?: string;
}

function KullaniciForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [roller, setRoller] = useState<Rol[]>([]);
    const [personeller, setPersoneller] = useState<Personel[]>([]);
    const [sirketler, setSirketler] = useState<Sirket[]>([]);
    const [error, setError] = useState('');

    const [formData, setFormData] = useState({
        kullaniciAdi: '',
        ad: '',
        soyad: '',
        email: '',
        sifre: '',
        yeniSifre: '',
        rolId: '',
        personelId: '',
        aktif: true,
        yetkiliSirketler: [] as number[],
        varsayilanSirketId: 0
    });

    useEffect(() => {
        loadData();
        if (id) loadKullanici();
    }, [id]);

    const loadData = async () => {
        try {
            const [rolRes, personelRes, sirketRes] = await Promise.all([
                api.get('/rolyetki'),
                api.get('/Personel/all'),
                api.get('/Sirket')
            ]);
            console.log('Şirket verileri:', sirketRes.data); // Debug için
            setRoller(rolRes.data);
            setPersoneller(personelRes.data);
            setSirketler(sirketRes.data);
        } catch (error) {
            console.error('Veri yükleme hatası:', error);
            setError('Veriler yüklenirken hata oluştu. Lütfen sayfayı yenileyin.');
        }
    };

    const loadKullanici = async () => {
        if (!id) return;
        setLoading(true);
        try {
            const response = await api.get(`/Kullanici/${id}`);
            const data = response.data;

            // Varsayılan şirketi bul
            const varsayilanSirket = data.yetkiliSirketler?.find((s: any) => s.varsayilan);
            const varsayilanId = varsayilanSirket?.sirketId || data.yetkiliSirketler?.[0]?.sirketId || 0;

            setFormData({
                kullaniciAdi: data.kullaniciAdi || '',
                ad: data.ad || '',
                soyad: data.soyad || '',
                email: data.email || '',
                sifre: '',
                yeniSifre: '',
                rolId: data.rolId?.toString() || '',
                personelId: data.personelId?.toString() || '',
                aktif: data.aktif ?? true,
                yetkiliSirketler: data.yetkiliSirketler?.map((s: any) => s.sirketId) || [],
                varsayilanSirketId: varsayilanId
            });
        } catch (error) {
            console.error('Kullanıcı yüklenemedi:', error);
            setError('Kullanıcı bilgileri yüklenemedi!');
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        // Validasyon
        if (formData.yetkiliSirketler.length === 0) {
            setError('En az bir şirket seçmelisiniz!');
            return;
        }

        if (!formData.varsayilanSirketId || !formData.yetkiliSirketler.includes(formData.varsayilanSirketId)) {
            setError('Varsayılan şirket, yetkili şirketler arasından seçilmelidir!');
            return;
        }

        setLoading(true);

        try {
            const payload: any = {
                kullaniciAdi: formData.kullaniciAdi,
                ad: formData.ad,
                soyad: formData.soyad,
                email: formData.email,
                rolId: parseInt(formData.rolId),
                personelId: formData.personelId ? parseInt(formData.personelId) : null,
                aktif: formData.aktif,
                yetkiliSirketIdler: formData.yetkiliSirketler,
                varsayilanSirketId: formData.varsayilanSirketId
            };

            if (id) {
                // Güncelleme
                payload.id = parseInt(id);
                if (formData.yeniSifre) {
                    payload.yeniSifre = formData.yeniSifre;
                }
                await api.put(`/Kullanici/${id}`, payload);
                alert('Kullanıcı güncellendi!');
            } else {
                // Yeni kayıt
                if (!formData.sifre) {
                    setError('Şifre zorunludur!');
                    setLoading(false);
                    return;
                }
                payload.sifre = formData.sifre;
                await api.post('/Kullanici', payload);
                alert('Kullanıcı oluşturuldu!');
            }
            navigate('/kullanici');
        } catch (error: any) {
            console.error('Kayıt hatası:', error);
            const errorMsg = error.response?.data?.errors
                ? Object.values(error.response.data.errors).flat().join(', ')
                : error.response?.data?.message || 'Kayıt sırasında hata oluştu!';
            showError(error, setError);
            setError(errorMsg);
        } finally {
            setLoading(false);
        }
    };

    const handleSirketToggle = (sirketId: number) => {
        setFormData(prev => {
            const yeniSirketler = prev.yetkiliSirketler.includes(sirketId)
                ? prev.yetkiliSirketler.filter(id => id !== sirketId)
                : [...prev.yetkiliSirketler, sirketId];

            // Eğer varsayılan şirket kaldırıldıysa, ilk şirketi varsayılan yap
            let yeniVarsayilan = prev.varsayilanSirketId;
            if (sirketId === prev.varsayilanSirketId && !yeniSirketler.includes(sirketId)) {
                yeniVarsayilan = yeniSirketler.length > 0 ? yeniSirketler[0] : 0;
            }
            // Eğer hiç varsayılan yoksa ve şirket ekleniyorsa, onu varsayılan yap
            if (prev.varsayilanSirketId === 0 && yeniSirketler.includes(sirketId)) {
                yeniVarsayilan = sirketId;
            }

            return {
                ...prev,
                yetkiliSirketler: yeniSirketler,
                varsayilanSirketId: yeniVarsayilan
            };
        });
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                <Button
                    startIcon={<ArrowBack />}
                    onClick={() => navigate('/kullanici')}
                    sx={{ mr: 2 }}
                >
                    Geri
                </Button>
                <Typography variant="h4" fontWeight="bold">
                    {id ? 'Kullanıcı Düzenle' : 'Yeni Kullanıcı'}
                </Typography>
            </Box>

            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

            <Paper sx={{ p: 3 }}>
                <form onSubmit={handleSubmit}>
                    <Grid container spacing={3}>
                        {/* Kullanıcı Bilgileri */}
                        <Grid item xs={12}>
                            <Typography variant="h6" gutterBottom>Kullanıcı Bilgileri</Typography>
                            <Divider sx={{ mb: 2 }} />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Kullanıcı Adı"
                                value={formData.kullaniciAdi}
                                onChange={(e) => setFormData({ ...formData, kullaniciAdi: e.target.value })}
                                disabled={!!id}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Email"
                                type="email"
                                value={formData.email}
                                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Ad"
                                value={formData.ad}
                                onChange={(e) => setFormData({ ...formData, ad: e.target.value })}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Soyad"
                                value={formData.soyad}
                                onChange={(e) => setFormData({ ...formData, soyad: e.target.value })}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth required>
                                <InputLabel>Rol</InputLabel>
                                <Select
                                    value={formData.rolId}
                                    onChange={(e) => setFormData({ ...formData, rolId: e.target.value })}
                                    label="Rol"
                                >
                                    <MenuItem value="">Seçiniz</MenuItem>
                                    {roller.map(rol => (
                                        <MenuItem key={rol.id} value={rol.id}>{rol.rolAdi}</MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {!id && (
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    required
                                    type="password"
                                    label="Şifre"
                                    value={formData.sifre}
                                    onChange={(e) => setFormData({ ...formData, sifre: e.target.value })}
                                    helperText="En az 6 karakter"
                                />
                            </Grid>
                        )}

                        {id && (
                            <Grid item xs={12} md={6}>
                                <TextField
                                    fullWidth
                                    type="password"
                                    label="Yeni Şifre (Opsiyonel)"
                                    value={formData.yeniSifre}
                                    onChange={(e) => setFormData({ ...formData, yeniSifre: e.target.value })}
                                    helperText="Değiştirmek istemiyorsanız boş bırakın"
                                />
                            </Grid>
                        )}

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Personel (Opsiyonel)</InputLabel>
                                <Select
                                    value={formData.personelId}
                                    onChange={(e) => setFormData({ ...formData, personelId: e.target.value })}
                                    label="Personel (Opsiyonel)"
                                >
                                    <MenuItem value="">Seçiniz</MenuItem>
                                    {personeller.map(personel => (
                                        <MenuItem key={personel.id} value={personel.id}>
                                            {personel.adSoyad}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={formData.aktif}
                                        onChange={(e) => setFormData({ ...formData, aktif: e.target.checked })}
                                    />
                                }
                                label="Aktif"
                            />
                        </Grid>

                        {/* Yetkili Şirketler */}
                        <Grid item xs={12}>
                            <Typography variant="h6" gutterBottom>Yetkili Şirketler</Typography>
                            <Divider sx={{ mb: 2 }} />
                        </Grid>

                        <Grid item xs={12}>
                            <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                                Şirketlere tıklayarak yetkilendirin (en az 1 şirket zorunlu):
                            </Typography>
                            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
                                {sirketler.length === 0 ? (
                                    <Typography color="text.secondary">
                                        Henüz tanımlı şirket yok veya yüklenirken hata oluştu.
                                    </Typography>
                                ) : (
                                    sirketler.map(sirket => (
                                        <Chip
                                            key={sirket.id}
                                            label={sirket.unvan || sirket.sirketAdi || 'İsimsiz Şirket'}
                                            onClick={() => handleSirketToggle(sirket.id)}
                                            color={formData.yetkiliSirketler.includes(sirket.id) ? 'primary' : 'default'}
                                            variant={formData.yetkiliSirketler.includes(sirket.id) ? 'filled' : 'outlined'}
                                        />
                                    ))
                                )}
                            </Box>
                        </Grid>

                        {formData.yetkiliSirketler.length > 0 && (
                            <Grid item xs={12} md={6}>
                                <FormControl fullWidth required>
                                    <InputLabel>Varsayılan Şirket</InputLabel>
                                    <Select
                                        value={formData.varsayilanSirketId}
                                        onChange={(e) => setFormData({ ...formData, varsayilanSirketId: Number(e.target.value) })}
                                        label="Varsayılan Şirket"
                                    >
                                        {sirketler
                                            .filter(s => formData.yetkiliSirketler.includes(s.id))
                                            .map(sirket => (
                                                <MenuItem key={sirket.id} value={sirket.id}>
                                                    {sirket.unvan || sirket.sirketAdi}
                                                </MenuItem>
                                            ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        )}

                        {/* Butonlar */}
                        <Grid item xs={12}>
                            <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
                                <Button
                                    type="submit"
                                    variant="contained"
                                    startIcon={<Save />}
                                    disabled={loading}
                                >
                                    {loading ? 'Kaydediliyor...' : 'Kaydet'}
                                </Button>
                                <Button
                                    variant="outlined"
                                    onClick={() => navigate('/kullanici')}
                                    disabled={loading}
                                >
                                    İptal
                                </Button>
                            </Box>
                        </Grid>
                    </Grid>
                </form>
            </Paper>
        </Box>
    );
}

export default KullaniciForm;
