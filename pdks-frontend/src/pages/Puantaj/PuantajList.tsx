import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon } from '@mui/icons-material';
import puantajService, { GirisCikis } from '../../services/puantajService';

function PuantajList() {
  const [kayitlar, setKayitlar] = useState<GirisCikis[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await puantajService.getAll();
      setKayitlar(data);
    } catch (error) {
      console.error('Puantaj kayıtları yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const columns: GridColDef[] = [
    { field: 'personelAdi', headerName: 'Personel', width: 200 },
    {
      field: 'girisZamani',
      headerName: 'Giriş',
      width: 180,
      valueFormatter: (value) => value ? new Date(value).toLocaleString('tr-TR') : '-',
    },
    {
      field: 'cikisZamani',
      headerName: 'Çıkış',
      width: 180,
      valueFormatter: (value) => value ? new Date(value).toLocaleString('tr-TR') : '-',
    },
    { field: 'durum', headerName: 'Durum', width: 150 },
    {
      field: 'actions',
      headerName: 'İşlemler',
      width: 100,
      renderCell: () => (
        <IconButton size="small">
          <EditIcon fontSize="small" />
        </IconButton>
      ),
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">Puantaj / Giriş-Çıkış</Typography>
        <Button variant="contained" startIcon={<AddIcon />}>Manuel Giriş Ekle</Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid rows={kayitlar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
      </Box>
    </Box>
  );
}

export default PuantajList;
