import { useState, useEffect } from 'react';
import { 
    Box, Typography, Paper, Table, TableBody, TableCell, TableContainer, 
    TableHead, TableRow, Button, TextField, InputAdornment, IconButton 
} from '@mui/material';
import { 
    Search as SearchIcon, Add as AddIcon, Edit as EditIcon, 
    Visibility as VisibilityIcon 
} from '@mui/icons-material';

interface PersonelOzlukBilgi {
    id: number;
    personelAdSoyad: string;
    tcKimlik: string;
    dogumTarihi: string;
    dogumYeri: string;
    medeniDurum: string;
    cinsiyet: string;
    kanGrubu: string;
    uyruk: string;
}

function PersonelOzluk() {
    const [ozlukBilgileri, setOzlukBilgileri] = useState<PersonelOzlukBilgi[]>([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        setLoading(true);
        try {
            // TODO: API çağrısı yapılacak
            // const data = await personelOzlukService.getAll();
            // setOzlukBilgileri(data);
        } catch (error) {
            console.error('Veriler yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const filteredData = ozlukBilgileri.filter(ozluk =>
        ozluk.personelAdSoyad.toLowerCase().includes(searchTerm.toLowerCase()) ||
        ozluk.tcKimlik.includes(searchTerm)
    );

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">
                    Personel Özlük Bilgileri
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => {/* Yeni kayıt modal aç */}}
                >
                    Yeni Kayıt
                </Button>
            </Box>

            <Paper sx={{ p: 2, mb: 3 }}>
                <TextField
                    fullWidth
                    placeholder="Personel adı veya TC Kimlik ile ara..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    InputProps={{
                        startAdornment: (
                            <InputAdornment position="start">
                                <SearchIcon />
                            </InputAdornment>
                        ),
                    }}
                />
            </Paper>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell><strong>Personel</strong></TableCell>
                            <TableCell><strong>TC Kimlik</strong></TableCell>
                            <TableCell><strong>Doğum Tarihi</strong></TableCell>
                            <TableCell><strong>Doğum Yeri</strong></TableCell>
                            <TableCell><strong>Medeni Durum</strong></TableCell>
                            <TableCell><strong>Cinsiyet</strong></TableCell>
                            <TableCell><strong>Kan Grubu</strong></TableCell>
                            <TableCell align="center"><strong>İşlemler</strong></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {filteredData.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={8} align="center">
                                    <Typography color="text.secondary" sx={{ py: 3 }}>
                                        Henüz özlük bilgisi bulunmamaktadır.
                                    </Typography>
                                </TableCell>
                            </TableRow>
                        ) : (
                            filteredData.map((ozluk) => (
                                <TableRow key={ozluk.id} hover>
                                    <TableCell>{ozluk.personelAdSoyad}</TableCell>
                                    <TableCell>{ozluk.tcKimlik}</TableCell>
                                    <TableCell>{ozluk.dogumTarihi}</TableCell>
                                    <TableCell>{ozluk.dogumYeri}</TableCell>
                                    <TableCell>{ozluk.medeniDurum}</TableCell>
                                    <TableCell>{ozluk.cinsiyet}</TableCell>
                                    <TableCell>{ozluk.kanGrubu}</TableCell>
                                    <TableCell align="center">
                                        <IconButton size="small" color="primary">
                                            <VisibilityIcon />
                                        </IconButton>
                                        <IconButton size="small" color="secondary">
                                            <EditIcon />
                                        </IconButton>
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

export default PersonelOzluk;
