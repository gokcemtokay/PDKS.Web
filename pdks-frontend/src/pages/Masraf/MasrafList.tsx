import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import masrafService, { Masraf } from '../../services/masrafService';

function MasrafList() {
  const [masraflar, setMasraflar] = useState<Masraf[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await masrafService.getAll();
      setMasraflar(data);
    } catch (error) {
      console.error('Masraflar yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'personelAdi', headerName: 'Personel', width: 180 },
    { field: 'masrafTipi', headerName: 'Masraf Tipi', width: 150 },
    { field: 'tutar', headerName: 'Tutar', width: 120, valueFormatter: (value) => `₺${value}` },
    { field: 'tarih', headerName: 'Tarih', width: 130, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
    {
      field: 'onayDurumu',
      headerName: 'Durum',
      width: 130,
      renderCell: (params) => <Chip label={params.value} color={params.value === 'Onaylandi' ? 'success' : 'warning'} size="small" />,
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">Masraflar</Typography>
        <Button variant="contained" startIcon={<AddIcon />}>Masraf Talebi Oluştur</Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid rows={masraflar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
      </Box>
    </Box>
  );
}

export default MasrafList;
