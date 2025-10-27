import { useState, useEffect } from 'react';
import { Box, Typography, Paper, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Chip } from '@mui/material';
import { Add as AddIcon, Refresh as RefreshIcon } from '@mui/icons-material';

interface GirisCikis {
    id: number;
    personelAdSoyad: string;
    tarih: string;
    girisZamani: string;
    cikisZamani: string;
    toplamSure: string;
    durum: string;
}

function GirisCikisList() {
    const [girisCikislar, setGirisCikislar] = useState<GirisCikis[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        setLoading(true);
        try {
            // TODO: API çağrısı yapılacak
            // const data = await girisCikisService.getAll();
            // setGirisCikislar(data);
        } catch (error) {
            console.error('Veriler yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const getDurumColor = (durum: string) => {
        switch (durum.toLowerCase()) {
            case 'normal': return 'success';
            case 'geç': return 'warning';
            case 'eksik': return 'error';
            default: return 'default';
        }
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">
                    Giriş-Çıkış Bilgileri
                </Typography>
                <Button
                    variant="outlined"
                    startIcon={<RefreshIcon />}
                    onClick={loadData}
                >
                    Yenile
                </Button>
            </Box>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell><strong>Personel</strong></TableCell>
                            <TableCell><strong>Tarih</strong></TableCell>
                            <TableCell><strong>Giriş</strong></TableCell>
                            <TableCell><strong>Çıkış</strong></TableCell>
                            <TableCell><strong>Toplam Süre</strong></TableCell>
                            <TableCell><strong>Durum</strong></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {girisCikislar.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={6} align="center">
                                    <Typography color="text.secondary" sx={{ py: 3 }}>
                                        Henüz giriş-çıkış kaydı bulunmamaktadır.
                                    </Typography>
                                </TableCell>
                            </TableRow>
                        ) : (
                            girisCikislar.map((gc) => (
                                <TableRow key={gc.id} hover>
                                    <TableCell>{gc.personelAdSoyad}</TableCell>
                                    <TableCell>{gc.tarih}</TableCell>
                                    <TableCell>{gc.girisZamani}</TableCell>
                                    <TableCell>{gc.cikisZamani}</TableCell>
                                    <TableCell>{gc.toplamSure}</TableCell>
                                    <TableCell>
                                        <Chip label={gc.durum} color={getDurumColor(gc.durum)} size="small" />
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
}

export default GirisCikisList;
