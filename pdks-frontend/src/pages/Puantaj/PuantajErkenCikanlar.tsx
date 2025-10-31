import { useState } from 'react';
import {
  Box,
  Container,
  Typography,
  Paper,
  TextField,
  Button,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  CircularProgress,
  Alert,
  Chip,
  Card,
  CardContent,
  Grid,
} from '@mui/material';
import {
  Search as SearchIcon,
  Download as DownloadIcon,
  ExitToApp as ExitToAppIcon,
} from '@mui/icons-material';
import puantajService from '../../services/puantajService';
import { ErkenCikanRapor } from '../../types/puantaj.types';
import { formatTarih, formatSaat24 } from '../../utils/puantajUtils';

function PuantajErkenCikanlar() {
  const [baslangicTarihi, setBaslangicTarihi] = useState<string>(
    new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString().split('T')[0]
  );
  const [bitisTarihi, setBitisTarihi] = useState<string>(
    new Date().toISOString().split('T')[0]
  );
  const [loading, setLoading] = useState(false);
  const [rapor, setRapor] = useState<ErkenCikanRapor[]>([]);
  const [error, setError] = useState<string | null>(null);

  const handleAra = async () => {
    try {
      setError(null);
      setLoading(true);
      const data = await puantajService.getErkenCikanlar(baslangicTarihi, bitisTarihi);
      setRapor(data);
    } catch (error: any) {
      console.error('Erken çıkanlar raporu yüklenemedi:', error);
      setError('Rapor yüklenirken hata oluştu. Lütfen tekrar deneyin.');
    } finally {
      setLoading(false);
    }
  };

  const handleExport = () => {
    // Excel export işlemi
    console.log('Excel export');
  };

  const getStatistics = () => {
    const toplamKayit = rapor.length;
    const toplamPersonel = new Set(rapor.map(r => r.personelId)).size;
    const toplamErkenCikisSuresi = rapor.reduce((sum, r) => sum + r.erkenCikisSuresi, 0);
    const ortalama = toplamKayit > 0 ? toplamErkenCikisSuresi / toplamKayit : 0;

    return {
      toplamKayit,
      toplamPersonel,
      toplamErkenCikisSuresi: Math.floor(toplamErkenCikisSuresi / 60) + ' saat ' + (toplamErkenCikisSuresi % 60) + ' dk',
      ortalama: Math.floor(ortalama) + ' dakika',
    };
  };

  const stats = getStatistics();

  return (
    <Container maxWidth="xl" sx={{ py: 4 }}>
      <Stack spacing={3}>
        {/* Header */}
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Box>
            <Typography variant="h4" fontWeight="bold" gutterBottom>
              Erken Çıkanlar Raporu
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Belirlenen tarih aralığında mesai bitiş saatinden önce çıkan personellerin detaylı raporu
            </Typography>
          </Box>
          <ExitToAppIcon sx={{ fontSize: 48, color: 'info.main' }} />
        </Box>

        {/* Filtreler */}
        <Paper sx={{ p: 3 }}>
          <Stack direction="row" spacing={2} alignItems="center">
            <TextField
              label="Başlangıç Tarihi"
              type="date"
              value={baslangicTarihi}
              onChange={(e) => setBaslangicTarihi(e.target.value)}
              InputLabelProps={{ shrink: true }}
              sx={{ minWidth: 200 }}
            />
            <TextField
              label="Bitiş Tarihi"
              type="date"
              value={bitisTarihi}
              onChange={(e) => setBitisTarihi(e.target.value)}
              InputLabelProps={{ shrink: true }}
              sx={{ minWidth: 200 }}
            />
            <Button
              variant="contained"
              startIcon={<SearchIcon />}
              onClick={handleAra}
              disabled={loading}
            >
              Ara
            </Button>
            {rapor.length > 0 && (
              <Button
                variant="outlined"
                startIcon={<DownloadIcon />}
                onClick={handleExport}
              >
                Excel'e Aktar
              </Button>
            )}
          </Stack>
        </Paper>

        {/* İstatistikler */}
        {rapor.length > 0 && (
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6} md={3}>
              <Card>
                <CardContent>
                  <Typography color="text.secondary" gutterBottom>
                    Toplam Kayıt
                  </Typography>
                  <Typography variant="h4" fontWeight="bold">
                    {stats.toplamKayit}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Card>
                <CardContent>
                  <Typography color="text.secondary" gutterBottom>
                    Erken Çıkan Personel
                  </Typography>
                  <Typography variant="h4" fontWeight="bold" color="info.main">
                    {stats.toplamPersonel}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Card>
                <CardContent>
                  <Typography color="text.secondary" gutterBottom>
                    Toplam Eksik Süre
                  </Typography>
                  <Typography variant="h4" fontWeight="bold" color="error.main">
                    {stats.toplamErkenCikisSuresi}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Card>
                <CardContent>
                  <Typography color="text.secondary" gutterBottom>
                    Ortalama Eksiklik
                  </Typography>
                  <Typography variant="h4" fontWeight="bold">
                    {stats.ortalama}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        )}

        {/* Hata Mesajı */}
        {error && (
          <Alert severity="error" onClose={() => setError(null)}>
            {error}
          </Alert>
        )}

        {/* Loading */}
        {loading && (
          <Box display="flex" justifyContent="center" py={4}>
            <CircularProgress />
          </Box>
        )}

        {/* Tablo */}
        {!loading && rapor.length > 0 && (
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow sx={{ bgcolor: 'grey.100' }}>
                  <TableCell><strong>Sicil No</strong></TableCell>
                  <TableCell><strong>Personel</strong></TableCell>
                  <TableCell><strong>Departman</strong></TableCell>
                  <TableCell><strong>Tarih</strong></TableCell>
                  <TableCell><strong>Vardiya</strong></TableCell>
                  <TableCell><strong>Planlanan Çıkış</strong></TableCell>
                  <TableCell><strong>Gerçekleşen Çıkış</strong></TableCell>
                  <TableCell><strong>Erken Çıkış Süresi</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {rapor.map((item, index) => (
                  <TableRow key={index} hover>
                    <TableCell>{item.sicilNo}</TableCell>
                    <TableCell>{item.adSoyad}</TableCell>
                    <TableCell>{item.departman}</TableCell>
                    <TableCell>{formatTarih(item.tarih)}</TableCell>
                    <TableCell>{item.vardiya || '-'}</TableCell>
                    <TableCell>{formatSaat24(item.planlananCikis)}</TableCell>
                    <TableCell>
                      <Chip
                        label={formatSaat24(item.gerceklesenCikis)}
                        color="info"
                        size="small"
                      />
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={item.erkenCikisSuresiFormatli || `${item.erkenCikisSuresi} dk`}
                        color="error"
                        size="small"
                      />
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}

        {/* Sonuç Yok */}
        {!loading && rapor.length === 0 && baslangicTarihi && bitisTarihi && (
          <Paper sx={{ p: 4, textAlign: 'center' }}>
            <Typography variant="h6" color="text.secondary">
              Seçilen tarih aralığında erken çıkan personel bulunamadı
            </Typography>
          </Paper>
        )}
      </Stack>
    </Container>
  );
}

export default PuantajErkenCikanlar;
