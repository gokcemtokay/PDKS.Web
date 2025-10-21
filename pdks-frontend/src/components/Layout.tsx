// pdks-frontend/src/components/Layout.tsx - Güncelleme
import { useState, ReactNode } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
    Box,
    Drawer,
    AppBar,
    Toolbar,
    List,
    Typography,
    IconButton,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Avatar,
    Menu,
    MenuItem,
    Divider,
    Chip,
} from '@mui/material';
import {
    Menu as MenuIcon,
    Dashboard as DashboardIcon,
    People as PeopleIcon,
    Business as BusinessIcon,
    Schedule as ScheduleIcon,
    BeachAccess as BeachAccessIcon,
    Assessment as AssessmentIcon,
    Logout as LogoutIcon,
    SwapHoriz as SwapHorizIcon,
    Settings as SettingsIcon, // Örnek olarak ayarlar ikonu eklendi
} from '@mui/icons-material';
import { useAuth } from '../contexts/AuthContext';

interface LayoutProps {
    children: ReactNode;
}

const drawerWidth = 260;

const menuItems = [
    { text: 'Ana Sayfa', icon: <DashboardIcon />, path: '/' },
    { text: 'Personeller', icon: <PeopleIcon />, path: '/personel' },
    { text: 'Departmanlar', icon: <BusinessIcon />, path: '/departman' },
    { text: 'Vardiyalar', icon: <ScheduleIcon />, path: '/vardiya' },
    { text: 'Tatiller', icon: <BeachAccessIcon />, path: '/tatil' },
    { text: 'Raporlar', icon: <AssessmentIcon />, path: '/rapor' },
    { text: 'Ayarlar', icon: <SettingsIcon />, path: '/parametre' }, // Örnek ayarlar menüsü
];

