import { useEffect, useState } from 'react';
import {
  Box,
  Typography,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  IconButton,
  Chip,
  Switch,
  FormControlLabel,
} from '@mui/material';
import { DataGrid, GridColDef, GridActionsCellItem } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon, Business as BusinessIcon } from '@mui/icons-material';
import sirketService, { Sirket, SirketCreateDTO, SirketUpdateDTO } from '../../services/sirketService';

function SirketList() {
  const [sirketler, setSirketler] = useState<Sirket[]>([]);
  const [loading, setLoading] = useState(true);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [currentSirket, setCurrentSirket] = useState<Partial<Sirket>>({
    unvan: '',
    vergiNo: '',
    vergiDairesi: '',
    telefon: '',
    email: '',
    adres: '',
    logoUrl: '',
    aktif: true,
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await sirketService.getAll();
      setSirketler(data);
    } catch (error) {
      console.error('Şirketler yüklenemedi:', error);
      alert('Şirketler yüklenirken hata oluştu!');
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDialog = (sirket?: Sirket) => {
    if (sirket) {
      setEditMode(true);
      setCurrentSirket(sirket);
    } else {
      setEditMode(false);
      setCurrentSirket({
        unvan: '',
        vergiNo: '',
        vergiDairesi: '',
        telefon: '',
        email: '',
        adres: '',
        logoUrl: '',
        aktif: true,
      });
    }
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setCurrentSirket({
      unvan: '',
      vergiNo: '',
      vergiDairesi: '',
      telefon: '',
      email: '',
      adres: '',
      logoUrl: '',
      aktif: true,
    });
  };

  const handleSave = async () => {
    try {
      if (editMode && currentSirket.id) {
        await sirketService.update(currentSirket.id, currentSirket as SirketUpdateDTO);
        alert('Şirket güncellendi!');
      } else {
        await sirketService.create(currentSirket as SirketCreateDTO);
        alert('Şirket oluşturuldu!');
      }
      handleCloseDialog();
      loadData();
    } catch (error) {
      console.error('Kaydetme hatası:', error);
      alert('Kaydetme sırasında hata oluştu!');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bu şirketi silmek istediğinize emin misiniz?')) {
      try {
        await sirketService.delete(id);
        alert('Şirket silindi!');
        loadData();
      } catch (error) {
        console.error('Silme hatası:', error);
        alert('Silme sırasında hata oluştu!');
      }
    }
  };

  const columns: GridColDef[] = [
    {
      field: 'unvan',
      headerName: 'Şirket Ünvanı',
      width: 250,
      renderCell: (params) => (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <BusinessIcon sx={{ mr: 1, color: 'primary.main' }} />
          <strong>{params.value}</strong>
        </Box>
      ),
    },
    { field: 'vergiNo', headerName: 'Vergi No', width: 150 },
    { field: 'vergiDairesi', headerName: 'Vergi Dairesi', width: 150 },
    { field: 'telefon', headerName: 'Telefon', width: 130 },
    { field: 'email', headerName: 'E-posta', width: 200 },
    {
      field: 'aktif',
      headerName: 'Durum',
      width: 100,
      renderCell: (params) => (
        <Chip label={params.value ? 'Aktif' : 'Pasif'} color={params.value ? 'success' : 'default'} size="small" />
      ),
    },
    {
      field: 'kayitTarihi',
      headerName: 'Kayıt Tarihi',
      width: 130,
      valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR'),
    },
    {
      field: 'actions',
      type: 'actions',
      headerName: 'İşlemler',
      width: 100,
      getActions: (params) => [
        <GridActionsCellItem icon={<EditIcon />} label="Düzenle" onClick={() => handleOpenDialog(params.row)} />,
        <GridActionsCellItem icon={<DeleteIcon />} label="Sil" onClick={() => handleDelete(params.row.id)} />,
      ],
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">
          Şirket Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={() => handleOpenDialog()}>
          Yeni Şirket
        </Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={sirketler}
          columns={columns}
          loading={loading}
          pageSizeOptions={[10, 25, 50]}
          initialState={{
            pagination: { paginationModel: { pageSize: 10 } },
          }}
        />
      </Box>

      {/* Dialog */}
      <Dialog open={dialogOpen} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editMode ? 'Şirket Düzenle' : 'Yeni Şirket Ekle'}</DialogTitle>
        <DialogContent>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
            <TextField
              label="Şirket Ünvanı"
              value={currentSirket.unvan}
              onChange={(e) => setCurrentSirket({ ...currentSirket, unvan: e.target.value })}
              required
              fullWidth
            />
            <Box sx={{ display: 'flex', gap: 2 }}>
              <TextField
                label="Vergi No"
                value={currentSirket.vergiNo}
                onChange={(e) => setCurrentSirket({ ...currentSirket, vergiNo: e.target.value })}
                required
                fullWidth
              />
              <TextField
                label="Vergi Dairesi"
                value={currentSirket.vergiDairesi}
                onChange={(e) => setCurrentSirket({ ...currentSirket, vergiDairesi: e.target.value })}
                required
                fullWidth
              />
            </Box>
            <Box sx={{ display: 'flex', gap: 2 }}>
              <TextField
                label="Telefon"
                value={currentSirket.telefon}
                onChange={(e) => setCurrentSirket({ ...currentSirket, telefon: e.target.value })}
                required
                fullWidth
              />
              <TextField
                label="E-posta"
                type="email"
                value={currentSirket.email}
                onChange={(e) => setCurrentSirket({ ...currentSirket, email: e.target.value })}
                required
                fullWidth
              />
            </Box>
            <TextField
              label="Adres"
              value={currentSirket.adres}
              onChange={(e) => setCurrentSirket({ ...currentSirket, adres: e.target.value })}
              required
              multiline
              rows={3}
              fullWidth
            />
            <TextField
              label="Logo URL (opsiyonel)"
              value={currentSirket.logoUrl}
              onChange={(e) => setCurrentSirket({ ...currentSirket, logoUrl: e.target.value })}
              fullWidth
            />
            {editMode && (
              <FormControlLabel
                control={
                  <Switch
                    checked={currentSirket.aktif}
                    onChange={(e) => setCurrentSirket({ ...currentSirket, aktif: e.target.checked })}
                  />
                }
                label="Aktif"
              />
            )}
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>İptal</Button>
          <Button onClick={handleSave} variant="contained">
            {editMode ? 'Güncelle' : 'Kaydet'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}

export default SirketList;
