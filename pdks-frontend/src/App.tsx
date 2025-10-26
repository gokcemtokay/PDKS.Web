import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { AuthProvider } from './contexts/AuthContext';
import Layout from './components/Layout';
import ErrorBoundary from './components/ErrorBoundary';
import LoginPage from './pages/LoginPage';
import AnaDashboard from './pages/Dashboard/AnaDashboard';
import PersonelList from './pages/Personel/PersonelList';
import PersonelForm from './pages/Personel/PersonelForm';
import PersonelDetay from './pages/Personel/PersonelDetay/PersonelDetay';
import IzinList from './pages/Izin/IzinList';
import IzinTalepForm from './pages/Izin/IzinTalepForm';
import AvansList from './pages/Avans/AvansList';
import MasrafList from './pages/Masraf/MasrafList';
import ZimmetList from './pages/Zimmet/ZimmetList';
import AracList from './pages/Arac/AracList';
import SeyahatList from './pages/Seyahat/SeyahatList';
import PuantajList from './pages/Puantaj/PuantajList';
import OnayList from './pages/Onay/OnayList';
import RaporPage from './pages/Rapor/RaporPage';
import CihazList from './pages/Cihaz/CihazList';
import RolList from './pages/Rol/RolList';
import TatilList from './pages/Tatil/TatilList';
import VardiyaList from './pages/Vardiya/VardiyaList';
import ToplantiList from './pages/Toplanti/ToplantiList';
import OneriList from './pages/Oneri/OneriList';
import GorevList from './pages/Gorev/GorevList';

const theme = createTheme({
    palette: {
        primary: { main: '#667eea' },
        secondary: { main: '#764ba2' },
    },
    typography: {
        fontFamily: '"Roboto", "Helvetica", "Arial", sans-serif',
    },
});

function App() {
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <AuthProvider>
                <ErrorBoundary>
                    <BrowserRouter>
                        <Routes>
                            <Route path="/login" element={<LoginPage />} />
                            <Route path="/*" element={
                                <Layout>
                                    <Routes>
                                        <Route path="/" element={<AnaDashboard />} />
                                        <Route path="/personel" element={<PersonelList />} />
                                        <Route path="/personel/yeni" element={<PersonelForm />} />
                                        <Route path="/personel/duzenle/:id" element={<PersonelForm />} />
                                        <Route path="/personel/detay/:id" element={<PersonelDetay />} />
                                        <Route path="/izin" element={<IzinList />} />
                                        <Route path="/izin/talep" element={<IzinTalepForm />} />
                                        <Route path="/avans" element={<AvansList />} />
                                        <Route path="/masraf" element={<MasrafList />} />
                                        <Route path="/zimmet" element={<ZimmetList />} />
                                        <Route path="/arac" element={<AracList />} />
                                        <Route path="/seyahat" element={<SeyahatList />} />
                                        <Route path="/puantaj" element={<PuantajList />} />
                                        <Route path="/onay" element={<OnayList />} />
                                        <Route path="/gorev" element={<GorevList />} />
                                        <Route path="/rapor" element={<RaporPage />} />
                                        <Route path="/cihaz" element={<CihazList />} />
                                        <Route path="/rol" element={<RolList />} />
                                        <Route path="/tatil" element={<TatilList />} />
                                        <Route path="/vardiya" element={<VardiyaList />} />
                                        <Route path="/toplanti" element={<ToplantiList />} />
                                        <Route path="/oneri" element={<OneriList />} />
                                        <Route path="*" element={<Navigate to="/" replace />} />
                                    </Routes>
                                </Layout>
                            } />
                        </Routes>
                    </BrowserRouter>
                </ErrorBoundary>
            </AuthProvider>
        </ThemeProvider>
    );
}

export default App;