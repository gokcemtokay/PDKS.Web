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
    Delete as DeleteIcon,
    BeachAccess as BeachAccessIcon,
    Event as EventIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';

interface Tatil {
    id: number;
    tatilAdi: string;
    tarih: string;
    aciklama: string;
}

interface TatilFormData {
    tatilAdi: string;
    tarih: string;
    aciklama: string;
}

function TatilList() {
    const [tatiller, setTatiller] = useState<Tatil[]>([]);
    const [loading, setLoading] = useState(false);
    const [formDialog, setFormDialog] = useState<{ open: boolean; data: TatilFormData | null }>({
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
        loadTatiller();
    }, []);

    const loadTatiller = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Tatil');
            setTatiller(response.data);
        } catch (error) {
            console.error('Tatiller yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Tatiller yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleOpenForm = () => {
        setFormDialog({
            open: true,
            data: {
                tatilAdi: '',
                tarih: '',
                aciklama: '',
            },
        });
    };

    const handleCloseForm = () => {
        setFormDialog({ open: false, data: null });
    };

    const handleSubmit = async () => {
        if (!formDialog.data) return;

        // Validation
        if (!formDialog.data.tatilAdi || !formDialog.data.tarih) {
            setSnackbar({ open: true, message: 'Tatil adı ve tarih zorunludur!', severity: 'error' });
            return;
        }

        try {
            await api.post('/Tatil', formDialog.data);
            setSnackbar({ open: true, message: 'Tatil eklendi!', severity: 'success' });
            handleCloseForm();
            loadTatiller();
        } catch (error: any) {
            const errorMsg = error.response?.data || 'İşlem başarısız!';
            setSnackbar({ open: true, message: errorMsg, severity: 'error' });
        }
    };

    const handleDelete = async () => {
        if (!deleteDialog.id) return;

        try {
            await api.delete(`/Tatil/${deleteDialog.id}`);
            setSnackbar({ open: true, message: 'Tatil silindi!', severity: 'success' });
            loadTatiller();
        } catch (error) {
            setSnackbar({ open: true, message: 'Tatil silinemedi!', severity: 'error' });
        } finally {
            setDeleteDialog({ open: false, id: null });
        }
    };

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('tr-TR', {
            day: '2-digit',
            month: 'long',
            year: 'numeric',
        });
    };

    const isUpcoming = (dateString: string) => {
        const tatilDate = new Date(dateString);
        const today = new Date();
        return tatilDate >= today;
    };

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'ID',
            width: 80,
        },
        {
            field: 'tatilAdi',
            headerName: 'Tatil Adı',
            width: 250,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <BeachAccessIcon color="primary" fontSize="small" />
                    <Typography fontWeight="500">{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'tarih',
            headerName: 'Tarih',
            width: 200,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <EventIcon fontSize="small" color="action" />
                    <Typography>{formatDate(params.value)}</Typography>
                </Box>
            ),
        },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => (
                <Chip
                    label={isUpcoming(params.row.tarih) ? 'Yaklaşan' : 'Geçmiş'}
                    color={isUpcoming(params.row.tarih) ? 'success' : 'default'}
                    size="small"
                />
            ),
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
            width: 100,
            sortable: false,
            renderCell: (params) => (
                <Box>
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
                    Tatiller
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleOpenForm}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Yeni Tatil
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={tatiller}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 10 } },
                        sorting: {
                            sortModel: [{ field: 'tarih', sort: 'desc' }],
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

            {/* Form Dialog */}
            <Dialog open={formDialog.open} onClose={handleCloseForm} maxWidth="sm" fullWidth>
                <DialogTitle>Yeni Tatil Ekle</DialogTitle>
                <DialogContent>
                    <Grid container spacing={2} sx={{ mt: 1 }}>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                label="Tatil Adı"
                                placeholder="Örn: Ramazan Bayramı, Yılbaşı"
                                value={formDialog.data?.tatilAdi || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, tatilAdi: e.target.value },
                                    }))
                                }
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                type="date"
                                label="Tarih"
                                value={formDialog.data?.tarih || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, tarih: e.target.value },
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
                                placeholder="Tatil hakkında ek bilgiler..."
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
                <DialogTitle>Tatili Sil</DialogTitle>
                <DialogContent>
                    <Typography>Bu tatili silmek istediğinize emin misiniz?</Typography>
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

export default TatilList;