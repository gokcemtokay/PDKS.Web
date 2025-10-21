import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

// Yeni güncellenen CihazListDTO'ya uygun TypeScript Interface
interface CihazListDTO {
    id: number;
    cihazAdi: string;
    ipAdres: string;
    lokasyon: string;
    durum: boolean;
    durumText: string;
    sonBaglantiZamani: string | null;
    bugunkuOkumaSayisi: number;
}

const API_BASE_URL = 'api/Cihaz'; // API Controller'ýnýzýn yolu

const CihazList = () => {
    const [cihazlar, setCihazlar] = useState<CihazListDTO[]>([]);

    useEffect(() => {
        fetchCihazlar();
    }, []);

    const fetchCihazlar = async () => {
        try {
            // Gerçek API çaðrýsýný yapýn
            const response = await axios.get<CihazListDTO[]>(API_BASE_URL);
            setCihazlar(response.data);
        } catch (error) {
            console.error("Cihazlar yüklenirken hata oluþtu:", error);
            // Hata durumunda boþ liste dönsün
            setCihazlar([]);
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm("Bu cihazý silmek istediðinizden emin misiniz?")) return;
        try {
            await axios.delete(`${API_BASE_URL}/${id}`); // API çaðrýsý
            fetchCihazlar(); // Listeyi yeniden yükle
        } catch (error) {
            console.error("Cihaz silinirken hata oluþtu:", error);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Cihazlar</h2>
            <Link to="/cihaz/create" className="btn btn-primary mb-3">Yeni Cihaz Ekle</Link>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Cihaz Adý</th>
                        <th>IP Adresi</th>
                        <th>Lokasyon</th>
                        <th>Durum</th>
                        <th>Son Baðlantý</th>
                        <th>Okuma Sayýsý</th>
                        <th>Ýþlemler</th>
                    </tr>
                </thead>
                <tbody>
                    {cihazlar.map((cihaz) => (
                        <tr key={cihaz.id}>
                            <td>{cihaz.id}</td>
                            <td>{cihaz.cihazAdi}</td>
                            <td>{cihaz.ipAdres}</td>
                            <td>{cihaz.lokasyon}</td>
                            <td><span className={`badge ${cihaz.durum ? 'bg-success' : 'bg-danger'}`}>{cihaz.durumText}</span></td>
                            <td>{cihaz.sonBaglantiZamani ? new Date(cihaz.sonBaglantiZamani).toLocaleString() : 'Hiç Yok'}</td>
                            <td>{cihaz.bugunkuOkumaSayisi}</td>
                            <td>
                                <Link to={`/cihaz/edit/${cihaz.id}`} className="btn btn-sm btn-warning me-2">Düzenle</Link>
                                <button onClick={() => handleDelete(cihaz.id)} className="btn btn-sm btn-danger">Sil</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default CihazList;