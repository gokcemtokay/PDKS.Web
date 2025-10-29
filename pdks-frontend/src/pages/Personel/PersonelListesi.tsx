import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box, Paper, Typography, Button, TextField, InputAdornment,
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
    IconButton, Chip, Avatar, Menu, MenuItem, Dialog, DialogTitle,
    DialogContent, DialogActions, Alert, Pagination, Select, FormControl,
    InputLabel, Grid,
} from '@mui/material';
import {
    Add, Search, FilterList, Edit, Delete, Visibility,
    MoreVert, FileDownload, CloudUpload, Person,
} from '@mui/icons-material';
import personelService from '../../services/personelService';
import { useAuth } from '../../contexts/AuthContext';

function PersonelListesi() {
    const navigate = useNavigate();
    const { aktifSirket } = useAuth();
    const [personeller, setPersoneller] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [selectedPersonel, setSelectedPersonel] = useState<any>(null);
    const [deleteDialog, setDeleteDialog] = useState(false);
    const [filterDepartman, setFilterDepartman] = useState('');
    const [filterDurum, setFilterDurum] = useState('');

    useEffect(() => {
        console.log('📊 PersonelListesi useEffect');
        console.log('🏢 Aktif Şirket:', aktifSirket);

        if (aktifSirket) {
            loadPersoneller();
        }
    }, [page, searchTerm, filterDepartman, filterDurum, aktifSirket]);

    const loadPersoneller = async () => {
        setLoading(true);
        try {
            console.log('🌐 API isteği...');
            const data = await personelService.getAll();
            console.log('✅ Personel sayısı:', data.length);

            let filtered = data;

            if (searchTerm) {
                filtered = filtered.filter((p: any) =>
                    p.adSoyad?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                    p.sicilNo?.toString().includes(searchTerm) ||
                    p.email?.toLowerCase().includes(searchTerm.toLowerCase())
                );
            }

            if (filterDepartman) {
                filtered = filtered.filter((p: any) => p.departmanId === parseInt(filterDepartman));
            }

            if (filterDurum !== '') {
                filtered = filtered.filter((p: any) => p.durum === (filterDurum === 'true'));
            }

            setPersoneller(filtered);
            setTotalPages(Math.ceil(filtered.length / 10));
        } catch (error: any) {
            console.error('❌ Hata:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleMenuClick = (event: React.MouseEvent<HTMLElement>, personel: any) => {
        setAnchorEl(event.currentTarget);
        setSelectedPersonel(personel);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
        setSelectedPersonel(null);
    };

    const handleDelete = async () => {
        if (!selectedPersonel) return;
        try {
            await personelService.delete(selectedPersonel.id);
            loadPersoneller();
            setDeleteDialog(false);
            handleMenuClose();
        } catch (error) {
            console.error('Sil hatası:', error);
        }
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Box>
                    <Typography variant="h4" fontWeight="bold">Personel Yönetimi</Typography>
                    {aktifSirket && (
                        <Typography variant="caption" color="text.secondary">
                            Şirket: {aktifSirket.sirketAdi} (ID: {aktifSirket.sirketId})
                        </Typography>
                    )}
                </Box>
                <Box sx={{ display: 'flex', gap: 2 }}>
                    <Button variant="outlined" startIcon={<FileDownload />}>
                        Dışa Aktar
                    </Button>
                    <Button variant="contained" startIcon={<Add />} onClick={() => navigate('/personel/yeni')}>
                        Yeni Personel
                    </Button>
                </Box>
            </Box>

            <Paper sx={{ p: 2, mb: 3 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={12} md={4}>
                        <TextField
                            fullWidth
                            size="small"
                            placeholder="Ad, sicil no veya email..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <Search />
                                    </InputAdornment>
                                ),
                            }}
                        />
                    </Grid>
                    <Grid item xs={12} md={3}>
                        <FormControl fullWidth size="small">
                            <InputLabel>Departman</InputLabel>
                            <Select value={filterDepartman} label="Departman" onChange={(e) => setFilterDepartman(e.target.value)}>
                                <MenuItem value="">Tümü</MenuItem>
                            </Select>
                        </FormControl>
                    </Grid>
                    <Grid item xs={12} md={3}>
                        <FormControl fullWidth size="small">
                            <InputLabel>Durum</InputLabel>
                            <Select value={filterDurum} label="Durum" onChange={(e) => setFilterDurum(e.target.value)}>
                                <MenuItem value="">Tümü</MenuItem>
                                <MenuItem value="true">Aktif</MenuItem>
                                <MenuItem value="false">Pasif</MenuItem>
                            </Select>
                        </FormControl>
                    </Grid>
                    <Grid item xs={12} md={2}>
                        <Button fullWidth variant="outlined" startIcon={<FilterList />} onClick={() => {
                            setSearchTerm('');
                            setFilterDepartman('');
                            setFilterDurum('');
                        }}>
                            Temizle
                        </Button>
                    </Grid>
                </Grid>
            </Paper>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: 'primary.light' }}>
                            <TableCell width={70}>Fotoğraf</TableCell>
                            <TableCell>Sicil No</TableCell>
                            <TableCell>Ad Soyad</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Telefon</TableCell>
                            <TableCell>Departman</TableCell>
                            <TableCell>Durum</TableCell>
                            <TableCell width={100}>İşlemler</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {loading ? (
                            <TableRow>
                                <TableCell colSpan={8} align="center">Yükleniyor...</TableCell>
                            </TableRow>
                        ) : personeller.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={8} align="center">Personel bulunamadı</TableCell>
                            </TableRow>
                        ) : (
                            personeller.slice((page - 1) * 10, page * 10).map((personel) => (
                                <TableRow key={personel.id} hover>
                                    <TableCell>
                                        <Avatar src={personel.profilResmi} alt={personel.adSoyad} sx={{ width: 40, height: 40 }}>
                                            {personel.adSoyad?.[0] || <Person />}
                                        </Avatar>
                                    </TableCell>
                                    <TableCell>{personel.sicilNo}</TableCell>
                                    <TableCell>{personel.adSoyad}</TableCell>
                                    <TableCell>{personel.email}</TableCell>
                                    <TableCell>{personel.telefon}</TableCell>
                                    <TableCell>{personel.departmanAdi || '-'}</TableCell>
                                    <TableCell>
                                        <Chip label={personel.durum ? 'Aktif' : 'Pasif'} color={personel.durum ? 'success' : 'default'} size="small" />
                                    </TableCell>
                                    <TableCell>
                                        <IconButton size="small" onClick={(e) => handleMenuClick(e, personel)}>
                                            <MoreVert />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            {totalPages > 1 && (
                <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                    <Pagination count={totalPages} page={page} onChange={(_, value) => setPage(value)} color="primary" />
                </Box>
            )}

            <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleMenuClose}>
                <MenuItem onClick={() => { navigate(`/personel/${selectedPersonel?.id}`); handleMenuClose(); }}>
                    <Visibility sx={{ mr: 1 }} fontSize="small" />
                    Görüntüle
                </MenuItem>
                <MenuItem onClick={() => { navigate(`/personel/duzenle/${selectedPersonel?.id}`); handleMenuClose(); }}>
                    <Edit sx={{ mr: 1 }} fontSize="small" />
                    Düzenle
                </MenuItem>
                <MenuItem onClick={() => { setDeleteDialog(true); handleMenuClose(); }}>
                    <Delete sx={{ mr: 1 }} fontSize="small" />
                    Sil
                </MenuItem>
            </Menu>

            <Dialog open={deleteDialog} onClose={() => setDeleteDialog(false)}>
                <DialogTitle>Personel Sil</DialogTitle>
                <DialogContent>
                    <Alert severity="warning">
                        {selectedPersonel?.adSoyad} adlı personeli silmek istediğinize emin misiniz?
                    </Alert>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialog(false)}>İptal</Button>
                    <Button onClick={handleDelete} color="error" variant="contained">Sil</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default PersonelListesi;
