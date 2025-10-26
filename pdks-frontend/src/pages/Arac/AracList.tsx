import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import aracService, { AracTalebi } from '../../services/aracService';

function AracList() {
  const [talepler, setTalepler] = useState<AracTalebi[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await aracService.getAll();
      setTalepler(data);
    } catch (error) {
      console.error('Araç talepleri yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'personelAdi', headerName: 'Personel', width: 180 },
    { field: 'gidisSehri', headerName: 'Gidiş', width: 130 },
    { field: 'kalkisTarihi', headerName: 'Kalkış', width: 150, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
    { field: 'donusTarihi', headerName: 'Dönüş', width: 150, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
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
        <Typography variant="h4" fontWeight="bold">Araç Talepleri</Typography>
        <Button variant="contained" startIcon={<AddIcon />}>Araç Talebi Oluştur</Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid rows={talepler} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
      </Box>
    </Box>
  );
}

export default AracList;
