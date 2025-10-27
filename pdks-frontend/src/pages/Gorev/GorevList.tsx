import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import gorevService, { Gorev } from '../../services/gorevService';

function GorevList() {
    const [gorevler, setGorevler] = useState<Gorev[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await gorevService.getAll();
            setGorevler(data);
        } catch (error) {
            console.error('Görevler yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'baslik', headerName: 'Görev', width: 250 },
        { field: 'atananPersonelAdi', headerName: 'Atanan', width: 180 },
        {
            field: 'oncelik',
            headerName: 'Öncelik',
            width: 120,
            renderCell: (params) => {
                const color = params.value === 'Yüksek' ? 'error' : params.value === 'Orta' ? 'warning' : 'success';
                return <Chip label={params.value} color={color} size="small" />;
            },
        },
        { field: 'baslangicTarihi', headerName: 'Başlangıç', width: 130, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
        { field: 'bitisTarihi', headerName: 'Bitiş', width: 130, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 130,
            renderCell: (params) => {
                const color = params.value === 'Tamamlandı' ? 'success' : params.value === 'Devam Ediyor' ? 'info' : 'default';
                return <Chip label={params.value} color={color} size="small" />;
            },
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Görev Yönetimi</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Görev</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={gorevler} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default GorevList;