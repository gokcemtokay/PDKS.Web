import React, { useState } from 'react';
import {
    Container,
    Box,
    TextField,
    Button,
    Typography,
    Alert,
    Paper, // Gölge ve arka plan için Paper bileşenini ekliyoruz
} from '@mui/material';

function LoginPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setError('');

        if (!email || !password) {
            setError('E-posta ve şifre alanları zorunludur.');
            return;
        }

        console.log('Giriş deneniyor:', { email, password });
        // TODO: Bu kısımda API'ye istek atacağız.
    };

    return (
        <Container component="main" maxWidth="xs">
            {/* Box'ı Paper ile sararak daha belirgin hale getiriyoruz */}
            <Paper
                elevation={6} // Gölge efekti
                sx={{
                    marginTop: 8,
                    padding: 4, // İç boşlukları artırıyoruz
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    borderRadius: 2 // Kenarları yuvarlatıyoruz
                }}
            >
                <Typography component="h1" variant="h5">
                    PDKS Giriş
                </Typography>
                <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
                    {error && <Alert severity="error" sx={{ width: '100%', mb: 2 }}>{error}</Alert>}

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
                        type="password"
                        id="password"
                        autoComplete="current-password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Giriş Yap
                    </Button>
                </Box>
            </Paper>
        </Container>
    );
}

export default LoginPage;