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
    guncellemeTarihi: string | null; // Güncellenen DTO'ya uygun
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
            console.error("Giriþ/Çýkýþ kayýtlarý yüklenirken hata oluþtu:", error);
        }
    };

    const formatDate = (dateString: string | null) => {
        return dateString ? new Date(dateString).toLocaleString() : '-';
    };

    return (
        <div className="container mt-4">
            <h2>Giriþ/Çýkýþ Kayýtlarý</h2>
            <Link to="/giriscikis/create" className="btn btn-primary mb-3">Yeni Kayýt Ekle (Manuel)</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Personel</th>
                        <th>Sicil No</th>
                        <th>Giriþ Zamaný</th>
                        <th>Çýkýþ Zamaný</th>
                        <th>Çalýþma Süresi</th>
                        <th>Durum</th>
                        <th>Elle Giriþ</th>
                        <th>Not</th>
                        <th>Ýþlemler</th>
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
                            <td>{kayit.elleGiris ? 'Evet' : 'Hayýr'}</td>
                            <td>{kayit.not || '-'}</td>
                            <td>
                                <Link to={`/giriscikis/edit/${kayit.id}`} className="btn btn-sm btn-outline-primary me-2">Düzenle</Link>
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