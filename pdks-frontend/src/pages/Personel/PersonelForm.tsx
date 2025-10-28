import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
    Box, Paper, Typography, Button, Tabs, Tab, Grid, TextField,
    Avatar, IconButton, Divider, FormControl, InputLabel, Select,
    MenuItem, Card, CardContent, Table, TableBody, TableCell,
    TableContainer, TableHead, TableRow, Dialog, DialogTitle,
    DialogContent, DialogActions, Alert,
} from '@mui/material';
import {
    ArrowBack, Save, PhotoCamera, Add, Delete, Edit,
    Person, ContactPhone, Description, School, Work,
    CardMembership, HealthAndSafety, FamilyRestroom,
    AccountBalance, Upload,
} from '@mui/icons-material';
import personelService from '../../services/personelService';

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;
    return (
        <div hidden={value !== index} {...other}>
            {value === index && <Box sx={{ py: 3 }}>{children}</Box>}
        </div>
    );
}

function PersonelForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [tabValue, setTabValue] = useState(0);
    const [loading, setLoading] = useState(false);
    const [photoPreview, setPhotoPreview] = useState('');
    const [photoFile, setPhotoFile] = useState<File | null>(null);

    // Form States
    const [formData, setFormData] = useState({
        sicilNo: '',
        adSoyad: '',
        tcKimlikNo: '', // ✅ Backend TcKimlikNo bekliyor
        dogumTarihi: '',
        cinsiyet: '',
        medeniDurum: '', // UI için, backend'e gönderilmeyecek
        departmanId: '',
        gorev: '',
        unvan: '', // ✅ Backend Unvan bekliyor
        girisTarihi: '',
        durum: true,
        email: '',
        telefon: '',
        adres: '',
        kanGrubu: '',
        maas: null as number | null,
        avansLimiti: null as number | null,
        notlar: '',
        profilResmi: '', // ✅ Profil resmi URL'si
        // UI-only alanlar (backend'e gitmeyecek)
        isTelefon: '',
        il: '',
        ilce: '',
        acilDurumKisi: '',
        acilDurumTelefon: '',
        kronikHastalik: '',
        bankaAdi: '',
        iban: '',
    });

    const [egitimler, setEgitimler] = useState<any[]>([]);
    const [deneyimler, setDeneyimler] = useState<any[]>([]);
    const [sertifikalar, setSertifikalar] = useState<any[]>([]);
    const [aileBireyleri, setAileBireyleri] = useState<any[]>([]);

    useEffect(() => {
        if (id) loadPersonel();
    }, [id]);

    // Cinsiyet normalize fonksiyonu
    const normalizeCinsiyet = (cinsiyet: string): string => {
        if (!cinsiyet) return '';
        const lowerCase = cinsiyet.toLowerCase();
        if (lowerCase === 'kadın' || lowerCase === 'kadin') return 'Kadın';
        if (lowerCase === 'erkek') return 'Erkek';
        return cinsiyet;
    };

    const loadPersonel = async () => {
        if (!id) return;
        setLoading(true);
        try {
            const data = await personelService.getById(parseInt(id));

            // ✅ Medeni durumu localStorage'dan oku (geçici çözüm)
            const savedMedeniDurum = localStorage.getItem(`personel_${id}_medeniDurum`) || '';

            setFormData({
                sicilNo: data.sicilNo || '',
                adSoyad: data.adSoyad || '',
                tcKimlikNo: data.tcKimlikNo || '',
                dogumTarihi: data.dogumTarihi ? data.dogumTarihi.split('T')[0] : '',
                cinsiyet: normalizeCinsiyet(data.cinsiyet || ''),
                medeniDurum: savedMedeniDurum, // ✅ localStorage'dan
                departmanId: data.departmanId?.toString() || '',
                gorev: data.gorev || '',
                unvan: data.unvan || data.gorev || '',
                girisTarihi: data.girisTarihi ? data.girisTarihi.split('T')[0] : '',
                durum: data.durum ?? true,
                email: data.email || '',
                telefon: data.telefon || '',
                adres: data.adres || '',
                kanGrubu: data.kanGrubu || '',
                maas: data.maas || null,
                avansLimiti: data.avansLimiti || null,
                notlar: data.notlar || '',
                profilResmi: data.profilResmi || '', // ✅ Eklendi
                // UI-only alanlar
                isTelefon: data.telefon || '',
                il: '',
                ilce: '',
                acilDurumKisi: '',
                acilDurumTelefon: '',
                kronikHastalik: '',
                bankaAdi: '',
                iban: ''
            });

            // ✅ ProfilResmi'yi set et
            if (data.profilResmi) {
                console.log('Loading photo URL:', data.profilResmi);
                // Eğer relative path ise, mevcut origin'i kullan
                const photoUrl = data.profilResmi.startsWith('http')
                    ? data.profilResmi
                    : `${window.location.origin}${data.profilResmi.startsWith('/') ? '' : '/'}${data.profilResmi}`;
                console.log('Photo preview URL:', photoUrl);
                setPhotoPreview(photoUrl);
            } else {
                console.log('No profilResmi in response');
            }
        } catch (error) {
            console.error('Personel yüklenemedi:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (field: string, value: any) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    const handlePhotoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const file = e.target.files[0];
            setPhotoFile(file);
            setPhotoPreview(URL.createObjectURL(file));
        }
    };

    const handleSubmit = async () => {
        setLoading(true);
        try {
            let uploadedPhotoUrl = formData.profilResmi;

            // ✅ 1. Eğer yeni fotoğraf seçildiyse önce onu yükle
            if (photoFile && id) {
                try {
                    uploadedPhotoUrl = await personelService.uploadPhoto(parseInt(id), photoFile);
                    console.log('Fotoğraf yüklendi:', uploadedPhotoUrl);
                } catch (photoError) {
                    console.error('Fotoğraf yükleme hatası:', photoError);
                    // Fotoğraf yükleme hatası olsa bile devam et
                }
            }

            // ✅ 2. Personel bilgilerini güncelle
            const submitData = {
                ...formData,
                profilResmi: uploadedPhotoUrl,
                profilFoto: uploadedPhotoUrl
            };

            if (id) {
                await personelService.update(parseInt(id), submitData);
            } else {
                const newId = await personelService.create(submitData);
                // Yeni personel oluşturulmuşsa ve fotoğraf varsa, fotoğrafı yükle
                if (photoFile && newId) {
                    try {
                        await personelService.uploadPhoto(newId, photoFile);
                    } catch (photoError) {
                        console.error('Yeni personel fotoğraf yükleme hatası:', photoError);
                    }
                }
            }

            navigate('/personel');
        } catch (error) {
            console.error('Kayıt başarısız:', error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box>
            {/* Header */}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Button startIcon={<ArrowBack />} onClick={() => navigate('/personel')} sx={{ mr: 2 }}>
                        Geri
                    </Button>
                    <Typography variant="h4" fontWeight="bold">
                        {id ? 'Personel Düzenle' : 'Yeni Personel'}
                    </Typography>
                </Box>
                <Button
                    variant="contained"
                    startIcon={<Save />}
                    onClick={handleSubmit}
                    disabled={loading}
                >
                    Kaydet
                </Button>
            </Box>

            {/* Fotoğraf Bölümü */}
            <Paper sx={{ p: 3, mb: 3, textAlign: 'center' }}>
                <Box sx={{ position: 'relative', display: 'inline-block' }}>
                    <Avatar
                        src={photoPreview}
                        sx={{ width: 150, height: 150, mb: 2 }}
                    >
                        {formData.adSoyad?.[0]}
                    </Avatar>
                    <IconButton
                        color="primary"
                        component="label"
                        sx={{
                            position: 'absolute',
                            bottom: 10,
                            right: 10,
                            bgcolor: 'background.paper',
                            '&:hover': { bgcolor: 'background.default' },
                        }}
                    >
                        <PhotoCamera />
                        <input hidden accept="image/*" type="file" onChange={handlePhotoChange} />
                    </IconButton>
                </Box>
            </Paper>

            {/* Tabs */}
            <Paper sx={{ mb: 3 }}>
                <Tabs value={tabValue} onChange={(e, v) => setTabValue(v)} variant="scrollable" scrollButtons="auto">
                    <Tab label="Genel Bilgiler" icon={<Person />} iconPosition="start" />
                    <Tab label="İletişim" icon={<ContactPhone />} iconPosition="start" />
                    <Tab label="Eğitim" icon={<School />} iconPosition="start" />
                    <Tab label="Deneyim" icon={<Work />} iconPosition="start" />
                    <Tab label="Sertifikalar" icon={<CardMembership />} iconPosition="start" />
                    <Tab label="Sağlık" icon={<HealthAndSafety />} iconPosition="start" />
                    <Tab label="Aile Bilgileri" icon={<FamilyRestroom />} iconPosition="start" />
                    <Tab label="Banka" icon={<AccountBalance />} iconPosition="start" />
                    <Tab label="Belgeler" icon={<Description />} iconPosition="start" />
                </Tabs>
            </Paper>

            {/* Tab Panels */}
            <Paper sx={{ p: 3 }}>
                {/* Genel Bilgiler */}
                <TabPanel value={tabValue} index={0}>
                    <Grid container spacing={3}>
                        <Grid item xs={12} md={6}>
                            <Card variant="outlined">
                                <CardContent>
                                    <Typography variant="h6" gutterBottom>Kişisel Bilgiler</Typography>
                                    <Divider sx={{ mb: 2 }} />
                                    <TextField
                                        fullWidth
                                        label="Ad Soyad"
                                        value={formData.adSoyad}
                                        onChange={(e) => handleChange('adSoyad', e.target.value)}
                                        margin="normal"
                                        required
                                    />
                                    <TextField
                                        fullWidth
                                        label="TC Kimlik No"
                                        value={formData.tcKimlikNo}
                                        onChange={(e) => handleChange('tcKimlikNo', e.target.value)}
                                        margin="normal"
                                        required
                                        inputProps={{ maxLength: 11 }}
                                    />
                                    <TextField
                                        fullWidth
                                        label="Doğum Tarihi"
                                        type="date"
                                        value={formData.dogumTarihi}
                                        onChange={(e) => handleChange('dogumTarihi', e.target.value)}
                                        margin="normal"
                                        InputLabelProps={{ shrink: true }}
                                    />
                                    <FormControl fullWidth margin="normal">
                                        <InputLabel>Cinsiyet</InputLabel>
                                        <Select
                                            value={formData.cinsiyet}
                                            onChange={(e) => handleChange('cinsiyet', e.target.value)}
                                            label="Cinsiyet"
                                        >
                                            <MenuItem value="Erkek">Erkek</MenuItem>
                                            <MenuItem value="Kadın">Kadın</MenuItem>
                                        </Select>
                                    </FormControl>
                                    <FormControl fullWidth margin="normal">
                                        <InputLabel>Medeni Durum</InputLabel>
                                        <Select
                                            value={formData.medeniDurum}
                                            onChange={(e) => handleChange('medeniDurum', e.target.value)}
                                            label="Medeni Durum"
                                        >
                                            <MenuItem value="Bekar">Bekar</MenuItem>
                                            <MenuItem value="Evli">Evli</MenuItem>
                                        </Select>
                                    </FormControl>
                                </CardContent>
                            </Card>
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <Card variant="outlined">
                                <CardContent>
                                    <Typography variant="h6" gutterBottom>İş Bilgileri</Typography>
                                    <Divider sx={{ mb: 2 }} />
                                    <TextField
                                        fullWidth
                                        label="Sicil No"
                                        value={formData.sicilNo}
                                        onChange={(e) => handleChange('sicilNo', e.target.value)}
                                        margin="normal"
                                        required
                                    />
                                    <FormControl fullWidth margin="normal">
                                        <InputLabel>Departman</InputLabel>
                                        <Select
                                            value={formData.departmanId}
                                            onChange={(e) => handleChange('departmanId', e.target.value)}
                                            label="Departman"
                                        >
                                            <MenuItem value="">Seçiniz</MenuItem>
                                            <MenuItem value="1">IT</MenuItem>
                                            <MenuItem value="2">İnsan Kaynakları</MenuItem>
                                            <MenuItem value="3">Muhasebe</MenuItem>
                                            <MenuItem value="4">Satış</MenuItem>
                                        </Select>
                                    </FormControl>
                                    <TextField
                                        fullWidth
                                        label="Görev/Unvan"
                                        value={formData.gorev}
                                        onChange={(e) => {
                                            const value = e.target.value;
                                            setFormData(prev => ({ ...prev, gorev: value, unvan: value }));
                                        }}
                                        margin="normal"
                                        required
                                        helperText="Görev ve Unvan aynı anda güncellenir"
                                    />
                                    <TextField
                                        fullWidth
                                        label="İşe Giriş Tarihi"
                                        type="date"
                                        value={formData.girisTarihi}
                                        onChange={(e) => handleChange('girisTarihi', e.target.value)}
                                        margin="normal"
                                        InputLabelProps={{ shrink: true }}
                                    />
                                    <FormControl fullWidth margin="normal">
                                        <InputLabel>Durum</InputLabel>
                                        <Select
                                            value={formData.durum ? 'aktif' : 'pasif'}
                                            onChange={(e) => handleChange('durum', e.target.value === 'aktif')}
                                            label="Durum"
                                        >
                                            <MenuItem value="aktif">Aktif</MenuItem>
                                            <MenuItem value="pasif">Pasif</MenuItem>
                                        </Select>
                                    </FormControl>
                                </CardContent>
                            </Card>
                        </Grid>
                    </Grid>
                </TabPanel>

                {/* İletişim */}
                <TabPanel value={tabValue} index={1}>
                    <Grid container spacing={3}>
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="E-posta"
                                type="email"
                                value={formData.email}
                                onChange={(e) => handleChange('email', e.target.value)}
                                margin="normal"
                                required
                            />
                            <TextField
                                fullWidth
                                label="Telefon"
                                value={formData.telefon}
                                onChange={(e) => handleChange('telefon', e.target.value)}
                                margin="normal"
                            />
                            <TextField
                                fullWidth
                                label="İş Telefonu"
                                value={formData.isTelefon}
                                onChange={(e) => handleChange('isTelefon', e.target.value)}
                                margin="normal"
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField
                                fullWidth
                                label="İl"
                                value={formData.il}
                                onChange={(e) => handleChange('il', e.target.value)}
                                margin="normal"
                            />
                            <TextField
                                fullWidth
                                label="İlçe"
                                value={formData.ilce}
                                onChange={(e) => handleChange('ilce', e.target.value)}
                                margin="normal"
                            />
                            <TextField
                                fullWidth
                                label="Adres"
                                multiline
                                rows={3}
                                value={formData.adres}
                                onChange={(e) => handleChange('adres', e.target.value)}
                                margin="normal"
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Divider sx={{ my: 2 }} />
                            <Typography variant="h6" gutterBottom>Acil Durum İletişim</Typography>
                            <Grid container spacing={2}>
                                <Grid item xs={12} md={6}>
                                    <TextField
                                        fullWidth
                                        label="Acil Durum Kişi"
                                        value={formData.acilDurumKisi}
                                        onChange={(e) => handleChange('acilDurumKisi', e.target.value)}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6}>
                                    <TextField
                                        fullWidth
                                        label="Acil Durum Telefon"
                                        value={formData.acilDurumTelefon}
                                        onChange={(e) => handleChange('acilDurumTelefon', e.target.value)}
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                </TabPanel>

                {/* Diğer Tab'lar için placeholder */}
                {[2, 3, 4, 5, 6, 7, 8].map((index) => (
                    <TabPanel key={index} value={tabValue} index={index}>
                        <Alert severity="info">Bu sekme içeriği geliştirilme aşamasındadır.</Alert>
                    </TabPanel>
                ))}
            </Paper>
        </Box>
    );
}

export default PersonelForm;
