import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Delete as DeleteIcon } from '@mui/icons-material';
import tatilService, { Tatil } from '../../services/tatilService';

function TatilList() {
    const [tatiller, setTatiller] = useState<Tatil[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await tatilService.getAll();
            setTatiller(data);
        } catch (error) {
            console.error('Tatiller y�klenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'tatilAdi', headerName: 'Tatil Ad�', width: 250 },
        { field: 'tarih', headerName: 'Tarih', width: 150, valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR') },
        { field: 'tatilTipi', headerName: 'Tip', width: 150 },
        { field: 'aciklama', headerName: 'A��klama', width: 300 },
        {
            field: 'actions',
            headerName: '��lemler',
            width: 100,
            renderCell: () => (
                <IconButton size="small" color="error"><DeleteIcon fontSize="small" /></IconButton>
            ),
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Tatil Y�netimi</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Tatil Ekle</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={tatiller} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default TatilList;