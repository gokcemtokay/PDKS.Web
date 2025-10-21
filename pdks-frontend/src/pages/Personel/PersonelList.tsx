// src/pages/personel/PersonelList.tsx - DÜZELTİLMİŞ VERSİYON

import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
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
    TextField,
    InputAdornment,
} from '@mui/material';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
    Search as SearchIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';

interface Personel {
    id: number;
    ad: string;
    soyad: string;
    adSoyad?: string;
    tcKimlikNo: string;
    email: string;
    telefon: string;
    departmanAdi: string;
    vardiyaAdi: string;
    gorev: string;
    unvan: string;
    iseBaslamaTarihi?: string;
    cikisTarihi?: string;
    aktif: boolean;
}

function PersonelList() {
    const navigate = useNavigate();
    const [personeller, setPersoneller] = useState<Personel[]>([]);
    const [loading, setLoading] = useState(false);
    const [searchText, setSearchText] = useState('');
    const [deleteDialog, setDeleteDialog] = useState<{ open: boolean; id: number | null }>({
        open: false,
        id: null,
    });
    const [snackbar, setSnackbar] = useState<{ open: boolean; message: string; severity: 'success' | 'error' }>({
        open: false,
        message: '',
        severity: 'success',
    });

    useEffect(() => {
        loadPersoneller();
    }, []);

    const loadPersoneller = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Personel');
            console.log('Full Response:', response);
            console.log('Response Data:', response.data);
            console.log('Data Type:', typeof response.data);
            console.log('Is Array?', Array.isArray(response.data));

            if (response.data && Array.isArray(response.data)) {
                console.log('Personel Count:', response.data.length);
                if (response.data.length > 0) {
                    console.log('İlk Personel:', response.data[0]);
                    console.log('Alanlar:', Object.keys(response.data[0]));
                }
            }

            setPersoneller(response.data || []);
        } catch (error) {
            console.error('Personeller yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Personeller yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!deleteDialog.id) return;

        try {
            await api.delete(`/Personel/${deleteDialog.id}`);
            setSnackbar({ open: true, message: 'Personel başarıyla silindi!', severity: 'success' });
            loadPersoneller();
        } catch (error) {
            setSnackbar({ open: true, message: 'Personel silinemedi!', severity: 'error' });
        } finally {
            setDeleteDialog({ open: false, id: null });
        }
    };

    const filteredPersoneller = personeller.filter((p) =>
        `${p.ad} ${p.soyad} ${p.tcKimlikNo} ${p.email}`.toLowerCase().includes(searchText.toLowerCase())
    );

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'ID',
            width: 70,
        },
        {
            field: 'adSoyad',
            headerName: 'Ad Soyad',
            width: 200,
            valueGetter: (_value: unknown, row: Personel) => `${row.ad} ${row.soyad}`,
        },
        {
            field: 'tcKimlikNo',
            headerName: 'TC Kimlik No',
            width: 130,
        },
        {
            field: 'email',
            headerName: 'E-posta',
            width: 200,
        },
        {
            field: 'telefon',
            headerName: 'Telefon',
            width: 140,
        },
        {
            field: 'departmanAdi',
            headerName: 'Departman',
            width: 150,
            valueGetter: (_value: unknown, row: Personel) => {
                return row.departmanAdi || row.departman || '-';
            },
        },
        {
            field: 'gorev',
            headerName: 'Görev',
            width: 150,
        },
        {
            field: 'unvan',
            headerName: 'Ünvan',
            width: 120,
        },
        {
            field: 'vardiyaAdi',
            headerName: 'Vardiya',
            width: 150,
            valueGetter: (_value: unknown, row: Personel) => {
                return row.vardiyaAdi || '-';
            },
        },
        {
            field: 'iseBaslamaTarihi',
            headerName: 'İşe Başlama',
            width: 130,
            valueGetter: (_value: unknown, row: Personel) =>
                row.iseBaslamaTarihi ? new Date(row.iseBaslamaTarihi).toLocaleDateString('tr-TR') : '-',
        },
        {
            field: 'cikisTarihi',
            headerName: 'İşten Ayrılma',
            width: 130,
            valueGetter: (_value: unknown, row: Personel) =>
                row.cikisTarihi ? new Date(row.cikisTarihi).toLocaleDateString('tr-TR') : '-',
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
            field: 'actions',
            headerName: 'İşlemler',
            width: 120,
            sortable: false,
            renderCell: (params) => (
                <Box>
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={() => navigate(`/personel/duzenle/${params.row.id}`)}
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
                    Personeller
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => navigate('/personel/yeni')}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Yeni Personel
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <TextField
                    fullWidth
                    placeholder="Personel ara (Ad, Soyad, TC, Email)..."
                    value={searchText}
                    onChange={(e) => setSearchText(e.target.value)}
                    sx={{ mb: 3 }}
                    InputProps={{
                        startAdornment: (
                            <InputAdornment position="start">
                                <SearchIcon />
                            </InputAdornment>
                        ),
                    }}
                />

                <DataGrid
                    rows={filteredPersoneller}
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
                <DialogTitle>Personeli Sil</DialogTitle>
                <DialogContent>
                    <Typography>Bu personeli silmek istediğinize emin misiniz?</Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialog({ open: false, id: null })}>İptal</Button>
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
    ); // ✅ Burada kapatılıyor - aşağıdaki gereksiz tag'ler silindi
}

export default PersonelList;