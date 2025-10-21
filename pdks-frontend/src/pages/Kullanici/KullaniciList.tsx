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
    Delete as DeleteIcon,
    Person as PersonIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';
import { useNavigate } from 'react-router-dom';

interface Kullanici {
    id: number;
    personelAdi: string;
    personelSicilNo: string;
    email: string;
    rolAdi: string;
    aktif: boolean;
    sonGirisTarihi: string | null;
}

function KullaniciList() {
    const navigate = useNavigate();
    const [kullanicilar, setKullanicilar] = useState<Kullanici[]>([]);
    const [loading, setLoading] = useState(false);
    const [deleteDialog, setDeleteDialog] = useState<{ open: boolean; id: number | null }>({
        open: false,
        id: null,
    });
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
        loadKullanicilar();
    }, []);

    const loadKullanicilar = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Kullanici');
            setKullanicilar(response.data);
        } catch (error) {
            console.error('Kullanýcýlar yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Kullanýcýlar yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!deleteDialog.id) return;

        try {
            await api.delete(`/Kullanici/${deleteDialog.id}`);
            setSnackbar({ open: true, message: 'Kullanýcý silindi!', severity: 'success' });
            loadKullanicilar();
        } catch (error) {
            setSnackbar({ open: true, message: 'Kullanýcý silinemedi!', severity: 'error' });
        } finally {
            setDeleteDialog({ open: false, id: null });
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
            headerName: 'Personel Adý',
            width: 200,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <PersonIcon fontSize="small" color="primary" />
                    <Typography fontWeight="500">{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'personelSicilNo',
            headerName: 'Sicil No',
            width: 120,
        },
        {
            field: 'email',
            headerName: 'Email',
            width: 220,
        },
        {
            field: 'rolAdi',
            headerName: 'Rol',
            width: 130,
            renderCell: (params) => (
                <Chip
                    label={params.value}
                    color={params.value === 'Admin' ? 'error' : 'primary'}
                    size="small"
                />
            ),
        },
        {
            field: 'aktif',
            headerName: 'Durum',
            width: 100,
            renderCell: (params) => (
                <Chip
                    label={params.value ? 'Aktif' : 'Pasif'}
                    color={params.value ? 'success' : 'default'}
                    size="small"
                />
            ),
        },
        {
            field: 'sonGirisTarihi',
            headerName: 'Son Giriþ',
            width: 180,
            valueGetter: (_value, row) =>
                row.sonGirisTarihi ? new Date(row.sonGirisTarihi).toLocaleString('tr-TR') : '-',
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
                        onClick={() => navigate(`/kullanici/duzenle/${params.row.id}`)}
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                        size="small"
                        color="error"
                        onClick={() => setDeleteDialog({ open: true, id: params.row.id })}
                    >
                        <DeleteIcon fontSize="small" />
                    </IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h4" fontWeight="bold">
                    Kullanýcýlar
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => navigate('/kullanici/yeni')}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Yeni Kullanýcý
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={kullanicilar}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 10 } },
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

            {/* Delete Dialog */}
            <Dialog open={deleteDialog.open} onClose={() => setDeleteDialog({ open: false, id: null })}>
                <DialogTitle>Kullanýcýyý Sil</DialogTitle>
                <DialogContent>
                    <Typography>Bu kullanýcýyý silmek istediðinize emin misiniz?</Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialog({ open: false, id: null })}>Ýptal</Button>
                    <Button onClick={handleDelete} color="error" variant="contained">
                        Sil
                    </Button>
                </DialogActions>
            </Dialog>

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

export default KullaniciList;