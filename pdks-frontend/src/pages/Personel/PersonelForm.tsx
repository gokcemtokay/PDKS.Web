import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Paper, Typography, TextField, Button, Grid } from '@mui/material';
import { Save as SaveIcon, ArrowBack as ArrowBackIcon } from '@mui/icons-material';
import personelService from '../../services/personelService';

function PersonelForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    sicilNo: '',
    adSoyad: '',
    email: '',
    telefon: '',
    tcKimlik: '',
  });

  useEffect(() => {
    if (id) {
      loadPersonel();
    }
  }, [id]);

  const loadPersonel = async () => {
    if (!id) return;
    try {
      const data = await personelService.getById(parseInt(id));
      setFormData(data);
    } catch (error) {
      console.error('Personel yüklenemedi:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      if (id) {
        await personelService.update(parseInt(id), formData);
      } else {
        await personelService.create(formData);
      }
      navigate('/personel');
    } catch (error) {
      console.error('Kaydetme hatası:', error);
      alert('Kaydetme başarısız!');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
        <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/personel')} sx={{ mr: 2 }}>
          Geri
        </Button>
        <Typography variant="h4" fontWeight="bold">
          {id ? 'Personel Düzenle' : 'Yeni Personel'}
        </Typography>
      </Box>

      <Paper sx={{ p: 3 }}>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                required
                label="Sicil No"
                value={formData.sicilNo}
                onChange={(e) => setFormData({ ...formData, sicilNo: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                required
                label="Ad Soyad"
                value={formData.adSoyad}
                onChange={(e) => setFormData({ ...formData, adSoyad: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                required
                type="email"
                label="E-posta"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Telefon"
                value={formData.telefon}
                onChange={(e) => setFormData({ ...formData, telefon: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="TC Kimlik"
                value={formData.tcKimlik}
                onChange={(e) => setFormData({ ...formData, tcKimlik: e.target.value })}
              />
            </Grid>
            <Grid item xs={12}>
              <Button
                type="submit"
                variant="contained"
                size="large"
                startIcon={<SaveIcon />}
                disabled={loading}
              >
                {loading ? 'Kaydediliyor...' : 'Kaydet'}
              </Button>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Box>
  );
}

export default PersonelForm;
