import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Container,
    Box,
    TextField,
    Button,
    Typography,
    Alert,
    Paper,
    InputAdornment,
    IconButton,
} from '@mui/material';
import { Visibility, VisibilityOff, Login as LoginIcon } from '@mui/icons-material';
import api from '../services/api';

interface LoginPageProps {
    onLogin: (token: string) => void;
}

function LoginPage({ onLogin }: LoginPageProps) {
    const [email, setEmail] = useState('admin@pdks.com');
    const [password, setPassword] = useState('admin123');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        // ÖNEMLİ: Sayfa yenilenmesini engelle
        e.preventDefault();
        e.stopPropagation();

        console.log('🔐 Form submit - preventDefault çağrıldı');

        setError('');
        setLoading(true);

        try {
            console.log('📡 API isteği gönderiliyor...');

            const response = await api.post('/Auth/login', {
                email: email.trim(),
                password: password
            });

            console.log('✅ Response:', response.data);

            const token = response.data?.token || response.data?.Token;

            if (token) {
                console.log('💾 Token kaydediliyor...');
                localStorage.setItem('token', token);
                onLogin(token);

                console.log('🚀 Navigate ediliyor...');
                navigate('/', { replace: true });
            } else {
                console.error('❌ Token bulunamadı!');
                setError('Giriş başarılı ama token alınamadı.');
            }

        } catch (err: any) {
            console.error('❌ Catch bloğu:', err);

            let errorMessage = 'Giriş başarısız!';

            if (err?.response) {
                if (err.response.status === 401) {
                    errorMessage = 'Geçersiz e-posta veya şifre!';
                } else if (err.response.data) {
                    errorMessage = typeof err.response.data === 'string'
                        ? err.response.data
                        : err.response.data.message || errorMessage;
                }
            } else if (err?.message) {
                errorMessage = err.message;
            }

            console.log('⚠️ Hata mesajı set ediliyor:', errorMessage);
            setError(errorMessage);

        } finally {
            setLoading(false);
        }
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                display: 'flex',
                alignItems: 'center',
                background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
            }}
        >
            <Container component="main" maxWidth="xs">
                <Paper
                    elevation={12}
                    sx={{
                        padding: 4,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        borderRadius: 3,
                    }}
                >
                    <Box
                        sx={{
                            width: 60,
                            height: 60,
                            borderRadius: '50%',
                            background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            mb: 2,
                        }}
                    >
                        <LoginIcon sx={{ color: 'white', fontSize: 32 }} />
                    </Box>

                    <Typography component="h1" variant="h5" fontWeight="bold">
                        PDKS Giriş
                    </Typography>

                    <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                        Personel Devam Kontrol Sistemi
                    </Typography>

                    {/* onSubmit burada, form tag'inde */}
                    <Box
                        component="form"
                        onSubmit={handleSubmit}
                        noValidate
                        sx={{ mt: 3, width: '100%' }}
                    >
                        {error && (
                            <Alert severity="error" sx={{ mb: 2 }}>
                                {error}
                            </Alert>
                        )}

                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id="email"
                            label="E-posta Adresi"
                            name="email"
                            autoComplete="email"
                            autoFocus
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />

                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Şifre"
                            type={showPassword ? 'text' : 'password'}
                            id="password"
                            autoComplete="current-password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position="end">
                                        <IconButton
                                            onClick={() => setShowPassword(!showPassword)}
                                            edge="end"
                                            type="button"
                                        >
                                            {showPassword ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                ),
                            }}
                        />

                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            disabled={loading}
                            sx={{
                                mt: 3,
                                mb: 2,
                                py: 1.5,
                                background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                '&:hover': {
                                    background: 'linear-gradient(135deg, #764ba2 0%, #667eea 100%)',
                                },
                            }}
                        >
                            {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
                        </Button>

                        <Box sx={{ textAlign: 'center', mt: 2 }}>
                            <Typography variant="caption" color="text.secondary">
                                Varsayılan: admin@pdks.com / admin123
                            </Typography>
                        </Box>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
}

export default LoginPage;
