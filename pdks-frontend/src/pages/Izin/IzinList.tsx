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
            console.error("�zinler y�klenirken hata olu�tu:", error);
        }
    };

    const getStatusBadge = (status: string) => {
        switch (status) {
            case 'Onayland�': return 'bg-success';
            case 'Reddedildi': return 'bg-danger';
            case 'Beklemede': return 'bg-warning text-dark';
            default: return 'bg-secondary';
        }
    };

    return (
        <div className="container mt-4">
            <h2>�zin Talepleri</h2>
            <Link to="/izin/create" className="btn btn-primary mb-3">Yeni �zin Talebi Olu�tur</Link>
            <Link to="/izin/bekleyen" className="btn btn-info mb-3 ms-2">Bekleyen �zinler</Link>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Personel</th>
                        <th>Sicil No</th>
                        <th>�zin Tipi</th>
                        <th>Ba�lang��</th>
                        <th>Biti�</th>
                        <th>G�n</th>
                        <th>Durum</th>
                        <th>Onaylayan</th>
                        <th>��lemler</th>
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
                                <Link to={`/izin/edit/${izin.id}`} className="btn btn-sm btn-outline-warning">D�zenle</Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default IzinList;