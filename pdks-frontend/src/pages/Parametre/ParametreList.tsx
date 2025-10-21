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
            console.error("Parametreler y�klenirken hata olu�tu:", error);
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
            alert("De�er bo� olamaz.");
            return;
        }

        // ParametreUpdateDTO'ya uygun veri yap�s�n� olu�turma
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
            fetchParametreler(); // Listeyi g�ncelleyerek de�i�ikli�i yans�t
        } catch (error) {
            console.error("Parametre g�ncellenirken hata olu�tu:", error);
            alert("Parametre g�ncellenirken bir hata olu�tu.");
        }
    };

    if (loading) {
        return <div className="container mt-4">Y�kleniyor...</div>;
    }

    // Yaln�zca 'Admin' rol�ndekilerin eri�imi olmal�d�r
    const isAdmin = currentRole === 'Admin';
    if (!isAdmin) {
        return <div className="container mt-4 alert alert-danger">Bu sayfay� g�r�nt�leme yetkiniz yoktur.</div>;
    }


    return (
        <div className="container mt-4">
            <h2>Sistem Parametreleri</h2>
            <p>Sistem davran��lar�n� kontrol eden temel parametreler.</p>
            <table className="table table-striped table-sm">
                <thead>
                    <tr>
                        <th>Ad�</th>
                        <th>Kategori</th>
                        <th>A��klama</th>
                        <th>De�er</th>
                        <th>��lemler</th>
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
                                        <button onClick={() => setEditingId(null)} className="btn btn-sm btn-secondary">�ptal</button>
                                    </>
                                ) : (
                                    <button onClick={() => handleEditStart(parametre)} className="btn btn-sm btn-warning">D�zenle</button>
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