import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton, Chip } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import cihazService, { Cihaz } from '../../services/cihazService';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField } from '@mui/material';
function CihazList() {
    const [cihazlar, setCihazlar] = useState<Cihaz[]>([]);
    const [loading, setLoading] = useState(true);
    const [openDialog, setOpenDialog] = useState(false); // ✅ Ekle
    const [editingCihaz, setEditingCihaz] = useState<any>(null); // ✅ Ekle

    const [formData, setFormData] = useState({
        cihazAdi: '',
        cihazTipi: '',
        ipAdresi: '',
        port: '',
        lokasyon: '',
        durum: true
    });

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await cihazService.getAll();
            setCihazlar(data);
        } catch (error) {
            console.error('Cihazlar yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'cihazAdi', headerName: 'Cihaz Adı', width: 200 },
        { field: 'cihazTipi', headerName: 'Tip', width: 150 },
        { field: 'ipAdresi', headerName: 'IP Adresi', width: 150 },
        { field: 'port', headerName: 'Port', width: 100 },
        { field: 'lokasyon', headerName: 'Lokasyon', width: 150 },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => (
                <Chip label={params.value ? 'Aktif' : 'Pasif'} color={params.value ? 'success' : 'error'} size="small" />
            ),
        },
        {
            field: 'actions',
            headerName: 'İşlemler',
            width: 120,
            renderCell: (params) => ( // ✅ params ekle
                <Box>
                    <IconButton onClick={() => handleOpenDialog(params.row)}> {/* ✅ params.row */}
                        <EditIcon />
                    </IconButton>
                    <IconButton onClick={() => handleDelete(params.row.id)}> {/* ✅ params.row.id */}
                        <DeleteIcon />
                    </IconButton>
                </Box>
            )
        },
    ];

    const handleOpenDialog = (cihaz?: any) => {
        if (cihaz) {
            setEditingCihaz(cihaz);
            setFormData({
                cihazAdi: cihaz.cihazAdi || '',
                cihazTipi: cihaz.cihazTipi || '',
                ipAdresi: cihaz.ipAdresi || '',
                port: cihaz.port || '',
                lokasyon: cihaz.lokasyon || '',
                durum: cihaz.durum ?? true
            });
        } else {
            setEditingCihaz(null);
            setFormData({
                cihazAdi: '',
                cihazTipi: '',
                ipAdresi: '',
                port: '',
                lokasyon: '',
                durum: true
            });
        }
        setOpenDialog(true);
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setEditingCihaz(null);
    };

    const handleSave = async () => {
        try {
            if (editingCihaz) {
                await cihazService.update(editingCihaz.id, formData);
            } else {
                await cihazService.create(formData);
            }
            handleCloseDialog();
            loadData(); // Listeyi yenile
        } catch (error) {
            console.error('Hata:', error);
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm('Silmek istediğinizden emin misiniz?')) return;

        try {
            await cihazService.delete(id);
            loadData();
        } catch (error) {
            console.error('Hata:', error);
        }
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Cihaz Yönetimi</Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => handleOpenDialog()}
                >
                    Yeni Cihaz Ekle
                </Button>
            </Box>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={cihazlar} columns={columns} loading={loading} pageSizeOptions={[10, 25, 50]} />
            </Box>

            <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
                <DialogTitle>
                    {editingCihaz ? 'Cihaz Düzenle' : 'Yeni Cihaz Ekle'}
                </DialogTitle>
                <DialogContent>
                    <TextField
                        label="Cihaz Adı"
                        value={formData.cihazAdi}
                        onChange={(e) => setFormData({ ...formData, cihazAdi: e.target.value })}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label="Cihaz Tipi"
                        value={formData.cihazTipi}
                        onChange={(e) => setFormData({ ...formData, cihazTipi: e.target.value })}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label="IP Adresi"
                        value={formData.ipAdresi}
                        onChange={(e) => setFormData({ ...formData, ipAdresi: e.target.value })}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label="Port"
                        value={formData.port}
                        onChange={(e) => setFormData({ ...formData, port: e.target.value })}
                        fullWidth
                        margin="normal"
                    />
                    <TextField
                        label="Lokasyon"
                        value={formData.lokasyon}
                        onChange={(e) => setFormData({ ...formData, lokasyon: e.target.value })}
                        fullWidth
                        margin="normal"
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseDialog}>İptal</Button>
                    <Button onClick={handleSave} variant="contained">Kaydet</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default CihazList;