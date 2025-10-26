import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import cihazService, { Cihaz } from '../../services/cihazService';

function CihazList() {
    const [cihazlar, setCihazlar] = useState<Cihaz[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await cihazService.getAll();
            setCihazlar(data);
        } catch (error) {
            console.error('Cihazlar yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'cihazAdi', headerName: 'Cihaz Adý', width: 200 },
        { field: 'cihazTipi', headerName: 'Tip', width: 150 },
        { field: 'ipAdresi', headerName: 'IP Adresi', width: 150 },
        { field: 'port', headerName: 'Port', width: 100 },
        { field: 'lokasyon', headerName: 'Lokasyon', width: 150 },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => (
                <Chip label={params.value ? 'Aktif' : 'Pasif'} color={params.value ? 'success' : 'error'} size="small" />
            ),
        },
        {
            field: 'actions',
            headerName: 'Ýþlemler',
            width: 120,
            renderCell: () => (
                <Box>
                    <IconButton size="small"><EditIcon fontSize="small" /></IconButton>
                    <IconButton size="small" color="error"><DeleteIcon fontSize="small" /></IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Cihaz Yönetimi</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Cihaz Ekle</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={cihazlar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default CihazList;