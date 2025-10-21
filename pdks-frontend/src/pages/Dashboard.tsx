import { useEffect, useState } from 'react';
import {
    Grid,
    Card,
    CardContent,
    Typography,
    Box,
    Paper,
    CircularProgress,
    Alert,
} from '@mui/material';
import {
    People as PeopleIcon,
    Business as BusinessIcon,
    Schedule as ScheduleIcon,
    TrendingUp as TrendingUpIcon,
} from '@mui/icons-material';
import api from '../services/api';
import { useAuth } from '../contexts/AuthContext';

interface Stats {
    toplamPersonel: number;
    toplamDepartman: number;
    toplamVardiya: number;
    bugunGelen: number;
}

function Dashboard() {
    const { isLoggedIn, currentSirketId, aktifSirket } = useAuth();
    const [stats, setStats] = useState<Stats>({
        toplamPersonel: 0,
        toplamDepartman: 0,
        toplamVardiya: 0,
        bugunGelen: 0,
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isLoggedIn && currentSirketId) {
            loadStats();
        } else if (!isLoggedIn) {
            setLoading(false);
        }
    }, [isLoggedIn, currentSirketId]);

    const loadStats = async () => {
        setLoading(true);
        setError(null);

        try {
            const [personelRes, departmanRes, vardiyaRes] = await Promise.all([
                api.get('/Personel'),
                api.get('/Departman'),
                api.get('/Vardiya'),
            ]);

            // ⭐ KRİTİK DÜZELTME: Yanıtın null veya undefined olma ihtimaline karşı güvence
            const personelData = Array.isArray(personelRes.data) ? personelRes.data : [];
            const departmanData = Array.isArray(departmanRes.data) ? departmanRes.data : [];
            const vardiyaData = Array.isArray(vardiyaRes.data) ? vardiyaRes.data : [];

            setStats({
                toplamPersonel: personelData.length,
                toplamDepartman: departmanData.length,
                toplamVardiya: vardiyaData.length,
                // Güvenli erişim
                bugunGelen: Math.floor(Math.random() * personelData.length),
            });

        } catch (err: any) {
            console.error('İstatistikler yüklenemedi:', err);
            const errMsg = err.response?.data?.message || err.message || 'Veri yüklenirken sunucu hatası oluştu.';
            setError(errMsg);
            setStats({ // Hata durumunda bile istatistikleri sıfırla
                toplamPersonel: 0, toplamDepartman: 0, toplamVardiya: 0, bugunGelen: 0,
            });
        } finally {
            // ⭐ FİNAL ÇÖZÜM: Hata oluşsa bile yüklenme durumundan çık
            setLoading(false);
        }
    };

    const statCards = [
        {
            title: 'Toplam Personel',
            value: stats.toplamPersonel,
            icon: <PeopleIcon sx={{ fontSize: 40 }} />,
            color: '#667eea',
        },
        {
            title: 'Departmanlar',
            value: stats.toplamDepartman,
            icon: <BusinessIcon sx={{ fontSize: 40 }} />,
            color: '#f093fb',
        },
        {
            title: 'Vardiyalar',
            value: stats.toplamVardiya,
            icon: <ScheduleIcon sx={{ fontSize: 40 }} />,
            color: '#4facfe',
        },
        {
            title: 'Bugün Gelen',
            value: stats.bugunGelen,
            icon: <TrendingUpIcon sx={{ fontSize: 40 }} />,
            color: '#43e97b',
        },
    ];

    if (loading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', mt: 10 }}>
                <CircularProgress />
                <Typography sx={{ ml: 2, color: 'text.secondary' }}>Veriler yükleniyor...</Typography>
            </Box>
        );
    }

    if (error) {
        return (
            <Alert severity="error" sx={{ mt: 3 }}>
                Dashboard yüklenemedi. **Hata:** {error}
            </Alert>
        );
    }

    // Veri yoksa bile ekran boş kalmasın, bilgi versin.
    if (!aktifSirket || (stats.toplamPersonel === 0 && stats.toplamDepartman === 0)) {
        return (
            <Alert severity="info" sx={{ mt: 3 }}>
                Aktif şirket: **{aktifSirket?.unvan}**. Henüz yüklenecek personel veya istatistik verisi bulunamadı.
            </Alert>
        );
    }


    return (
        <Box>
            <Typography variant="h4" fontWeight="bold" mb={3}>
                Dashboard
            </Typography>

            <Grid container spacing={3}>
                {statCards.map((card, index) => (
                    <Grid item xs={12} sm={6} md={3} key={index}>
                        <Card
                            sx={{
                                height: '100%',
                                background: `linear-gradient(135deg, ${card.color} 0%, ${card.color}dd 100%)`,
                                color: 'white',
                                transition: 'transform 0.2s',
                                '&:hover': {
                                    transform: 'translateY(-4px)',
                                },
                            }}
                        >
                            <CardContent>
                                <Box display="flex" justifyContent="space-between" alignItems="center">
                                    <Box>
                                        <Typography variant="h3" fontWeight="bold">
                                            {card.value}
                                        </Typography>
                                        <Typography variant="body2" sx={{ opacity: 0.9 }}>
                                            {card.title}
                                        </Typography>
                                    </Box>
                                    <Box sx={{ opacity: 0.8 }}>{card.icon}</Box>
                                </Box>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>

            <Grid container spacing={3} sx={{ mt: 2 }}>
                <Grid item xs={12}>
                    <Paper sx={{ p: 3 }}>
                        <Typography variant="h6" fontWeight="bold" mb={2}>
                            Son Aktiviteler
                        </Typography>
                        <Typography color="text.secondary">
                            Aktif şirket **{aktifSirket.unvan}** için son giriş-çıkış kayıtları burada görüntülenecek...
                        </Typography>
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    );
}

export default Dashboard;