// pdks-frontend/src/components/Layout.tsx
import { useState, ReactNode } from 'react';
import { useNavigate, useLocation, Navigate } from 'react-router-dom';
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
    Settings as SettingsIcon,
    SwapHoriz as SwapHorizIcon,
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
    { text: 'Ayarlar', icon: <SettingsIcon />, path: '/parametre' },
];

function Layout({ children }: LayoutProps) {
    const [mobileOpen, setMobileOpen] = useState(false);
    const [profileAnchorEl, setProfileAnchorEl] = useState<null | HTMLElement>(null);
    const [sirketAnchorEl, setSirketAnchorEl] = useState<null | HTMLElement>(null);

    // AuthContext'ten tüm gerekli değerleri çek
    const { user, logout, isLoggedIn, yetkiliSirketler, aktifSirket, switchSirket } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    // Kullanıcı kontrolü
    if (!isLoggedIn || !user) {
        if (isLoggedIn) {
            return (
                <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    Yükleniyor...
                </Box>
            );
        }
        return <Navigate to="/login" replace />;
    }

    const handleSirketMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setSirketAnchorEl(event.currentTarget);
    };

    const handleSirketMenuClose = () => {
        setSirketAnchorEl(null);
    };

    const handleSirketSwitch = async (sirketId: number) => {
        try {
            await switchSirket(sirketId);
            handleSirketMenuClose();
        } catch (error) {
            console.error('Şirket değiştirme hatası:', error);
            alert('Şirket değiştirilemedi!');
        }
    };

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

    const handleLogout = () => {
        logout();
        navigate('/login');
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

                    {/* Şirket Seçici - Birden fazla şirket varsa */}
                    {yetkiliSirketler.length > 1 && aktifSirket && (
                        <Chip
                            icon={<BusinessIcon />}
                            label={aktifSirket.unvan}
                            onClick={handleSirketMenuOpen}
                            deleteIcon={<SwapHorizIcon />}
                            onDelete={handleSirketMenuOpen}
                            sx={{ mr: 2, cursor: 'pointer' }}
                            color="primary"
                            variant="outlined"
                        />
                    )}

                    {/* Tek şirket varsa sadece göster */}
                    {yetkiliSirketler.length === 1 && aktifSirket && (
                        <Chip
                            icon={<BusinessIcon />}
                            label={aktifSirket.unvan}
                            sx={{ mr: 2 }}
                            color="primary"
                            variant="outlined"
                        />
                    )}

                    {/* Şirket bilgisi yüklenene kadar fallback */}
                    {yetkiliSirketler.length === 0 && (
                        <Chip
                            icon={<BusinessIcon />}
                            label={user.sirketAdi || 'Şirket'}
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
                        {yetkiliSirketler.map((sirket) => (
                            <MenuItem
                                key={sirket.id}
                                onClick={() => handleSirketSwitch(sirket.id)}
                                disabled={sirket.id === aktifSirket?.id}
                            >
                                <ListItemIcon>
                                    <SwapHorizIcon
                                        fontSize="small"
                                        color={sirket.id === aktifSirket?.id ? 'primary' : 'inherit'}
                                    />
                                </ListItemIcon>
                                {sirket.unvan}
                                {sirket.varsayilan && (
                                    <Chip
                                        label="Varsayılan"
                                        size="small"
                                        sx={{ ml: 'auto' }}
                                        color="success"
                                        variant="outlined"
                                    />
                                )}
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