import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Visibility as VisibilityIcon } from '@mui/icons-material';
import personelService, { Personel } from '../../services/personelService';

function PersonelList() {
  const [personeller, setPersoneller] = useState<Personel[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await personelService.getAll();
      setPersoneller(data);
    } catch (error) {
      console.error('Personeller yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'sicilNo', headerName: 'Sicil No', width: 120 },
    { field: 'adSoyad', headerName: 'Ad Soyad', width: 200 },
    { field: 'email', headerName: 'E-posta', width: 200 },
    { field: 'telefon', headerName: 'Telefon', width: 150 },
    { field: 'departmanAdi', headerName: 'Departman', width: 150 },
    { field: 'durum', headerName: 'Durum', width: 100, renderCell: (params) => params.value ? 'Aktif' : 'Pasif' },
    {
      field: 'actions',
      headerName: 'İşlemler',
      width: 150,
      renderCell: (params) => (
        <Box>
          <IconButton size="small" onClick={() => navigate(`/personel/detay/${params.row.id}`)}>
            <VisibilityIcon fontSize="small" />
          </IconButton>
          <IconButton size="small" onClick={() => navigate(`/personel/duzenle/${params.row.id}`)}>
            <EditIcon fontSize="small" />
          </IconButton>
        </Box>
      ),
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">Personeller</Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={() => navigate('/personel/yeni')}>
          Yeni Personel
        </Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={personeller}
          columns={columns}
          loading={loading}
          pageSizeOptions={[10, 25, 50]}
          initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
          disableRowSelectionOnClick
        />
      </Box>
    </Box>
  );
}

export default PersonelList;
