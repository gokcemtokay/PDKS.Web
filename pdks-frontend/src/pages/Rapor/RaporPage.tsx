import { useState, useEffect } from 'react';
import {
    Box,
    Paper,
    Typography,
    Grid,
    Card,
    CardContent,
    TextField,
    Button,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Alert,
    Tabs,
    Tab,
    Divider,
    Chip,
} from '@mui/material';
import {
    Assessment as AssessmentIcon,
    Download as DownloadIcon,
    Search as SearchIcon,
    Person as PersonIcon,
    Group as GroupIcon,
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import type { GridColDef } from '@mui/x-data-grid';
import api from '../../services/api';

interface Personel {
    id: number;
    ad: string;
    soyad: string;
}

interface RaporFiltre {
    baslangicTarihi: string;
    bitisTarihi: string;
    personelId: number | '';
    departman: string;
}

interface GirisCikisRapor {
    personelAdi: string;
    departman: string;
    tarih: string;
    girisSaati: string;
    cikisSaati: string;
    toplamSaat: string;
}

function RaporPage() {
    const [activeTab, setActiveTab] = useState(0);
    const [personeller, setPersoneller] = useState<Personel[]>([]);
    const [raporData, setRaporData] = useState<GirisCikisRapor[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const [filtre, setFiltre] = useState<RaporFiltre>({
        baslangicTarihi: new Date(new Date().setDate(1)).toISOString().split('T')[0], // Ayın ilk günü
        bitisTarihi: new Date().toISOString().split('T')[0], // Bugün
        personelId: '',
        departman: '',
    });

    useEffect(() => {
        loadPersoneller();
    }, []);

    const loadPersoneller = async () => {
        try {
            const response = await api.get('/Personel');
            setPersoneller(response.data);
        } catch (error) {
            console.error('Personeller yüklenemedi:', error);
        }
    };

    const handleFiltreChange = (field: keyof RaporFiltre, value: any) => {
        setFiltre((prev) => ({ ...prev, [field]: value }));
    };

    const handleRaporGetir = async () => {
        setError('');
        setLoading(true);

        try {
            let endpoint = '';

            if (activeTab === 0) {
                if (!filtre.personelId) {
                    setError('Lütfen personel seçiniz!');
                    setLoading(false);
                    return;
                }
                endpoint = '/Rapor/kisi-bazinda-giris-cikis';
            } else if (activeTab === 1) {
                endpoint = '/Rapor/genel-giris-cikis';
            }

            const response = await api.post(endpoint, filtre);
            setRaporData(response.data);
        } catch (err) { // 'error' yerine 'err' kullan
            const errorMessage = err instanceof Error ? err.message : 'Rapor oluşturulamadı!';
            setError(errorMessage);
            setRaporData([]);
        } finally {
            setLoading(false);
        }
    };

    const handleExcelIndir = () => {
        // Excel indirme işlemi - Backend'den Excel dosyası döndürmesi gerekir
        alert('Excel indirme özelliği backend tarafında implement edilecek');
    };

    const columns: GridColDef[] = [
        {
            field: 'personelAdi',
            headerName: 'Personel',
            width: 200,
        },
        {
            field: 'departman',
            headerName: 'Departman',
            width: 150,
        },
        {
            field: 'tarih',
            headerName: 'Tarih',
            width: 130,
            valueFormatter: (value) => {
                if (value) {
                    return new Date(value).toLocaleDateString('tr-TR');
                }
                return '';
            },
        },
        {
            field: 'girisSaati',
            headerName: 'Giriş Saati',
            width: 120,
            renderCell: (params) => <Chip label={params.value} color="success" size="small" />,
        },
        {
            field: 'cikisSaati',
            headerName: 'Çıkış Saati',
            width: 120,
            renderCell: (params) => <Chip label={params.value} color="error" size="small" />,
        },
        {
            field: 'toplamSaat',
            headerName: 'Toplam Saat',
            width: 130,
            renderCell: (params) => <Chip label={params.value} color="primary" size="small" />,
        },
    ];

    return (
        <Box>
            <Typography variant="h4" fontWeight="bold" mb={3}>
                Raporlar
            </Typography>

            <Grid container spacing={3}>
                {/* Filtre Kartı */}
                <Grid item xs={12}>
                    <Paper sx={{ p: 3 }}>
                        <Box display="flex" alignItems="center" gap={1} mb={3}>
                            <AssessmentIcon color="primary" />
                            <Typography variant="h6" fontWeight="bold">
                                Rapor Filtreleri
                            </Typography>
                        </Box>

                        <Tabs value={activeTab} onChange={(e, val) => setActiveTab(val)} sx={{ mb: 3 }}>
                            <Tab icon={<PersonIcon />} label="Kişi Bazında" />
                            <Tab icon={<GroupIcon />} label="Genel Rapor" />
                        </Tabs>

                        <Divider sx={{ mb: 3 }} />

                        <Grid container spacing={2}>
                            <Grid item xs={12} md={3}>
                                <TextField
                                    fullWidth
                                    type="date"
                                    label="Başlangıç Tarihi"
                                    value={filtre.baslangicTarihi}
                                    onChange={(e) => handleFiltreChange('baslangicTarihi', e.target.value)}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>

                            <Grid item xs={12} md={3}>
                                <TextField
                                    fullWidth
                                    type="date"
                                    label="Bitiş Tarihi"
                                    value={filtre.bitisTarihi}
                                    onChange={(e) => handleFiltreChange('bitisTarihi', e.target.value)}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>

                            {activeTab === 0 && (
                                <Grid item xs={12} md={3}>
                                    <FormControl fullWidth>
                                        <InputLabel>Personel Seçin</InputLabel>
                                        <Select
                                            value={filtre.personelId}
                                            onChange={(e) => handleFiltreChange('personelId', e.target.value)}
                                            label="Personel Seçin"
                                        >
                                            <MenuItem value="">
                                                <em>Seçiniz</em>
                                            </MenuItem>
                                            {personeller.map((p) => (
                                                <MenuItem key={p.id} value={p.id}>
                                                    {p.ad} {p.soyad}
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    </FormControl>
                                </Grid>
                            )}

                            {activeTab === 1 && (
                                <Grid item xs={12} md={3}>
                                    <TextField
                                        fullWidth
                                        label="Departman Filtrele"
                                        placeholder="Örn: IT"
                                        value={filtre.departman}
                                        onChange={(e) => handleFiltreChange('departman', e.target.value)}
                                    />
                                </Grid>
                            )}

                            <Grid item xs={12} md={3}>
                                <Button
                                    fullWidth
                                    variant="contained"
                                    startIcon={<SearchIcon />}
                                    onClick={handleRaporGetir}
                                    disabled={loading}
                                    sx={{
                                        height: '56px',
                                        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                                    }}
                                >
                                    {loading ? 'Yükleniyor...' : 'Rapor Getir'}
                                </Button>
                            </Grid>
                        </Grid>

                        {error && (
                            <Alert severity="error" sx={{ mt: 2 }}>
                                {error}
                            </Alert>
                        )}
                    </Paper>
                </Grid>

                {/* Rapor Sonuçları */}
                <Grid item xs={12}>
                    <Paper sx={{ p: 3 }}>
                        <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                            <Typography variant="h6" fontWeight="bold">
                                Rapor Sonuçları ({raporData.length} kayıt)
                            </Typography>
                            {raporData.length > 0 && (
                                <Button
                                    variant="outlined"
                                    startIcon={<DownloadIcon />}
                                    onClick={handleExcelIndir}
                                    sx={{ borderColor: '#667eea', color: '#667eea' }}
                                >
                                    Excel İndir
                                </Button>
                            )}
                        </Box>

                        <DataGrid
                            rows={raporData.map((item, index) => ({ ...item, id: index }))}
                            columns={columns}
                            loading={loading}
                            pageSizeOptions={[10, 25, 50, 100]}
                            initialState={{
                                pagination: { paginationModel: { pageSize: 25 } },
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
                </Grid>

                {/* İstatistik Kartları */}
                {raporData.length > 0 && (
                    <>
                        <Grid item xs={12} md={4}>
                            <Card sx={{ background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)', color: 'white' }}>
                                <CardContent>
                                    <Typography variant="h4" fontWeight="bold">
                                        {raporData.length}
                                    </Typography>
                                    <Typography variant="body2">Toplam Kayıt</Typography>
                                </CardContent>
                            </Card>
                        </Grid>

                        <Grid item xs={12} md={4}>
                            <Card sx={{ background: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)', color: 'white' }}>
                                <CardContent>
                                    <Typography variant="h4" fontWeight="bold">
                                        {new Set(raporData.map((r) => r.personelAdi)).size}
                                    </Typography>
                                    <Typography variant="body2">Farklı Personel</Typography>
                                </CardContent>
                            </Card>
                        </Grid>

                        <Grid item xs={12} md={4}>
                            <Card sx={{ background: 'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)', color: 'white' }}>
                                <CardContent>
                                    <Typography variant="h4" fontWeight="bold">
                                        {new Set(raporData.map((r) => r.departman)).size}
                                    </Typography>
                                    <Typography variant="body2">Farklı Departman</Typography>
                                </CardContent>
                            </Card>
                        </Grid>
                    </>
                )}
            </Grid>
        </Box>
    );
}

export default RaporPage;