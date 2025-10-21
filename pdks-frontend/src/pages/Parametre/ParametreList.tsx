import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuth } from '../../contexts/AuthContext';

// ParametreListDTO.cs'ye uygun TypeScript Interface
interface ParametreListDTO {
    id: number;
    ad: string;
    deger: string;
    birim: string | null;
    aciklama: string | null;
    kategori: string | null;
}

const API_BASE_URL = 'api/Parametre';

const ParametreList = () => {
    const { currentRole } = useAuth();
    const [parametreler, setParametreler] = useState<ParametreListDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [editingId, setEditingId] = useState<number | null>(null);
    const [newDeger, setNewDeger] = useState<string>('');

    useEffect(() => {
        fetchParametreler();
    }, []);

    const fetchParametreler = async () => {
        setLoading(true);
        try {
            const response = await axios.get<ParametreListDTO[]>(API_BASE_URL);
            setParametreler(response.data);
        } catch (error) {
            console.error("Parametreler yüklenirken hata oluþtu:", error);
        } finally {
            setLoading(false);
        }
    };

    const handleEditStart = (parametre: ParametreListDTO) => {
        setEditingId(parametre.id);
        setNewDeger(parametre.deger);
    };

    const handleEditSave = async (parametre: ParametreListDTO) => {
        if (!newDeger.trim()) {
            alert("Deðer boþ olamaz.");
            return;
        }

        // ParametreUpdateDTO'ya uygun veri yapýsýný oluþturma
        const updateDTO = {
            id: parametre.id,
            ad: parametre.ad,
            deger: newDeger,
            birim: parametre.birim,
            aciklama: parametre.aciklama,
            kategori: parametre.kategori
        };

        try {
            await axios.put(`${API_BASE_URL}/${parametre.id}`, updateDTO);
            setEditingId(null);
            fetchParametreler(); // Listeyi güncelleyerek deðiþikliði yansýt
        } catch (error) {
            console.error("Parametre güncellenirken hata oluþtu:", error);
            alert("Parametre güncellenirken bir hata oluþtu.");
        }
    };

    if (loading) {
        return <div className="container mt-4">Yükleniyor...</div>;
    }

    // Yalnýzca 'Admin' rolündekilerin eriþimi olmalýdýr
    const isAdmin = currentRole === 'Admin';
    if (!isAdmin) {
        return <div className="container mt-4 alert alert-danger">Bu sayfayý görüntüleme yetkiniz yoktur.</div>;
    }


    return (
        <div className="container mt-4">
            <h2>Sistem Parametreleri</h2>
            <p>Sistem davranýþlarýný kontrol eden temel parametreler.</p>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>Adý</th>
                        <th>Kategori</th>
                        <th>Açýklama</th>
                        <th>Deðer</th>
                        <th>Ýþlemler</th>
                    </tr>
                </thead>
                <tbody>
                    {parametreler.map(parametre => (
                        <tr key={parametre.id}>
                            <td>{parametre.ad}</td>
                            <td>{parametre.kategori || '-'}</td>
                            <td>{parametre.aciklama || '-'}</td>
                            <td>
                                {editingId === parametre.id ? (
                                    <div className="input-group input-group-sm">
                                        <input
                                            type="text"
                                            value={newDeger}
                                            onChange={(e) => setNewDeger(e.target.value)}
                                            className="form-control"
                                        />
                                        {parametre.birim && <span className="input-group-text">{parametre.birim}</span>}
                                    </div>
                                ) : (
                                    <span>
                                        {parametre.deger} {parametre.birim && `(${parametre.birim})`}
                                    </span>
                                )}
                            </td>
                            <td>
                                {editingId === parametre.id ? (
                                    <>
                                        <button onClick={() => handleEditSave(parametre)} className="btn btn-sm btn-success me-2">Kaydet</button>
                                        <button onClick={() => setEditingId(null)} className="btn btn-sm btn-secondary">Ýptal</button>
                                    </>
                                ) : (
                                    <button onClick={() => handleEditStart(parametre)} className="btn btn-sm btn-warning">Düzenle</button>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ParametreList;