function Layout({ children }: LayoutProps) {
    const [mobileOpen, setMobileOpen] = useState(false);
    const [profileAnchorEl, setProfileAnchorEl] = useState<null | HTMLElement>(null);
    const [sirketAnchorEl, setSirketAnchorEl] = useState<null | HTMLElement>(null);

    // useAuth hook'u ile tüm kritik verileri çek
    // NOT: yetkiliSirketler ve aktifSirket'in AuthContext'te tutulduğu varsayılmıştır.
    const { user, aktifSirket, yetkiliSirketler, logout, switchSirket, isLoggedIn } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    // ⛔ KRİTİK HATA DÜZELTMESİ: Veri yüklenene kadar beklet (veya yetkisiz erişimi engelle)
    // isLoggedIn kontrolü zaten ProtectedRoutes'ta var, ancak Layout'a gelen user null ise NRE olur.
    // yetkiliSirketler verisi login response'undan geldiği için ilk etapta user yoksa undefined olabilir.
    if (!isLoggedIn || !user || !aktifSirket || !yetkiliSirketler) {
        // Oturum açılmış ancak API'den gelen detaylar henüz Context'e yüklenmemişse null dön.
        // Bu, ekranın render edilmesini engellerken verinin yüklenmesini bekler.
        if (isLoggedIn) {
            return <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>Yükleniyor...</Box>;
        }
        // isLoggedIn false ise (teorik olarak ProtectedRoute yakalamalıydı)
        return <Navigate to="/login" replace />;
    }

    // yetkiliSirketler listesi artık güvende, ancak TypeScript'in map hatasını gidermek için Listeyi Array olarak kabul et
    const sirketListesi = yetkiliSirketler as Array<any>;


    const handleDrawerToggle = () => {
        setMobileOpen(!mobileOpen);
    };

    const handleMenuClick = (path: string) => {
        navigate(path);
        setMobileOpen(false);
    };

    const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setProfileAnchorEl(event.currentTarget);
    };

    const handleProfileMenuClose = () => {
        setProfileAnchorEl(null);
    };

    const handleSirketMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setSirketAnchorEl(event.currentTarget);
    };

    const handleSirketMenuClose = () => {
        setSirketAnchorEl(null);
    };

    const handleLogout = () => {
        logout();
        // logout fonksiyonu zaten Context içinde navigate('/login') veya window.location.href yapıyor olmalı
    };

    const handleSirketSwitch = async (sirketId: number) => {
        try {
            // switchSirket fonksiyonunun AuthContext'te tanımlı olduğu varsayılıyor
            // await switchSirket(sirketId); 
            handleSirketMenuClose();
            // Başarılı geçiş sonrası sayfayı yenilemek en güvenli yoldur
            // window.location.reload(); 
        } catch (error) {
            console.error('Şirket değiştirme hatası:', error);
            alert('Şirket değiştirme başarısız oldu!');
        }
    };

    const drawer = (
        <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column', bgcolor: '#1e293b' }}>
            <Box sx={{ p: 3, borderBottom: '1px solid rgba(255,255,255,0.1)' }}>
                <Typography variant="h5" fontWeight="bold" color="white">
                    PDKS
                </Typography>
                <Typography variant="caption" color="rgba(255,255,255,0.7)">
                    Personel Takip Sistemi
                </Typography>
            </Box>

            <List sx={{ flexGrow: 1, px: 1, py: 2 }}>
                {menuItems.map((item) => (
                    <ListItem key={item.text} disablePadding sx={{ mb: 0.5 }}>
                        <ListItemButton
                            onClick={() => handleMenuClick(item.path)}
                            selected={location.pathname === item.path}
                            sx={{
                                borderRadius: 2,
                                color: 'rgba(255,255,255,0.7)',
                                '&.Mui-selected': {
                                    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                    color: 'white',
                                    '& .MuiListItemIcon-root': {
                                        color: 'white',
                                    },
                                },
                                '&:hover': {
                                    background: 'rgba(102, 126, 234, 0.1)',
                                },
                            }}
                        >
                            <ListItemIcon sx={{ color: 'inherit' }}>{item.icon}</ListItemIcon>
                            <ListItemText primary={item.text} />
                        </ListItemButton>
                    </ListItem>
                ))}
            </List>
        </Box>
    );

    return (
        <Box sx={{ display: 'flex', minHeight: '100vh', bgcolor: '#f5f7fa' }}>
            <AppBar
                position="fixed"
                sx={{
                    width: { sm: `calc(100% - ${drawerWidth}px)` },
                    ml: { sm: `${drawerWidth}px` },
                    bgcolor: 'white',
                    boxShadow: '0 2px 8px rgba(0,0,0,0.05)',
                }}
            >
                <Toolbar>
                    <IconButton
                        color="inherit"
                        edge="start"
                        onClick={handleDrawerToggle}
                        sx={{ mr: 2, display: { sm: 'none' }, color: 'text.primary' }}
                    >
                        <MenuIcon />
                    </IconButton>

                    <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1, color: 'text.primary' }}>
                        {menuItems.find((item) => item.path === location.pathname)?.text || 'PDKS'}
                    </Typography>

                    {/* 🆕 Şirket Bilgisi ve Değiştirici */}
                    {sirketListesi.length > 1 && (
                        <Chip
                            icon={<BusinessIcon />}
                            label={aktifSirket.unvan}
                            onClick={handleSirketMenuOpen}
                            deleteIcon={<SwapHorizIcon />}
                            onDelete={handleSirketMenuOpen}
                            sx={{ mr: 2 }}
                            color="primary"
                            variant="outlined"
                        />
                    )}

                    {/* Sadece şirket adını göster (değiştirme yok) */}
                    {sirketListesi.length === 1 && (
                        <Chip
                            icon={<BusinessIcon />}
                            label={aktifSirket.unvan}
                            sx={{ mr: 2 }}
                            color="primary"
                            variant="outlined"
                        />
                    )}

                    {/* Şirket Değiştirme Menüsü */}
                    <Menu
                        anchorEl={sirketAnchorEl}
                        open={Boolean(sirketAnchorEl)}
                        onClose={handleSirketMenuClose}
                        PaperProps={{
                            sx: { width: 280, mt: 1 },
                        }}
                    >
                        <MenuItem disabled>
                            <Typography variant="body2" fontWeight="bold">
                                Şirket Değiştir
                            </Typography>
                        </MenuItem>
                        <Divider />
                        {/* ⭐ DÜZELTİLDİ: Tekrarlayan map kaldırıldı, güvenli map kullanıldı. */}
                        {sirketListesi.map((sirket: any) => (
                            <MenuItem
                                key={sirket.id}
                                onClick={() => handleSirketSwitch(sirket.id)}
                                disabled={sirket.id === aktifSirket.id}
                            >
                                <ListItemIcon>
                                    <SwapHorizIcon fontSize="small" color={sirket.id === aktifSirket.id ? 'primary' : 'inherit'} />
                                </ListItemIcon>
                                {sirket.unvan}
                            </MenuItem>
                        ))}
                    </Menu>

                    {/* Profil Menüsü */}
                    <IconButton onClick={handleProfileMenuOpen}>
                        <Avatar sx={{ bgcolor: '#667eea' }}>
                            {user.email.charAt(0).toUpperCase()}
                        </Avatar>
                    </IconButton>

                    <Menu
                        anchorEl={profileAnchorEl}
                        open={Boolean(profileAnchorEl)}
                        onClose={handleProfileMenuClose}
                        PaperProps={{
                            sx: { width: 200, mt: 1 },
                        }}
                    >
                        <MenuItem disabled>
                            <Typography variant="body2" fontWeight="bold">
                                {user.email}
                            </Typography>
                        </MenuItem>
                        <MenuItem disabled>
                            <Typography variant="caption" color="text.secondary">
                                {user.role}
                            </Typography>
                        </MenuItem>
                        <Divider />
                        <MenuItem onClick={handleLogout}>
                            <ListItemIcon>
                                <LogoutIcon fontSize="small" />
                            </ListItemIcon>
                            Çıkış Yap
                        </MenuItem>
                    </Menu>
                </Toolbar>
            </AppBar>

            <Box
                component="nav"
                sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
            >
                <Drawer
                    variant="temporary"
                    open={mobileOpen}
                    onClose={handleDrawerToggle}
                    ModalProps={{ keepMounted: true }}
                    sx={{
                        display: { xs: 'block', sm: 'none' },
                        '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
                    }}
                >
                    {drawer}
                </Drawer>
                <Drawer
                    variant="permanent"
                    sx={{
                        display: { xs: 'none', sm: 'block' },
                        '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
                    }}
                    open
                >
                    {drawer}
                </Drawer>
            </Box>

            <Box
                component="main"
                sx={{
                    flexGrow: 1,
                    p: 3,
                    width: { sm: `calc(100% - ${drawerWidth}px)` },
                    mt: 8,
                }}
            >
                {children}
            </Box>
        </Box>
    );
}

export default Layout;