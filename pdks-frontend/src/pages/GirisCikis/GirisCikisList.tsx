import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

interface GirisCikisListDTO {
    id: number;
    personelAdi: string;
    sicilNo: string;
    girisZamani: string | null;
    cikisZamani: string | null;
    calismaSuresi: string;
    durum: string;
    elleGiris: boolean;
    not: string;
    guncellemeTarihi: string | null; // G�ncellenen DTO'ya uygun
}

const API_BASE_URL = 'api/GirisCikis';

const GirisCikisList = () => {
    const [kayitlar, setKayitlar] = useState<GirisCikisListDTO[]>([]);

    useEffect(() => {
        fetchKayitlar();
    }, []);

    const fetchKayitlar = async () => {
        try {
            const response = await axios.get<GirisCikisListDTO[]>(API_BASE_URL);
            setKayitlar(response.data);
        } catch (error) {
            console.error("Giri�/��k�� kay�tlar� y�klenirken hata olu�tu:", error);
        }
    };

    const formatDate = (dateString: string | null) => {
        return dateString ? new Date(dateString).toLocaleString() : '-';
    };

    return (
        <div className="container mt-4">
            <h2>Giri�/��k�� Kay�tlar�</h2>
            <Link to="/giriscikis/create" className="btn btn-primary mb-3">Yeni Kay�t Ekle (Manuel)</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Personel</th>
                        <th>Sicil No</th>
                        <th>Giri� Zaman�</th>
                        <th>��k�� Zaman�</th>
                        <th>�al��ma S�resi</th>
                        <th>Durum</th>
                        <th>Elle Giri�</th>
                        <th>Not</th>
                        <th>��lemler</th>
                    </tr>
                </thead>
                <tbody>
                    {kayitlar.map((kayit) => (
                        <tr key={kayit.id}>
                            <td>{kayit.id}</td>
                            <td>{kayit.personelAdi}</td>
                            <td>{kayit.sicilNo}</td>
                            <td>{formatDate(kayit.girisZamani)}</td>
                            <td>{formatDate(kayit.cikisZamani)}</td>
                            <td>{kayit.calismaSuresi}</td>
                            <td>{kayit.durum}</td>
                            <td>{kayit.elleGiris ? 'Evet' : 'Hay�r'}</td>
                            <td>{kayit.not || '-'}</td>
                            <td>
                                <Link to={`/giriscikis/edit/${kayit.id}`} className="btn btn-sm btn-outline-primary me-2">D�zenle</Link>
                                <Link to={`/giriscikis/details/${kayit.id}`} className="btn btn-sm btn-outline-info">Detay</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default GirisCikisList;