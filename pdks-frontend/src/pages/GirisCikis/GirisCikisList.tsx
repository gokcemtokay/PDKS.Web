import { useEffect, useState } from 'react';
import {
    Box,
    Button,
    Paper,
    Typography,
    IconButton,
    Chip,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Alert,
    Snackbar,
} from '@mui/material';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Info as InfoIcon,
    AccessTime as AccessTimeIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';
import { useNavigate } from 'react-router-dom';

interface GirisCikis {
    id: number;
    personelAdi: string;
    sicilNo: string;
    girisZamani: string | null;
    cikisZamani: string | null;
    calismaSuresi: string;
    durum: string;
    elleGiris: boolean;
    not: string;
    guncellemeTarihi: string | null;
}

function GirisCikisList() {
    const navigate = useNavigate();
    const [kayitlar, setKayitlar] = useState<GirisCikis[]>([]);
    const [loading, setLoading] = useState(false);
    const [snackbar, setSnackbar] = useState<{
        open: boolean;
        message: string;
        severity: 'success' | 'error';
    }>({
        open: false,
        message: '',
        severity: 'success',
    });

    useEffect(() => {
        loadKayitlar();
    }, []);

    const loadKayitlar = async () => {
        setLoading(true);
        try {
            const response = await api.get('/GirisCikis');
            setKayitlar(response.data);
        } catch (error) {
            console.error('Giriþ/Çýkýþ kayýtlarý yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Kayýtlar yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const formatDate = (dateString: string | null) => {
        return dateString ? new Date(dateString).toLocaleString('tr-TR') : '-';
    };

    const getDurumColor = (durum: string) => {
        switch (durum.toLowerCase()) {
            case 'normal':
                return 'success';
            case 'geç giriþ':
                return 'warning';
            case 'erken çýkýþ':
                return 'error';
            default:
                return 'default';
        }
    };

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'ID',
            width: 70,
        },
        {
            field: 'personelAdi',
            headerName: 'Personel',
            width: 200,
        },
        {
            field: 'sicilNo',
            headerName: 'Sicil No',
            width: 120,
        },
        {
            field: 'girisZamani',
            headerName: 'Giriþ Zamaný',
            width: 180,
            valueGetter: (_value, row) => formatDate(row.girisZamani),
        },
        {
            field: 'cikisZamani',
            headerName: 'Çýkýþ Zamaný',
            width: 180,
            valueGetter: (_value, row) => formatDate(row.cikisZamani),
        },
        {
            field: 'calismaSuresi',
            headerName: 'Çalýþma Süresi',
            width: 150,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <AccessTimeIcon fontSize="small" color="action" />
                    <Typography>{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => (
                <Chip
                    label={params.value}
                    color={getDurumColor(params.value)}
                    size="small"
                />
            ),
        },
        {
            field: 'elleGiris',
            headerName: 'Elle Giriþ',
            width: 100,
            renderCell: (params) => (
                <Chip
                    label={params.value ? 'Evet' : 'Hayýr'}
                    color={params.value ? 'warning' : 'default'}
                    size="small"
                    variant="outlined"
                />
            ),
        },
        {
            field: 'not',
            headerName: 'Not',
            flex: 1,
            minWidth: 200,
        },
        {
            field: 'actions',
            headerName: 'Ýþlemler',
            width: 120,
            sortable: false,
            renderCell: (params) => (
                <Box>
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={() => navigate(`/giriscikis/duzenle/${params.row.id}`)}
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                        size="small"
                        color="info"
                        onClick={() => navigate(`/giriscikis/detay/${params.row.id}`)}
                    >
                        <InfoIcon fontSize="small" />
                    </IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h4" fontWeight="bold">
                    Giriþ/Çýkýþ Kayýtlarý
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => navigate('/giriscikis/yeni')}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Manuel Kayýt Ekle
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={kayitlar}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50, 100]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 25 } },
                        sorting: {
                            sortModel: [{ field: 'girisZamani', sort: 'desc' }],
                        },
                    }}
                    disableRowSelectionOnClick
                    autoHeight
                    sx={{
                        '& .MuiDataGrid-cell:focus': {
                            outline: 'none',
                        },
                    }}
                />
            </Paper>

            {/* Snackbar */}
            <Snackbar
                open={snackbar.open}
                autoHideDuration={4000}
                onClose={() => setSnackbar({ ...snackbar, open: false })}
            >
                <Alert severity={snackbar.severity} onClose={() => setSnackbar({ ...snackbar, open: false })}>
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </Box>
    );
}

export default GirisCikisList;