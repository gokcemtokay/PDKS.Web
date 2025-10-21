import React, { useState, useEffect } from 'react';
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
import axios from 'axios'; // Doğrudan axios'u kullanacağız. api.ts kullanılıyorsa, onu da kullanabilirsiniz.
import { useAuth } from '../contexts/AuthContext'; // ⭐ KRİTİK: useAuth'u import ettik

const API_LOGIN_URL = '/api/Auth/login';

// LoginPageProps artık kullanılmayacak, onLogin kaldırıldı.
// function LoginPage({ onLogin }: LoginPageProps) {
function LoginPage() {
    const [email, setEmail] = useState('admin@pdks.com');
    const [password, setPassword] = useState('admin123');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);

    const navigate = useNavigate();
    // ⭐ KRİTİK DEĞİŞİKLİK: useAuth'tan login ve isLoggedIn durumunu çekiyoruz.
    const { login, isLoggedIn } = useAuth();

    // Bileşen yüklendiğinde veya isLoggedIn değiştiğinde yönlendirme kontrolü
    useEffect(() => {
        // Eğer giriş yapılmışsa, direkt ana sayfaya yönlendir.
        if (isLoggedIn) {
            navigate('/', { replace: true });
        }
    }, [isLoggedIn, navigate]);

    // isLoggedIn true ise, bu bileşeni render etmeye gerek yok, yönlendirme zaten yapılıyor.
    if (isLoggedIn) {
        return null;
    }


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            const response = await axios.post(API_LOGIN_URL, {
                email: email.trim(),
                password: password
            });

            const token = response.data?.token || response.data?.Token;

            if (token) {
                // ⭐ KRİTİK KISIM: Token'ı AuthContext'e kaydet. 
                // Bu, hem global state'i günceller hem de App.tsx'teki yönlendirmeyi tetikler.
                login(token);
                // Başarılı giriş sonrası yönlendirme, useEffect içinde zaten handle ediliyor.
                // İsteğe bağlı olarak direkt yönlendirme çağrılabilir:
                // navigate('/', { replace: true });

            } else {
                setError('Giriş başarılı ama sunucudan token alınamadı.');
            }

        } catch (err) {
            let errorMessage = 'Giriş başarısız!';

            if (axios.isAxiosError(err) && err.response) {
                // Sunucudan gelen hata mesajını kullan
                errorMessage = (err.response.data.message || err.response.data || 'Geçersiz e-posta veya şifre').toString();
            } else if (err instanceof Error) {
                errorMessage = err.message;
            }

            setError(errorMessage);
            localStorage.removeItem('token'); // Başarısız denemede token'ı temizle
        } finally {
            setLoading(false);
        }
    };

    // Kalan render kodu aynı kalır.
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