// src/App.tsx - DAHA BASIT VE GÜVENLİ VERSİYON

import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import Layout from './components/Layout';
import LoginPage from './pages/LoginPage';
import PersonelList from './pages/personel/PersonelList';
import PersonelForm from './pages/personel/PersonelForm';
import DepartmanList from './pages/departman/DepartmanList';
import VardiyaList from './pages/vardiya/VardiyaList';
import TatilList from './pages/tatil/TatilList';
import ParametreList from './pages/parametre/ParametreList';
import RaporPage from './pages/rapor/RaporPage';
import ErrorBoundary from './components/ErrorBoundary';
import AnaDashboard from './pages/dashboard/AnaDashboard';

function App() {
    return (
        <AuthProvider>
            <ErrorBoundary>
                <BrowserRouter>
                    <Routes>
                        {/* Login - Layout olmadan */}
                        <Route path="/login" element={<LoginPage />} />

                        {/* Tüm diğer route'lar Layout içinde */}
                        <Route
                            path="/*"
                            element={
                                <Layout>
                                    <Routes>
                                        <Route path="/personel" element={<PersonelList />} />
                                        <Route path="/personel/yeni" element={<PersonelForm />} />
                                        <Route path="/personel/duzenle/:id" element={<PersonelForm />} />
                                        <Route path="/departman" element={<DepartmanList />} />
                                        <Route path="/vardiya" element={<VardiyaList />} />
                                        <Route path="/tatil" element={<TatilList />} />
                                        <Route path="/parametre" element={<ParametreList />} />
                                        <Route path="/rapor" element={<RaporPage />} />
                                        <Route path="*" element={<Navigate to="/" replace />} />
                                        <Route path="/" element={<AnaDashboard />} />
                                        <Route path="/dashboard" element={<AnaDashboard />} />
                                    </Routes>
                                </Layout>
                            }
                        />
                    </Routes>
                </BrowserRouter>
            </ErrorBoundary>
        </AuthProvider>
    );
}

export default App;