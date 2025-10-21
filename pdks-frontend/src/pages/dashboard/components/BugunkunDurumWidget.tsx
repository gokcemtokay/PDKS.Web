// src/pages/dashboard/components/BugunkunDurumWidget.tsx - GEÇİCİ ÇÖZÜM

import React, { useEffect, useState } from 'react';
import { Grid } from '@mui/material';
import {
    People,
    Login,
    CheckCircle,
    BeachAccess,
    LocalHospital,
    AccessTime,
    ReportProblem,
} from '@mui/icons-material';
import StatCard from './StatCard';
import dashboardService from '../../../services/dashboardService';

// ⬅️ TYPE'LARI BURAYA TAŞI (GEÇİCİ)
interface BugunkunDurum {
    toplamPersonel: number;
    bugunkuGiris: number;
    aktifPersonel: number;
    izinliPersonel: number;
    raporluPersonel: number;
    gecKalanPersonel: number;
    devamsizPersonel: number;
    girisCikisOrani: number;
}

const BugunkunDurumWidget: React.FC = () => {
    const [durum, setDurum] = useState<BugunkunDurum | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await dashboardService.getBugunkunDurum();
            setDurum(data);
        } catch (error) {
            console.error('Bugünkü durum yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    if (loading || !durum) {
        return <div>Yükleniyor...</div>;
    }

    return (
        <Grid container spacing={3}>
            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Toplam Personel"
                    value={durum.toplamPersonel}
                    icon={<People />}
                    color="#667eea"
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Bugünkü Giriş"
                    value={durum.bugunkuGiris}
                    icon={<Login />}
                    color="#764ba2"
                    subtitle={`${durum.girisCikisOrani.toFixed(1)}% giriş oranı`}
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Aktif Personel"
                    value={durum.aktifPersonel}
                    icon={<CheckCircle />}
                    color="#43cea2"
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="İzinli"
                    value={durum.izinliPersonel}
                    icon={<BeachAccess />}
                    color="#f093fb"
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Raporlu"
                    value={durum.raporluPersonel}
                    icon={<LocalHospital />}
                    color="#fa709a"
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Geç Kalan"
                    value={durum.gecKalanPersonel}
                    icon={<AccessTime />}
                    color="#ff9a56"
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Devamsız"
                    value={durum.devamsizPersonel}
                    icon={<ReportProblem />}
                    color="#f6416c"
                />
            </Grid>

            <Grid item xs={12} sm={6} md={3}>
                <StatCard
                    title="Giriş Oranı"
                    value={`${durum.girisCikisOrani.toFixed(1)}%`}
                    icon={<CheckCircle />}
                    color="#30cfd0"
                    trend={durum.girisCikisOrani >= 90 ? 'up' : durum.girisCikisOrani >= 70 ? 'flat' : 'down'}
                    trendValue={durum.girisCikisOrani >= 90 ? 'Mükemmel' : durum.girisCikisOrani >= 70 ? 'İyi' : 'Düşük'}
                />
            </Grid>
        </Grid>
    );
};

export default BugunkunDurumWidget;