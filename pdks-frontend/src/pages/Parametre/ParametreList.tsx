import { useEffect, useState } from 'react';
import {
    Box,
    Button,
    Paper,
    Typography,
    IconButton,
    Alert,
    Snackbar,
    TextField,
    InputAdornment,
} from '@mui/material';
import {
    Edit as EditIcon,
    Save as SaveIcon,
    Cancel as CancelIcon,
    Settings as SettingsIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';
import { useAuth } from '../../contexts/AuthContext';

interface Parametre {
    id: number;
    ad: string;
    deger: string;
    birim: string | null;
    aciklama: string | null;
    kategori: string | null;
}

function ParametreList() {
    const { currentRole } = useAuth();
    const [parametreler, setParametreler] = useState<Parametre[]>([]);
    const [loading, setLoading] = useState(false);
    const [editingId, setEditingId] = useState<number | null>(null);
    const [newDeger, setNewDeger] = useState<string>('');
    const [snackbar, setSnackbar] = useState<{
        open: boolean;
        message: string;
        severity: 'success' | 'error';
    }>({
        open: false,
        message: '',
        severity: 'success',
    });

    useEffect(() => {
        loadParametreler();
    }, []);

    const loadParametreler = async () => {
        setLoading(true);
        try {
            const response = await api.get('/Parametre');
            setParametreler(response.data);
        } catch (error) {
            console.error('Parametreler yüklenemedi:', error);
            setSnackbar({ open: true, message: 'Parametreler yüklenemedi!', severity: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleEditStart = (parametre: Parametre) => {
        setEditingId(parametre.id);
        setNewDeger(parametre.deger);
    };

    const handleEditSave = async (parametre: Parametre) => {
        if (!newDeger.trim()) {
            setSnackbar({ open: true, message: 'Deðer boþ olamaz!', severity: 'error' });
            return;
        }

        const updateDTO = {
            id: parametre.id,
            ad: parametre.ad,
            deger: newDeger,
            birim: parametre.birim,
            aciklama: parametre.aciklama,
            kategori: parametre.kategori,
        };

        try {
            await api.put(`/Parametre/${parametre.id}`, updateDTO);
            setSnackbar({ open: true, message: 'Parametre güncellendi!', severity: 'success' });
            setEditingId(null);
            loadParametreler();
        } catch (error) {
            console.error('Parametre güncellenemedi:', error);
            setSnackbar({ open: true, message: 'Parametre güncellenemedi!', severity: 'error' });
        }
    };

    const handleEditCancel = () => {
        setEditingId(null);
        setNewDeger('');
    };

    // Admin kontrolü
    if (currentRole !== 'Admin') {
        return (
            <Box>
                <Alert severity="error">
                    Bu sayfayý görüntüleme yetkiniz yoktur. Sadece Admin kullanýcýlarý parametreleri düzenleyebilir.
                </Alert>
            </Box>
        );
    }

    const columns: GridColDef[] = [
        {
            field: 'ad',
            headerName: 'Parametre Adý',
            width: 250,
            renderCell: (params) => (
                <Box display="flex" alignItems="center" gap={1}>
                    <SettingsIcon fontSize="small" color="primary" />
                    <Typography fontWeight="500">{params.value}</Typography>
                </Box>
            ),
        },
        {
            field: 'kategori',
            headerName: 'Kategori',
            width: 150,
            valueGetter: (_value, row) => row.kategori || '-',
        },
        {
            field: 'aciklama',
            headerName: 'Açýklama',
            flex: 1,
            minWidth: 250,
            valueGetter: (_value, row) => row.aciklama || '-',
        },
        {
            field: 'deger',
            headerName: 'Deðer',
            width: 200,
            renderCell: (params) => {
                if (editingId === params.row.id) {
                    return (
                        <TextField
                            size="small"
                            value={newDeger}
                            onChange={(e) => setNewDeger(e.target.value)}
                            InputProps={{
                                endAdornment: params.row.birim ? (
                                    <InputAdornment position="end">{params.row.birim}</InputAdornment>
                                ) : null,
                            }}
                            fullWidth
                        />
                    );
                }
                return (
                    <Typography>
                        {params.value} {params.row.birim && `(${params.row.birim})`}
                    </Typography>
                );
            },
        },
        {
            field: 'actions',
            headerName: 'Ýþlemler',
            width: 120,
            sortable: false,
            renderCell: (params) => {
                if (editingId === params.row.id) {
                    return (
                        <Box>
                            <IconButton
                                size="small"
                                color="success"
                                onClick={() => handleEditSave(params.row)}
                            >
                                <SaveIcon fontSize="small" />
                            </IconButton>
                            <IconButton size="small" color="error" onClick={handleEditCancel}>
                                <CancelIcon fontSize="small" />
                            </IconButton>
                        </Box>
                    );
                }
                return (
                    <IconButton
                        size="small"
                        color="primary"
                        onClick={() => handleEditStart(params.row)}
                    >
                        <EditIcon fontSize="small" />
                    </IconButton>
                );
            },
        },
    ];

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Box>
                    <Typography variant="h4" fontWeight="bold">
                        Sistem Parametreleri
                    </Typography>
                    <Typography variant="body2" color="text.secondary" mt={1}>
                        Sistem davranýþlarýný kontrol eden temel parametreler
                    </Typography>
                </Box>
            </Box>

            <Paper sx={{ p: 3 }}>
                <DataGrid
                    rows={parametreler}
                    columns={columns}
                    loading={loading}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 10 } },
                    }}
                    disableRowSelectionOnClick
                    autoHeight
                    sx={{
                        '& .MuiDataGrid-cell:focus': {
                            outline: 'none',
                        },
                    }}
                />
            </Paper>

            {/* Snackbar */}
            <Snackbar
                open={snackbar.open}
                autoHideDuration={4000}
                onClose={() => setSnackbar({ ...snackbar, open: false })}
            >
                <Alert severity={snackbar.severity} onClose={() => setSnackbar({ ...snackbar, open: false })}>
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </Box>
    );
}

export default ParametreList;