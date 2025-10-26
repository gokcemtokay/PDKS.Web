import { useState, ReactNode, useEffect } from 'react';
import { useNavigate, useLocation, Navigate } from 'react-router-dom';
import {
    Box, Drawer, AppBar, Toolbar, List, Typography, IconButton, ListItem,
    ListItemButton, ListItemIcon, ListItemText, Avatar, Menu, MenuItem, Divider, Chip,
    Badge, Select, FormControl,
} from '@mui/material';
import {
    Menu as MenuIcon, Dashboard as DashboardIcon, People as PeopleIcon,
    BeachAccess as BeachAccessIcon, AttachMoney as AttachMoneyIcon,
    Receipt as ReceiptIcon, DirectionsCar as DirectionsCarIcon,
    Flight as FlightIcon, AccessTime as AccessTimeIcon,
    Assignment as AssignmentIcon, Assessment as AssessmentIcon,
    Logout as LogoutIcon, Business as BusinessIcon, Notifications as NotificationsIcon,
    Settings as SettingsIcon, CalendarToday as CalendarTodayIcon,
    Devices as DevicesIcon, Security as SecurityIcon, MeetingRoom as MeetingRoomIcon,
    Feedback as FeedbackIcon, Task as TaskIcon,
} from '@mui/icons-material';
import { useAuth } from '../contexts/AuthContext';

const drawerWidth = 260;

interface MenuItemType {
    text: string;
    icon: ReactNode;
    path: string;
    roles?: string[];
}

const menuItems: MenuItemType[] = [
    { text: 'Ana Sayfa', icon: <DashboardIcon />, path: '/' },
    { text: 'Personeller', icon: <PeopleIcon />, path: '/personel', roles: ['Admin', 'IK'] },
    { text: 'İzinler', icon: <BeachAccessIcon />, path: '/izin' },
    { text: 'Avanslar', icon: <AttachMoneyIcon />, path: '/avans' },
    { text: 'Masraflar', icon: <ReceiptIcon />, path: '/masraf' },
    { text: 'Zimmetler', icon: <AssignmentIcon />, path: '/zimmet' },
    { text: 'Araç Talepleri', icon: <DirectionsCarIcon />, path: '/arac' },
    { text: 'Seyahat', icon: <FlightIcon />, path: '/seyahat' },
    { text: 'Puantaj', icon: <AccessTimeIcon />, path: '/puantaj', roles: ['Admin', 'IK'] },
    { text: 'Onaylar', icon: <AssignmentIcon />, path: '/onay' },
    { text: 'Görevler', icon: <TaskIcon />, path: '/gorev' },
    { text: 'Raporlar', icon: <AssessmentIcon />, path: '/rapor', roles: ['Admin', 'IK', 'Yönetici'] },
    { text: 'Cihazlar', icon: <DevicesIcon />, path: '/cihaz', roles: ['Admin'] },
    { text: 'Tatiller', icon: <CalendarTodayIcon />, path: '/tatil', roles: ['Admin', 'IK'] },
    { text: 'Vardiyalar', icon: <AccessTimeIcon />, path: '/vardiya', roles: ['Admin', 'IK'] },
    { text: 'Toplantı Odası', icon: <MeetingRoomIcon />, path: '/toplanti' },
    { text: 'Öneri & Şikayet', icon: <FeedbackIcon />, path: '/oneri' },
    { text: 'Rol & Yetki', icon: <SecurityIcon />, path: '/rol', roles: ['Admin'] },
    { text: 'Ayarlar', icon: <SettingsIcon />, path: '/parametre', roles: ['Admin'] },
];

