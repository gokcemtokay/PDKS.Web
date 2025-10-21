import { useEffect, useState } from 'react';
import {
    Box,
    Button,
    Paper,
    Typography,
    IconButton,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Alert,
    Snackbar,
    TextField,
    Grid,
    Chip,
} from '@mui/material';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
    Router as RouterIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';
import { useAuth } from '../../contexts/AuthContext';

interface Cihaz {
    id: number;
    cihazAdi: string;
    ipAdres: string;
    lokasyon: string;
    durum: boolean;
    durumText: string;
    sonBaglantiZamani: string | null;
    bugunkuOkumaSayisi: number;
}

interface CihazFormData {
    id?: number;
    cihazAdi: string;
    ipAdres: string;
    lokasyon: string;
    durum: boolean;
}

function CihazList() {
    const { currentSirketId } = useAuth();
    const [cihazlar, setCihazlar] = useState<Cihaz[]>([]);
    const [loading, setLoading] = useState(false);
    const [formDialog, setFormDialog] = useState<{ open: boolean; data: CihazFormData | null }>({
        open: false,
        data: null,
    });
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
        loadCihazlar();
    }, []);

    const loadCihazlar = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Cihaz');
            setCihazlar(response.data);
        } catch (error) {
            console.error('Cihazlar yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Cihazlar yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleOpenForm = (cihaz?: Cihaz) => {
        if (cihaz) {
            setFormDialog({
                open: true,
                data: {
                    id: cihaz.id,
                    cihazAdi: cihaz.cihazAdi,
                    ipAdres: cihaz.ipAdres,
                    lokasyon: cihaz.lokasyon,
                    durum: cihaz.durum,
                },
            });
        } else {
            setFormDialog({
                open: true,
                data: {
                    cihazAdi: '',
                    ipAdres: '',
                    lokasyon: '',
                    durum: true,
                },
            });
        }
    };

    const handleCloseForm = () => {
        setFormDialog({ open: false, data: null });
    };

    const handleSubmit = async () => {
        if (!formDialog.data) return;

        if (!formDialog.data.cihazAdi || !formDialog.data.ipAdres) {
            setSnackbar({ open: true, message: 'Cihaz adý ve IP adresi zorunludur!', severity: 'error' });
            return;
        }

        if (!currentSirketId) {
            setSnackbar({ open: true, message: 'Þirket bilgisi bulunamadý!', severity: 'error' });
            return;
        }

        try {
            const payload = {
                ...formDialog.data,
                sirketId: currentSirketId,
            };

            if (formDialog.data.id) {
                await api.put(`/Cihaz/${formDialog.data.id}`, payload);
                setSnackbar({ open: true, message: 'Cihaz güncellendi!', severity: 'success' });
            } else {
                await api.post('/Cihaz', payload);
                setSnackbar({ open: true, message: 'Cihaz eklendi!', severity: 'success' });
            }
            handleCloseForm();
            loadCihazlar();
        } catch (error: any) {
            console.error('Cihaz kayýt hatasý:', error);
            const errorMsg = error.response?.data?.message || 'Ýþlem baþarýsýz!';
            setSnackbar({ open: true, message: errorMsg, severity: 'error' });
        }
    };

    const handleDelete = async () => {
        if (!deleteDialog.id) return;

        try {
            await api.delete(`/Cihaz/${deleteDialog.id}`);
            setSnackbar({ open: true, message: 'Cihaz silindi!', severity: 'success' });
            loadCihazlar();
        } catch (error) {
            setSnackbar({ open: true, message: 'Cihaz silinemedi!', severity: 'error' });
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
            field: 'cihazAdi',
            headerName: 'Cihaz Adý',
            width: 200,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <RouterIcon color="primary" fontSize="small" />
                    <Typography fontWeight="500">{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'ipAdres',
            headerName: 'IP Adresi',
            width: 150,
        },
        {
            field: 'lokasyon',
            headerName: 'Lokasyon',
            width: 200,
        },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => (
                <Chip
                    label={params.row.durumText}
                    color={params.value ? 'success' : 'error'}
                    size="small"
                />
            ),
        },
        {
            field: 'sonBaglantiZamani',
            headerName: 'Son Baðlantý',
            width: 180,
            valueGetter: (_value, row) =>
                row.sonBaglantiZamani ? new Date(row.sonBaglantiZamani).toLocaleString('tr-TR') : 'Hiç yok',
        },
        {
            field: 'bugunkuOkumaSayisi',
            headerName: 'Okuma Sayýsý',
            width: 130,
        },
        {
            field: 'actions',
            headerName: 'Ýþlemler',
            width: 120,
            sortable: false,
            renderCell: (params) => (
                <Box>
                    <IconButton size="small" color="primary" onClick={() => handleOpenForm(params.row)}>
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
                    Cihazlar
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => handleOpenForm()}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Yeni Cihaz
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={cihazlar}
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

            {/* Form Dialog */}
            <Dialog open={formDialog.open} onClose={handleCloseForm} maxWidth="sm" fullWidth>
                <DialogTitle>{formDialog.data?.id ? 'Cihaz Düzenle' : 'Yeni Cihaz'}</DialogTitle>
                <DialogContent>
                    <Grid container spacing={2} sx={{ mt: 1 }}>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                label="Cihaz Adý"
                                value={formDialog.data?.cihazAdi || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, cihazAdi: e.target.value },
                                    }))
                                }
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                label="IP Adresi"
                                placeholder="192.168.1.100"
                                value={formDialog.data?.ipAdres || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, ipAdres: e.target.value },
                                    }))
                                }
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                label="Lokasyon"
                                placeholder="Giriþ Kapýsý"
                                value={formDialog.data?.lokasyon || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, lokasyon: e.target.value },
                                    }))
                                }
                            />
                        </Grid>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseForm}>Ýptal</Button>
                    <Button
                        onClick={handleSubmit}
                        variant="contained"
                        sx={{
                            background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                        }}
                    >
                        Kaydet
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Delete Dialog */}
            <Dialog open={deleteDialog.open} onClose={() => setDeleteDialog({ open: false, id: null })}>
                <DialogTitle>Cihazý Sil</DialogTitle>
                <DialogContent>
                    <Typography>Bu cihazý silmek istediðinize emin misiniz?</Typography>
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

export default CihazList;