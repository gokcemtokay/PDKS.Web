import { useEffect, useState } from 'react';
import { Box, Typography, Button, IconButton, Chip, Alert, FormControlLabel, Switch } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import cihazService, { Cihaz } from '../../services/cihazService';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField } from '@mui/material';
import { showError } from '../../utils/errorUtils';
import { useAuth } from '../../contexts/AuthContext'; // ← EKLE

function CihazList() {
    const { aktifSirket } = useAuth(); // ← EKLE
    const [cihazlar, setCihazlar] = useState<Cihaz[]>([]);
    const [loading, setLoading] = useState(true);
    const [openDialog, setOpenDialog] = useState(false);
    const [editingCihaz, setEditingCihaz] = useState<any>(null);
    const [error, setError] = useState('');

    const [formData, setFormData] = useState({
        cihazAdi: '',
        cihazTipi: '',
        ipAdresi: '',
        port: '',
        lokasyon: '',
        durum: true // ← Varsayılan true
    });

    useEffect(() => {
        if (aktifSirket) { // ← Şirket varsa yükle
            loadData();
        }
    }, [aktifSirket]); // ← Şirket değişince yeniden yükle

    const loadData = async () => {
        setError('');
        try {
            const data = await cihazService.getAll();
            console.log('Backend response:', data);
            setCihazlar(data);
        } catch (error: any) {
            console.error('Cihazlar yüklenemedi:', error);
            showError(error, setError);
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        { field: 'cihazAdi', headerName: 'Cihaz Adı', width: 200 },
        {
            field: 'cihazTipi',
            headerName: 'Tip',
            width: 150,
            valueGetter: (value) => value || '-'
        },
        {
            field: 'ipAdres',
            headerName: 'IP Adresi',
            width: 150
        },
        {
            field: 'port',
            headerName: 'Port',
            width: 100,
            valueGetter: (value) => value || '-'
        },
        { field: 'lokasyon', headerName: 'Lokasyon', width: 200 },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 120,
            renderCell: (params) => (
                <Chip
                    label={params.row.durumText || (params.value ? 'Aktif' : 'Pasif')}
                    color={params.value ? 'success' : 'error'}
                    size="small"
                />
            ),
        },
        {
            field: 'actions',
            headerName: 'İşlemler',
            width: 120,
            renderCell: (params) => (
                <Box>
                    <IconButton onClick={() => handleOpenDialog(params.row)}>
                        <EditIcon />
                    </IconButton>
                    <IconButton onClick={() => handleDelete(params.row.id)}>
                        <DeleteIcon />
                    </IconButton>
                </Box>
            )
        },
    ];

    const handleOpenDialog = (cihaz?: any) => {
        setError('');
        if (cihaz) {
            console.log('Editing cihaz:', cihaz);
            setEditingCihaz(cihaz);
            setFormData({
                cihazAdi: cihaz.cihazAdi || '',
                cihazTipi: cihaz.cihazTipi || '',
                ipAdresi: cihaz.ipAdres || '',
                port: cihaz.port?.toString() || '',
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
        setError('');
    };

    const handleSave = async () => {
        setError('');
        try {
            if (editingCihaz) {
                const payload = {
                    id: editingCihaz.id,
                    ...formData
                };
                console.log('Update payload:', payload);
                await cihazService.update(editingCihaz.id, payload);
                alert('Cihaz başarıyla güncellendi!');
            } else {
                console.log('Create payload:', formData);
                await cihazService.create(formData);
                alert('Cihaz başarıyla eklendi!');
            }
            handleCloseDialog();
            loadData();
        } catch (error: any) {
            console.error('Hata:', error);
            showError(error, setError);
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm('Silmek istediğinizden emin misiniz?')) return;

        setError('');
        try {
            await cihazService.delete(id);
            alert('Cihaz başarıyla silindi!');
            loadData();
        } catch (error: any) {
            console.error('Hata:', error);
            showError(error, setError);
        }
    };

    return (
        <Box>
            {/* Header */}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Box>
                    <Typography variant="h4" fontWeight="bold">Cihaz Yönetimi</Typography>
                    {aktifSirket && (
                        <Typography variant="body2" color="text.secondary">
                            {aktifSirket.sirketAdi}
                        </Typography>
                    )}
                </Box>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => handleOpenDialog()}
                >
                    Yeni Cihaz Ekle
                </Button>
            </Box>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError('')}>
                    {error}
                </Alert>
            )}

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid
                    rows={cihazlar}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    disableRowSelectionOnClick
                />
            </Box>

            <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
                <DialogTitle>
                    {editingCihaz ? 'Cihaz Düzenle' : 'Yeni Cihaz Ekle'}
                </DialogTitle>
                <DialogContent>
                    {error && (
                        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError('')}>
                            {error}
                        </Alert>
                    )}

                    <TextField
                        label="Cihaz Adı"
                        value={formData.cihazAdi}
                        onChange={(e) => setFormData({ ...formData, cihazAdi: e.target.value })}
                        fullWidth
                        margin="normal"
                        required
                    />
                    <TextField
                        label="Cihaz Tipi"
                        value={formData.cihazTipi}
                        onChange={(e) => setFormData({ ...formData, cihazTipi: e.target.value })}
                        fullWidth
                        margin="normal"
                        placeholder="Kart Okuyucu, Parmak İzi, Yüz Tanıma"
                        helperText="Opsiyonel"
                    />
                    <TextField
                        label="IP Adresi"
                        value={formData.ipAdresi}
                        onChange={(e) => setFormData({ ...formData, ipAdresi: e.target.value })}
                        fullWidth
                        margin="normal"
                        placeholder="192.168.1.100"
                    />
                    <TextField
                        label="Port"
                        value={formData.port}
                        onChange={(e) => setFormData({ ...formData, port: e.target.value })}
                        fullWidth
                        margin="normal"
                        type="number"
                        placeholder="4370"
                        helperText="Opsiyonel"
                    />
                    <TextField
                        label="Lokasyon"
                        value={formData.lokasyon}
                        onChange={(e) => setFormData({ ...formData, lokasyon: e.target.value })}
                        fullWidth
                        margin="normal"
                        placeholder="Giriş Kapısı 1. Kat"
                    />

                    {/* ← DURUM SWITCH EKLEME */}
                    <FormControlLabel
                        control={
                            <Switch
                                checked={formData.durum}
                                onChange={(e) => setFormData({ ...formData, durum: e.target.checked })}
                                color="success"
                            />
                        }
                        label={formData.durum ? 'Aktif' : 'Pasif'}
                        sx={{ mt: 2 }}
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