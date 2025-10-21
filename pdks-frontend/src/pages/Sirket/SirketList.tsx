import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../../contexts/AuthContext'; // Yetki kontrol� i�in

// SirketListDTO.cs dosyas�ndaki alanlara uygun TypeScript Interface
interface SirketListDTO {
    id: number;
    ad: string; // C# DTO'da Unvan'a kar��l�k gelir
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
            console.error("�irketler y�klenirken hata olu�tu:", error);
            // Hata durumunda kullan�c�ya bilgi g�sterilebilir
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id: number, sirketAdi: string) => {
        if (!window.confirm(`'${sirketAdi}' adl� �irketi silmek istedi�inizden emin misiniz?`)) return;
        try {
            await axios.delete(`${API_BASE_URL}/${id}`);
            fetchSirketler(); // Listeyi yeniden y�kle
        } catch (error) {
            console.error("�irket silinirken hata olu�tu:", error);
            alert("�irket silinirken bir hata olu�tu. Ba�l� kay�tlar� kontrol edin.");
        }
    };

    if (loading) {
        return <div className="container mt-4">Y�kleniyor...</div>;
    }

    // Yaln�zca 'Admin' rol�ndekilerin eri�imi olmal�d�r
    const isAdmin = currentRole === 'Admin';
    if (!isAdmin) {
        return <div className="container mt-4 alert alert-danger">Bu sayfaya eri�im yetkiniz yoktur.</div>;
    }

    return (
        <div className="container mt-4">
            <h2>�irketler Listesi</h2>
            <Link to="/sirket/create" className="btn btn-primary mb-3">Yeni �irket Ekle</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Ad� (Unvan)</th>
                        <th>Vergi No</th>
                        <th>Telefon</th>
                        <th>Durum</th>
                        <th>Ana �irket</th>
                        <th>��lemler</th>
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
                            <td>{sirket.anaSirketAdi || 'Ana �irket'}</td>
                            <td>
                                <Link to={`/sirket/edit/${sirket.id}`} className="btn btn-sm btn-warning me-2">D�zenle</Link>
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