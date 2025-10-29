import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
    Box, Paper, Typography, TextField, Button, FormControl,
    InputLabel, Select, MenuItem, Switch, FormControlLabel,
    Grid, Checkbox, FormGroup, FormLabel, Radio, RadioGroup,
    Alert,
} from '@mui/material';
import { Save, Cancel, Business } from '@mui/icons-material';
import api from '../../services/api';

interface Sirket {
    id: number;
    unvan: string;
    aktif: boolean;
}

interface Rol {
    id: number;
    rolAdi: string;
}

function KullaniciForm() {
    const navigate = useNavigate();
    const { id } = useParams();
    const isEditMode = !!id;

    const [formData, setFormData] = useState({
        kullaniciAdi: '',
        ad: '',
        soyad: '',
        email: '',
        sifre: '',
        rolId: '',
        aktif: true,
        yetkiliSirketIdler: [] as number[],
        varsayilanSirketId: 0,
    });

    const [sirketler, setSirketler] = useState<Sirket[]>([]);
    const [roller, setRoller] = useState<Rol[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        loadSirketler();
        loadRoller();
        if (isEditMode) {
            loadKullanici();
        }
    }, [id]);

    const loadSirketler = async () => {
        try {
            const response = await api.get('/Sirket');
            setSirketler(response.data.filter((s: Sirket) => s.aktif));
        } catch (error) {
            console.error('�irketler y�klenemedi:', error);
        }
    };

    const loadRoller = async () => {
        try {
            const response = await api.get('/Rol');
            setRoller(response.data);
        } catch (error) {
            console.error('Roller y�klenemedi:', error);
        }
    };

    const loadKullanici = async () => {
        try {
            const response = await api.get(`/Kullanici/${id}`);
            const kullanici = response.data;

            setFormData({
                kullaniciAdi: kullanici.kullaniciAdi,
                ad: kullanici.ad,
                soyad: kullanici.soyad,
                email: kullanici.email,
                sifre: '',
                rolId: kullanici.rolId,
                aktif: kullanici.aktif,
                yetkiliSirketIdler: kullanici.yetkiliSirketler.map((s: any) => s.sirketId),
                varsayilanSirketId: kullanici.yetkiliSirketler.find((s: any) => s.varsayilan)?.sirketId || 0,
            });
        } catch (error) {
            console.error('Kullan�c� y�klenemedi:', error);
            alert('Kullan�c� y�klenirken hata olu�tu!');
        }
    };

    const handleSirketToggle = (sirketId: number) => {
        const yeniListe = formData.yetkiliSirketIdler.includes(sirketId)
            ? formData.yetkiliSirketIdler.filter(id => id !== sirketId)
            : [...formData.yetkiliSirketIdler, sirketId];

        setFormData({
            ...formData,
            yetkiliSirketIdler: yeniListe,
            // Varsay�lan �irket ��kar�l�rsa, ilk yetkili �irketi varsay�lan yap
            varsayilanSirketId: yeniListe.includes(formData.varsayilanSirketId)
                ? formData.varsayilanSirketId
                : (yeniListe[0] || 0)
        });
    };

    const handleVarsayilanSirketChange = (sirketId: number) => {
        setFormData({
            ...formData,
            varsayilanSirketId: sirketId,
        });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Validasyon
        if (formData.yetkiliSirketIdler.length === 0) {
            alert('En az bir �irket se�melisiniz!');
            return;
        }

        if (!formData.varsayilanSirketId || !formData.yetkiliSirketIdler.includes(formData.varsayilanSirketId)) {
            alert('Varsay�lan �irket se�melisiniz!');
            return;
        }

        if (!isEditMode && !formData.sifre) {
            alert('�ifre zorunludur!');
            return;
        }

        setLoading(true);
        try {
            const payload = {
                ...formData,
                rolId: parseInt(formData.rolId as any),
            };

            if (isEditMode) {
                await api.put(`/Kullanici/${id}`, payload);
                alert('Kullan�c� g�ncellendi!');
            } else {
                await api.post('/Kullanici', payload);
                alert('Kullan�c� olu�turuldu!');
            }

            navigate('/kullanici');
        } catch (error: any) {
            console.error('Kay�t hatas�:', error);
            const errorMsg = error.response?.data?.message || error.message || 'Kay�t ba�ar�s�z!';
            alert(errorMsg);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box>
            <Typography variant="h4" fontWeight="bold" mb={3}>
                {isEditMode ? 'Kullan�c� D�zenle' : 'Yeni Kullan�c�'}
            </Typography>

            <Paper sx={{ p: 3 }}>
                <form onSubmit={handleSubmit}>
                    <Grid container spacing={3}>
                        {/* Kullan�c� Bilgileri */}
                        <Grid item xs={12}>
                            <Typography variant="h6" gutterBottom>Kullan�c� Bilgileri</Typography>
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Kullan�c� Ad�"
                                value={formData.kullaniciAdi}
                                onChange={(e) => setFormData({ ...formData, kullaniciAdi: e.target.value })}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                type="email"
                                label="Email"
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
                            <TextField
                                fullWidth
                                required={!isEditMode}
                                type="password"
                                label={isEditMode ? 'Yeni �ifre (bo� b�rak�l�rsa de�i�mez)' : '�ifre'}
                                value={formData.sifre}
                                onChange={(e) => setFormData({ ...formData, sifre: e.target.value })}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth required>
                                <InputLabel>Rol</InputLabel>
                                <Select
                                    value={formData.rolId}
                                    label="Rol"
                                    onChange={(e) => setFormData({ ...formData, rolId: e.target.value })}
                                >
                                    {roller.map((rol) => (
                                        <MenuItem key={rol.id} value={rol.id}>
                                            {rol.rolAdi}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        <Grid item xs={12}>
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

                        {/* �irket Yetkileri */}
                        <Grid item xs={12}>
                            <Typography variant="h6" gutterBottom sx={{ mt: 2 }}>
                                <Business sx={{ mr: 1, verticalAlign: 'middle' }} />
                                �irket Yetkileri
                            </Typography>
                            <Alert severity="info" sx={{ mb: 2 }}>
                                Kullan�c�n�n eri�im yetkisi olaca�� �irketleri se�in ve varsay�lan �irketi belirleyin.
                            </Alert>
                        </Grid>

                        <Grid item xs={12}>
                            <FormControl component="fieldset">
                                <FormLabel component="legend">Yetkili �irketler</FormLabel>
                                <FormGroup>
                                    {sirketler.map((sirket) => (
                                        <Box key={sirket.id} sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                            <FormControlLabel
                                                control={
                                                    <Checkbox
                                                        checked={formData.yetkiliSirketIdler.includes(sirket.id)}
                                                        onChange={() => handleSirketToggle(sirket.id)}
                                                    />
                                                }
                                                label={sirket.unvan}
                                            />
                                            {formData.yetkiliSirketIdler.includes(sirket.id) && (
                                                <FormControlLabel
                                                    control={
                                                        <Radio
                                                            checked={formData.varsayilanSirketId === sirket.id}
                                                            onChange={() => handleVarsayilanSirketChange(sirket.id)}
                                                        />
                                                    }
                                                    label="Varsay�lan"
                                                    sx={{ ml: 2 }}
                                                />
                                            )}
                                        </Box>
                                    ))}
                                </FormGroup>
                            </FormControl>
                        </Grid>

                        {/* Butonlar */}
                        <Grid item xs={12}>
                            <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                                <Button
                                    variant="outlined"
                                    startIcon={<Cancel />}
                                    onClick={() => navigate('/kullanici')}
                                >
                                    �ptal
                                </Button>
                                <Button
                                    type="submit"
                                    variant="contained"
                                    startIcon={<Save />}
                                    disabled={loading}
                                >
                                    {loading ? 'Kaydediliyor...' : 'Kaydet'}
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
