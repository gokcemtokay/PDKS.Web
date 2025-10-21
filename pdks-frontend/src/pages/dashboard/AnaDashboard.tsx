// src/pages/dashboard/AnaDashboard.tsx - YENİ DOSYA

import React, { useEffect, useState } from 'react';
import { Box, Grid, Typography, Paper } from '@mui/material';
import BugunkunDurumWidget from './components/BugunkunDurumWidget';
import BekleyenOnaylarWidget from './components/BekleyenOnaylarWidget';
import SonAktivitelerWidget from './components/SonAktivitelerWidget';
import DogumGunleriWidget from './components/DogumGunleriWidget';

const AnaDashboard: React.FC = () => {
    const [currentTime, setCurrentTime] = useState(new Date());

    useEffect(() => {
        const timer = setInterval(() => {
            setCurrentTime(new Date());
        }, 1000);

        return () => clearInterval(timer);
    }, []);

    const getGreeting = () => {
        const hour = currentTime.getHours();
        if (hour < 12) return 'Günaydın';
        if (hour < 18) return 'İyi günler';
        return 'İyi akşamlar';
    };

    return (
        <Box>
            {/* Header */}
            <Box mb={4}>
                <Typography variant="h4" fontWeight="bold" gutterBottom>
                    {getGreeting()} 👋
                </Typography>
                <Typography variant="body1" color="text.secondary">
                    {currentTime.toLocaleDateString('tr-TR', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric',
                    })}
                    {' • '}
                    {currentTime.toLocaleTimeString('tr-TR')}
                </Typography>
            </Box>

            {/* Bugünkü Durum Kartları */}
            <Box mb={4}>
                <Typography variant="h6" fontWeight="bold" mb={2}>
                    📊 Bugünkü Durum
                </Typography>
                <BugunkunDurumWidget />
            </Box>

            {/* Alt Bölüm */}
            <Grid container spacing={3}>
                {/* Sol Kolon */}
                <Grid item xs={12} lg={8}>
                    <Grid container spacing={3}>
                        {/* Bekleyen Onaylar */}
                        <Grid item xs={12}>
                            <BekleyenOnaylarWidget />
                        </Grid>

                        {/* Son Aktiviteler */}
                        <Grid item xs={12}>
                            <SonAktivitelerWidget />
                        </Grid>
                    </Grid>
                </Grid>

                {/* Sağ Kolon */}
                <Grid item xs={12} lg={4}>
                    <Grid container spacing={3}>
                        {/* Doğum Günleri */}
                        <Grid item xs={12}>
                            <DogumGunleriWidget />
                        </Grid>

                        {/* Hızlı Aksiyonlar */}
                        <Grid item xs={12}>
                            <Paper
                                sx={{
                                    p: 3,
                                    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                    color: 'white',
                                }}
                            >
                                <Typography variant="h6" fontWeight="bold" mb={2}>
                                    ⚡ Hızlı Aksiyonlar
                                </Typography>
                                <Box display="flex" flexDirection="column" gap={1}>
                                    <Typography variant="body2">• İzin Talebi Oluştur</Typography>
                                    <Typography variant="body2">• Giriş-Çıkış Yap</Typography>
                                    <Typography variant="body2">• Zimmet Görüntüle</Typography>
                                    <Typography variant="body2">• Raporları İncele</Typography>
                                </Box>
                            </Paper>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Box>
    );
};

export default AnaDashboard;