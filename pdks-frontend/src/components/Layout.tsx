import { useState, useEffect, ReactNode } from 'react';
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
} from '@mui/icons-material';
import api from '../services/api';

interface LayoutProps {
    children: ReactNode;
    onLogout: () => void;
}

interface Sirket {
    id: number;
    sirketAdi: string;
}

const drawerWidth = 260;

const menuItems = [
    { text: 'Ana Sayfa', icon: <DashboardIcon />, path: '/' },
    { text: 'Personeller', icon: <PeopleIcon />, path: '/personel' },
    { text: 'Departmanlar', icon: <BusinessIcon />, path: '/departman' },
    { text: 'Vardiyalar', icon: <ScheduleIcon />, path: '/vardiya' },
    { text: 'Tatiller', icon: <BeachAccessIcon />, path: '/tatil' },
    { text: 'Raporlar', icon: <AssessmentIcon />, path: '/rapor' },
];

function Layout({ children, onLogout }: LayoutProps) {
    const [mobileOpen, setMobileOpen] = useState(false);
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [sirket, setSirket] = useState<Sirket | null>(null);
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        loadSirket();
    }, []);

    const loadSirket = async () => {
        try {
            // Kullanıcının şirketini al (JWT token'dan veya API'den)
            const response = await api.get('/Sirket');
            if (response.data && response.data.length > 0) {
                setSirket(response.data[0]); // İlk şirketi varsayılan yap
                localStorage.setItem('sirketId', response.data[0].id.toString());
            }
        } catch (error) {
            console.error('Sirket bilgisi alinamadi:', error);
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
        setAnchorEl(event.currentTarget);
    };

    const handleProfileMenuClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = () => {
        handleProfileMenuClose();
        onLogout();
        navigate('/login');
    };

    const drawer = (
        <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            <Box
                sx={{
                    p: 2.5,
                    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    color: 'white',
                }}
            >
                <Typography variant="h6" fontWeight="bold">
                    PDKS Sistemi
                </Typography>
                <Typography variant="caption" sx={{ opacity: 0.9 }}>
                    Personel Devam Kontrol
                </Typography>

                {/* Şirket Bilgisi */}
                {sirket && (
                    <Chip
                        label={sirket.sirketAdi}
                        size="small"
                        sx={{
                            mt: 1,
                            bgcolor: 'rgba(255,255,255,0.2)',
                            color: 'white',
                            fontWeight: 'bold',
                        }}
                    />
                )}
            </Box>

            <List sx={{ flexGrow: 1, px: 1, py: 2 }}>
                {menuItems.map((item) => (
                    <ListItem key={item.text} disablePadding sx={{ mb: 0.5 }}>
                        <ListItemButton
                            onClick={() => handleMenuClick(item.path)}
                            selected={location.pathname === item.path}
                            sx={{
                                borderRadius: 2,
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
                            <ListItemIcon>{item.icon}</ListItemIcon>
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

                    {/* Şirket Bilgisi - Header'da */}
                    {sirket && (
                        <Chip
                            icon={<BusinessIcon />}
                            label={sirket.sirketAdi}
                            sx={{ mr: 2 }}
                            color="primary"
                            variant="outlined"
                        />
                    )}

                    <IconButton onClick={handleProfileMenuOpen}>
                        <Avatar sx={{ bgcolor: '#667eea' }}>A</Avatar>
                    </IconButton>

                    <Menu
                        anchorEl={anchorEl}
                        open={Boolean(anchorEl)}
                        onClose={handleProfileMenuClose}
                        PaperProps={{
                            sx: { width: 200, mt: 1 },
                        }}
                    >
                        <MenuItem disabled>
                            <Typography variant="body2" fontWeight="bold">
                                Admin User
                            </Typography>
                        </MenuItem>
                        {sirket && (
                            <MenuItem disabled>
                                <Typography variant="caption" color="text.secondary">
                                    {sirket.sirketAdi}
                                </Typography>
                            </MenuItem>
                        )}
                        <Divider />
                        <MenuItem onClick={handleLogout}>
                            <ListItemIcon>
                                <LogoutIcon fontSize="small" />
                            </ListItemIcon>
                            Cikis Yap
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