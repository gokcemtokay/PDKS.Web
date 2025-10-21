// src/pages/dashboard/components/StatCard.tsx - YENÝ DOSYA

import React from 'react';
import { Box, Card, CardContent, Typography, Avatar } from '@mui/material';
import { TrendingUp, TrendingDown, TrendingFlat } from '@mui/icons-material';

interface StatCardProps {
    title: string;
    value: number | string;
    icon: React.ReactNode;
    color: string;
    trend?: 'up' | 'down' | 'flat';
    trendValue?: string;
    subtitle?: string;
}

const StatCard: React.FC<StatCardProps> = ({
    title,
    value,
    icon,
    color,
    trend,
    trendValue,
    subtitle,
}) => {
    const getTrendIcon = () => {
        if (trend === 'up') return <TrendingUp fontSize="small" />;
        if (trend === 'down') return <TrendingDown fontSize="small" />;
        return <TrendingFlat fontSize="small" />;
    };

    const getTrendColor = () => {
        if (trend === 'up') return 'success.main';
        if (trend === 'down') return 'error.main';
        return 'text.secondary';
    };

    return (
        <Card
            sx={{
                height: '100%',
                background: `linear-gradient(135deg, ${color}15 0%, ${color}05 100%)`,
                border: `1px solid ${color}30`,
                transition: 'all 0.3s ease',
                '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: 4,
                },
            }}
        >
            <CardContent>
                <Box display="flex" justifyContent="space-between" alignItems="flex-start" mb={2}>
                    <Box>
                        <Typography variant="body2" color="text.secondary" gutterBottom>
                            {title}
                        </Typography>
                        <Typography variant="h4" fontWeight="bold" color="text.primary">
                            {value}
                        </Typography>
                    </Box>
                    <Avatar
                        sx={{
                            bgcolor: color,
                            width: 48,
                            height: 48,
                        }}
                    >
                        {icon}
                    </Avatar>
                </Box>

                {subtitle && (
                    <Typography variant="body2" color="text.secondary">
                        {subtitle}
                    </Typography>
                )}

                {trend && trendValue && (
                    <Box display="flex" alignItems="center" gap={0.5} mt={1}>
                        <Box color={getTrendColor()} display="flex" alignItems="center">
                            {getTrendIcon()}
                        </Box>
                        <Typography variant="body2" color={getTrendColor()}>
                            {trendValue}
                        </Typography>
                    </Box>
                )}
            </CardContent>
        </Card>
    );
};

export default StatCard;