import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
    Box,
    Paper,
    Typography,
    TextField,
    Button,
    Grid,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Switch,
    FormControlLabel,
    Alert,
    CircularProgress,
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material/Select'; // ⬅️ BURADAN IMPORT EDİN
import { Save as SaveIcon, ArrowBack as ArrowBackIcon } from '@mui/icons-material';
import api from '../../services/api';
import { jwtDecode } from 'jwt-decode';
import { useAuth } from '../../contexts/AuthContext';

interface PersonelFormData {
    ad: string;
    soyad: string;
    tcKimlikNo: string;
    email: string;
    telefon: string;
    adres: string;
    dogumTarihi: string;
    iseBaslamaTarihi: string;
    departmanId: number | string;
    vardiyaId: number | string;
    maas: number | string;
    aktif: boolean;
    gorev: string;
    unvan: string;
    kanGrubu: string;
    cinsiyet: string;
}

interface Departman {
    id: number;
    departmanAdi: string;
}

interface Vardiya {
    id: number;
    ad?: string;           // Backend'den bu geliyor
    baslangicSaati?: string;
    bitisSaati?: string;
}

interface Parametre {
    id: number;
    ad: string;
    deger: string;
    birim?: string;
    aciklama?: string;
    kategori?: string;
}

// Sabit listeler
const KAN_GRUPLARI = [
    'A Rh+',
    'A Rh-',
    'B Rh+',
    'B Rh-',
    'AB Rh+',
    'AB Rh-',
    '0 Rh+',
    '0 Rh-',
];

const CINSIYETLER = ['Erkek', 'Kadin', 'Belirtmek Istemiyorum'];

