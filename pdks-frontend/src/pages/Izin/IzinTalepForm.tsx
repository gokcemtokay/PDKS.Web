import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Paper, Typography, TextField, Button, Grid, MenuItem } from '@mui/material';
import { Save as SaveIcon, ArrowBack as ArrowBackIcon } from '@mui/icons-material';
import izinService from '../../services/izinService';

const izinTipleri = ['Yıllık İzin', 'Mazeret İzni', 'Ücretsiz İzin', 'Hastalık İzni'];

function IzinTalepForm() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        personelId: 1,
        izinTipi: '',
        baslangicTarihi: '',
        bitisTarihi: '',
        aciklama: '',
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await izinService.create({
                ...formData,
                baslangicTarihi: new Date(formData.baslangicTarihi).toISOString(),
                bitisTarihi: new Date(formData.bitisTarihi).toISOString(),
            });
            alert('İzin talebi oluşturuldu!');
            navigate('/izin');
        } catch (error) {
            console.error('Hata:', error);
            alert('İzin talebi oluşturulamadı!');
        }
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/izin')} sx={{ mr: 2 }}>
                    Geri
                </Button>
                <Typography variant="h4" fontWeight="bold">İzin Talebi Oluştur</Typography>
            </Box>

            <Paper sx={{ p: 3 }}>
                <form onSubmit={handleSubmit}>
                    <Grid container spacing={3}>
                        <Grid item xs={12} md={6}>
                            <TextField
                                select
                                fullWidth
                                required
                                label="İzin Tipi"
                                value={formData.izinTipi}
                                onChange={(e) => setFormData({ ...formData, izinTipi: e.target.value })}
                            >
                                {izinTipleri.map((tip) => (
                                    <MenuItem key={tip} value={tip}>{tip}</MenuItem>
                                ))}
                            </TextField>
                        </Grid>
                        <Grid item xs={12} md={6} />
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                type="date"
                                label="Başlangıç Tarihi"
                                value={formData.baslangicTarihi}
                                onChange={(e) => setFormData({ ...formData, baslangicTarihi: e.target.value })}
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                type="date"
                                label="Bitiş Tarihi"
                                value={formData.bitisTarihi}
                                onChange={(e) => setFormData({ ...formData, bitisTarihi: e.target.value })}
                                InputLabelProps={{ shrink: true }}
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
                        <Grid item xs={12}>
                            <Button type="submit" variant="contained" size="large" startIcon={<SaveIcon />}>
                                Talebi Gönder
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            </Paper>
        </Box>
    );
}

export default IzinTalepForm;