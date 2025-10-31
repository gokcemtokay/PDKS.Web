import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton, Alert } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Visibility as VisibilityIcon } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import puantajService from '../../services/puantajService';
import { PuantajList as PuantajListType } from '../../types/puantaj.types';
import { formatDakika } from '../../utils/puantajUtils';

function PuantajList() {
  const navigate = useNavigate();
  const [kayitlar, setKayitlar] = useState<PuantajListType[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Mevcut yıl ve ay
  const currentDate = new Date();
  const currentYear = currentDate.getFullYear();
  const currentMonth = currentDate.getMonth() + 1;

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setError(null);
      setLoading(true);
      // Mevcut dönem için puantaj kayıtlarını getir
      const data = await puantajService.getByDonem(currentYear, currentMonth);
      setKayitlar(data);
    } catch (error: any) {
      console.error('Puantaj kayıtları yüklenemedi:', error);
      setError('Puantaj kayıtları yüklenemedi. Lütfen tekrar deneyin.');
    } finally {
      setLoading(false);
    }
  };

  const handleDetayGor = (id: number) => {
    navigate(`/puantaj/${id}`);
  };

  const columns: GridColDef[] = [
    { 
      field: 'sicilNo', 
      headerName: 'Sicil No', 
      width: 120,
      sortable: true,
    },
    { 
      field: 'personelAdi', 
      headerName: 'Personel', 
      width: 200,
      sortable: true,
    },
    { 
      field: 'departman', 
      headerName: 'Departman', 
      width: 150,
      sortable: true,
    },
    {
      field: 'toplamCalismaSaati',
      headerName: 'Çalışma Saati',
      width: 150,
      valueFormatter: (value) => formatDakika(value),
    },
    {
      field: 'toplamCalisilanGun',
      headerName: 'Çalışılan Gün',
      width: 130,
      valueFormatter: (value) => `${value} gün`,
    },
    {
      field: 'fazlaMesaiSaati',
      headerName: 'Fazla Mesai',
      width: 130,
      valueFormatter: (value) => value > 0 ? formatDakika(value) : '-',
    },
    {
      field: 'devamsizlikGunu',
      headerName: 'Devamsızlık',
      width: 120,
      valueFormatter: (value) => value > 0 ? `${value} gün` : '-',
    },
    { 
      field: 'durum', 
      headerName: 'Durum', 
      width: 120,
      sortable: true,
    },
    {
      field: 'actions',
      headerName: 'İşlemler',
      width: 100,
      sortable: false,
      renderCell: (params) => (
        <IconButton 
          size="small" 
          onClick={() => handleDetayGor(params.row.id)}
          color="primary"
        >
          <VisibilityIcon fontSize="small" />
        </IconButton>
      ),
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Box>
          <Typography variant="h4" fontWeight="bold">Puantaj Listesi</Typography>
          <Typography variant="body2" color="text.secondary">
            {currentMonth}/{currentYear} dönemi
          </Typography>
        </Box>
        <Button 
          variant="contained" 
          startIcon={<AddIcon />}
          onClick={() => navigate('/puantaj')}
        >
          Puantaj Sayfasına Git
        </Button>
      </Box>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid 
          rows={kayitlar} 
          columns={columns} 
          loading={loading}
          pageSizeOptions={[10, 25, 50]}
          initialState={{
            pagination: {
              paginationModel: { pageSize: 25 },
            },
          }}
          disableRowSelectionOnClick
        />
      </Box>
    </Box>
  );
}

export default PuantajList;
