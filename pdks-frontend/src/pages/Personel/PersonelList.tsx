import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Button, IconButton, Alert, Avatar } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Add as AddIcon, Edit as EditIcon, Visibility as VisibilityIcon } from '@mui/icons-material';
import personelService, { Personel } from '../../services/personelService';
import { useAuth } from '../../contexts/AuthContext';

function PersonelList() {
    const [personeller, setPersoneller] = useState<Personel[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { aktifSirket } = useAuth();

    useEffect(() => {
        if (aktifSirket) {
            loadData();
        }
    }, [aktifSirket]);

    const loadData = async () => {
        try {
            setError(null);
            setLoading(true);
            const data = await personelService.getAll();
            setPersoneller(data);
        } catch (error: any) {
            console.error('Personeller yüklenemedi:', error);

            if (error.response?.status === 500) {
                setError('Backend sunucu hatası. Lütfen backend console loglarını kontrol edin.');
            } else if (error.response?.status === 401) {
                setError('Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.');
                setTimeout(() => navigate('/login'), 2000);
            } else if (error.response?.status === 403) {
                setError('Bu işlem için yetkiniz yok.');
            } else {
                setError(`Personeller yüklenirken hata oluştu: ${error.message || 'Bilinmeyen hata'}`);
            }
        } finally {
            setLoading(false);
        }
    };

    const columns: GridColDef[] = [
        {
            field: 'profilFoto',
            headerName: '',
            width: 70,
            sortable: false,
            renderCell: (params) => (
                <Avatar
                    src={params.row.profilFoto || params.row.profilResmi || ''}
                    alt={params.row.adSoyad}
                    sx={{ width: 40, height: 40 }}
                >
                    {params.row.adSoyad?.[0] || '?'}
                </Avatar>
            ),
        },
        { field: 'sicilNo', headerName: 'Sicil No', width: 120 },
        { field: 'adSoyad', headerName: 'Ad Soyad', width: 200 },
        { field: 'email', headerName: 'E-posta', width: 200 },
        { field: 'telefon', headerName: 'Telefon', width: 150 },
        { field: 'departmanAdi', headerName: 'Departman', width: 150 },
        {
            field: 'durum',
            headerName: 'Durum',
            width: 100,
            renderCell: (params) => params.value ? 'Aktif' : 'Pasif'
        },
        {
            field: 'actions',
            headerName: 'İşlemler',
            width: 150,
            renderCell: (params) => (
                <Box>
                    <IconButton
                        size="small"
                        onClick={() => navigate(`/personel/detay/${params.row.id}`)}
                        title="Görüntüle"
                    >
                        <VisibilityIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                        size="small"
                        onClick={() => navigate(`/personel/duzenle/${params.row.id}`)}
                        title="Düzenle"
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Box>
                    <Typography variant="h4" fontWeight="bold">Personeller</Typography>
                    {aktifSirket && (
                        <Typography variant="body2" color="text.secondary">
                            {aktifSirket.sirketAdi}
                        </Typography>
                    )}
                </Box>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => navigate('/personel/yeni')}
                >
                    Yeni Personel
                </Button>
            </Box>

            {error && (
                <Alert
                    severity="error"
                    sx={{ mb: 3 }}
                    onClose={() => setError(null)}
                >
                    {error}
                </Alert>
            )}

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid
                    rows={personeller}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: {
                            paginationModel: { pageSize: 10 }
                        }
                    }}
                    disableRowSelectionOnClick
                    localeText={{
                        noRowsLabel: error
                            ? 'Veriler yüklenemedi'
                            : 'Henüz personel kaydı yok',
                    }}
                    sx={{
                        '& .MuiDataGrid-cell': {
                            display: 'flex',
                            alignItems: 'center',
                        }
                    }}
                />
            </Box>
        </Box>
    );
}

export default PersonelList;
