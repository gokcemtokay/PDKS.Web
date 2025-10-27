import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container, Box, TextField, Button, Typography, Alert, Paper,
  InputAdornment, IconButton, CircularProgress, Checkbox, FormControlLabel, Link,
  Grid,
} from '@mui/material';
import {
  Visibility, VisibilityOff, Login as LoginIcon, Business as BusinessIcon,
  Email as EmailIcon, Lock as LockIcon,
} from '@mui/icons-material';
import { useAuth } from '../contexts/AuthContext';
import authService from '../services/authService';

function LoginPage() {
  const [email, setEmail] = useState('admin@pdks.com');
  const [password, setPassword] = useState('admin123');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);
  const navigate = useNavigate();
  const { login, isLoggedIn } = useAuth();

  useEffect(() => {
    if (isLoggedIn) {
      navigate('/', { replace: true });
    }
  }, [isLoggedIn, navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await authService.login({ email: email.trim(), password });
      // AuthContext'in login metoduna tüm response'u geç
      login(response);
      navigate('/', { replace: true });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Giriş başarısız! Lütfen bilgilerinizi kontrol edin.');
    } finally {
      setLoading(false);
    }
  };

  if (isLoggedIn) return null;

  return (
    <Box
      sx={{
        minHeight: '100vh',
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        p: 2,
      }}
    >
      <Container maxWidth="lg">
        <Paper
          elevation={10}
          sx={{
            borderRadius: 4,
            overflow: 'hidden',
            background: 'white',
          }}
        >
          <Grid container>
            {/* Sol Panel - Branding */}
            <Grid
              item
              xs={12}
              md={5}
              sx={{
                background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                color: 'white',
                p: 6,
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'space-between',
                minHeight: { xs: '300px', md: '600px' },
              }}
            >
              <Box>
                <BusinessIcon sx={{ fontSize: 64, mb: 3, opacity: 0.9 }} />
                <Typography variant="h3" fontWeight="bold" gutterBottom>
                  PDKS Sistemi
                </Typography>
                <Typography variant="h6" sx={{ mb: 3, opacity: 0.9 }}>
                  Personel Devam Kontrol Sistemi
                </Typography>
                <Typography variant="body1" sx={{ opacity: 0.8, lineHeight: 1.8 }}>
                  Modern ve kullanıcı dostu arayüzü ile personel yönetimi, giriş-çıkış takibi, izin ve avans işlemlerini kolayca yönetin.
                </Typography>
              </Box>
              <Typography variant="caption" sx={{ opacity: 0.7, mt: 4 }}>
                © 2025 PDKS - Tüm hakları saklıdır
              </Typography>
            </Grid>

            {/* Sağ Panel - Login Form */}
            <Grid item xs={12} md={7} sx={{ p: 6 }}>
              <Box sx={{ maxWidth: 450, mx: 'auto' }}>
                <Typography
                  variant="h4"
                  fontWeight="bold"
                  gutterBottom
                  sx={{ color: 'primary.main', mb: 1 }}
                >
                  Hoş Geldiniz!
                </Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 4 }}>
                  Sisteme giriş yapmak için bilgilerinizi girin
                </Typography>

                {error && (
                  <Alert severity="error" sx={{ mb: 3 }}>
                    {error}
                  </Alert>
                )}

                <form onSubmit={handleSubmit}>
                  <TextField
                    fullWidth
                    label="E-posta Adresi"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    margin="normal"
                    required
                    autoFocus
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <EmailIcon color="primary" />
                        </InputAdornment>
                      ),
                    }}
                    sx={{ mb: 2 }}
                  />

                  <TextField
                    fullWidth
                    label="Şifre"
                    type={showPassword ? 'text' : 'password'}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    margin="normal"
                    required
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <LockIcon color="primary" />
                        </InputAdornment>
                      ),
                      endAdornment: (
                        <InputAdornment position="end">
                          <IconButton onClick={() => setShowPassword(!showPassword)} edge="end">
                            {showPassword ? <VisibilityOff /> : <Visibility />}
                          </IconButton>
                        </InputAdornment>
                      ),
                    }}
                    sx={{ mb: 2 }}
                  />

                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={rememberMe}
                          onChange={(e) => setRememberMe(e.target.checked)}
                          color="primary"
                        />
                      }
                      label="Beni Hatırla"
                    />
                    <Link href="#" underline="hover" variant="body2" color="primary">
                      Şifremi Unuttum?
                    </Link>
                  </Box>

                  <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    size="large"
                    disabled={loading}
                    startIcon={loading ? <CircularProgress size={20} color="inherit" /> : <LoginIcon />}
                    sx={{
                      py: 1.5,
                      fontSize: '1rem',
                      fontWeight: 'bold',
                      textTransform: 'uppercase',
                      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                      '&:hover': {
                        background: 'linear-gradient(135deg, #5568d3 0%, #6a3e8f 100%)',
                      },
                    }}
                  >
                    {loading ? 'Giriş yapılıyor...' : 'GİRİŞ YAP'}
                  </Button>
                </form>

                {/* Varsayılan Giriş Bilgileri */}
                <Box
                  sx={{
                    mt: 4,
                    p: 2,
                    bgcolor: 'grey.50',
                    borderRadius: 2,
                    border: '1px solid',
                    borderColor: 'grey.200',
                  }}
                >
                  <Typography variant="caption" color="text.secondary" display="block" fontWeight="bold">
                    Varsayılan Giriş Bilgileri:
                  </Typography>
                  <Typography variant="caption" color="text.secondary" display="block">
                    E-posta: admin@pdks.com
                  </Typography>
                  <Typography variant="caption" color="text.secondary" display="block">
                    Şifre: admin123
                  </Typography>
                </Box>
              </Box>
            </Grid>
          </Grid>
        </Paper>
      </Container>
    </Box>
  );
}

export default LoginPage;
