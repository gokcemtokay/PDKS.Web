import { useEffect, useState } from 'react';
import {
    Box, Typography, Button, IconButton, Dialog, DialogTitle,
    DialogContent, DialogActions, TextField, Switch, FormControlLabel,
    Alert, Snackbar
} from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
    Security as SecurityIcon
} from '@mui/icons-material';
import rolService, { Rol } from '../../services/rolService';
import { useNavigate } from 'react-router-dom';

function RolList() {
    const [roller, setRoller] = useState<Rol[]>([]);
    const [loading, setLoading] = useState(true);
    const [openDialog, setOpenDialog] = useState(false);
    const [editingRol, setEditingRol] = useState<Rol | null>(null);
    const [formData, setFormData] = useState({
        rolAdi: '',
        aciklama: '',
        aktif: true
    });
    const [snackbar, setSnackbar] = useState({
        open: false,
        message: '',
        severity: 'success' as 'success' | 'error'
    });
    const navigate = useNavigate();

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            setLoading(true);
            const data = await rolService.getAll();
            setRoller(data);
        } catch (error) {
            console.error('Roller yüklenemedi:', error);
            showSnackbar('Roller yüklenemedi!', 'error');
        } finally {
            setLoading(false);
        }
    };

    const handleOpenDialog = (rol?: Rol) => {
        if (rol) {
            setEditingRol(rol);
            setFormData({
                rolAdi: rol.rolAdi,
                aciklama: rol.aciklama || '',
                aktif: rol.aktif
            });
        } else {
            setEditingRol(null);
            setFormData({
                rolAdi: '',
                aciklama: '',
                aktif: true
            });
        }
        setOpenDialog(true);
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setEditingRol(null);
    };

    const handleSave = async () => {
        try {
            if (editingRol) {
                await rolService.update(editingRol.id, formData);
                showSnackbar('Rol başarıyla güncellendi!', 'success');
            } else {
                await rolService.create(formData);
                showSnackbar('Rol başarıyla oluşturuldu!', 'success');
            }
            handleCloseDialog();
            loadData();
        } catch (error: any) {
            console.error('Rol kaydedilemedi:', error);
            showSnackbar(error.response?.data?.message || 'Rol kaydedilemedi!', 'error');
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm('Bu rolü silmek istediğinizden emin misiniz?')) {
            return;
        }

        try {
            await rolService.delete(id);
            showSnackbar('Rol başarıyla silindi!', 'success');
            loadData();
        } catch (error: any) {
            console.error('Rol silinemedi:', error);
            showSnackbar(error.response?.data?.message || 'Rol silinemedi!', 'error');
        }
    };

    const handleYetkiAta = (rolId: number) => {
        navigate(`/rol/${rolId}/yetki-ata`);
    };

    const showSnackbar = (message: string, severity: 'success' | 'error') => {
        setSnackbar({ open: true, message, severity });
    };

    const columns: GridColDef[] = [
        {
            field: 'rolAdi',
            headerName: 'Rol Adı',
            width: 200,
            flex: 1
        },
        {
            field: 'aciklama',
            headerName: 'Açıklama',
            width: 300,
            flex: 2
        },
        {
            field: 'aktif',
            headerName: 'Aktif',
            width: 100,
            renderCell: (params) => (
                <Switch checked={params.value} disabled size="small" />
            )
        },
        {
            field: 'actions',
            headerName: 'İşlemler',
            width: 200,
            sortable: false,
            renderCell: (params) => (
                <Box>
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={() => handleYetkiAta(params.row.id)}
                        title="Yetki Ata"
                    >
                        <SecurityIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={() => handleOpenDialog(params.row)}
                        title="Düzenle"
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                        size="small"
                        color="error"
                        onClick={() => handleDelete(params.row.id)}
                        title="Sil"
                    >
                        <DeleteIcon fontSize="small" />
                    </IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">
                    Rol Yönetimi
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => handleOpenDialog()}
                >
                    Yeni Rol Ekle
                </Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid
                    rows={roller}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 25 } }
                    }}
                />
            </Box>

            {/* Rol Ekle/Düzenle Dialog */}
            <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
                <DialogTitle>
                    {editingRol ? 'Rol Düzenle' : 'Yeni Rol Ekle'}
                </DialogTitle>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
                        <TextField
                            label="Rol Adı"
                            value={formData.rolAdi}
                            onChange={(e) => setFormData({ ...formData, rolAdi: e.target.value })}
                            required
                            fullWidth
                        />
                        <TextField
                            label="Açıklama"
                            value={formData.aciklama}
                            onChange={(e) => setFormData({ ...formData, aciklama: e.target.value })}
                            multiline
                            rows={3}
                            fullWidth
                        />
                        <FormControlLabel
                            control={
                                <Switch
                                    checked={formData.aktif}
                                    onChange={(e) => setFormData({ ...formData, aktif: e.target.checked })}
                                />
                            }
                            label="Aktif"
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseDialog}>İptal</Button>
                    <Button
                        onClick={handleSave}
                        variant="contained"
                        disabled={!formData.rolAdi}
                    >
                        Kaydet
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Snackbar */}
            <Snackbar
                open={snackbar.open}
                autoHideDuration={6000}
                onClose={() => setSnackbar({ ...snackbar, open: false })}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
            >
                <Alert
                    onClose={() => setSnackbar({ ...snackbar, open: false })}
                    severity={snackbar.severity}
                    sx={{ width: '100%' }}
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </Box>
    );
}

export default RolList;
