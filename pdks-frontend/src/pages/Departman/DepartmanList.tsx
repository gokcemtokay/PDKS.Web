import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box, Paper, Typography, Button, Table, TableBody, TableCell,
    TableContainer, TableHead, TableRow, IconButton, Chip, Menu, MenuItem,
    Dialog, DialogTitle, DialogContent, DialogActions, Alert
} from '@mui/material';
import { Add, Edit, Delete, MoreVert } from '@mui/icons-material';
import { useAuth } from '../../contexts/AuthContext';
import departmanService, { Departman } from '../../services/departmanService';

function DepartmanList() {
    const navigate = useNavigate();
    const { aktifSirket } = useAuth();
    const [departmanlar, setDepartmanlar] = useState<Departman[]>([]);
    const [loading, setLoading] = useState(true);
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [selectedDepartman, setSelectedDepartman] = useState<Departman | null>(null);
    const [deleteDialog, setDeleteDialog] = useState(false);

    useEffect(() => {
        if (aktifSirket) {
            loadDepartmanlar();
        }
    }, [aktifSirket]);

    const loadDepartmanlar = async () => {
        setLoading(true);
        try {
            const data = await departmanService.getAll();
            setDepartmanlar(data);
        } catch (error) {
            console.error('Departmanlar yüklenemedi:', error);
            alert('Departmanlar yüklenirken hata oluştu!');
        } finally {
            setLoading(false);
        }
    };

    const handleMenuClick = (event: React.MouseEvent<HTMLElement>, departman: Departman) => {
        setAnchorEl(event.currentTarget);
        setSelectedDepartman(departman);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
        setSelectedDepartman(null);
    };

    const handleDelete = async () => {
        if (!selectedDepartman) return;
        try {
            await departmanService.delete(selectedDepartman.id);
            alert('Departman silindi!');
            loadDepartmanlar();
            setDeleteDialog(false);
            handleMenuClose();
        } catch (error) {
            console.error('Departman silinemedi:', error);
            alert('Departman silinirken hata oluştu!');
        }
    };

    return (
        <Box>
            {/* Header */}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Box>
                    <Typography variant="h4" fontWeight="bold">Departmanlar</Typography>
                    {aktifSirket && (
                        <Typography variant="body2" color="text.secondary">
                            {aktifSirket.sirketAdi}
                        </Typography>
                    )}
                </Box>
                <Button
                    variant="contained"
                    startIcon={<Add />}
                    onClick={() => navigate('/departman/yeni')}
                >
                    Yeni Departman
                </Button>
            </Box>

            {/* Tablo */}
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: 'primary.light' }}>
                            <TableCell>Departman Adı</TableCell>
                            <TableCell>Kod</TableCell>
                            <TableCell>Üst Departman</TableCell>
                            <TableCell>Personel Sayısı</TableCell>
                            <TableCell>Durum</TableCell>
                            <TableCell width={100}>İşlemler</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {loading ? (
                            <TableRow>
                                <TableCell colSpan={6} align="center">Yükleniyor...</TableCell>
                            </TableRow>
                        ) : departmanlar.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={6} align="center">
                                    Henüz departman kaydı bulunmuyor
                                </TableCell>
                            </TableRow>
                        ) : (
                            departmanlar.map((departman) => (
                                <TableRow key={departman.id} hover>
                                    <TableCell>
                                        <Typography fontWeight="bold">{departman.departmanAdi}</Typography>
                                        {departman.aciklama && (
                                            <Typography variant="caption" color="text.secondary">
                                                {departman.aciklama}
                                            </Typography>
                                        )}
                                    </TableCell>
                                    <TableCell>{departman.kod || '-'}</TableCell>
                                    <TableCell>{departman.ustDepartmanAdi || '-'}</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={departman.personelSayisi}
                                            size="small"
                                            color="primary"
                                            variant="outlined"
                                        />
                                    </TableCell>
                                    <TableCell>
                                        <Chip
                                            label={departman.durum ? 'Aktif' : 'Pasif'}
                                            color={departman.durum ? 'success' : 'default'}
                                            size="small"
                                        />
                                    </TableCell>
                                    <TableCell>
                                        <IconButton
                                            size="small"
                                            onClick={(e) => handleMenuClick(e, departman)}
                                        >
                                            <MoreVert />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            {/* İşlem Menüsü */}
            <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleMenuClose}
            >
                <MenuItem onClick={() => {
                    navigate(`/departman/duzenle/${selectedDepartman?.id}`);
                    handleMenuClose();
                }}>
                    <Edit sx={{ mr: 1 }} fontSize="small" />
                    Düzenle
                </MenuItem>
                <MenuItem onClick={() => {
                    setDeleteDialog(true);
                    handleMenuClose();
                }}>
                    <Delete sx={{ mr: 1 }} fontSize="small" />
                    Sil
                </MenuItem>
            </Menu>

            {/* Silme Onay Dialogu */}
            <Dialog open={deleteDialog} onClose={() => setDeleteDialog(false)}>
                <DialogTitle>Departman Sil</DialogTitle>
                <DialogContent>
                    <Alert severity="warning">
                        {selectedDepartman?.departmanAdi} departmanını silmek istediğinize emin misiniz?
                    </Alert>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialog(false)}>İptal</Button>
                    <Button onClick={handleDelete} color="error" variant="contained">
                        Sil
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default DepartmanList;
