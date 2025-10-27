import { useState, ReactNode } from 'react';
import { useNavigate, useLocation, Navigate } from 'react-router-dom';
import {
    Box, Drawer, AppBar, Toolbar, List, Typography, IconButton, ListItem,
    ListItemButton, ListItemIcon, ListItemText, Avatar, Menu, MenuItem, Divider, Chip,
    Badge, Select, FormControl, Collapse,
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
    Feedback as FeedbackIcon, Task as TaskIcon, ExpandLess, ExpandMore,
    Login as LoginIcon, Article as ArticleIcon, School as SchoolIcon,
    CardMembership as CardMembershipIcon, HealthAndSafety as HealthIcon,
    Schedule as ScheduleIcon, Group as GroupIcon, CheckCircle as CheckCircleIcon,
    SupervisorAccount as SupervisorAccountIcon,
} from '@mui/icons-material';
import { useAuth } from '../contexts/AuthContext';

const drawerWidth = 260;

interface MenuItemType {
    text: string;
    icon: ReactNode;
    path?: string;
    roles?: string[];
    children?: MenuItemType[];
}

const menuItems: MenuItemType[] = [
    { text: 'Ana Sayfa', icon: <DashboardIcon />, path: '/' },
    { text: 'Personel Yönetimi', icon: <PeopleIcon />, path: '/personel' },
    {
        text: 'Giriş-Çıkış',
        icon: <LoginIcon />,
        roles: ['Admin', 'IK', 'Yönetici', 'admin', 'ADMIN'],
        children: [
            { text: 'Günlük Giriş-Çıkış', icon: <AccessTimeIcon />, path: '/giris-cikis' },
            { text: 'Puantaj Raporu', icon: <AssessmentIcon />, path: '/puantaj' },
            { text: 'Geç Kalanlar', icon: <ScheduleIcon />, path: '/puantaj/gec-kalanlar' },
            { text: 'Erken Çıkanlar', icon: <ScheduleIcon />, path: '/puantaj/erken-cikanlar' },
        ],
    },
    { text: 'İzinler', icon: <BeachAccessIcon />, path: '/izin' },
    { text: 'Avanslar', icon: <AttachMoneyIcon />, path: '/avans' },
    { text: 'Masraflar', icon: <ReceiptIcon />, path: '/masraf' },
    { text: 'Zimmetler', icon: <AssignmentIcon />, path: '/zimmet' },
    { text: 'Araç Talepleri', icon: <DirectionsCarIcon />, path: '/arac' },
    { text: 'Seyahat', icon: <FlightIcon />, path: '/seyahat' },
    { text: 'Onay İşlemleri', icon: <CheckCircleIcon />, path: '/onay' },
    { text: 'Görevler', icon: <TaskIcon />, path: '/gorev' },
    {
        text: 'Raporlar',
        icon: <AssessmentIcon />,
        roles: ['Admin', 'IK', 'Yönetici', 'admin', 'ADMIN'],
        children: [
            { text: 'Rapor Merkezi', icon: <DashboardIcon />, path: '/rapor' },
            { text: 'Personel Raporları', icon: <PeopleIcon />, path: '/rapor/personel' },
            { text: 'İzin Raporları', icon: <BeachAccessIcon />, path: '/rapor/izin' },
            { text: 'Puantaj Raporları', icon: <AccessTimeIcon />, path: '/rapor/puantaj' },
            { text: 'Masraf Raporları', icon: <ReceiptIcon />, path: '/rapor/masraf' },
        ],
    },
    { text: 'Toplantı Odası', icon: <MeetingRoomIcon />, path: '/toplanti' },
    { text: 'Öneri & Şikayet', icon: <FeedbackIcon />, path: '/oneri' },
    
    // ✅ YENİ EKLENEN MENÜLER
    { text: 'Şirket Yönetimi', icon: <BusinessIcon />, path: '/sirket', roles: ['Admin', 'admin', 'ADMIN'] },
    { text: 'Kullanıcı Yönetimi', icon: <SupervisorAccountIcon />, path: '/kullanici', roles: ['Admin', 'admin', 'ADMIN'] },
    
    { text: 'Cihaz Yönetimi', icon: <DevicesIcon />, path: '/cihaz', roles: ['Admin', 'admin', 'ADMIN'] },
    { text: 'Tatil Günleri', icon: <CalendarTodayIcon />, path: '/tatil', roles: ['Admin', 'IK', 'admin', 'ADMIN'] },
    { text: 'Vardiya Yönetimi', icon: <AccessTimeIcon />, path: '/vardiya', roles: ['Admin', 'IK', 'admin', 'ADMIN'] },
    { text: 'Rol & Yetki', icon: <SecurityIcon />, path: '/rol', roles: ['Admin', 'admin', 'ADMIN'] },
    { text: 'Sistem Ayarları', icon: <SettingsIcon />, path: '/parametre', roles: ['Admin', 'admin', 'ADMIN'] },
];

