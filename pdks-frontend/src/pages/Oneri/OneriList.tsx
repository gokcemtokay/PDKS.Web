import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import oneriService, { Oneri } from '../../services/oneriService';

function OneriList() {
    const [oneriler, setOneriler] = useState<Oneri[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await oneriService.getAll();
            setOneriler(data);
        } catch (error) {
            console.error('Öneriler yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'personelAdi', headerName: 'Gönderen', width: 180, valueGetter: (value, row) => row.anonim ? 'Anonim' : value },
        { field: 'baslik', headerName: 'Baþlýk', width: 250 },
        { field: 'kategori', headerName: 'Kategori', width: 150 },
        { field: 'oneriTarihi', headerName: 'Tarih', width: 150, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 150,
            renderCell: (params) => {
                const color = params.value === 'Kabul' ? 'success' : params.value === 'Red' ? 'error' : 'warning';
                return <Chip label={params.value} color={color} size="small" />;
            },
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Öneri & Þikayet</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Öneri/Þikayet</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={oneriler} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default OneriList;