import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon } from '@mui/icons-material';
import toplantiService, { ToplantiRezervasyonu } from '../../services/toplantiService';

function ToplantiList() {
    const [rezervasyonlar, setRezervasyonlar] = useState<ToplantiRezervasyonu[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await toplantiService.getAllRezervasyonlar();
            setRezervasyonlar(data);
        } catch (error) {
            console.error('Rezervasyonlar yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'odaAdi', headerName: 'Oda', width: 150 },
        { field: 'personelAdi', headerName: 'Rezerve Eden', width: 180 },
        { field: 'konu', headerName: 'Konu', width: 250 },
        { field: 'baslangic', headerName: 'Baþlangýç', width: 180, valueFormatter: (value) => new Date(value).toLocaleString('tr-TR') },
        { field: 'bitis', headerName: 'Bitiþ', width: 180, valueFormatter: (value) => new Date(value).toLocaleString('tr-TR') },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => <Chip label={params.value} color="success" size="small" />,
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Toplantý Odasý Rezervasyonlarý</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Rezervasyon</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={rezervasyonlar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default ToplantiList;