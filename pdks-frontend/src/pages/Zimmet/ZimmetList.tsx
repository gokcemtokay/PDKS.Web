import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';

function ZimmetList() {
  const [zimmetler, setZimmetler] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns: GridColDef[] = [
    { field: 'personelAdSoyad', headerName: 'Personel', width: 200 },
    { field: 'esyaTipi', headerName: 'Eşya Tipi', width: 150 },
    { field: 'esyaAdi', headerName: 'Eşya Adı', width: 200 },
    { field: 'zimmetTarihi', headerName: 'Zimmet Tarihi', width: 150, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
    {
      field: 'zimmetDurumu',
      headerName: 'Durum',
      width: 130,
      renderCell: (params) => <Chip label={params.value} color={params.value === 'Kullanımda' ? 'success' : 'default'} size="small" />,
    },
  ];

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4" fontWeight="bold">Zimmetler</Typography>
        <Button variant="contained" startIcon={<AddIcon />}>Zimmet Ata</Button>
      </Box>

      <Box sx={{ height: 600, width: '100%' }}>
        <DataGrid rows={zimmetler} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
      </Box>
    </Box>
  );
}

export default ZimmetList;
