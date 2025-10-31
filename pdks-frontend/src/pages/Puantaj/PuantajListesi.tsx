import React, { useState, useEffect } from 'react';
import {
  Box,
  Container,
  Typography,
  Stack,
  Button,
  Select,
  MenuItem,
  Table,
  TableHead,
  TableBody,
  TableRow,
  TableCell,
  TableContainer,
  Chip,
  IconButton,
  CircularProgress,
  Card,
  CardContent,
  Paper,
  FormControl,
  FormLabel,
  Menu,
  MenuList,
  MenuItem as MuiMenuItem,
  Snackbar,
  Alert,
  Grid,
} from '@mui/material';
import {
  Visibility as VisibilityIcon,
  Refresh as RefreshIcon,
  Download as DownloadIcon,
  MoreVert as MoreVertIcon,
  Add as AddIcon,
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import puantajService from '../../services/puantajService';
import departmanService from '../../services/departmanService';
import { PuantajList } from '../../types/puantaj.types';
import {
  formatDakika,
  getDurumBadgeClass,
  getAyListesi,
  getYilListesi,
  getTurkceAyAdi,
} from '../../utils/puantajUtils';
import TopluPuantajHesaplaModal from '../../components/TopluPuantajHesaplaModal';

const PuantajListesi: React.FC = () => {
  const navigate = useNavigate();
  const [modalOpen, setModalOpen] = useState(false);
  const [snackbar, setSnackbar] = useState<{ open: boolean; message: string; severity: 'success' | 'error' }>({
    open: false,
    message: '',
    severity: 'success',
  });

  const [puantajlar, setPuantajlar] = useState<PuantajList[]>([]);
  const [departmanlar, setDepartmanlar] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [yil, setYil] = useState<number>(new Date().getFullYear());
  const [ay, setAy] = useState<number>(new Date().getMonth() + 1);
  const [departmanId, setDepartmanId] = useState<number | undefined>();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedPuantajId, setSelectedPuantajId] = useState<number | null>(null);

  const ayListesi = getAyListesi();
  const yilListesi = getYilListesi();

  useEffect(() => {
    loadDepartmanlar();
  }, []);

  useEffect(() => {
    loadPuantajlar();
  }, [yil, ay, departmanId]);

  const loadDepartmanlar = async () => {
    try {
      const data = await departmanService.getAll();
      setDepartmanlar(data);
    } catch (error: any) {
      console.error('Departmanlar yüklenemedi:', error);
    }
  };

  const loadPuantajlar = async () => {
    setLoading(true);
    try {
      const data = await puantajService.getByDonem(yil, ay, departmanId);
      setPuantajlar(data);
      
      // Hata mesajını kaldır
      if (snackbar.message === 'Puantajlar yüklenemedi') {
        setSnackbar({ open: false, message: '', severity: 'success' });
      }
    } catch (error: any) {
      console.error('Puantaj yükleme hatası:', error);
      setSnackbar({
        open: true,
        message: error.response?.data?.message || 'Puantajlar yüklenemedi',
        severity: 'error',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleDetayGor = (id: number) => {
    navigate(`/puantaj/${id}`);
  };

  const handleYenidenHesapla = async (id: number) => {
    handleMenuClose();
    try {
      await puantajService.yenidenHesapla(id);
      setSnackbar({
        open: true,
        message: 'Puantaj yeniden hesaplandı',
        severity: 'success',
      });
      loadPuantajlar();
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.response?.data?.message || 'Yeniden hesaplanamadı',
        severity: 'error',
      });
    }
  };

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>, puantajId: number) => {
    setAnchorEl(event.currentTarget);
    setSelectedPuantajId(puantajId);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedPuantajId(null);
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

  const getStatistics = () => {
    const toplamPersonel = puantajlar.length;
    const toplamCalismaSaati = puantajlar.reduce((sum, p) => sum + p.toplamCalismaSaati, 0);
    const toplamFazlaMesai = puantajlar.reduce((sum, p) => sum + p.fazlaMesaiSaati, 0);
    const toplamDevamsizlik = puantajlar.reduce((sum, p) => sum + p.devamsizlikGunu, 0);
    const onayBekleyen = puantajlar.filter(p => p.durum === 'Taslak').length;

    return {
      toplamPersonel,
      toplamCalismaSaati,
      toplamFazlaMesai,
      toplamDevamsizlik,
      onayBekleyen,
    };
  };

  const stats = getStatistics();

  return (
    <Container maxWidth="xl" sx={{ py: 4 }}>
      <Stack spacing={3}>
        {/* Header */}
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Typography variant="h4" fontWeight="bold">
            Puantaj Yönetimi
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => setModalOpen(true)}
          >
            Toplu Hesapla
          </Button>
        </Box>

        {/* İstatistikler */}
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6} md={2.4}>
            <Card>
              <CardContent>
                <Typography color="text.secondary" variant="body2" gutterBottom>
                  Toplam Çalışma
                </Typography>
                <Typography variant="h5" fontWeight="bold">
                  {formatDakika(stats.toplamCalismaSaati)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={2.4}>
            <Card>
              <CardContent>
                <Typography color="text.secondary" variant="body2" gutterBottom>
                  Fazla Mesai
                </Typography>
                <Typography variant="h5" fontWeight="bold" color="primary">
                  {formatDakika(stats.toplamFazlaMesai)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={2.4}>
            <Card>
              <CardContent>
                <Typography color="text.secondary" variant="body2" gutterBottom>
                  Onay Bekleyen
                </Typography>
                <Typography variant="h5" fontWeight="bold" color="warning.main">
                  {stats.onayBekleyen}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={2.4}>
            <Card>
              <CardContent>
                <Typography color="text.secondary" variant="body2" gutterBottom>
                  Devamsızlık
                </Typography>
                <Typography variant="h5" fontWeight="bold" color="error">
                  {stats.toplamDevamsizlik}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={2.4}>
            <Card>
              <CardContent>
                <Typography color="text.secondary" variant="body2" gutterBottom>
                  Toplam Personel
                </Typography>
                <Typography variant="h5" fontWeight="bold">
                  {stats.toplamPersonel}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>

        {/* Filters */}
        <Card>
          <CardContent>
            <Stack direction="row" spacing={2} alignItems="flex-end">
              <FormControl sx={{ minWidth: 120 }}>
                <FormLabel>Yıl</FormLabel>
                <Select
                  value={yil}
                  onChange={(e) => setYil(Number(e.target.value))}
                  size="small"
                >
                  {yilListesi.map((y) => (
                    <MenuItem key={y} value={y}>
                      {y}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>

              <FormControl sx={{ minWidth: 150 }}>
                <FormLabel>Ay</FormLabel>
                <Select
                  value={ay}
                  onChange={(e) => setAy(Number(e.target.value))}
                  size="small"
                >
                  {ayListesi.map((a) => (
                    <MenuItem key={a.value} value={a.value}>
                      {a.label}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>

              <FormControl sx={{ flex: 1 }}>
                <FormLabel>Departman</FormLabel>
                <Select
                  value={departmanId || ''}
                  onChange={(e) =>
                    setDepartmanId(e.target.value ? Number(e.target.value) : undefined)
                  }
                  size="small"
                  displayEmpty
                >
                  <MenuItem value="">Tüm Departmanlar</MenuItem>
                  {departmanlar.map((dept) => (
                    <MenuItem key={dept.id} value={dept.id}>
                      {dept.departmanAdi}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>

              <Button
                variant="outlined"
                startIcon={<RefreshIcon />}
                onClick={loadPuantajlar}
                disabled={loading}
              >
                Yenile
              </Button>
            </Stack>
          </CardContent>
        </Card>

        {/* Loading */}
        {loading && (
          <Box display="flex" justifyContent="center" py={4}>
            <CircularProgress />
          </Box>
        )}

        {/* Tablo */}
        {!loading && (
          <Paper>
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow sx={{ bgcolor: 'grey.100' }}>
                    <TableCell><strong>Sicil No</strong></TableCell>
                    <TableCell><strong>Personel</strong></TableCell>
                    <TableCell><strong>Departman</strong></TableCell>
                    <TableCell><strong>Çalışma Saati</strong></TableCell>
                    <TableCell><strong>Çalışılan Gün</strong></TableCell>
                    <TableCell><strong>Fazla Mesai</strong></TableCell>
                    <TableCell><strong>Devamsızlık</strong></TableCell>
                    <TableCell><strong>Durum</strong></TableCell>
                    <TableCell align="center"><strong>İşlemler</strong></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {puantajlar.length === 0 ? (
                    <TableRow>
                      <TableCell colSpan={9} align="center">
                        <Typography variant="body2" color="text.secondary" py={4}>
                          {getTurkceAyAdi(ay)} {yil} için puantaj bulunamadı
                        </Typography>
                      </TableCell>
                    </TableRow>
                  ) : (
                    puantajlar.map((puantaj) => (
                      <TableRow key={puantaj.id} hover>
                        <TableCell>{puantaj.sicilNo}</TableCell>
                        <TableCell>{puantaj.personelAdi}</TableCell>
                        <TableCell>{puantaj.departman}</TableCell>
                        <TableCell>{formatDakika(puantaj.toplamCalismaSaati)}</TableCell>
                        <TableCell>{puantaj.toplamCalisilanGun} gün</TableCell>
                        <TableCell>
                          {puantaj.fazlaMesaiSaati > 0 ? formatDakika(puantaj.fazlaMesaiSaati) : '-'}
                        </TableCell>
                        <TableCell>
                          {puantaj.devamsizlikGunu > 0 ? `${puantaj.devamsizlikGunu} gün` : '-'}
                        </TableCell>
                        <TableCell>
                          <Chip
                            label={puantaj.durum}
                            color={getBadgeColor(puantaj.durum)}
                            size="small"
                          />
                        </TableCell>
                        <TableCell align="center">
                          <IconButton
                            size="small"
                            color="primary"
                            onClick={() => handleDetayGor(puantaj.id)}
                          >
                            <VisibilityIcon />
                          </IconButton>
                          <IconButton
                            size="small"
                            onClick={(e) => handleMenuOpen(e, puantaj.id)}
                          >
                            <MoreVertIcon />
                          </IconButton>
                        </TableCell>
                      </TableRow>
                    ))
                  )}
                </TableBody>
              </Table>
            </TableContainer>
          </Paper>
        )}
      </Stack>

      {/* Context Menu */}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MuiMenuItem onClick={() => selectedPuantajId && handleYenidenHesapla(selectedPuantajId)}>
          <RefreshIcon fontSize="small" sx={{ mr: 1 }} />
          Yeniden Hesapla
        </MuiMenuItem>
        <MuiMenuItem onClick={handleMenuClose}>
          <DownloadIcon fontSize="small" sx={{ mr: 1 }} />
          Excel İndir
        </MuiMenuItem>
      </Menu>

      {/* Modal */}
      <TopluPuantajHesaplaModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSuccess={() => {
          setModalOpen(false);
          loadPuantajlar();
        }}
        defaultYil={yil}
        defaultAy={ay}
        departmanlar={departmanlar}
      />

      {/* Snackbar */}
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

export default PuantajListesi;
