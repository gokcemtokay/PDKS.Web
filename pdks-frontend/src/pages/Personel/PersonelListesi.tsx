import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box, Paper, Typography, Button, TextField, InputAdornment,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  IconButton, Chip, Avatar, Menu, MenuItem, Dialog, DialogTitle,
  DialogContent, DialogActions, Alert, Pagination, Select, FormControl,
  InputLabel, Grid,
} from '@mui/material';
import {
  Add, Search, FilterList, Edit, Delete, Visibility,
  MoreVert, FileDownload, CloudUpload, Person,
} from '@mui/icons-material';
import personelService from '../../services/personelService';

function PersonelListesi() {
  const navigate = useNavigate();
  const [personeller, setPersoneller] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedPersonel, setSelectedPersonel] = useState<any>(null);
  const [deleteDialog, setDeleteDialog] = useState(false);
  const [filterDepartman, setFilterDepartman] = useState('');
  const [filterDurum, setFilterDurum] = useState('');

  useEffect(() => {
    loadPersoneller();
  }, [page, searchTerm, filterDepartman, filterDurum]);

  const loadPersoneller = async () => {
    setLoading(true);
    try {
      const data = await personelService.getAll();
      let filtered = data;

      // Arama filtresi
      if (searchTerm) {
        filtered = filtered.filter((p: any) =>
          p.adSoyad?.toLowerCase().includes(searchTerm.toLowerCase()) ||
          p.sicilNo?.toString().includes(searchTerm) ||
          p.email?.toLowerCase().includes(searchTerm.toLowerCase())
        );
      }

      // Departman filtresi
      if (filterDepartman) {
        filtered = filtered.filter((p: any) => p.departmanId === parseInt(filterDepartman));
      }

      // Durum filtresi
      if (filterDurum !== '') {
        filtered = filtered.filter((p: any) => p.durum === (filterDurum === 'true'));
      }

      setPersoneller(filtered);
      setTotalPages(Math.ceil(filtered.length / 10));
    } catch (error) {
      console.error('Personel listesi yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleMenuClick = (event: React.MouseEvent<HTMLElement>, personel: any) => {
    setAnchorEl(event.currentTarget);
    setSelectedPersonel(personel);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedPersonel(null);
  };

  const handleDelete = async () => {
    if (!selectedPersonel) return;
    try {
      await personelService.delete(selectedPersonel.id);
      loadPersoneller();
      setDeleteDialog(false);
      handleMenuClose();
    } catch (error) {
      console.error('Personel silinemedi:', error);
    }
  };

  const handleExport = () => {
    // Excel export işlemi
    console.log('Excel export...');
  };

  return (
    <Box>
      {/* Header */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">Personel Yönetimi</Typography>
        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button
            variant="outlined"
            startIcon={<FileDownload />}
            onClick={handleExport}
          >
            Dışa Aktar
          </Button>
          <Button
            variant="contained"
            startIcon={<Add />}
            onClick={() => navigate('/personel/yeni')}
          >
            Yeni Personel
          </Button>
        </Box>
      </Box>

      {/* Filtreler */}
      <Paper sx={{ p: 2, mb: 3 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={4}>
            <TextField
              fullWidth
              size="small"
              placeholder="Ad, Sicil No veya E-posta ile ara..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Search />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          <Grid item xs={12} md={3}>
            <FormControl fullWidth size="small">
              <InputLabel>Departman</InputLabel>
              <Select
                value={filterDepartman}
                onChange={(e) => setFilterDepartman(e.target.value)}
                label="Departman"
              >
                <MenuItem value="">Tümü</MenuItem>
                <MenuItem value="1">IT</MenuItem>
                <MenuItem value="2">İnsan Kaynakları</MenuItem>
                <MenuItem value="3">Muhasebe</MenuItem>
                <MenuItem value="4">Satış</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={3}>
            <FormControl fullWidth size="small">
              <InputLabel>Durum</InputLabel>
              <Select
                value={filterDurum}
                onChange={(e) => setFilterDurum(e.target.value)}
                label="Durum"
              >
                <MenuItem value="">Tümü</MenuItem>
                <MenuItem value="true">Aktif</MenuItem>
                <MenuItem value="false">Pasif</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={2}>
            <Button
              fullWidth
              variant="outlined"
              startIcon={<FilterList />}
              onClick={() => {
                setSearchTerm('');
                setFilterDepartman('');
                setFilterDurum('');
              }}
            >
              Temizle
            </Button>
          </Grid>
        </Grid>
      </Paper>

      {/* Tablo */}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow sx={{ backgroundColor: 'grey.100' }}>
              <TableCell width="60px"><strong>#</strong></TableCell>
              <TableCell><strong>Fotoğraf</strong></TableCell>
              <TableCell><strong>Sicil No</strong></TableCell>
              <TableCell><strong>Ad Soyad</strong></TableCell>
              <TableCell><strong>Departman</strong></TableCell>
              <TableCell><strong>Görev</strong></TableCell>
              <TableCell><strong>E-posta</strong></TableCell>
              <TableCell><strong>Telefon</strong></TableCell>
              <TableCell><strong>Durum</strong></TableCell>
              <TableCell width="100px" align="center"><strong>İşlemler</strong></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={10} align="center">Yükleniyor...</TableCell>
              </TableRow>
            ) : personeller.length === 0 ? (
              <TableRow>
                <TableCell colSpan={10} align="center">
                  <Box sx={{ py: 3 }}>
                    <Person sx={{ fontSize: 60, color: 'grey.400', mb: 2 }} />
                    <Typography color="text.secondary">Henüz personel kaydı bulunmamaktadır</Typography>
                    <Button
                      variant="contained"
                      startIcon={<Add />}
                      sx={{ mt: 2 }}
                      onClick={() => navigate('/personel/yeni')}
                    >
                      İlk Personeli Ekle
                    </Button>
                  </Box>
                </TableCell>
              </TableRow>
            ) : (
              personeller.slice((page - 1) * 10, page * 10).map((personel, index) => (
                <TableRow
                  key={personel.id}
                  hover
                  sx={{ cursor: 'pointer' }}
                  onClick={() => navigate(`/personel/${personel.id}`)}
                >
                  <TableCell>{(page - 1) * 10 + index + 1}</TableCell>
                  <TableCell>
                    <Avatar src={personel.profilFoto || ''} alt={personel.adSoyad}>
                      {personel.adSoyad?.[0]}
                    </Avatar>
                  </TableCell>
                  <TableCell>{personel.sicilNo}</TableCell>
                  <TableCell>
                    <Typography variant="body2" fontWeight="bold">{personel.adSoyad}</Typography>
                  </TableCell>
                  <TableCell>{personel.departmanAdi || '-'}</TableCell>
                  <TableCell>{personel.gorev || '-'}</TableCell>
                  <TableCell>{personel.email || '-'}</TableCell>
                  <TableCell>{personel.telefon || '-'}</TableCell>
                  <TableCell>
                    <Chip
                      label={personel.durum ? 'Aktif' : 'Pasif'}
                      color={personel.durum ? 'success' : 'error'}
                      size="small"
                    />
                  </TableCell>
                  <TableCell align="center">
                    <IconButton
                      size="small"
                      onClick={(e) => {
                        e.stopPropagation();
                        handleMenuClick(e, personel);
                      }}
                    >
                      <MoreVert />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Pagination */}
      {totalPages > 1 && (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
          <Pagination
            count={totalPages}
            page={page}
            onChange={(_, value) => setPage(value)}
            color="primary"
          />
        </Box>
      )}

      {/* Menu */}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={() => {
          navigate(`/personel/${selectedPersonel?.id}`);
          handleMenuClose();
        }}>
          <Visibility fontSize="small" sx={{ mr: 1 }} />
          Detay
        </MenuItem>
        <MenuItem onClick={() => {
          navigate(`/personel/duzenle/${selectedPersonel?.id}`);
          handleMenuClose();
        }}>
          <Edit fontSize="small" sx={{ mr: 1 }} />
          Düzenle
        </MenuItem>
        <MenuItem onClick={() => {
          setDeleteDialog(true);
          handleMenuClose();
        }}>
          <Delete fontSize="small" sx={{ mr: 1 }} />
          Sil
        </MenuItem>
      </Menu>

      {/* Delete Dialog */}
      <Dialog open={deleteDialog} onClose={() => setDeleteDialog(false)}>
        <DialogTitle>Personel Silme</DialogTitle>
        <DialogContent>
          <Alert severity="warning" sx={{ mb: 2 }}>
            Bu işlem geri alınamaz!
          </Alert>
          <Typography>
            <strong>{selectedPersonel?.adSoyad}</strong> adlı personeli silmek istediğinizden emin misiniz?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialog(false)}>İptal</Button>
          <Button onClick={handleDelete} variant="contained" color="error">
            Sil
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}

export default PersonelListesi;
