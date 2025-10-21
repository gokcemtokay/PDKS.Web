import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

interface IzinListDTO {
    id: number;
    personelAdi: string;
    personelSicilNo: string;
    izinTipi: string;
    baslangicTarihi: string;
    bitisTarihi: string;
    gunSayisi: number;
    onayDurumu: string;
    onaylayanKullaniciAdi: string | null;
}

const API_BASE_URL = 'api/Izin';

const IzinList = () => {
    const [izinler, setIzinler] = useState<IzinListDTO[]>([]);

    useEffect(() => {
        fetchIzinler();
    }, []);

    const fetchIzinler = async () => {
        try {
            const response = await axios.get<IzinListDTO[]>(API_BASE_URL);
            setIzinler(response.data);
        } catch (error) {
            console.error("Ýzinler yüklenirken hata oluþtu:", error);
        }
    };

    const getStatusBadge = (status: string) => {
        switch (status) {
            case 'Onaylandý': return 'bg-success';
            case 'Reddedildi': return 'bg-danger';
            case 'Beklemede': return 'bg-warning text-dark';
            default: return 'bg-secondary';
        }
    };

    return (
        <div className="container mt-4">
            <h2>Ýzin Talepleri</h2>
            <Link to="/izin/create" className="btn btn-primary mb-3">Yeni Ýzin Talebi Oluþtur</Link>
            <Link to="/izin/bekleyen" className="btn btn-info mb-3 ms-2">Bekleyen Ýzinler</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Personel</th>
                        <th>Sicil No</th>
                        <th>Ýzin Tipi</th>
                        <th>Baþlangýç</th>
                        <th>Bitiþ</th>
                        <th>Gün</th>
                        <th>Durum</th>
                        <th>Onaylayan</th>
                        <th>Ýþlemler</th>
                    </tr>
                </thead>
                <tbody>
                    {izinler.map((izin) => (
                        <tr key={izin.id}>
                            <td>{izin.id}</td>
                            <td>{izin.personelAdi}</td>
                            <td>{izin.personelSicilNo}</td>
                            <td>{izin.izinTipi}</td>
                            <td>{new Date(izin.baslangicTarihi).toLocaleDateString()}</td>
                            <td>{new Date(izin.bitisTarihi).toLocaleDateString()}</td>
                            <td>{izin.gunSayisi}</td>
                            <td><span className={`badge ${getStatusBadge(izin.onayDurumu)}`}>{izin.onayDurumu}</span></td>
                            <td>{izin.onaylayanKullaniciAdi || '-'}</td>
                            <td>
                                <Link to={`/izin/details/${izin.id}`} className="btn btn-sm btn-outline-info me-2">Detay</Link>
                                <Link to={`/izin/edit/${izin.id}`} className="btn btn-sm btn-outline-warning">Düzenle</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default IzinList;