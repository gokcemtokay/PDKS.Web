import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import React, { useEffect } from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { AuthProvider, useAuth } from './contexts/AuthContext'; // AuthProvider ve useAuth Context'ten gelmeli
import LoginPage from './pages/LoginPage';
import Dashboard from './pages/Dashboard';
import PersonelList from './pages/Personel/PersonelList';
import PersonelForm from './pages/Personel/PersonelForm';
import DepartmanList from './pages/Departman/DepartmanList';
import VardiyaList from './pages/Vardiya/VardiyaList';
import TatilList from './pages/Tatil/TatilList';
import RaporPage from './pages/Rapor/RaporPage';
import Layout from './components/Layout';
// --- Eksik Ekranlar i�in importlar (�nceki cevaplarda kodlar� verilmi�ti) ---
import CihazList from './pages/Cihaz/CihazList';
import GirisCikisList from './pages/GirisCikis/GirisCikisList';
import IzinList from './pages/Izin/IzinList';
import KullaniciList from './pages/Kullanici/KullaniciList';
import SirketList from './pages/Sirket/SirketList';
import ParametreList from './pages/Parametre/ParametreList';

// Tema olu�tur
const theme = createTheme({
    typography: {
        fontFamily: [
            'Roboto',
            '-apple-system',
            'BlinkMacSystemFont',
            '"Segoe UI"',
            'Arial',
            'sans-serif',
        ].join(','),
    },
});

// 1. ADIM: Korumal� Rota Bile�eni (T�m kimlik do�rulama mant��� useAuth i�inde olmal�)
// Layout bile�eni burada sar�c� olarak kullan�l�r.
function ProtectedRoutes() {
    const { isLoggedIn, logout } = useAuth(); // AuthContext'teki isLoggedIn kullan�l�r.

    if (!isLoggedIn) {
        return <Navigate to="/login" replace />; // Giri� yap�lmad�ysa Login'e y�nlendir
    }

    // Giri� yap�lm��sa, t�m uygulamay� Layout ile sararak korumal� rotalar� g�ster
    return (
        <Layout onLogout={logout}>
            <Routes>
                {/* Ana Sayfa */}
                <Route path="/" element={<Dashboard />} />

                {/* Personel */}
                <Route path="/personel" element={<PersonelList />} />
                <Route path="/personel/yeni" element={<PersonelForm />} />
                <Route path="/personel/duzenle/:id" element={<PersonelForm />} />

                {/* Y�netim */}
                <Route path="/departman" element={<DepartmanList />} />
                <Route path="/vardiya" element={<VardiyaList />} />
                <Route path="/tatil" element={<TatilList />} />
                <Route path="/sirket" element={<SirketList />} />
                <Route path="/parametre" element={<ParametreList />} />

                {/* Di�er Mod�ller */}
                <Route path="/cihaz" element={<CihazList />} />
                <Route path="/giriscikis" element={<GirisCikisList />} />
                <Route path="/izin" element={<IzinList />} />
                <Route path="/kullanici" element={<KullaniciList />} />
                <Route path="/rapor" element={<RaporPage />} />

                {/* Tan�ms�z Rotalar� Ana Sayfaya Y�nlendir */}
                <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
        </Layout>
    );
}

// 2. ADIM: Ana Uygulama Bile�eni
function App() {
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Router>
                <AuthProvider> {/* AuthProvider Router'� de�il, rotalar� sarmal� */}
                    <Routes>
                        {/* 1. Login Rotas� (Korumas�z) */}
                        <Route path="/login" element={<LoginPage />} />

                        {/* 2. Korumal� Rotalar */}
                        {/* T�m di�er rotalar ProtectedRoutes alt�nda y�netilir */}
                        <Route path="/*" element={<ProtectedRoutes />} />
                    </Routes>
                </AuthProvider>
            </Router>
        </ThemeProvider>
    );
}

export default App;