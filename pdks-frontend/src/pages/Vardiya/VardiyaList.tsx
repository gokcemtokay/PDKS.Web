import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon } from '@mui/icons-material';
import vardiyaService, { Vardiya } from '../../services/vardiyaService';
import { useAuth } from '../../contexts/AuthContext';

function VardiyaList() {
    const [vardiyalar, setVardiyalar] = useState<Vardiya[]>([]);
    const [loading, setLoading] = useState(true);
    const { aktifSirket } = useAuth();

    useEffect(() => {
        if (aktifSirket) {
            loadData();
        }
    }, [aktifSirket]);

    const loadData = async () => {
        try {
            setLoading(true);
            const data = await vardiyaService.getAll();
            setVardiyalar(data);
        } catch (error) {
            console.error('Vardiyalar yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'vardiyaAdi', headerName: 'Vardiya Adı', width: 200 },
        { field: 'baslangicSaati', headerName: 'Başlangıç', width: 150 },
        { field: 'bitisSaati', headerName: 'Bitiş', width: 150 },
        { field: 'aciklama', headerName: 'Açıklama', width: 300 },
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
                <Box>
                    <Typography variant="h4" fontWeight="bold">Vardiya Yönetimi</Typography>
                    {aktifSirket && (
                        <Typography variant="body2" color="text.secondary">
                            {aktifSirket.sirketAdi}
                        </Typography>
                    )}
                </Box>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Vardiya Ekle</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={vardiyalar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default VardiyaList;
