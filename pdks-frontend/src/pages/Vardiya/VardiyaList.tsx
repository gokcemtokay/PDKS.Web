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
    Schedule as ScheduleIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';
import { useAuth } from '../../contexts/AuthContext'; // ✅ Import ekleyin

interface Vardiya {
    id: number;
    vardiyaAdi: string;
    baslangicSaati: string;
    bitisSaati: string;
    aciklama: string;
}

interface VardiyaFormData {
    id?: number;
    vardiyaAdi: string;
    baslangicSaati: string;
    bitisSaati: string;
    aciklama: string;
}

function VardiyaList() {
    const { currentSirketId } = useAuth(); // ✅ Ekleyin
    const [vardiyalar, setVardiyalar] = useState<Vardiya[]>([]);
    const [loading, setLoading] = useState(false);
    const [formDialog, setFormDialog] = useState<{ open: boolean; data: VardiyaFormData | null }>({
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
        loadVardiyalar();
    }, []);

    const loadVardiyalar = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Vardiya');
            setVardiyalar(response.data);
        } catch (error) {
            console.error('Vardiyalar yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Vardiyalar yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleOpenForm = (vardiya?: Vardiya) => {
        if (vardiya) {
            setFormDialog({
                open: true,
                data: {
                    id: vardiya.id,
                    vardiyaAdi: vardiya.vardiyaAdi,
                    baslangicSaati: vardiya.baslangicSaati,
                    bitisSaati: vardiya.bitisSaati,
                    aciklama: vardiya.aciklama,
                },
            });
        } else {
            setFormDialog({
                open: true,
                data: {
                    vardiyaAdi: '',
                    baslangicSaati: '',
                    bitisSaati: '',
                    aciklama: '',
                },
            });
        }
    };

    const handleCloseForm = () => {
        setFormDialog({ open: false, data: null });
    };

    const handleSubmit = async () => {
        if (!formDialog.data) return;

        if (!formDialog.data.vardiyaAdi || !formDialog.data.baslangicSaati || !formDialog.data.bitisSaati) {
            setSnackbar({ open: true, message: 'Tüm zorunlu alanları doldurun!', severity: 'error' });
            return;
        }

        if (!currentSirketId) {
            setSnackbar({ open: true, message: 'Şirket bilgisi bulunamadı!', severity: 'error' });
            return;
        }

        try {
            const payload = {
                ...formDialog.data,
                sirketId: currentSirketId, // ✅ SirketId ekle
            };

            if (formDialog.data.id) {
                await api.put(`/Vardiya/${formDialog.data.id}`, payload);
                setSnackbar({ open: true, message: 'Vardiya güncellendi!', severity: 'success' });
            } else {
                await api.post('/Vardiya', payload);
                setSnackbar({ open: true, message: 'Vardiya eklendi!', severity: 'success' });
            }
            handleCloseForm();
            loadVardiyalar();
        } catch (error: any) {
            console.error('Vardiya kayıt hatası:', error);
            const errorMsg = error.response?.data?.message
                || JSON.stringify(error.response?.data?.errors)
                || 'İşlem başarısız!';
            setSnackbar({ open: true, message: errorMsg, severity: 'error' });
        }
    };

    const handleDelete = async () => {
        if (!deleteDialog.id) return;

        try {
            await api.delete(`/Vardiya/${deleteDialog.id}`);
            setSnackbar({ open: true, message: 'Vardiya silindi!', severity: 'success' });
            loadVardiyalar();
        } catch (error) {
            setSnackbar({ open: true, message: 'İşlem başarısız!', severity: 'error' });
        } finally {
            setDeleteDialog({ open: false, id: null });
        }
    };

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'ID',
            width: 80,
        },
        {
            field: 'vardiyaAdi',
            headerName: 'Vardiya Adı',
            width: 200,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <ScheduleIcon color="primary" fontSize="small" />
                    <Typography fontWeight="500">{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'baslangicSaati',
            headerName: 'Başlangıç',
            width: 150,
            renderCell: (params) => <Chip label={params.value} color="success" size="small" />,
        },
        {
            field: 'bitisSaati',
            headerName: 'Bitiş',
            width: 150,
            renderCell: (params) => <Chip label={params.value} color="error" size="small" />,
        },
        {
            field: 'aciklama',
            headerName: 'Açıklama',
            flex: 1,
            minWidth: 250,
        },
        {
            field: 'actions',
            headerName: 'İşlemler',
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
                    Vardiyalar
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => handleOpenForm()}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Yeni Vardiya
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={vardiyalar}
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
                <DialogTitle>{formDialog.data?.id ? 'Vardiya Düzenle' : 'Yeni Vardiya'}</DialogTitle>
                <DialogContent>
                    <Grid container spacing={2} sx={{ mt: 1 }}>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                label="Vardiya Adı"
                                value={formDialog.data?.vardiyaAdi || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, vardiyaAdi: e.target.value },
                                    }))
                                }
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                type="time"
                                label="Başlangıç Saati"
                                value={formDialog.data?.baslangicSaati || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, baslangicSaati: e.target.value },
                                    }))
                                }
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                required
                                type="time"
                                label="Bitiş Saati"
                                value={formDialog.data?.bitisSaati || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, bitisSaati: e.target.value },
                                    }))
                                }
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                multiline
                                rows={3}
                                label="Açıklama"
                                value={formDialog.data?.aciklama || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, aciklama: e.target.value },
                                    }))
                                }
                            />
                        </Grid>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseForm}>İptal</Button>
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
                <DialogTitle>Vardiyayı Sil</DialogTitle>
                <DialogContent>
                    <Typography>Bu vardiyayı silmek istediğinize emin misiniz?</Typography>
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
    );
}

export default VardiyaList;