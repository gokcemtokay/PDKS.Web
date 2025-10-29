import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import avansService, { Avans } from '../../services/avansService';
import { useAuth } from '../../contexts/AuthContext';

function AvansList() {
  const [avanslar, setAvanslar] = useState<Avans[]>([]);
  const [loading, setLoading] = useState(true);
  const { aktifSirket } = useAuth();

  useEffect(() => {
    if (aktifSirket) {
      loadData();
    }
  }, [aktifSirket]);

  const loadData = async () => {
    try {
      setLoading(true);
      const data = await avansService.getAll();
      setAvanslar(data);
    } catch (error) {
      console.error('Avanslar yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'personelAdi', headerName: 'Personel', width: 200 },
    { field: 'tutar', headerName: 'Tutar', width: 150, valueFormatter: (value) => `₺${value}` },
    { field: 'talepTarihi', headerName: 'Talep Tarihi', width: 150, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
    {
      field: 'durum',
      headerName: 'Durum',
      width: 130,
      renderCell: (params) => <Chip label={params.value} color={params.value === 'Onaylandi' ? 'success' : 'warning'} size="small" />,
    },
    { field: 'aciklama', headerName: 'Açıklama', width: 250 },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Box>
          <Typography variant="h4" fontWeight="bold">Avanslar</Typography>
          {aktifSirket && (
            <Typography variant="body2" color="text.secondary">
              {aktifSirket.sirketAdi}
            </Typography>
          )}
        </Box>
        <Button variant="contained" startIcon={<AddIcon />}>Avans Talebi Oluştur</Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid rows={avanslar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
      </Box>
    </Box>
  );
}

export default AvansList;
