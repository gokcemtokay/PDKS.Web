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
// --- Eksik Ekranlar için importlar (Önceki cevaplarda kodlarý verilmiþti) ---
import CihazList from './pages/Cihaz/CihazList';
import GirisCikisList from './pages/GirisCikis/GirisCikisList';
import IzinList from './pages/Izin/IzinList';
import KullaniciList from './pages/Kullanici/KullaniciList';
import SirketList from './pages/Sirket/SirketList';
import ParametreList from './pages/Parametre/ParametreList';

// Tema oluþtur
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

// 1. ADIM: Korumalý Rota Bileþeni (Tüm kimlik doðrulama mantýðý useAuth içinde olmalý)
// Layout bileþeni burada sarýcý olarak kullanýlýr.
function ProtectedRoutes() {
    const { isLoggedIn, logout } = useAuth(); // AuthContext'teki isLoggedIn kullanýlýr.

    if (!isLoggedIn) {
        return <Navigate to="/login" replace />; // Giriþ yapýlmadýysa Login'e yönlendir
    }

    // Giriþ yapýlmýþsa, tüm uygulamayý Layout ile sararak korumalý rotalarý göster
    return (
        <Layout onLogout={logout}>
            <Routes>
                {/* Ana Sayfa */}
                <Route path="/" element={<Dashboard />} />

                {/* Personel */}
                <Route path="/personel" element={<PersonelList />} />
                <Route path="/personel/yeni" element={<PersonelForm />} />
                <Route path="/personel/duzenle/:id" element={<PersonelForm />} />

                {/* Yönetim */}
                <Route path="/departman" element={<DepartmanList />} />
                <Route path="/vardiya" element={<VardiyaList />} />
                <Route path="/tatil" element={<TatilList />} />
                <Route path="/sirket" element={<SirketList />} />
                <Route path="/parametre" element={<ParametreList />} />

                {/* Diðer Modüller */}
                <Route path="/cihaz" element={<CihazList />} />
                <Route path="/giriscikis" element={<GirisCikisList />} />
                <Route path="/izin" element={<IzinList />} />
                <Route path="/kullanici" element={<KullaniciList />} />
                <Route path="/rapor" element={<RaporPage />} />

                {/* Tanýmsýz Rotalarý Ana Sayfaya Yönlendir */}
                <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
        </Layout>
    );
}

// 2. ADIM: Ana Uygulama Bileþeni
function App() {
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Router>
                <AuthProvider> {/* AuthProvider Router'ý deðil, rotalarý sarmalý */}
                    <Routes>
                        {/* 1. Login Rotasý (Korumasýz) */}
                        <Route path="/login" element={<LoginPage />} />

                        {/* 2. Korumalý Rotalar */}
                        {/* Tüm diðer rotalar ProtectedRoutes altýnda yönetilir */}
                        <Route path="/*" element={<ProtectedRoutes />} />
                    </Routes>
                </AuthProvider>
            </Router>
        </ThemeProvider>
    );
}

export default App;