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
    BeachAccess as BeachAccessIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';
import { useNavigate } from 'react-router-dom';

interface Izin {
    id: number;
    personelAdi: string;
    personelSicilNo: string;
    izinTipi: string;
    baslangicTarihi: string;
    bitisTarihi: string;
    gunSayisi: number;
    onayDurumu: string;
    onaylayanKullaniciAdi: string | null;
}

function IzinList() {
    const navigate = useNavigate();
    const [izinler, setIzinler] = useState<Izin[]>([]);
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
        loadIzinler();
    }, []);

    const loadIzinler = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Izin');
            setIzinler(response.data);
        } catch (error) {
            console.error('Ýzinler yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Ýzinler yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const getStatusColor = (status: string) => {
        switch (status) {
            case 'Onaylandý':
                return 'success';
            case 'Reddedildi':
                return 'error';
            case 'Beklemede':
                return 'warning';
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
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <BeachAccessIcon fontSize="small" color="action" />
                    <Typography>{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'personelSicilNo',
            headerName: 'Sicil No',
            width: 120,
        },
        {
            field: 'izinTipi',
            headerName: 'Ýzin Tipi',
            width: 150,
        },
        {
            field: 'baslangicTarihi',
            headerName: 'Baþlangýç',
            width: 130,
            valueGetter: (_value, row) => new Date(row.baslangicTarihi).toLocaleDateString('tr-TR'),
        },
        {
            field: 'bitisTarihi',
            headerName: 'Bitiþ',
            width: 130,
            valueGetter: (_value, row) => new Date(row.bitisTarihi).toLocaleDateString('tr-TR'),
        },
        {
            field: 'gunSayisi',
            headerName: 'Gün Sayýsý',
            width: 100,
            renderCell: (params) => (
                <Chip label={`${params.value} Gün`} size="small" color="primary" variant="outlined" />
            ),
        },
        {
            field: 'onayDurumu',
            headerName: 'Durum',
            width: 130,
            renderCell: (params) => (
                <Chip
                    label={params.value}
                    color={getStatusColor(params.value)}
                    size="small"
                />
            ),
        },
        {
            field: 'onaylayanKullaniciAdi',
            headerName: 'Onaylayan',
            width: 150,
            valueGetter: (_value, row) => row.onaylayanKullaniciAdi || '-',
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
                        color="info"
                        onClick={() => navigate(`/izin/detay/${params.row.id}`)}
                    >
                        <InfoIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={() => navigate(`/izin/duzenle/${params.row.id}`)}
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h4" fontWeight="bold">
                    Ýzin Talepleri
                </Typography>
                <Box display="flex" gap={2}>
                    <Button
                        variant="outlined"
                        onClick={() => navigate('/izin/bekleyen')}
                        sx={{ borderColor: '#667eea', color: '#667eea' }}
                    >
                        Bekleyen Ýzinler
                    </Button>
                    <Button
                        variant="contained"
                        startIcon={<AddIcon />}
                        onClick={() => navigate('/izin/yeni')}
                        sx={{
                            background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                        }}
                    >
                        Yeni Ýzin Talebi
                    </Button>
                </Box>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={izinler}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 25 } },
                        sorting: {
                            sortModel: [{ field: 'baslangicTarihi', sort: 'desc' }],
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

export default IzinList;