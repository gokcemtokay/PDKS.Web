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
} from '@mui/material';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
    Business as BusinessIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';

interface Departman {
    id: number;
    departmanAdi: string;
    aciklama: string;
    personelSayisi?: number;
}

interface DepartmanFormData {
    id?: number;
    departmanAdi: string;
    aciklama: string;
}

function DepartmanList() {
    const [departmanlar, setDepartmanlar] = useState<Departman[]>([]);
    const [loading, setLoading] = useState(false);
    const [formDialog, setFormDialog] = useState<{ open: boolean; data: DepartmanFormData | null }>({
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
        loadDepartmanlar();
    }, []);

    const loadDepartmanlar = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Departman');
            setDepartmanlar(response.data);
        } catch (error) {
            console.error('Departmanlar yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Departmanlar yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleOpenForm = (departman?: Departman) => {
        if (departman) {
            setFormDialog({
                open: true,
                data: {
                    id: departman.id,
                    departmanAdi: departman.departmanAdi,
                    aciklama: departman.aciklama,
                },
            });
        } else {
            setFormDialog({
                open: true,
                data: {
                    departmanAdi: '',
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

        try {
            if (formDialog.data.id) {
                await api.put(`/Departman/${formDialog.data.id}`, formDialog.data);
                setSnackbar({ open: true, message: 'Departman güncellendi!', severity: 'success' });
            } else {
                await api.post('/Departman', formDialog.data);
                setSnackbar({ open: true, message: 'Departman eklendi!', severity: 'success' });
            }
            handleCloseForm();
            loadDepartmanlar();
        } catch (error) {
            setSnackbar({ open: true, message: 'İşlem başarısız!', severity: 'error' });
        }
    };

    const handleDelete = async () => {
        if (!deleteDialog.id) return;

        try {
            await api.delete(`/Departman/${deleteDialog.id}`);
            setSnackbar({ open: true, message: 'Departman silindi!', severity: 'success' });
            loadDepartmanlar();
        } catch (error) {
            setSnackbar({ open: true, message: 'Departman silinemedi!', severity: 'error' });
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
            field: 'departmanAdi',
            headerName: 'Departman Adı',
            width: 250,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <BusinessIcon color="primary" fontSize="small" />
                    <Typography fontWeight="500">{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'aciklama',
            headerName: 'Açıklama',
            flex: 1,
            minWidth: 300,
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
                    Departmanlar
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => handleOpenForm()}
                    sx={{
                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    }}
                >
                    Yeni Departman
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={departmanlar}
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
                <DialogTitle>{formDialog.data?.id ? 'Departman Düzenle' : 'Yeni Departman'}</DialogTitle>
                <DialogContent>
                    <Grid container spacing={2} sx={{ mt: 1 }}>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                label="Departman Adı"
                                value={formDialog.data?.departmanAdi || ''}
                                onChange={(e) =>
                                    setFormDialog((prev) => ({
                                        ...prev,
                                        data: { ...prev.data!, departmanAdi: e.target.value },
                                    }))
                                }
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
                <DialogTitle>Departmanı Sil</DialogTitle>
                <DialogContent>
                    <Typography>Bu departmanı silmek istediğinize emin misiniz?</Typography>
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

export default DepartmanList;