import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import seyahatService, { SeyahatTalebi } from '../../services/seyahatService';

function SeyahatList() {
  const [talepler, setTalepler] = useState<SeyahatTalebi[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await seyahatService.getAll();
      setTalepler(data);
    } catch (error) {
      console.error('Seyahat talepleri yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'personelAdi', headerName: 'Personel', width: 180 },
    { field: 'seyahatTipi', headerName: 'Tip', width: 120 },
    { field: 'gidisSehri', headerName: 'Gidiş', width: 130 },
    { field: 'varisSehri', headerName: 'Varış', width: 130 },
    { field: 'kalkisTarihi', headerName: 'Kalkış', width: 130, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
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
        <Typography variant="h4" fontWeight="bold">Seyahat Talepleri</Typography>
        <Button variant="contained" startIcon={<AddIcon />}>Seyahat Talebi Oluştur</Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid rows={talepler} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
      </Box>
    </Box>
  );
}

export default SeyahatList;
