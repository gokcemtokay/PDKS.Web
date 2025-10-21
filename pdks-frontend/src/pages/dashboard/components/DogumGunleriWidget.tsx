// src/pages/dashboard/components/DogumGunleriWidget.tsx - YENİ DOSYA

import React, { useEffect, useState } from 'react';
import {
    Card,
    CardContent,
    CardHeader,
    Box,
    Typography,
    Avatar,
    List,
    ListItem,
    ListItemAvatar,
    ListItemText,
    Chip,
    Divider,
} from '@mui/material';
import { Cake } from '@mui/icons-material';
import dashboardService from '../../../services/dashboardService';


// En üste type ekle
interface DogumGunu {
    personelId: number;
    adSoyad: string;
    profilFoto: string;
    dogumTarihi: string;
    kacGunSonra: number;
    bugun: boolean;
}

const DogumGunleriWidget: React.FC = () => {
    const [dogumGunleri, setDogumGunleri] = useState<DogumGunu[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await dashboardService.getDogumGunleri();
            setDogumGunleri(data);
        } catch (error) {
            console.error('Doğum günleri yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <Card>
                <CardContent>
                    <Typography>Yükleniyor...</Typography>
                </CardContent>
            </Card>
        );
    }

    return (
        <Card>
            <CardHeader
                avatar={<Cake color="primary" />}
                title={
                    <Typography variant="h6" fontWeight="bold">
                        🎂 Yaklaşan Doğum Günleri
                    </Typography>
                }
            />
            <CardContent sx={{ pt: 0 }}>
                {dogumGunleri.length === 0 ? (
                    <Box textAlign="center" py={4}>
                        <Typography variant="body2" color="text.secondary">
                            Önümüzdeki 30 gün içinde doğum günü yok
                        </Typography>
                    </Box>
                ) : (
                    <List sx={{ width: '100%' }}>
                        {dogumGunleri.map((dogumGunu, index) => (
                            <React.Fragment key={dogumGunu.personelId}>
                                {index > 0 && <Divider variant="inset" component="li" />}
                                <ListItem
                                    sx={{
                                        borderRadius: 2,
                                        mb: 1,
                                        bgcolor: dogumGunu.bugun ? 'action.selected' : 'transparent',
                                        '&:hover': {
                                            bgcolor: 'action.hover',
                                        },
                                    }}
                                >
                                    <ListItemAvatar>
                                        <Avatar
                                            src={dogumGunu.profilFoto}
                                            sx={{
                                                width: 48,
                                                height: 48,
                                                border: dogumGunu.bugun ? '2px solid' : 'none',
                                                borderColor: 'primary.main',
                                            }}
                                        >
                                            {dogumGunu.adSoyad.charAt(0)}
                                        </Avatar>
                                    </ListItemAvatar>
                                    <ListItemText
                                        primary={
                                            <Box display="flex" alignItems="center" gap={1}>
                                                <Typography variant="subtitle2" fontWeight="bold">
                                                    {dogumGunu.adSoyad}
                                                </Typography>
                                                {dogumGunu.bugun && (
                                                    <Chip
                                                        label="🎉 Bugün!"
                                                        color="primary"
                                                        size="small"
                                                        sx={{
                                                            height: 20,
                                                            background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                                        }}
                                                    />
                                                )}
                                            </Box>
                                        }
                                        secondary={
                                            dogumGunu.bugun
                                                ? '🎂 Doğum günü bugün!'
                                                : `${dogumGunu.kacGunSonra} gün sonra`
                                        }
                                    />
                                    <Cake
                                        sx={{
                                            color: dogumGunu.bugun ? 'primary.main' : 'action.disabled',
                                        }}
                                    />
                                </ListItem>
                            </React.Fragment>
                        ))}
                    </List>
                )}
            </CardContent>
        </Card>
    );
};

export default DogumGunleriWidget;