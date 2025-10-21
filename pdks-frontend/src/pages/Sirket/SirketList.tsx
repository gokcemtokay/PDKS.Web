import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../../contexts/AuthContext'; // Yetki kontrolü için

// SirketListDTO.cs dosyasýndaki alanlara uygun TypeScript Interface
interface SirketListDTO {
    id: number;
    ad: string; // C# DTO'da Unvan'a karþýlýk gelir
    vergiNo: string;
    telefon: string;
    aktif: boolean;
    anaSirketAdi: string | null;
    adres: string;
    ticariUnvan: string | null;
}

const API_BASE_URL = 'api/Sirket';

const SirketList = () => {
    const { currentRole } = useAuth();
    const [sirketler, setSirketler] = useState<SirketListDTO[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchSirketler();
    }, []);

    const fetchSirketler = async () => {
        setLoading(true);
        try {
            const response = await axios.get<SirketListDTO[]>(API_BASE_URL);
            setSirketler(response.data);
        } catch (error) {
            console.error("Þirketler yüklenirken hata oluþtu:", error);
            // Hata durumunda kullanýcýya bilgi gösterilebilir
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id: number, sirketAdi: string) => {
        if (!window.confirm(`'${sirketAdi}' adlý þirketi silmek istediðinizden emin misiniz?`)) return;
        try {
            await axios.delete(`${API_BASE_URL}/${id}`);
            fetchSirketler(); // Listeyi yeniden yükle
        } catch (error) {
            console.error("Þirket silinirken hata oluþtu:", error);
            alert("Þirket silinirken bir hata oluþtu. Baðlý kayýtlarý kontrol edin.");
        }
    };

    if (loading) {
        return <div className="container mt-4">Yükleniyor...</div>;
    }

    // Yalnýzca 'Admin' rolündekilerin eriþimi olmalýdýr
    const isAdmin = currentRole === 'Admin';
    if (!isAdmin) {
        return <div className="container mt-4 alert alert-danger">Bu sayfaya eriþim yetkiniz yoktur.</div>;
    }

    return (
        <div className="container mt-4">
            <h2>Þirketler Listesi</h2>
            <Link to="/sirket/create" className="btn btn-primary mb-3">Yeni Þirket Ekle</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Adý (Unvan)</th>
                        <th>Vergi No</th>
                        <th>Telefon</th>
                        <th>Durum</th>
                        <th>Ana Þirket</th>
                        <th>Ýþlemler</th>
                    </tr>
                </thead>
                <tbody>
                    {sirketler.map(sirket => (
                        <tr key={sirket.id}>
                            <td>{sirket.id}</td>
                            <td>{sirket.ad}</td>
                            <td>{sirket.vergiNo}</td>
                            <td>{sirket.telefon || '-'}</td>
                            <td><span className={`badge ${sirket.aktif ? 'bg-success' : 'bg-danger'}`}>{sirket.aktif ? 'Aktif' : 'Pasif'}</span></td>
                            <td>{sirket.anaSirketAdi || 'Ana Þirket'}</td>
                            <td>
                                <Link to={`/sirket/edit/${sirket.id}`} className="btn btn-sm btn-warning me-2">Düzenle</Link>
                                <Link to={`/sirket/details/${sirket.id}`} className="btn btn-sm btn-info me-2">Detay</Link>
                                <button onClick={() => handleDelete(sirket.id, sirket.ad)} className="btn btn-sm btn-danger">Sil</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default SirketList;