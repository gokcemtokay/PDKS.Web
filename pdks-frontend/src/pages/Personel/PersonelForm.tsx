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
import { Save as SaveIcon, ArrowBack as ArrowBackIcon } from '@mui/icons-material';
import api from '../../services/api';

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
}

interface Departman {
    id: number;
    departmanAdi: string;
}

interface Vardiya {
    id: number;
    vardiyaAdi: string;
}

function PersonelForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState('');
    const [departmanlar, setDepartmanlar] = useState<Departman[]>([]);
    const [vardiyalar, setVardiyalar] = useState<Vardiya[]>([]);

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
    });

    useEffect(() => {
        loadDependencies();
        if (id) {
            loadPersonel();
        }
    }, [id]);

    const loadDependencies = async () => {
        try {
            const [departmanRes, vardiyaRes] = await Promise.all([
                api.get('/Departman'),
                api.get('/Vardiya'),
            ]);
            setDepartmanlar(departmanRes.data || []);
            setVardiyalar(vardiyaRes.data || []);
        } catch (error) {
            console.error('Bagimliliklarda hata:', error);
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

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        e.stopPropagation();

        setError('');
        setSubmitting(true);

        // Validation
        if (!formData.ad || !formData.soyad || !formData.tcKimlikNo || !formData.email) {
            setError('Lutfen zorunlu alanlari doldurun!');
            setSubmitting(false);
            return;
        }

        try {
            const payload = {
                ...formData,
                departmanId: formData.departmanId ? Number(formData.departmanId) : null,
                vardiyaId: formData.vardiyaId ? Number(formData.vardiyaId) : null,
                maas: formData.maas ? Number(formData.maas) : 0,
            };

            if (id) {
                await api.put(`/Personel/${id}`, { ...payload, id: parseInt(id) });
            } else {
                await api.post('/Personel', payload);
            }

            navigate('/personel');
        } catch (err: any) {
            const errorMsg = err.response?.data || 'Islem basarisiz!';
            setError(typeof errorMsg === 'string' ? errorMsg : JSON.stringify(errorMsg));
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
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Ad"
                                value={formData.ad}
                                onChange={(e) => handleChange('ad', e.target.value)}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Soyad"
                                value={formData.soyad}
                                onChange={(e) => handleChange('soyad', e.target.value)}
                            />
                        </Grid>

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

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="Telefon"
                                value={formData.telefon}
                                onChange={(e) => handleChange('telefon', e.target.value)}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="Maas"
                                type="number"
                                value={formData.maas}
                                onChange={(e) => handleChange('maas', e.target.value)}
                            />
                        </Grid>

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

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Departman</InputLabel>
                                <Select
                                    value={formData.departmanId}
                                    onChange={(e) => handleChange('departmanId', e.target.value)}
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

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Vardiya</InputLabel>
                                <Select
                                    value={formData.vardiyaId}
                                    onChange={(e) => handleChange('vardiyaId', e.target.value)}
                                    label="Vardiya"
                                >
                                    <MenuItem value="">
                                        <em>Seciniz</em>
                                    </MenuItem>
                                    {vardiyalar.map((vrd) => (
                                        <MenuItem key={vrd.id} value={vrd.id}>
                                            {vrd.vardiyaAdi}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>

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