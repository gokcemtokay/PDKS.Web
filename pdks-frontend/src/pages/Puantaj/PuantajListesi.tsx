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
  const [loading, setLoading] = useState<boolean>(false);
  const [yil, setYil] = useState<number>(new Date().getFullYear());
  const [ay, setAy] = useState<number>(new Date().getMonth() + 1);
  const [departmanId, setDepartmanId] = useState<number | undefined>();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedPuantajId, setSelectedPuantajId] = useState<number | null>(null);

  const ayListesi = getAyListesi();
  const yilListesi = getYilListesi();

  useEffect(() => {
    loadPuantajlar();
  }, [yil, ay, departmanId]);

  const loadPuantajlar = async () => {
    setLoading(true);
    try {
      const data = await puantajService.getByDonem(yil, ay, departmanId);
      setPuantajlar(data);
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Puantajlar yüklenemedi',
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
                  displayEmpty
                  size="small"
                >
                  <MenuItem value="">Tüm Departmanlar</MenuItem>
                  {/* Departmanlar buraya eklenecek */}
                </Select>
              </FormControl>

              <IconButton
                onClick={loadPuantajlar}
                disabled={loading}
                color="primary"
              >
                <RefreshIcon />
              </IconButton>
            </Stack>
          </CardContent>
        </Card>

        {/* Statistics */}
        <Stack direction="row" spacing={2}>
          <Card sx={{ flex: 1 }}>
            <CardContent>
              <Typography variant="body2" color="text.secondary">
                Toplam Personel
              </Typography>
              <Typography variant="h4" color="primary" fontWeight="bold">
                {stats.toplamPersonel}
              </Typography>
            </CardContent>
          </Card>

          <Card sx={{ flex: 1 }}>
            <CardContent>
              <Typography variant="body2" color="text.secondary">
                Toplam Çalışma
              </Typography>
              <Typography variant="h4" color="success.main" fontWeight="bold">
                {formatDakika(stats.toplamCalismaSaati)}
              </Typography>
            </CardContent>
          </Card>

          <Card sx={{ flex: 1 }}>
            <CardContent>
              <Typography variant="body2" color="text.secondary">
                Fazla Mesai
              </Typography>
              <Typography variant="h4" sx={{ color: 'purple' }} fontWeight="bold">
                {formatDakika(stats.toplamFazlaMesai)}
              </Typography>
            </CardContent>
          </Card>

          <Card sx={{ flex: 1 }}>
            <CardContent>
              <Typography variant="body2" color="text.secondary">
                Onay Bekleyen
              </Typography>
              <Typography variant="h4" color="warning.main" fontWeight="bold">
                {stats.onayBekleyen}
              </Typography>
            </CardContent>
          </Card>
        </Stack>

        {/* Table */}
        <Card>
          <CardContent>
            {loading ? (
              <Box display="flex" justifyContent="center" alignItems="center" minHeight={200}>
                <CircularProgress />
              </Box>
            ) : (
              <Box sx={{ overflowX: 'auto' }}>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Sicil No</TableCell>
                      <TableCell>Personel</TableCell>
                      <TableCell>Departman</TableCell>
                      <TableCell align="center">Çalışma Saati</TableCell>
                      <TableCell align="center">Çalışma Günü</TableCell>
                      <TableCell align="center">Fazla Mesai</TableCell>
                      <TableCell align="center">Devamsızlık</TableCell>
                      <TableCell align="center">Durum</TableCell>
                      <TableCell align="center">İşlemler</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {puantajlar.map((puantaj) => (
                      <TableRow key={puantaj.id} hover>
                        <TableCell sx={{ fontWeight: 'medium' }}>{puantaj.sicilNo}</TableCell>
                        <TableCell>{puantaj.personelAdi}</TableCell>
                        <TableCell>{puantaj.departman}</TableCell>
                        <TableCell align="center">
                          {formatDakika(puantaj.toplamCalismaSaati)}
                        </TableCell>
                        <TableCell align="center">{puantaj.toplamCalisilanGun} gün</TableCell>
                        <TableCell align="center">
                          {puantaj.fazlaMesaiSaati > 0 ? (
                            <Typography color="success.main" fontWeight="medium">
                              {formatDakika(puantaj.fazlaMesaiSaati)}
                            </Typography>
                          ) : (
                            '-'
                          )}
                        </TableCell>
                        <TableCell align="center">
                          {puantaj.devamsizlikGunu > 0 ? (
                            <Typography color="error.main" fontWeight="medium">
                              {puantaj.devamsizlikGunu} gün
                            </Typography>
                          ) : (
                            '-'
                          )}
                        </TableCell>
                        <TableCell align="center">
                          <Chip
                            label={puantaj.durum}
                            color={getBadgeColor(puantaj.durum)}
                            size="small"
                          />
                        </TableCell>
                        <TableCell align="center">
                          <Stack direction="row" spacing={1} justifyContent="center">
                            <IconButton
                              size="small"
                              onClick={() => handleDetayGor(puantaj.id)}
                              color="primary"
                            >
                              <VisibilityIcon fontSize="small" />
                            </IconButton>
                            <IconButton
                              size="small"
                              onClick={(e) => handleMenuOpen(e, puantaj.id)}
                            >
                              <MoreVertIcon fontSize="small" />
                            </IconButton>
                          </Stack>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>

                {puantajlar.length === 0 && (
                  <Box p={4} textAlign="center">
                    <Typography color="text.secondary">
                      {getTurkceAyAdi(ay)} {yil} için puantaj bulunamadı
                    </Typography>
                  </Box>
                )}
              </Box>
            )}
          </CardContent>
        </Card>
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
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        onSuccess={loadPuantajlar}
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
