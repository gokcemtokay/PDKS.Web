import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

// Yeni g�ncellenen CihazListDTO'ya uygun TypeScript Interface
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

const API_BASE_URL = 'api/Cihaz'; // API Controller'�n�z�n yolu

const CihazList = () => {
    const [cihazlar, setCihazlar] = useState<CihazListDTO[]>([]);

    useEffect(() => {
        fetchCihazlar();
    }, []);

    const fetchCihazlar = async () => {
        try {
            // Ger�ek API �a�r�s�n� yap�n
            const response = await axios.get<CihazListDTO[]>(API_BASE_URL);
            setCihazlar(response.data);
        } catch (error) {
            console.error("Cihazlar y�klenirken hata olu�tu:", error);
            // Hata durumunda bo� liste d�ns�n
            setCihazlar([]);
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm("Bu cihaz� silmek istedi�inizden emin misiniz?")) return;
        try {
            await axios.delete(`${API_BASE_URL}/${id}`); // API �a�r�s�
            fetchCihazlar(); // Listeyi yeniden y�kle
        } catch (error) {
            console.error("Cihaz silinirken hata olu�tu:", error);
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
                        <th>Cihaz Ad�</th>
                        <th>IP Adresi</th>
                        <th>Lokasyon</th>
                        <th>Durum</th>
                        <th>Son Ba�lant�</th>
                        <th>Okuma Say�s�</th>
                        <th>��lemler</th>
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
                            <td>{cihaz.sonBaglantiZamani ? new Date(cihaz.sonBaglantiZamani).toLocaleString() : 'Hi� Yok'}</td>
                            <td>{cihaz.bugunkuOkumaSayisi}</td>
                            <td>
                                <Link to={`/cihaz/edit/${cihaz.id}`} className="btn btn-sm btn-warning me-2">D�zenle</Link>
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