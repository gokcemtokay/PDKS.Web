import { useEffect, useState } from 'react';
import {
  Box,
  Grid,
  Card,
  CardContent,
  Typography,
  Avatar,
  Chip,
  LinearProgress,
  Paper,
} from '@mui/material';
import {
  People as PeopleIcon,
  BeachAccess as BeachAccessIcon,
  AttachMoney as AttachMoneyIcon,
  CheckCircle as CheckCircleIcon,
  Business as BusinessIcon,
  TrendingUp as TrendingUpIcon,
} from '@mui/icons-material';
import { useAuth } from '../../contexts/AuthContext';

function AnaDashboard() {
  const { user, aktifSirket } = useAuth();
  const [stats, setStats] = useState({
    toplamPersonel: 0,
    aktifPersonel: 0,
    bekleyenIzin: 0,
    bekleyenAvans: 0,
    bekleyenOnay: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, [aktifSirket]); // Şirket değiştiğinde veriyi yeniden yükle

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      // API çağrıları - Otomatik olarak X-Sirket-Id header'ı gönderilir
      // const personelData = await personelService.getAll();
      // const izinData = await izinService.getBekleyenler();
      // vs...
      
      // Mock data
      setTimeout(() => {
        setStats({
          toplamPersonel: 156,
          aktifPersonel: 142,
          bekleyenIzin: 8,
          bekleyenAvans: 3,
          bekleyenOnay: 12,
        });
        setLoading(false);
      }, 500);
    } catch (error) {
      console.error('Dashboard verileri yüklenemedi:', error);
      setLoading(false);
    }
  };

  const StatCard = ({ title, value, icon, color, subtitle }: any) => (
    <Card sx={{ height: '100%', position: 'relative', overflow: 'visible' }}>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <Box>
            <Typography color="text.secondary" variant="subtitle2" gutterBottom>
              {title}
            </Typography>
            <Typography variant="h4" fontWeight="bold">
              {value}
            </Typography>
            {subtitle && (
              <Typography variant="caption" color="text.secondary">
                {subtitle}
              </Typography>
            )}
          </Box>
          <Avatar
            sx={{
              bgcolor: `${color}.100`,
              color: `${color}.main`,
              width: 56,
              height: 56,
            }}
          >
            {icon}
          </Avatar>
        </Box>
      </CardContent>
    </Card>
  );

  if (loading) {
    return (
      <Box>
        <LinearProgress />
        <Typography sx={{ mt: 2, textAlign: 'center' }}>Yükleniyor...</Typography>
      </Box>
    );
  }

  return (
    <Box>
      {/* Hoş Geldin + Şirket Bilgisi */}
      <Paper
        sx={{
          p: 3,
          mb: 3,
          background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
          color: 'white',
        }}
      >
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={8}>
            <Typography variant="h4" fontWeight="bold" gutterBottom>
              Hoş Geldiniz, {user?.ad || user?.email}! 👋
            </Typography>
            <Typography variant="body1" sx={{ opacity: 0.9 }}>
              {new Date().toLocaleDateString('tr-TR', {
                weekday: 'long',
                year: 'numeric',
                month: 'long',
                day: 'numeric',
              })}
            </Typography>
          </Grid>
          <Grid item xs={12} md={4}>
            <Box
              sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: { xs: 'flex-start', md: 'flex-end' },
                gap: 1,
              }}
            >
              <BusinessIcon sx={{ fontSize: 32 }} />
              <Box>
                <Typography variant="caption" sx={{ opacity: 0.8 }}>
                  Aktif Şirket
                </Typography>
                <Typography variant="h6" fontWeight="bold">
                  {aktifSirket?.sirketAdi || 'Şirket Seçilmedi'}
                </Typography>
              </Box>
            </Box>
          </Grid>
        </Grid>
      </Paper>

      {/* İstatistikler */}
      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid item xs={12} sm={6} md={4}>
          <StatCard
            title="Toplam Personel"
            value={stats.toplamPersonel}
            subtitle={`${stats.aktifPersonel} aktif`}
            icon={<PeopleIcon />}
            color="primary"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <StatCard
            title="Bekleyen İzin"
            value={stats.bekleyenIzin}
            subtitle="Onay bekliyor"
            icon={<BeachAccessIcon />}
            color="warning"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <StatCard
            title="Bekleyen Avans"
            value={stats.bekleyenAvans}
            subtitle="İncelenmeli"
            icon={<AttachMoneyIcon />}
            color="success"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <StatCard
            title="Bekleyen Onay"
            value={stats.bekleyenOnay}
            subtitle="Tüm süreçler"
            icon={<CheckCircleIcon />}
            color="error"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <StatCard
            title="Personel Devamsızlık"
            value="%2.5"
            subtitle="Bu ay"
            icon={<TrendingUpIcon />}
            color="info"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={4}>
          <StatCard
            title="Aktif Vardiyalar"
            value="3"
            subtitle="Güncel vardiya sayısı"
            icon={<CheckCircleIcon />}
            color="secondary"
          />
        </Grid>
      </Grid>

      {/* Ek İçerikler */}
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Son İşlemler
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Yakında eklenecek...
              </Typography>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Duyurular
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Yakında eklenecek...
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>
  );
}

export default AnaDashboard;
