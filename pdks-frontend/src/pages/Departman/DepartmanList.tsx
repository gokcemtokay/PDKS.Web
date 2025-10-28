import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon } from '@mui/icons-material';
import { useAuth } from '../../contexts/AuthContext'; // ✅ EKLENDI

interface Departman {
    id: number;
    departmanAdi: string;
    kod: string;
    aciklama: string;
    durum: boolean;
}

function DepartmanList() {
    const [departmanlar, setDepartmanlar] = useState<Departman[]>([]);
    const [loading, setLoading] = useState(true);
    const { aktifSirket } = useAuth(); // ✅ EKLENDI

    // ✅ ŞİRKET DEĞİŞİNCE OTOMATİK YENİLENİYOR
    useEffect(() => {
        if (aktifSirket) {
            loadData();
        }
    }, [aktifSirket]); // ✅ DEPENDENCY

    const loadData = async () => {
        try {
            setLoading(true);
            // TODO: API çağrısı
            // const data = await departmanService.getAll();
            // setDepartmanlar(data);
            
            // Mock data
            setTimeout(() => {
                setDepartmanlar([]);
                setLoading(false);
            }, 500);
        } catch (error) {
            console.error('Departmanlar yüklenemedi:', error);
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'departmanAdi', headerName: 'Departman Adı', width: 200 },
        { field: 'kod', headerName: 'Kod', width: 120 },
        { field: 'aciklama', headerName: 'Açıklama', width: 300 },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 100,
            renderCell: (params) => params.value ? 'Aktif' : 'Pasif'
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
                <Box>
                    <Typography variant="h4" fontWeight="bold">Departmanlar</Typography>
                    {aktifSirket && (
                        <Typography variant="body2" color="text.secondary">
                            {aktifSirket.sirketAdi}
                        </Typography>
                    )}
                </Box>
                <Button variant="contained" startIcon={<AddIcon />}>Yeni Departman</Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid
                    rows={departmanlar}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    localeText={{
                        noRowsLabel: 'Henüz departman kaydı yok',
                    }}
                />
            </Box>
        </Box>
    );
}

export default DepartmanList;
