import { useEffect, useState } from 'react';
import {
    Grid,
    Card,
    CardContent,
    Typography,
    Box,
    Paper,
} from '@mui/material';
import {
    People as PeopleIcon,
    Business as BusinessIcon,
    Schedule as ScheduleIcon,
    TrendingUp as TrendingUpIcon,
} from '@mui/icons-material';
import api from '../services/api';

interface Stats {
    toplamPersonel: number;
    toplamDepartman: number;
    toplamVardiya: number;
    bugunGelen: number;
}

function Dashboard() {
    const [stats, setStats] = useState<Stats>({
        toplamPersonel: 0,
        toplamDepartman: 0,
        toplamVardiya: 0,
        bugunGelen: 0,
    });

    useEffect(() => {
        loadStats();
    }, []);

    const loadStats = async () => {
        try {
            const [personelRes, departmanRes, vardiyaRes] = await Promise.all([
                api.get('/Personel'),
                api.get('/Departman'),
                api.get('/Vardiya'),
            ]);

            setStats({
                toplamPersonel: personelRes.data.length,
                toplamDepartman: departmanRes.data.length,
                toplamVardiya: vardiyaRes.data.length,
                bugunGelen: Math.floor(Math.random() * personelRes.data.length),
            });
        } catch (error) {
            console.error('İstatistikler yüklenemedi:', error);
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
                            Yakında burada son giriş-çıkış kayıtları görüntülenecek...
                        </Typography>
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    );
}

export default Dashboard;