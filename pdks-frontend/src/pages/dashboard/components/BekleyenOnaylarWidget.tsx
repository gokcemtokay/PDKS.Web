// src/pages/dashboard/components/BekleyenOnaylarWidget.tsx

import React, { useEffect, useState } from 'react';
import {
    Card,
    CardContent,
    CardHeader,
    Box,
    Typography,
    Chip,
    Avatar,
    List,
    ListItem,
    ListItemAvatar,
    ListItemText,
    ListItemButton,
    Divider,
    Button,
} from '@mui/material';
import {
    Assignment,
    BeachAccess,
    AttachMoney,
    DirectionsCar,
    Flight,
    Receipt,
    ArrowForward,
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import dashboardService from '../../../services/dashboardService';
import { formatDistanceToNow } from 'date-fns';
import { tr } from 'date-fns/locale';

// ⬅️ TYPE BURAYA
interface BekleyenOnay {
    onayKaydiId: number;
    modulTipi: string;
    talepEden: string;
    adimAdi: string;
    talepTarihi: string;
    beklemeSuresi: number;
    oncelikDurumu: string;
}

const BekleyenOnaylarWidget: React.FC = () => {
    const navigate = useNavigate();
    const [onaylar, setOnaylar] = useState<BekleyenOnay[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await dashboardService.getBekleyenOnaylar();
            setOnaylar(data);
        } catch (error) {
            console.error('Bekleyen onaylar yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const getModulIcon = (modulTipi: string) => {
        switch (modulTipi) {
            case 'Izin':
                return <BeachAccess />;
            case 'Avans':
                return <AttachMoney />;
            case 'Masraf':
                return <Receipt />;
            case 'Seyahat':
                return <Flight />;
            case 'Arac':
                return <DirectionsCar />;
            default:
                return <Assignment />;
        }
    };

    const getModulColor = (modulTipi: string) => {
        switch (modulTipi) {
            case 'Izin':
                return '#43cea2';
            case 'Avans':
                return '#667eea';
            case 'Masraf':
                return '#fa709a';
            case 'Seyahat':
                return '#764ba2';
            case 'Arac':
                return '#30cfd0';
            default:
                return '#667eea';
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
                title={
                    <Box display="flex" alignItems="center" justifyContent="space-between">
                        <Typography variant="h6" fontWeight="bold">
                            Bekleyen Onaylar
                        </Typography>
                        {onaylar.length > 0 && (
                            <Chip
                                label={onaylar.length}
                                color="primary"
                                size="small"
                                sx={{
                                    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                }}
                            />
                        )}
                    </Box>
                }
            />
            <CardContent sx={{ pt: 0 }}>
                {onaylar.length === 0 ? (
                    <Box textAlign="center" py={4}>
                        <Typography variant="body2" color="text.secondary">
                            🎉 Bekleyen onayınız yok
                        </Typography>
                    </Box>
                ) : (
                    <>
                        <List sx={{ width: '100%' }}>
                            {onaylar.map((onay, index) => (
                                <React.Fragment key={onay.onayKaydiId}>
                                    {index > 0 && <Divider variant="inset" component="li" />}
                                    <ListItemButton
                                        onClick={() => navigate(`/onaylar/${onay.onayKaydiId}`)}
                                        sx={{
                                            borderRadius: 2,
                                            mb: 1,
                                            '&:hover': {
                                                bgcolor: 'action.hover',
                                            },
                                        }}
                                    >
                                        <ListItemAvatar>
                                            <Avatar
                                                sx={{
                                                    bgcolor: getModulColor(onay.modulTipi),
                                                }}
                                            >
                                                {getModulIcon(onay.modulTipi)}
                                            </Avatar>
                                        </ListItemAvatar>
                                        <ListItemText
                                            primary={
                                                <Box display="flex" alignItems="center" gap={1}>
                                                    <Typography variant="subtitle2" fontWeight="bold">
                                                        {onay.modulTipi} Talebi
                                                    </Typography>
                                                    {onay.oncelikDurumu === 'Acil' && (
                                                        <Chip
                                                            label="Acil"
                                                            color="error"
                                                            size="small"
                                                            sx={{ height: 20 }}
                                                        />
                                                    )}
                                                </Box>
                                            }
                                            secondary={
                                                <Box>
                                                    <Typography variant="body2" color="text.secondary">
                                                        {onay.talepEden} • {onay.adimAdi}
                                                    </Typography>
                                                    <Typography variant="caption" color="text.secondary">
                                                        {formatDistanceToNow(new Date(onay.talepTarihi), {
                                                            addSuffix: true,
                                                            locale: tr,
                                                        })}
                                                        {' • '}
                                                        {onay.beklemeSuresi} gündür bekliyor
                                                    </Typography>
                                                </Box>
                                            }
                                        />
                                        <ArrowForward color="action" />
                                    </ListItemButton>
                                </React.Fragment>
                            ))}
                        </List>

                        <Box mt={2}>
                            <Button
                                fullWidth
                                variant="outlined"
                                endIcon={<ArrowForward />}
                                onClick={() => navigate('/onaylar')}
                            >
                                Tüm Onayları Gör
                            </Button>
                        </Box>
                    </>
                )}
            </CardContent>
        </Card>
    );
};

export default BekleyenOnaylarWidget;