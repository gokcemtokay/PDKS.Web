import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

interface KullaniciListDTO {
    id: number;
    personelAdi: string;
    personelSicilNo: string;
    email: string;
    rolAdi: string;
    aktif: boolean;
    sonGirisTarihi: string | null;
}

const API_BASE_URL = 'api/Kullanici';

const KullaniciList = () => {
    const [kullanicilar, setKullanicilar] = useState<KullaniciListDTO[]>([]);

    useEffect(() => {
        fetchKullanicilar();
    }, []);

    const fetchKullanicilar = async () => {
        try {
            const response = await axios.get<KullaniciListDTO[]>(API_BASE_URL);
            setKullanicilar(response.data);
        } catch (error) {
            console.error("Kullan�c�lar y�klenirken hata olu�tu:", error);
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm("Bu kullan�c�y� silmek istedi�inizden emin misiniz?")) return;
        try {
            await axios.delete(`${API_BASE_URL}/${id}`);
            fetchKullanicilar(); // Listeyi yeniden y�kle
        } catch (error) {
            console.error("Kullan�c� silinirken hata olu�tu:", error);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Kullan�c�lar</h2>
            <Link to="/kullanici/create" className="btn btn-primary mb-3">Yeni Kullan�c� Ekle</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Personel Ad�</th>
                        <th>Sicil No</th>
                        <th>Email</th>
                        <th>Rol</th>
                        <th>Aktif</th>
                        <th>Son Giri�</th>
                        <th>��lemler</th>
                    </tr>
                </thead>
                <tbody>
                    {kullanicilar.map((kullanici) => (
                        <tr key={kullanici.id}>
                            <td>{kullanici.id}</td>
                            <td>{kullanici.personelAdi}</td>
                            <td>{kullanici.personelSicilNo}</td>
                            <td>{kullanici.email}</td>
                            <td>{kullanici.rolAdi}</td>
                            <td>{kullanici.aktif ? 'Evet' : 'Hay�r'}</td>
                            <td>{kullanici.sonGirisTarihi ? new Date(kullanici.sonGirisTarihi).toLocaleString() : '-'}</td>
                            <td>
                                <Link to={`/kullanici/edit/${kullanici.id}`} className="btn btn-sm btn-warning me-2">D�zenle</Link>
                                <button onClick={() => handleDelete(kullanici.id)} className="btn btn-sm btn-danger">Sil</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default KullaniciList;