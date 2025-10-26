import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import izinService, { Izin } from '../../services/izinService';

function IzinList() {
  const [izinler, setIzinler] = useState<Izin[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await izinService.getAll();
      setIzinler(data);
    } catch (error) {
      console.error('İzinler yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'personelAdi', headerName: 'Personel', width: 200 },
    { field: 'izinTipi', headerName: 'İzin Tipi', width: 150 },
    { field: 'baslangicTarihi', headerName: 'Başlangıç', width: 130, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
    { field: 'bitisTarihi', headerName: 'Bitiş', width: 130, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
    { field: 'gunSayisi', headerName: 'Gün', width: 80 },
    {
      field: 'onayDurumu',
      headerName: 'Durum',
      width: 130,
      renderCell: (params) => {
        const color = params.value === 'Onaylandi' ? 'success' : params.value === 'Reddedildi' ? 'error' : 'warning';
        return <Chip label={params.value} color={color} size="small" />;
      },
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">İzinler</Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={() => navigate('/izin/talep')}>
          İzin Talebi Oluştur
        </Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={izinler}
          columns={columns}
          loading={loading}
          pageSizeOptions={[10, 25, 50]}
          initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
        />
      </Box>
    </Box>
  );
}

export default IzinList;
