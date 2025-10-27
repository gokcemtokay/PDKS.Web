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
  MenuItem,
  Select,
  FormControl,
  InputLabel,
} from '@mui/material';
import { DataGrid, GridColDef, GridActionsCellItem } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon, Person as PersonIcon } from '@mui/icons-material';
import kullaniciService, { Kullanici, KullaniciCreateDTO, KullaniciUpdateDTO, Rol } from '../../services/kullaniciService';

function KullaniciList() {
  const [kullanicilar, setKullanicilar] = useState<Kullanici[]>([]);
  const [roller, setRoller] = useState<Rol[]>([]);
  const [loading, setLoading] = useState(true);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [currentKullanici, setCurrentKullanici] = useState<Partial<Kullanici & { sifre?: string; yeniSifre?: string }>>({
    email: '',
    ad: '',
    soyad: '',
    rolId: 0,
    personelId: undefined,
    aktif: true,
    sifre: '',
  });

  useEffect(() => {
    loadData();
    loadRoller();
  }, []);

  const loadData = async () => {
    try {
      const data = await kullaniciService.getAll();
      setKullanicilar(data);
    } catch (error) {
      console.error('Kullanıcılar yüklenemedi:', error);
      alert('Kullanıcılar yüklenirken hata oluştu!');
    } finally {
      setLoading(false);
    }
  };

  const loadRoller = async () => {
    try {
      const data = await kullaniciService.getRoller();
      setRoller(data);
    } catch (error) {
      console.error('Roller yüklenemedi:', error);
    }
  };

  const handleOpenDialog = (kullanici?: Kullanici) => {
    if (kullanici) {
      setEditMode(true);
      setCurrentKullanici({
        ...kullanici,
        yeniSifre: '',
      });
    } else {
      setEditMode(false);
      setCurrentKullanici({
        email: '',
        ad: '',
        soyad: '',
        rolId: 0,
        personelId: undefined,
        aktif: true,
        sifre: '',
      });
    }
    setDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setDialogOpen(false);
    setCurrentKullanici({
      email: '',
      ad: '',
      soyad: '',
      rolId: 0,
      personelId: undefined,
      aktif: true,
      sifre: '',
    });
  };

  const handleSave = async () => {
    try {
      // Validasyon
      if (!currentKullanici.email || !currentKullanici.ad || !currentKullanici.soyad || !currentKullanici.rolId) {
        alert('Lütfen tüm zorunlu alanları doldurun!');
        return;
      }

      if (!editMode && !currentKullanici.sifre) {
        alert('Lütfen şifre girin!');
        return;
      }

      if (editMode && currentKullanici.id) {
        const updateData: KullaniciUpdateDTO = {
          id: currentKullanici.id,
          email: currentKullanici.email,
          ad: currentKullanici.ad,
          soyad: currentKullanici.soyad,
          rolId: currentKullanici.rolId,
          personelId: currentKullanici.personelId,
          aktif: currentKullanici.aktif || true,
          yeniSifre: currentKullanici.yeniSifre,
        };
        await kullaniciService.update(currentKullanici.id, updateData);
        alert('Kullanıcı güncellendi!');
      } else {
        const createData: KullaniciCreateDTO = {
          email: currentKullanici.email!,
          sifre: currentKullanici.sifre!,
          ad: currentKullanici.ad!,
          soyad: currentKullanici.soyad!,
          rolId: currentKullanici.rolId!,
          personelId: currentKullanici.personelId,
        };
        await kullaniciService.create(createData);
        alert('Kullanıcı oluşturuldu!');
      }
      handleCloseDialog();
      loadData();
    } catch (error) {
      console.error('Kaydetme hatası:', error);
      alert('Kaydetme sırasında hata oluştu!');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Bu kullanıcıyı silmek istediğinize emin misiniz?')) {
      try {
        await kullaniciService.delete(id);
        alert('Kullanıcı silindi!');
        loadData();
      } catch (error) {
        console.error('Silme hatası:', error);
        alert('Silme sırasında hata oluştu!');
      }
    }
  };

  const columns: GridColDef[] = [
    {
      field: 'email',
      headerName: 'E-posta',
      width: 220,
      renderCell: (params) => (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <PersonIcon sx={{ mr: 1, color: 'primary.main' }} />
          <strong>{params.value}</strong>
        </Box>
      ),
    },
    { field: 'ad', headerName: 'Ad', width: 150 },
    { field: 'soyad', headerName: 'Soyad', width: 150 },
    {
      field: 'rolAdi',
      headerName: 'Rol',
      width: 120,
      renderCell: (params) => <Chip label={params.value} size="small" color="primary" />,
    },
    { field: 'personelAdi', headerName: 'Personel', width: 180 },
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
          Kullanıcı Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={() => handleOpenDialog()}>
          Yeni Kullanıcı
        </Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={kullanicilar}
          columns={columns}
          loading={loading}
          pageSizeOptions={[10, 25, 50]}
          initialState={{
            pagination: { paginationModel: { pageSize: 10 } },
          }}
        />
      </Box>

      {/* Dialog */}
      <Dialog open={dialogOpen} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
        <DialogTitle>{editMode ? 'Kullanıcı Düzenle' : 'Yeni Kullanıcı Ekle'}</DialogTitle>
        <DialogContent>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
            <TextField
              label="E-posta"
              type="email"
              value={currentKullanici.email}
              onChange={(e) => setCurrentKullanici({ ...currentKullanici, email: e.target.value })}
              required
              fullWidth
            />
            <Box sx={{ display: 'flex', gap: 2 }}>
              <TextField
                label="Ad"
                value={currentKullanici.ad}
                onChange={(e) => setCurrentKullanici({ ...currentKullanici, ad: e.target.value })}
                required
                fullWidth
              />
              <TextField
                label="Soyad"
                value={currentKullanici.soyad}
                onChange={(e) => setCurrentKullanici({ ...currentKullanici, soyad: e.target.value })}
                required
                fullWidth
              />
            </Box>
            <FormControl fullWidth required>
              <InputLabel>Rol</InputLabel>
              <Select
                value={currentKullanici.rolId || ''}
                label="Rol"
                onChange={(e) => setCurrentKullanici({ ...currentKullanici, rolId: Number(e.target.value) })}
              >
                {roller.map((rol) => (
                  <MenuItem key={rol.id} value={rol.id}>
                    {rol.rolAdi}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            {!editMode ? (
              <TextField
                label="Şifre"
                type="password"
                value={currentKullanici.sifre}
                onChange={(e) => setCurrentKullanici({ ...currentKullanici, sifre: e.target.value })}
                required
                fullWidth
                helperText="Minimum 6 karakter"
              />
            ) : (
              <TextField
                label="Yeni Şifre (opsiyonel)"
                type="password"
                value={currentKullanici.yeniSifre}
                onChange={(e) => setCurrentKullanici({ ...currentKullanici, yeniSifre: e.target.value })}
                fullWidth
                helperText="Boş bırakırsanız şifre değişmez"
              />
            )}
            {editMode && (
              <FormControlLabel
                control={
                  <Switch
                    checked={currentKullanici.aktif}
                    onChange={(e) => setCurrentKullanici({ ...currentKullanici, aktif: e.target.checked })}
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

export default KullaniciList;
