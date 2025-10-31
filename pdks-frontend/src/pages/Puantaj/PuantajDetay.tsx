import React, { useState, useEffect } from 'react';
import {
  Box,
  Container,
  Typography,
  Stack,
  Button,
  Chip,
  CircularProgress,
  Card,
  CardContent,
  CardHeader,
  Grid,
  Divider,
  IconButton,
  Tabs,
  Tab,
  Snackbar,
  Alert,
} from '@mui/material';
import {
  ArrowBack as ArrowBackIcon,
  Check as CheckIcon,
  Close as CloseIcon,
  Download as DownloadIcon,
} from '@mui/icons-material';
import { useParams, useNavigate } from 'react-router-dom';
import puantajService from '../../services/puantajService';
import { PuantajDetail } from '../../types/puantaj.types';
import {
  formatDakika,
  getDurumBadgeClass,
  getDonemText,
} from '../../utils/puantajUtils';
import GunlukDetayTablosu from '../../components/GunlukDetayTablosu';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`tabpanel-${index}`}
      aria-labelledby={`tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ py: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

const PuantajDetay: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [snackbar, setSnackbar] = useState<{ open: boolean; message: string; severity: 'success' | 'error' }>({
    open: false,
    message: '',
    severity: 'success',
  });

  const [puantaj, setPuantaj] = useState<PuantajDetail | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [tabValue, setTabValue] = useState(0);

  useEffect(() => {
    if (id) {
      loadPuantaj(Number(id));
    }
  }, [id]);

  const loadPuantaj = async (puantajId: number) => {
    setLoading(true);
    try {
      const data = await puantajService.getById(puantajId);
      setPuantaj(data);
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Puantaj detayı yüklenemedi',
        severity: 'error',
      });
      navigate('/puantaj');
    } finally {
      setLoading(false);
    }
  };

  const handleOnayla = async () => {
    if (!puantaj) return;

    try {
      const onaylayanKullaniciId = 1; // TODO: Actual user ID
      
      await puantajService.onayla({
        puantajId: puantaj.id,
        onaylayanKullaniciId,
      });

      setSnackbar({
        open: true,
        message: 'Puantaj onaylandı',
        severity: 'success',
      });

      loadPuantaj(puantaj.id);
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.response?.data?.message || 'Onaylama başarısız',
        severity: 'error',
      });
    }
  };

  const handleOnayIptal = async () => {
    if (!puantaj) return;

    try {
      await puantajService.onayIptal(puantaj.id);

      setSnackbar({
        open: true,
        message: 'Puantaj onayı iptal edildi',
        severity: 'success',
      });

      loadPuantaj(puantaj.id);
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.response?.data?.message || 'İptal başarısız',
        severity: 'error',
      });
    }
  };

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  const getBadgeColor = (durum: string): 'success' | 'warning' | 'default' | 'primary' => {
    const classMap: { [key: string]: 'success' | 'warning' | 'default' | 'primary' } = {
      success: 'success',
      warning: 'warning',
      secondary: 'default',
      primary: 'primary',
    };
    return classMap[getDurumBadgeClass(durum)] || 'default';
  };

  if (loading) {
    return (
      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Box display="flex" justifyContent="center" alignItems="center" minHeight={400}>
          <CircularProgress size={60} />
        </Box>
      </Container>
    );
  }

  if (!puantaj) {
    return null;
  }

  return (
    <Container maxWidth="xl" sx={{ py: 4 }}>
      <Stack spacing={3}>
        {/* Header */}
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Stack direction="row" spacing={2} alignItems="center">
            <IconButton
              onClick={() => navigate('/puantaj')}
              color="primary"
            >
              <ArrowBackIcon />
            </IconButton>
            <Box>
              <Typography variant="h4" fontWeight="bold">
                {puantaj.personelAdi}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {getDonemText(puantaj.yil, puantaj.ay)} Puantajı
              </Typography>
            </Box>
          </Stack>
          
          <Stack direction="row" spacing={2}>
            {puantaj.durum === 'Taslak' && (
              <Button
                variant="contained"
                color="success"
                startIcon={<CheckIcon />}
                onClick={handleOnayla}
              >
                Onayla
              </Button>
            )}
            {puantaj.durum === 'Onaylandı' && (
              <Button
                variant="contained"
                color="warning"
                startIcon={<CloseIcon />}
                onClick={handleOnayIptal}
              >
                Onayı İptal Et
              </Button>
            )}
            <Button 
              variant="outlined" 
              startIcon={<DownloadIcon />}
            >
              Excel İndir
            </Button>
          </Stack>
        </Box>

        {/* Personel Bilgileri */}
        <Card>
          <CardContent>
            <Grid container spacing={3}>
              <Grid item xs={3}>
                <Typography variant="body2" color="text.secondary">
                  Sicil No
                </Typography>
                <Typography fontWeight="medium">{puantaj.sicilNo}</Typography>
              </Grid>
              <Grid item xs={3}>
                <Typography variant="body2" color="text.secondary">
                  Departman
                </Typography>
                <Typography fontWeight="medium">{puantaj.departman}</Typography>
              </Grid>
              <Grid item xs={3}>
                <Typography variant="body2" color="text.secondary">
                  Unvan
                </Typography>
                <Typography fontWeight="medium">{puantaj.unvan}</Typography>
              </Grid>
              <Grid item xs={3}>
                <Typography variant="body2" color="text.secondary">
                  Durum
                </Typography>
                <Chip
                  label={puantaj.durum}
                  color={getBadgeColor(puantaj.durum)}
                  size="small"
                />
              </Grid>
            </Grid>
          </CardContent>
        </Card>

        {/* İstatistikler */}
        <Grid container spacing={2}>
          <Grid item xs={3}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Toplam Çalışma
                </Typography>
                <Typography variant="h4" fontWeight="bold">
                  {formatDakika(puantaj.toplamCalismaSaati)}
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  {puantaj.toplamCalisilanGun} gün
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={3}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Normal Mesai
                </Typography>
                <Typography variant="h4" fontWeight="bold">
                  {formatDakika(puantaj.normalMesaiSaati)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={3}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Fazla Mesai
                </Typography>
                <Typography variant="h4" fontWeight="bold" color="success.main">
                  {formatDakika(puantaj.fazlaMesaiSaati)}
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  Gece: {formatDakika(puantaj.geceMesaiSaati)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={3}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Hafta Sonu
                </Typography>
                <Typography variant="h4" fontWeight="bold" sx={{ color: 'purple' }}>
                  {formatDakika(puantaj.haftaSonuMesaiSaati)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>

        {/* Devamsızlık & İzin */}
        <Grid container spacing={2}>
          <Grid item xs={2.4}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Devamsızlık
                </Typography>
                <Typography variant="h4" fontWeight="bold" color="error.main">
                  {puantaj.devamsizlikGunu} gün
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={2.4}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  İzin
                </Typography>
                <Typography variant="h4" fontWeight="bold" color="primary.main">
                  {puantaj.izinGunu} gün
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={2.4}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Raporlu
                </Typography>
                <Typography variant="h4" fontWeight="bold">
                  {puantaj.raporluGun} gün
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={2.4}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Geç Kalma
                </Typography>
                <Typography variant="h4" fontWeight="bold" color="warning.main">
                  {puantaj.gecKalmaGunu} gün
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  {formatDakika(puantaj.gecKalmaSuresi)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={2.4}>
            <Card>
              <CardContent>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Erken Çıkış
                </Typography>
                <Typography variant="h4" fontWeight="bold" color="warning.main">
                  {puantaj.erkenCikisGunu} gün
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  {formatDakika(puantaj.erkenCikisSuresi)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>

        {/* Günlük Detaylar */}
        <Card>
          <CardHeader
            title={<Typography variant="h6">Günlük Detaylar</Typography>}
          />
          <CardContent>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
              <Tabs value={tabValue} onChange={(e, newValue) => setTabValue(newValue)}>
                <Tab label={`Tümü (${puantaj.gunlukDetaylar.length})`} />
                <Tab label="Normal Günler" />
                <Tab label="Özel Durumlar" />
              </Tabs>
            </Box>

            <TabPanel value={tabValue} index={0}>
              <GunlukDetayTablosu detaylar={puantaj.gunlukDetaylar} />
            </TabPanel>

            <TabPanel value={tabValue} index={1}>
              <GunlukDetayTablosu
                detaylar={puantaj.gunlukDetaylar.filter(
                  (d) => d.gunDurumu === 'Normal'
                )}
              />
            </TabPanel>

            <TabPanel value={tabValue} index={2}>
              <GunlukDetayTablosu
                detaylar={puantaj.gunlukDetaylar.filter(
                  (d) => d.gunDurumu !== 'Normal'
                )}
              />
            </TabPanel>
          </CardContent>
        </Card>

        {/* Onay Bilgileri */}
        {puantaj.onayTarihi && (
          <Card sx={{ bgcolor: 'success.lighter', borderLeft: 4, borderColor: 'success.main' }}>
            <CardContent>
              <Stack direction="row" spacing={2} alignItems="center">
                <CheckIcon sx={{ fontSize: 32, color: 'success.main' }} />
                <Box>
                  <Typography fontWeight="medium" color="success.dark">
                    Puantaj Onaylandı
                  </Typography>
                  <Typography variant="body2" color="success.dark">
                    {puantaj.onaylayanKisi} tarafından{' '}
                    {new Date(puantaj.onayTarihi).toLocaleDateString('tr-TR')} tarihinde onaylandı
                  </Typography>
                </Box>
              </Stack>
            </CardContent>
          </Card>
        )}

        {/* Notlar */}
        {puantaj.notlar && (
          <Card>
            <CardHeader
              title={<Typography variant="h6">Notlar</Typography>}
            />
            <CardContent>
              <Typography>{puantaj.notlar}</Typography>
            </CardContent>
          </Card>
        )}
      </Stack>

      <Snackbar
        open={snackbar.open}
        autoHideDuration={5000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <Alert onClose={handleCloseSnackbar} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default PuantajDetay;
