import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
    Box, Paper, Typography, TextField, Button, Grid,
    FormControl, InputLabel, Select, MenuItem, Switch,
    FormControlLabel, Alert, Divider
} from '@mui/material';
import { Save, ArrowBack } from '@mui/icons-material';
import { useAuth } from '../../contexts/AuthContext';
import departmanService, { Departman } from '../../services/departmanService';
import { showError } from '../../utils/errorUtils';
function DepartmanForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { aktifSirket } = useAuth();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [departmanlar, setDepartmanlar] = useState<Departman[]>([]);

    const [formData, setFormData] = useState({
        departmanAdi: '',
        kod: '',
        aciklama: '',
        ustDepartmanId: '',
        durum: true
    });

    useEffect(() => {
        loadDepartmanlar();
        if (id) loadDepartman();
    }, [id]);

    const loadDepartmanlar = async () => {
        try {
            const data = await departmanService.getAll();
            setDepartmanlar(data);
        } catch (error) {
            console.error('Departmanlar yüklenemedi:', error);
        }
    };

    const loadDepartman = async () => {
        if (!id) return;
        setLoading(true);
        try {
            const data = await departmanService.getById(parseInt(id));
            setFormData({
                departmanAdi: data.departmanAdi || '',
                kod: data.kod || '',
                aciklama: data.aciklama || '',
                ustDepartmanId: data.ustDepartmanId?.toString() || '',
                durum: data.durum ?? true
            });
        } catch (error) {
            console.error('Departman yüklenemedi:', error);
            setError('Departman bilgileri yüklenemedi!');
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            const payload: any = {
                departmanAdi: formData.departmanAdi,
                kod: formData.kod || undefined,
                aciklama: formData.aciklama || undefined,
                ustDepartmanId: formData.ustDepartmanId ? parseInt(formData.ustDepartmanId) : undefined,
                durum: formData.durum
            };

            if (id) {
                // Güncelleme
                payload.id = parseInt(id);
                payload.sirketId = aktifSirket?.sirketId || 0;
                await departmanService.update(parseInt(id), payload);
                alert('Departman güncellendi!');
            } else {
                // Yeni kayıt
                payload.sirketId = aktifSirket?.sirketId || 0;
                await departmanService.create(payload);
                alert('Departman oluşturuldu!');
            }
            navigate('/departman');
        } catch (error: any) {
            console.error('Kayıt hatası:', error);
            const errorMsg = error.response?.data?.message || 'Kayıt sırasında hata oluştu!';
            showError(error, setError);
            setError(errorMsg);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                <Button
                    startIcon={<ArrowBack />}
                    onClick={() => navigate('/departman')}
                    sx={{ mr: 2 }}
                >
                    Geri
                </Button>
                <Box>
                    <Typography variant="h4" fontWeight="bold">
                        {id ? 'Departman Düzenle' : 'Yeni Departman'}
                    </Typography>
                    {aktifSirket && (
                        <Typography variant="body2" color="text.secondary">
                            {aktifSirket.sirketAdi}
                        </Typography>
                    )}
                </Box>
            </Box>

            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

            <Paper sx={{ p: 3 }}>
                <form onSubmit={handleSubmit}>
                    <Grid container spacing={3}>
                        {/* Genel Bilgiler */}
                        <Grid item xs={12}>
                            <Typography variant="h6" gutterBottom>Departman Bilgileri</Typography>
                            <Divider sx={{ mb: 2 }} />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                label="Departman Adı"
                                value={formData.departmanAdi}
                                onChange={(e) => setFormData({ ...formData, departmanAdi: e.target.value })}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="Kod"
                                value={formData.kod}
                                onChange={(e) => setFormData({ ...formData, kod: e.target.value })}
                                helperText="Opsiyonel - Departman kısa kodu"
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth>
                                <InputLabel>Üst Departman (Opsiyonel)</InputLabel>
                                <Select
                                    value={formData.ustDepartmanId}
                                    onChange={(e) => setFormData({ ...formData, ustDepartmanId: e.target.value })}
                                    label="Üst Departman (Opsiyonel)"
                                >
                                    <MenuItem value="">Seçiniz</MenuItem>
                                    {departmanlar
                                        .filter(d => d.id !== parseInt(id || '0'))
                                        .map(departman => (
                                            <MenuItem key={departman.id} value={departman.id}>
                                                {departman.departmanAdi}
                                            </MenuItem>
                                        ))}
                                </Select>
                            </FormControl>
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={formData.durum}
                                        onChange={(e) => setFormData({ ...formData, durum: e.target.checked })}
                                    />
                                }
                                label="Aktif"
                            />
                        </Grid>

                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                multiline
                                rows={3}
                                label="Açıklama"
                                value={formData.aciklama}
                                onChange={(e) => setFormData({ ...formData, aciklama: e.target.value })}
                            />
                        </Grid>

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
                                    onClick={() => navigate('/departman')}
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

export default DepartmanForm;
