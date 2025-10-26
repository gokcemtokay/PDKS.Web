import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon } from '@mui/icons-material';
import vardiyaService, { Vardiya } from '../../services/vardiyaService';

function VardiyaList() {
    const [vardiyalar, setVardiyalar] = useState<Vardiya[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await vardiyaService.getAll();
            setVardiyalar(data);
        } catch (error) {
            console.error('Vardiyalar y�klenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'vardiyaAdi', headerName: 'Vardiya Ad�', width: 200 },
        { field: 'baslangicSaati', headerName: 'Ba�lang��', width: 150 },
        { field: 'bitisSaati', headerName: 'Biti�', width: 150 },
        { field: 'aciklama', headerName: 'A��klama', width: 300 },
        {
            field: 'actions',
            headerName: '��lemler',
            width: 100,
            renderCell: () => (
                <IconButton size="small"><EditIcon fontSize="small" /></IconButton>
            ),
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Vardiya Y�netimi</Typography>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Vardiya Ekle</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={vardiyalar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default VardiyaList;