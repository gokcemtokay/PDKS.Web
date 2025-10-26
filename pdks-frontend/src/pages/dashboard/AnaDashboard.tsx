import { useEffect, useState } from 'react';
import { Box, Grid, Card, CardContent, Typography, Paper } from '@mui/material';
import {
  People as PeopleIcon, BeachAccess as BeachAccessIcon,
  TrendingUp as TrendingUpIcon, CheckCircle as CheckCircleIcon,
} from '@mui/icons-material';
import dashboardService, { BugunkunDurum } from '../../services/dashboardService';

function AnaDashboard() {
  const [durum, setDurum] = useState<BugunkunDurum | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await dashboardService.getBugunkunDurum();
      setDurum(data);
    } catch (error) {
      console.error('Dashboard yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <Typography>Yükleniyor...</Typography>;

  return (
    <Box>
      <Typography variant="h4" gutterBottom fontWeight="bold">
        Ana Sayfa
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)', color: 'white' }}>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <div>
                  <Typography variant="h4" fontWeight="bold">{durum?.toplamPersonel || 0}</Typography>
                  <Typography variant="body2">Toplam Personel</Typography>
                </div>
                <PeopleIcon sx={{ fontSize: 48, opacity: 0.3 }} />
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ background: 'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)', color: 'white' }}>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <div>
                  <Typography variant="h4" fontWeight="bold">{durum?.bugunkuGiris || 0}</Typography>
                  <Typography variant="body2">Bugünkü Giriş</Typography>
                </div>
                <CheckCircleIcon sx={{ fontSize: 48, opacity: 0.3 }} />
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ background: 'linear-gradient(135deg, #fa709a 0%, #fee140 100%)', color: 'white' }}>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <div>
                  <Typography variant="h4" fontWeight="bold">{durum?.izinliPersonel || 0}</Typography>
                  <Typography variant="body2">İzinli Personel</Typography>
                </div>
                <BeachAccessIcon sx={{ fontSize: 48, opacity: 0.3 }} />
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ background: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)', color: 'white' }}>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <div>
                  <Typography variant="h4" fontWeight="bold">
                    {durum?.girisCikisOrani ? `%${durum.girisCikisOrani.toFixed(0)}` : '%0'}
                  </Typography>
                  <Typography variant="body2">Giriş Oranı</Typography>
                </div>
                <TrendingUpIcon sx={{ fontSize: 48, opacity: 0.3 }} />
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12}>
          <Paper sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>Hoş Geldiniz!</Typography>
            <Typography color="text.secondary">
              PDKS Personel Yönetim Sistemi'ne hoş geldiniz. Sistemdeki tüm işlemlerinizi sol menüden gerçekleştirebilirsiniz.
            </Typography>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
}

export default AnaDashboard;
