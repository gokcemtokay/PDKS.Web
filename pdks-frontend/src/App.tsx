import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import LoginPage from './pages/LoginPage';
import Dashboard from './pages/Dashboard';
import PersonelList from './pages/Personel/PersonelList';
import PersonelForm from './pages/Personel/PersonelForm';
import DepartmanList from './pages/Departman/DepartmanList';
import VardiyaList from './pages/Vardiya/VardiyaList';
import TatilList from './pages/Tatil/TatilList';
import RaporPage from './pages/Rapor/RaporPage';
import Layout from './components/Layout';


// Tema oluþtur - TR karakter desteði
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
function App() {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            setIsAuthenticated(true);
        }
    }, []);

    const handleLogin = (token: string) => {
        localStorage.setItem('token', token);
        setIsAuthenticated(true);
    };

    const handleLogout = () => {
        localStorage.removeItem('token');
        setIsAuthenticated(false);
    };

    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Router>
                <Routes>
                    <Route
                        path="/login"
                        element={!isAuthenticated ? <LoginPage onLogin={handleLogin} /> : <Navigate to="/" />}
                    />

                    <Route
                        path="/*"
                        element={
                            isAuthenticated ? (
                                <Layout onLogout={handleLogout}>
                                    <Routes>
                                        <Route path="/" element={<Dashboard />} />
                                        <Route path="/personel" element={<PersonelList />} />
                                        <Route path="/personel/yeni" element={<PersonelForm />} />
                                        <Route path="/personel/duzenle/:id" element={<PersonelForm />} />
                                        <Route path="/departman" element={<DepartmanList />} />
                                        <Route path="/vardiya" element={<VardiyaList />} />
                                        <Route path="/tatil" element={<TatilList />} />
                                        <Route path="/rapor" element={<RaporPage />} />
                                    </Routes>
                                </Layout>
                            ) : (
                                <Navigate to="/login" />
                            )
                        }
                    />
                </Routes>
            </Router>
        </ThemeProvider>
    );
}

export default App;