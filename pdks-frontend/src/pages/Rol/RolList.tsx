import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon } from '@mui/icons-material';
import rolService, { Rol } from '../../services/rolService';

function RolList() {
    const [roller, setRoller] = useState<Rol[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await rolService.getAll();
            setRoller(data);
        } catch (error) {
            console.error('Roller yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'rolAdi', headerName: 'Rol Adı', width: 200 },
        { field: 'aciklama', headerName: 'Açıklama', width: 300 },
        {
            field: 'yetkiler',
            headerName: 'Yetki Sayısı',
            width: 150,
            valueGetter: (value) => value?.length || 0,
        },
        {
            field: 'actions',
            headerName: 'İşlemler',
            width: 100,
            renderCell: () => (
                <IconButton size="small"><EditIcon fontSize="small" /></IconButton>
            ),
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Rol & Yetki Yönetimi</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Rol Ekle</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={roller} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default RolList;