function PersonelForm() {
    const { currentSirketId } = useAuth();
    const { id } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState('');
    const [departmanlar, setDepartmanlar] = useState<Departman[]>([]);
    const [vardiyalar, setVardiyalar] = useState<Vardiya[]>([]);
    const [gorevler, setGorevler] = useState<Parametre[]>([]);
    const [unvanlar, setUnvanlar] = useState<Parametre[]>([]);

    const [formData, setFormData] = useState<PersonelFormData>({
        ad: '',
        soyad: '',
        tcKimlikNo: '',
        email: '',
        telefon: '',
        adres: '',
        dogumTarihi: '',
        iseBaslamaTarihi: '',
        departmanId: '',
        vardiyaId: '',
        maas: '',
        aktif: true,
        gorev: '',
        unvan: '',
        kanGrubu: '',
        cinsiyet: '',
    });

    useEffect(() => {
        loadDependencies();
        if (id) {
            loadPersonel();
        }
    }, [id]);

    // PersonelForm.tsx - loadDependencies fonksiyonunu düzeltin
    const loadDependencies = async () => {
        try {
            const [departmanRes, vardiyaRes, gorevRes, unvanRes] = await Promise.all([
                api.get('/Departman'),
                api.get('/Vardiya'),
                api.get('/Parametre/kategori/GOREV'),
                api.get('/Parametre/kategori/UNVAN'),
            ]);

            console.log('Departman Response:', departmanRes.data);
            console.log('Vardiya Response:', vardiyaRes.data); // ⬅️ Bunu kontrol edin
            console.log('Gorev Response:', gorevRes.data);
            console.log('Unvan Response:', unvanRes.data);

            setDepartmanlar(Array.isArray(departmanRes.data) ? departmanRes.data : []);
            setVardiyalar(Array.isArray(vardiyaRes.data) ? vardiyaRes.data : []); // ⬅️ Güvenli set
            setGorevler(Array.isArray(gorevRes.data) ? gorevRes.data : []);
            setUnvanlar(Array.isArray(unvanRes.data) ? unvanRes.data : []);
        } catch (error) {
            console.error('Bağımlılıklarda hata:', error);
            if (error.response) {
                console.error('API Hatası:', error.response.status, error.response.data);
            }

            // Fallback veriler
            setDepartmanlar([]);
            setVardiyalar([]);
            setGorevler([
                { id: 1, ad: 'Yazılım Geliştirici', deger: 'Yazılım Geliştirici', kategori: 'GOREV' },
                { id: 2, ad: 'Sistem Yöneticisi', deger: 'Sistem Yöneticisi', kategori: 'GOREV' },
            ]);
            setUnvanlar([
                { id: 1, ad: 'Junior', deger: 'Junior', kategori: 'UNVAN' },
                { id: 2, ad: 'Senior', deger: 'Senior', kategori: 'UNVAN' },
            ]);
        }
    };

    const loadPersonel = async () => {
        if (!id) return;

        setLoading(true);
        try {
            const response = await api.get(`/Personel/${id}`);
            const data = response.data;

            setFormData({
                ad: data.ad || '',
                soyad: data.soyad || '',
                tcKimlikNo: data.tcKimlikNo || '',
                email: data.email || '',
                telefon: data.telefon || '',
                adres: data.adres || '',
                dogumTarihi: data.dogumTarihi ? data.dogumTarihi.split('T')[0] : '',
                iseBaslamaTarihi: data.iseBaslamaTarihi ? data.iseBaslamaTarihi.split('T')[0] : '',
                departmanId: data.departmanId || '',
                vardiyaId: data.vardiyaId || '',
                maas: data.maas || '',
                aktif: data.aktif !== undefined ? data.aktif : true,
                gorev: data.gorev || '',
                unvan: data.unvan || '',
                kanGrubu: data.kanGrubu || '',
                cinsiyet: data.cinsiyet || '',
            });
        } catch (err) {
            console.error('Personel yuklenemedi:', err);
            setError('Personel bilgileri yuklenemedi!');
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (field: keyof PersonelFormData, value: string | number | boolean) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSelectChange = (field: keyof PersonelFormData) => (event: SelectChangeEvent) => {
        setFormData((prev) => ({ ...prev, [field]: event.target.value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        e.stopPropagation();

        setError('');
        setSubmitting(true);

        // Validation
        if (!formData.ad || !formData.soyad || !formData.tcKimlikNo || !formData.email) {
            setError('Lütfen zorunlu alanları doldurun!');
            setSubmitting(false);
            return;
        }

        if (!formData.gorev || !formData.unvan) {
            setError('Görev ve Unvan alanları zorunludur!');
            setSubmitting(false);
            return;
        }

        if (!currentSirketId) {
            setError('Şirket bilgisi bulunamadı! Lütfen çıkış yapıp tekrar giriş yapın.');
            setSubmitting(false);
            return;
        }

        try {
            const payload = {
                ...(id && { id: Number(id) }),
                sirketId: currentSirketId,
                adSoyad: `${formData.ad} ${formData.soyad}`,
                sicilNo: formData.tcKimlikNo,
                tcKimlikNo: formData.tcKimlikNo,
                email: formData.email,
                telefon: formData.telefon || '',
                adres: formData.adres || '',
                dogumTarihi: formData.dogumTarihi || null,
                girisTarihi: formData.iseBaslamaTarihi || null,  // ✅ Backend "girisTarihi" bekliyor
                cikisTarihi: null,
                departmanId: formData.departmanId ? Number(formData.departmanId) : null,
                vardiyaId: formData.vardiyaId ? Number(formData.vardiyaId) : null,
                maas: formData.maas ? Number(formData.maas) : 0,
                durum: formData.aktif,  // ✅ Backend "durum" bekliyor
                gorev: formData.gorev,
                unvan: formData.unvan,
                kanGrubu: formData.kanGrubu || '',
                cinsiyet: formData.cinsiyet || '',
                avansLimiti: 0,
                notlar: ''
            };

            if (id) {
                // Güncelleme
                await api.put(`/Personel/${id}`, payload);
                alert('Personel başarıyla güncellendi!');
            } else {
                // Yeni kayıt
                await api.post('/Personel', payload);
                alert('Personel başarıyla eklendi!');
            }

            navigate('/personel');
        } catch (err: any) {
            console.error('Kayıt hatası:', err);
            const errorMsg = err.response?.data?.message || err.response?.data || err.message || 'Kayıt başarısız!';
            setError(errorMsg);
        } finally {
            setSubmitting(false);
        }
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Box>
            <Box display="flex" alignItems="center" mb={3}>
                <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/personel')} sx={{ mr: 2 }}>
                    Geri
                </Button>
                <Typography variant="h4" fontWeight="bold">
                    {id ? 'Personel Duzenle' : 'Yeni Personel'}
                </Typography>
            </Box>

            <Paper sx={{ p: 4 }}>
                {error && (
                    <Alert severity="error" sx={{ mb: 3 }}>
                        {error}
                    </Alert>
                )}

                <Box component="form" onSubmit={handleSubmit} noValidate>
                    <Grid container spacing={3}>
                        {/* Ad */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Ad"
                                value={formData.ad}
                                onChange={(e) => handleChange('ad', e.target.value)}
                            />
                        </Grid>

                        {/* Soyad */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Soyad"
                                value={formData.soyad}
                                onChange={(e) => handleChange('soyad', e.target.value)}
                            />
                        </Grid>

                        {/* TC Kimlik No */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="TC Kimlik No"
                                value={formData.tcKimlikNo}
                                onChange={(e) => handleChange('tcKimlikNo', e.target.value)}
                                inputProps={{ maxLength: 11 }}
                            />
                        </Grid>

                        {/* E-posta */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                type="email"
                                label="E-posta"
                                value={formData.email}
                                onChange={(e) => handleChange('email', e.target.value)}
                            />
                        </Grid>

                        {/* Telefon */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="Telefon"
                                value={formData.telefon}
                                onChange={(e) => handleChange('telefon', e.target.value)}
                            />
                        </Grid>

                        {/* Maas */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="Maas"
                                type="number"
                                value={formData.maas}
                                onChange={(e) => handleChange('maas', e.target.value)}
                            />
                        </Grid>

                        {/* Gorev */}
                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth required>
                                <InputLabel>Gorev</InputLabel>
                                <Select
                                    value={formData.gorev}
                                    onChange={handleSelectChange('gorev')}
                                    label="Gorev"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {gorevler.map((gorev) => (
                                        <MenuItem key={gorev.id} value={gorev.deger}>
                                            {gorev.ad || gorev.deger}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {/* Unvan - Parametreden */}
                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth required>
                                <InputLabel>Unvan</InputLabel>
                                <Select
                                    value={formData.unvan}
                                    onChange={handleSelectChange('unvan')}
                                    label="Unvan"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {unvanlar.map((unvan) => (
                                        <MenuItem key={unvan.id} value={unvan.deger}>
                                            {unvan.deger}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {/* Kan Grubu - Sabit */}
                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Kan Grubu</InputLabel>
                                <Select
                                    value={formData.kanGrubu}
                                    onChange={handleSelectChange('kanGrubu')}
                                    label="Kan Grubu"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {KAN_GRUPLARI.map((kan) => (
                                        <MenuItem key={kan} value={kan}>
                                            {kan}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {/* Cinsiyet - Sabit */}
                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Cinsiyet</InputLabel>
                                <Select
                                    value={formData.cinsiyet}
                                    onChange={handleSelectChange('cinsiyet')}
                                    label="Cinsiyet"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {CINSIYETLER.map((cinsiyet) => (
                                        <MenuItem key={cinsiyet} value={cinsiyet}>
                                            {cinsiyet}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {/* Dogum Tarihi */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                type="date"
                                label="Dogum Tarihi"
                                value={formData.dogumTarihi}
                                onChange={(e) => handleChange('dogumTarihi', e.target.value)}
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>

                        {/* Ise Baslama Tarihi */}
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                type="date"
                                label="Ise Baslama Tarihi"
                                value={formData.iseBaslamaTarihi}
                                onChange={(e) => handleChange('iseBaslamaTarihi', e.target.value)}
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>

                        {/* Departman */}
                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Departman</InputLabel>
                                <Select
                                    value={formData.departmanId}
                                    onChange={handleSelectChange('departmanId')}
                                    label="Departman"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {departmanlar.map((dep) => (
                                        <MenuItem key={dep.id} value={dep.id}>
                                            {dep.departmanAdi}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {/* Vardiya */}
                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Vardiya</InputLabel>
                                <Select
                                    value={formData.vardiyaId}
                                    onChange={handleSelectChange('vardiyaId')}
                                    label="Vardiya"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {vardiyalar.map((vrd) => (
                                        <MenuItem key={vrd.id} value={vrd.id}>
                                            {vrd.ad || vrd.vardiyaAdi || 'İsimsiz Vardiya'}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        {/* Adres */}
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                multiline
                                rows={3}
                                label="Adres"
                                value={formData.adres}
                                onChange={(e) => handleChange('adres', e.target.value)}
                            />
                        </Grid>

                        {/* Aktif */}
                        <Grid item xs={12}>
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={formData.aktif}
                                        onChange={(e) => handleChange('aktif', e.target.checked)}
                                    />
                                }
                                label="Aktif"
                            />
                        </Grid>

                        {/* Butonlar */}
                        <Grid item xs={12}>
                            <Box display="flex" gap={2}>
                                <Button
                                    type="submit"
                                    variant="contained"
                                    startIcon={<SaveIcon />}
                                    disabled={submitting}
                                    sx={{
                                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                    }}
                                >
                                    {submitting ? 'Kaydediliyor...' : 'Kaydet'}
                                </Button>
                                <Button
                                    variant="outlined"
                                    onClick={() => navigate('/personel')}
                                    disabled={submitting}
                                >
                                    Iptal
                                </Button>
                            </Box>
                        </Grid>
                    </Grid>
                </Box>
            </Paper>
        </Box>
    );
}

export default PersonelForm;