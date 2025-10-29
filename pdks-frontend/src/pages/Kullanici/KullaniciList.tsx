import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box, Paper, Typography, Button, Table, TableBody, TableCell,
    TableContainer, TableHead, TableRow, IconButton, Chip, Menu, MenuItem,
    Dialog, DialogTitle, DialogContent, DialogActions, Alert,
} from '@mui/material';
import {
    Add, Edit, Delete, MoreVert, Business,
} from '@mui/icons-material';
import api from '../../services/api';

interface KullaniciSirket {
    sirketId: number;
    sirketAdi: string;
    varsayilan: boolean;
    aktif: boolean;
}

interface Kullanici {
    id: number;
    kullaniciAdi: string;
    ad: string;
    soyad: string;
    email: string;
    rolAdi: string;
    aktif: boolean;
    sonGirisTarihi?: string;
    yetkiliSirketler: KullaniciSirket[];
}

function KullaniciList() {
    const navigate = useNavigate();
    const [kullanicilar, setKullanicilar] = useState<Kullanici[]>([]);
    const [loading, setLoading] = useState(true);
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [selectedKullanici, setSelectedKullanici] = useState<Kullanici | null>(null);
    const [deleteDialog, setDeleteDialog] = useState(false);

    useEffect(() => {
        loadKullanicilar();
    }, []);

    const loadKullanicilar = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Kullanici');
            setKullanicilar(response.data);
        } catch (error) {
            console.error('Kullanıcılar yüklenemedi:', error);
            alert('Kullanıcılar yüklenirken hata oluştu!');
        } finally {
            setLoading(false);
        }
    };

    const handleMenuClick = (event: React.MouseEvent<HTMLElement>, kullanici: Kullanici) => {
        setAnchorEl(event.currentTarget);
        setSelectedKullanici(kullanici);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
        setSelectedKullanici(null);
    };

    const handleDelete = async () => {
        if (!selectedKullanici) return;
        try {
            await api.delete(`/Kullanici/${selectedKullanici.id}`);
            alert('Kullanıcı silindi!');
            loadKullanicilar();
            setDeleteDialog(false);
            handleMenuClose();
        } catch (error) {
            console.error('Kullanıcı silinemedi:', error);
            alert('Kullanıcı silinirken hata oluştu!');
        }
    };

    return (
        <Box>
            {/* Header */}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">Kullanıcı Yönetimi</Typography>
                <Button
                    variant="contained"
                    startIcon={<Add />}
                    onClick={() => navigate('/kullanici/yeni')}
                >
                    Yeni Kullanıcı
                </Button>
            </Box>

            {/* Tablo */}
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: 'primary.light' }}>
                            <TableCell>Kullanıcı Adı</TableCell>
                            <TableCell>Ad Soyad</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Rol</TableCell>
                            <TableCell>Yetkili Şirketler</TableCell>
                            <TableCell>Durum</TableCell>
                            <TableCell width={100}>İşlemler</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {loading ? (
                            <TableRow>
                                <TableCell colSpan={7} align="center">Yükleniyor...</TableCell>
                            </TableRow>
                        ) : kullanicilar.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={7} align="center">Kullanıcı bulunamadı</TableCell>
                            </TableRow>
                        ) : (
                            kullanicilar.map((kullanici) => (
                                <TableRow key={kullanici.id} hover>
                                    <TableCell>{kullanici.kullaniciAdi}</TableCell>
                                    <TableCell>{kullanici.ad} {kullanici.soyad}</TableCell>
                                    <TableCell>{kullanici.email}</TableCell>
                                    <TableCell>{kullanici.rolAdi}</TableCell>
                                    <TableCell>
                                        <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                                            {kullanici.yetkiliSirketler.map((sirket) => (
                                                <Chip
                                                    key={sirket.sirketId}
                                                    icon={<Business />}
                                                    label={sirket.sirketAdi}
                                                    size="small"
                                                    color={sirket.varsayilan ? 'primary' : 'default'}
                                                    variant={sirket.varsayilan ? 'filled' : 'outlined'}
                                                />
                                            ))}
                                        </Box>
                                    </TableCell>
                                    <TableCell>
                                        <Chip
                                            label={kullanici.aktif ? 'Aktif' : 'Pasif'}
                                            color={kullanici.aktif ? 'success' : 'default'}
                                            size="small"
                                        />
                                    </TableCell>
                                    <TableCell>
                                        <IconButton
                                            size="small"
                                            onClick={(e) => handleMenuClick(e, kullanici)}
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
                    navigate(`/kullanici/duzenle/${selectedKullanici?.id}`);
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
                <DialogTitle>Kullanıcı Sil</DialogTitle>
                <DialogContent>
                    <Alert severity="warning">
                        {selectedKullanici?.kullaniciAdi} kullanıcısını silmek istediğinize emin misiniz?
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

export default KullaniciList;
