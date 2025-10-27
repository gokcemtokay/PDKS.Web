import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
    Box, Typography, Button, Paper, Checkbox, FormControlLabel,
    Grid, Divider, Alert, CircularProgress, FormGroup, Card, CardContent
} from '@mui/material';
import { Save as SaveIcon, ArrowBack as BackIcon } from '@mui/icons-material';
import rolService from '../../services/rolService';
import menuService, { Menu } from '../../services/menuService';

interface IslemYetki {
    id: number;
    islemKodu: string;
    islemAdi: string;
    modulAdi?: string;
}

function RolYetkiAtama() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [rolAdi, setRolAdi] = useState('');
    
    // Menüler
    const [menuler, setMenuler] = useState<Menu[]>([]);
    const [selectedMenuler, setSelectedMenuler] = useState<number[]>([]);
    
    // İşlem Yetkileri
    const [islemYetkiler, setIslemYetkiler] = useState<IslemYetki[]>([]);
    const [selectedIslemler, setSelectedIslemler] = useState<number[]>([]);
    
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    useEffect(() => {
        loadData();
    }, [id]);

    const loadData = async () => {
        if (!id) return;

        try {
            setLoading(true);
            setError(null);

            // Rol bilgisi
            const rol = await rolService.getById(Number(id));
            setRolAdi(rol.rolAdi);

            // Tüm menüleri çek
            const allMenus = await menuService.getAnaMenuler();
            setMenuler(allMenus);

            // Tüm işlem yetkilerini çek
            const allIslemler = await rolService.getAllIslemYetkileri();
            setIslemYetkiler(allIslemler);

            // Mevcut yetkileri çek
            const mevcutYetkiler = await rolService.getRolYetkileri(Number(id));
            setSelectedMenuler(mevcutYetkiler.menuler.map(m => m.menuId));
            setSelectedIslemler(mevcutYetkiler.islemler.map(i => i.islemYetkiId));

        } catch (err: any) {
            console.error('Veriler yüklenemedi:', err);
            setError('Veriler yüklenemedi: ' + (err.response?.data?.message || err.message));
        } finally {
            setLoading(false);
        }
    };

    const handleMenuToggle = (menuId: number) => {
        setSelectedMenuler(prev => 
            prev.includes(menuId) 
                ? prev.filter(id => id !== menuId)
                : [...prev, menuId]
        );
    };

    const handleMenuWithChildrenToggle = (menu: Menu) => {
        const allIds = [menu.id, ...(menu.altMenuler?.map(a => a.id) || [])];
        const allSelected = allIds.every(id => selectedMenuler.includes(id));

        if (allSelected) {
            // Tümünü kaldır
            setSelectedMenuler(prev => prev.filter(id => !allIds.includes(id)));
        } else {
            // Tümünü ekle
            setSelectedMenuler(prev => {
                const newSet = new Set([...prev, ...allIds]);
                return Array.from(newSet);
            });
        }
    };

    const handleIslemToggle = (islemId: number) => {
        setSelectedIslemler(prev =>
            prev.includes(islemId)
                ? prev.filter(id => id !== islemId)
                : [...prev, islemId]
        );
    };

    const handleModulToggleAll = (modulAdi: string) => {
        const modulIslemler = islemYetkiler
            .filter(i => i.modulAdi === modulAdi)
            .map(i => i.id);
        
        const allSelected = modulIslemler.every(id => selectedIslemler.includes(id));

        if (allSelected) {
            setSelectedIslemler(prev => prev.filter(id => !modulIslemler.includes(id)));
        } else {
            setSelectedIslemler(prev => {
                const newSet = new Set([...prev, ...modulIslemler]);
                return Array.from(newSet);
            });
        }
    };

    const handleSave = async () => {
        if (!id) return;

        try {
            setSaving(true);
            setError(null);

            await rolService.atama({
                rolId: Number(id),
                menuIdler: selectedMenuler,
                islemYetkiIdler: selectedIslemler
            });

            setSuccess(true);
            setTimeout(() => {
                navigate('/rol');
            }, 1500);

        } catch (err: any) {
            console.error('Kaydetme hatası:', err);
            setError('Kaydedilemedi: ' + (err.response?.data?.message || err.message));
        } finally {
            setSaving(false);
        }
    };

    // İşlem yetkilerini modüllere göre grupla
    const groupedIslemler = islemYetkiler.reduce((acc, islem) => {
        const modul = islem.modulAdi || 'Diğer';
        if (!acc[modul]) {
            acc[modul] = [];
        }
        acc[modul].push(islem);
        return acc;
    }, {} as Record<string, IslemYetki[]>);

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Box>
                    <Typography variant="h4" fontWeight="bold">
                        Yetki Ataması: {rolAdi}
                    </Typography>
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                        Bu role ait menü ve işlem yetkilerini belirleyin
                    </Typography>
                </Box>
                <Box>
                    <Button
                        variant="outlined"
                        startIcon={<BackIcon />}
                        onClick={() => navigate('/rol')}
                        sx={{ mr: 2 }}
                    >
                        Geri
                    </Button>
                    <Button
                        variant="contained"
                        startIcon={<SaveIcon />}
                        onClick={handleSave}
                        disabled={saving}
                    >
                        {saving ? 'Kaydediliyor...' : 'Kaydet'}
                    </Button>
                </Box>
            </Box>

            {error && (
                <Alert severity="error" sx={{ mb: 3 }} onClose={() => setError(null)}>
                    {error}
                </Alert>
            )}

            {success && (
                <Alert severity="success" sx={{ mb: 3 }}>
                    Yetkiler başarıyla kaydedildi! Yönlendiriliyorsunuz...
                </Alert>
            )}

            <Grid container spacing={3}>
                {/* Menü Yetkileri */}
                <Grid item xs={12} md={6}>
                    <Card>
                        <CardContent>
                            <Typography variant="h6" gutterBottom>
                                Menü Yetkileri
                            </Typography>
                            <Divider sx={{ mb: 2 }} />
                            
                            <FormGroup>
                                {menuler.map(menu => (
                                    <Box key={menu.id} sx={{ mb: 2 }}>
                                        <FormControlLabel
                                            control={
                                                <Checkbox
                                                    checked={selectedMenuler.includes(menu.id)}
                                                    onChange={() => handleMenuWithChildrenToggle(menu)}
                                                />
                                            }
                                            label={<strong>{menu.menuAdi}</strong>}
                                        />
                                        
                                        {/* Alt Menüler */}
                                        {menu.altMenuler && menu.altMenuler.length > 0 && (
                                            <Box sx={{ ml: 4 }}>
                                                {menu.altMenuler.map(altMenu => (
                                                    <FormControlLabel
                                                        key={altMenu.id}
                                                        control={
                                                            <Checkbox
                                                                checked={selectedMenuler.includes(altMenu.id)}
                                                                onChange={() => handleMenuToggle(altMenu.id)}
                                                            />
                                                        }
                                                        label={altMenu.menuAdi}
                                                    />
                                                ))}
                                            </Box>
                                        )}
                                    </Box>
                                ))}
                            </FormGroup>
                        </CardContent>
                    </Card>
                </Grid>

                {/* İşlem Yetkileri */}
                <Grid item xs={12} md={6}>
                    <Card>
                        <CardContent>
                            <Typography variant="h6" gutterBottom>
                                İşlem Yetkileri
                            </Typography>
                            <Divider sx={{ mb: 2 }} />

                            {Object.entries(groupedIslemler).map(([modulAdi, islemler]) => (
                                <Box key={modulAdi} sx={{ mb: 3 }}>
                                    <Box sx={{ 
                                        display: 'flex', 
                                        alignItems: 'center', 
                                        mb: 1,
                                        p: 1,
                                        bgcolor: 'grey.100',
                                        borderRadius: 1
                                    }}>
                                        <Checkbox
                                            checked={islemler.every(i => selectedIslemler.includes(i.id))}
                                            indeterminate={
                                                islemler.some(i => selectedIslemler.includes(i.id)) &&
                                                !islemler.every(i => selectedIslemler.includes(i.id))
                                            }
                                            onChange={() => handleModulToggleAll(modulAdi)}
                                        />
                                        <Typography variant="subtitle2" fontWeight="bold">
                                            {modulAdi}
                                        </Typography>
                                    </Box>
                                    
                                    <FormGroup sx={{ ml: 2 }}>
                                        {islemler.map(islem => (
                                            <FormControlLabel
                                                key={islem.id}
                                                control={
                                                    <Checkbox
                                                        checked={selectedIslemler.includes(islem.id)}
                                                        onChange={() => handleIslemToggle(islem.id)}
                                                    />
                                                }
                                                label={
                                                    <Box>
                                                        <Typography variant="body2">
                                                            {islem.islemAdi}
                                                        </Typography>
                                                        <Typography variant="caption" color="text.secondary">
                                                            {islem.islemKodu}
                                                        </Typography>
                                                    </Box>
                                                }
                                            />
                                        ))}
                                    </FormGroup>
                                </Box>
                            ))}
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        </Box>
    );
}

export default RolYetkiAtama;
