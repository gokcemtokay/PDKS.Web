// src/components/ErrorBoundary.tsx - YENİ DOSYA

import React, { Component } from 'react';
import type { ErrorInfo, ReactNode } from 'react';
import { Alert, Button, Box, Typography, Paper, Stack } from '@mui/material';
import { Error as ErrorIcon, Refresh as RefreshIcon, Home as HomeIcon } from '@mui/icons-material';

interface Props {
    children: ReactNode;
    fallback?: ReactNode; // Özel hata UI'ı için
}

interface State {
    hasError: boolean;
    error: Error | null;
    errorInfo: ErrorInfo | null;
}

class ErrorBoundary extends Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            hasError: false,
            error: null,
            errorInfo: null,
        };
    }

    static getDerivedStateFromError(error: Error): Partial<State> {
        // Hata oluştuğunda state'i güncelle
        return { hasError: true, error };
    }

    componentDidCatch(error: Error, errorInfo: ErrorInfo) {
        // Hata loglama servisi kullanabilirsiniz (Sentry, LogRocket vb.)
        console.error('❌ ErrorBoundary yakaladı:', error);
        console.error('📍 Component Stack:', errorInfo.componentStack);

        // State'e errorInfo'yu da ekle
        this.setState({ errorInfo });

        // TODO: Buraya Sentry/LogRocket integration eklenebilir
        // Sentry.captureException(error);
    }

    handleReset = () => {
        this.setState({
            hasError: false,
            error: null,
            errorInfo: null,
        });
    };

    handleGoHome = () => {
        window.location.href = '/';
    };

    render() {
        if (this.state.hasError) {
            // Özel fallback varsa onu göster
            if (this.props.fallback) {
                return this.props.fallback;
            }

            // Varsayılan hata UI'ı
            return (
                <Box
                    sx={{
                        minHeight: '100vh',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        bgcolor: '#f5f7fa',
                        p: 3,
                    }}
                >
                    <Paper
                        elevation={3}
                        sx={{
                            maxWidth: 600,
                            width: '100%',
                            p: 4,
                            textAlign: 'center',
                        }}
                    >
                        <Box
                            sx={{
                                width: 80,
                                height: 80,
                                borderRadius: '50%',
                                background: 'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                margin: '0 auto 24px',
                            }}
                        >
                            <ErrorIcon sx={{ fontSize: 48, color: 'white' }} />
                        </Box>

                        <Typography variant="h4" fontWeight="bold" gutterBottom>
                            Bir Şeyler Ters Gitti
                        </Typography>

                        <Typography variant="body1" color="text.secondary" paragraph>
                            Üzgünüz, beklenmeyen bir hata oluştu. Lütfen sayfayı yenileyin veya ana sayfaya dönün.
                        </Typography>

                        {/* Geliştirme modunda hata detaylarını göster */}
                        {import.meta.env.MODE === 'development' && this.state.error && (
                            <Alert severity="error" sx={{ mt: 3, textAlign: 'left' }}>
                                <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
                                    Hata Detayları (Sadece Development):
                                </Typography>
                                <Typography variant="body2" component="pre" sx={{ whiteSpace: 'pre-wrap' }}>
                                    {this.state.error.toString()}
                                </Typography>
                                {this.state.errorInfo && (
                                    <Typography
                                        variant="caption"
                                        component="pre"
                                        sx={{
                                            mt: 2,
                                            maxHeight: 200,
                                            overflow: 'auto',
                                            display: 'block',
                                            whiteSpace: 'pre-wrap',
                                        }}
                                    >
                                        {this.state.errorInfo.componentStack}
                                    </Typography>
                                )}
                            </Alert>
                        )}

                        <Stack direction="row" spacing={2} justifyContent="center" sx={{ mt: 4 }}>
                            <Button
                                variant="contained"
                                startIcon={<RefreshIcon />}
                                onClick={this.handleReset}
                                sx={{
                                    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                }}
                            >
                                Tekrar Dene
                            </Button>
                            <Button
                                variant="outlined"
                                startIcon={<HomeIcon />}
                                onClick={this.handleGoHome}
                            >
                                Ana Sayfa
                            </Button>
                        </Stack>
                    </Paper>
                </Box>
            );
        }

        return this.props.children;
    }
}

export default ErrorBoundary;