function Layout({ children }: { children: ReactNode }) {
    const [mobileOpen, setMobileOpen] = useState(false);
    const [profileAnchorEl, setProfileAnchorEl] = useState<null | HTMLElement>(null);
    const [notifAnchorEl, setNotifAnchorEl] = useState<null | HTMLElement>(null);
    const [unreadCount, setUnreadCount] = useState(3); // Mock data
    const { user, logout, isLoggedIn, aktifSirket, yetkiliSirketler, switchSirket } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    if (!isLoggedIn || !user) {
        return <Navigate to="/login" replace />;
    }

    const handleDrawerToggle = () => setMobileOpen(!mobileOpen);
    const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => setProfileAnchorEl(event.currentTarget);
    const handleProfileMenuClose = () => setProfileAnchorEl(null);
    const handleNotifMenuOpen = (event: React.MouseEvent<HTMLElement>) => setNotifAnchorEl(event.currentTarget);
    const handleNotifMenuClose = () => setNotifAnchorEl(null);
    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const filteredMenuItems = menuItems.filter(item =>
        !item.roles || item.roles.includes(user.role)
    );

    const drawer = (
        <Box>
            <Toolbar sx={{ background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)', color: 'white' }}>
                <BusinessIcon sx={{ mr: 1 }} />
                <Typography variant="h6" noWrap>
                    PDKS
                </Typography>
            </Toolbar>
            <Divider />
            {aktifSirket && (
                <Box sx={{ p: 2, bgcolor: 'grey.100' }}>
                    <Typography variant="caption" color="text.secondary">Aktif Şirket</Typography>
                    <FormControl fullWidth size="small" sx={{ mt: 1 }}>
                        <Select
                            value={aktifSirket.sirketId}
                            onChange={(e) => switchSirket(e.target.value as number)}
                        >
                            {yetkiliSirketler.map((sirket) => (
                                <MenuItem key={sirket.sirketId} value={sirket.sirketId}>
                                    {sirket.sirketAdi}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </Box>
            )}
            <Divider />
            <List>
                {filteredMenuItems.map((item) => (
                    <ListItem key={item.path} disablePadding>
                        <ListItemButton
                            selected={location.pathname === item.path}
                            onClick={() => navigate(item.path)}
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
        <Box sx={{ display: 'flex' }}>
            <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                <Toolbar>
                    <IconButton color="inherit" edge="start" onClick={handleDrawerToggle} sx={{ mr: 2, display: { sm: 'none' } }}>
                        <MenuIcon />
                    </IconButton>
                    <Typography variant="h6" noWrap sx={{ flexGrow: 1 }}>
                        Personel Yönetim Sistemi
                    </Typography>
                    <IconButton color="inherit" onClick={handleNotifMenuOpen}>
                        <Badge badgeContent={unreadCount} color="error">
                            <NotificationsIcon />
                        </Badge>
                    </IconButton>
                    <Menu
                        anchorEl={notifAnchorEl}
                        open={Boolean(notifAnchorEl)}
                        onClose={handleNotifMenuClose}
                        PaperProps={{ sx: { width: 320, maxHeight: 400 } }}
                    >
                        <MenuItem>
                            <Typography variant="subtitle2">Yeni izin talebi</Typography>
                        </MenuItem>
                        <MenuItem>
                            <Typography variant="subtitle2">Avans talebi onaylandı</Typography>
                        </MenuItem>
                        <MenuItem>
                            <Typography variant="subtitle2">Toplantı hatırlatması</Typography>
                        </MenuItem>
                        <Divider />
                        <MenuItem onClick={handleNotifMenuClose}>
                            <Typography variant="body2" color="primary">Tümünü Gör</Typography>
                        </MenuItem>
                    </Menu>
                    <Chip label={user.role} color="primary" size="small" sx={{ mr: 2, ml: 2 }} />
                    <IconButton onClick={handleProfileMenuOpen} color="inherit">
                        <Avatar sx={{ width: 32, height: 32 }}>{user.ad[0]}</Avatar>
                    </IconButton>
                    <Menu
                        anchorEl={profileAnchorEl}
                        open={Boolean(profileAnchorEl)}
                        onClose={handleProfileMenuClose}
                    >
                        <MenuItem disabled>
                            <Typography variant="body2">{user.email}</Typography>
                        </MenuItem>
                        <Divider />
                        <MenuItem onClick={handleLogout}>
                            <ListItemIcon><LogoutIcon fontSize="small" /></ListItemIcon>
                            Çıkış Yap
                        </MenuItem>
                    </Menu>
                </Toolbar>
            </AppBar>

            <Box component="nav" sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}>
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

            <Box component="main" sx={{ flexGrow: 1, p: 3, width: { sm: `calc(100% - ${drawerWidth}px)` }, mt: 8 }}>
                {children}
            </Box>
        </Box>
    );
}

export default Layout;