function Layout({ children }: { children: ReactNode }) {
    const [mobileOpen, setMobileOpen] = useState(false);
    const [profileAnchorEl, setProfileAnchorEl] = useState<null | HTMLElement>(null);
    const [notifAnchorEl, setNotifAnchorEl] = useState<null | HTMLElement>(null);
    const [openSubMenus, setOpenSubMenus] = useState<{ [key: string]: boolean }>({});
    const [unreadCount] = useState(3);
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

    const handleSubMenuToggle = (text: string) => {
        setOpenSubMenus(prev => ({ ...prev, [text]: !prev[text] }));
    };

    const handleSirketChange = (event: any) => {
        const sirketId = Number(event.target.value);
        if (switchSirket && sirketId) {
            switchSirket(sirketId);
            window.location.reload(); // Sayfayı yenile ki tüm datalar yeni şirkete göre yüklensin
        }
    };

    // Admin kontrolü - email, username veya role göre
    const isAdmin = () => {
        const email = user.email?.toLowerCase();
        const username = user.ad?.toLowerCase();
        const role = user.role?.toLowerCase();

        return (
            email?.includes('admin') ||
            username?.includes('admin') ||
            role === 'admin' ||
            role === 'administrator' ||
            email === 'admin@pdks.com'
        );
    };

    const hasRole = (roles?: string[]) => {
        if (!roles || roles.length === 0) return true;

        // Admin ise tüm menüleri göster
        if (isAdmin()) return true;

        // Rol kontrolü (büyük/küçük harf duyarsız)
        const userRole = user.role?.toLowerCase();
        return roles.some(role => role.toLowerCase() === userRole);
    };

    const renderMenuItem = (item: MenuItemType) => {
        if (!hasRole(item.roles)) return null;

        if (item.children) {
            return (
                <Box key={item.text}>
                    <ListItemButton onClick={() => handleSubMenuToggle(item.text)}>
                        <ListItemIcon>{item.icon}</ListItemIcon>
                        <ListItemText primary={item.text} />
                        {openSubMenus[item.text] ? <ExpandLess /> : <ExpandMore />}
                    </ListItemButton>
                    <Collapse in={openSubMenus[item.text]} timeout="auto" unmountOnExit>
                        <List component="div" disablePadding>
                            {item.children.map(child => (
                                <ListItemButton
                                    key={child.text}
                                    sx={{ pl: 4 }}
                                    onClick={() => child.path && navigate(child.path)}
                                    selected={location.pathname === child.path}
                                >
                                    <ListItemIcon sx={{ minWidth: 36 }}>{child.icon}</ListItemIcon>
                                    <ListItemText primary={child.text} />
                                </ListItemButton>
                            ))}
                        </List>
                    </Collapse>
                </Box>
            );
        }

        return (
            <ListItem key={item.text} disablePadding>
                <ListItemButton
                    onClick={() => item.path && navigate(item.path)}
                    selected={location.pathname === item.path}
                >
                    <ListItemIcon>{item.icon}</ListItemIcon>
                    <ListItemText primary={item.text} />
                </ListItemButton>
            </ListItem>
        );
    };

    const drawer = (
        <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            <Toolbar>
                <Typography variant="h6" noWrap component="div" sx={{ fontWeight: 'bold', color: 'primary.main' }}>
                    PDKS Sistemi
                </Typography>
            </Toolbar>
            <Divider />
            <Box sx={{ overflow: 'auto', flexGrow: 1 }}>
                <List>
                    {menuItems.map(item => renderMenuItem(item))}
                </List>
            </Box>
            <Divider />
            <Box sx={{ p: 2 }}>
                <Typography variant="caption" color="text.secondary">
                    © 2025 PDKS Sistemi
                </Typography>
            </Box>
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

                    {/* ✅ ŞİRKET SEÇİCİ - HER ZAMAN GÖRÜNÜR */}
                    {aktifSirket && (
                        <Box sx={{ display: 'flex', alignItems: 'center', mr: 2 }}>
                            {yetkiliSirketler && yetkiliSirketler.length > 1 ? (
                                // Çoklu şirket varsa - Select göster
                                <FormControl size="small" sx={{ minWidth: 200 }}>
                                    <Select
                                        value={aktifSirket.sirketId}
                                        onChange={handleSirketChange}
                                        sx={{
                                            color: 'white',
                                            '.MuiOutlinedInput-notchedOutline': { borderColor: 'rgba(255,255,255,0.3)' },
                                            '&:hover .MuiOutlinedInput-notchedOutline': { borderColor: 'rgba(255,255,255,0.5)' },
                                            '.MuiSvgIcon-root': { color: 'white' },
                                        }}
                                        startAdornment={<BusinessIcon sx={{ mr: 1, color: 'white' }} />}
                                    >
                                        {yetkiliSirketler.map((sirket: any) => (
                                            <MenuItem key={sirket.sirketId} value={sirket.sirketId}>
                                                {sirket.sirketAdi}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            ) : (
                                // Tek şirket varsa - Sadece göster
                                <Chip
                                    icon={<BusinessIcon />}
                                    label={aktifSirket.sirketAdi}
                                    variant="outlined"
                                    sx={{
                                        color: 'white',
                                        borderColor: 'rgba(255,255,255,0.3)',
                                        '& .MuiChip-icon': { color: 'white' },
                                    }}
                                />
                            )}
                        </Box>
                    )}

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
                    <Chip label={user.role || 'Kullanıcı'} color="secondary" size="small" sx={{ mr: 2, ml: 2 }} />
                    <IconButton onClick={handleProfileMenuOpen} color="inherit">
                        <Avatar sx={{ width: 32, height: 32 }}>{user.ad?.[0] || 'U'}</Avatar>
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
                        <MenuItem onClick={() => { navigate('/profil'); handleProfileMenuClose(); }}>
                            <ListItemIcon><PeopleIcon fontSize="small" /></ListItemIcon>
                            Profilim
                        </MenuItem>
                        <MenuItem onClick={() => { navigate('/ayarlar'); handleProfileMenuClose(); }}>
                            <ListItemIcon><SettingsIcon fontSize="small" /></ListItemIcon>
                            Ayarlar
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
