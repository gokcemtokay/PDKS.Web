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
    // Genel Bilgiler
    sicilNo: '',
    adSoyad: '',
    tcKimlik: '',
    dogumTarihi: '',
    cinsiyet: '',
    medeniDurum: '',
    departmanId: '',
    gorev: '',
    girisTarihi: '',
    durum: true,
    
    // İletişim
    email: '',
    telefon: '',
    isTelefon: '',
    adres: '',
    il: '',
    ilce: '',
    acilDurumKisi: '',
    acilDurumTelefon: '',
    
    // Sağlık
    kanGrubu: '',
    kronikHastalik: '',
    
    // Banka
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

  const loadPersonel = async () => {
    if (!id) return;
    setLoading(true);
    try {
      const data = await personelService.getById(parseInt(id));
      setFormData({
        sicilNo: data.sicilNo || '',
        adSoyad: data.adSoyad || '',
        tcKimlik: data.tcKimlik || '',
        dogumTarihi: data.dogumTarihi ? data.dogumTarihi.split('T')[0] : '',
        cinsiyet: data.cinsiyet || '',
        medeniDurum: data.medeniDurum || '',
        departmanId: data.departmanId || '',
        gorev: data.gorev || '',
        girisTarihi: data.girisTarihi ? data.girisTarihi.split('T')[0] : '',
        durum: data.durum !== false,
        email: data.email || '',
        telefon: data.telefon || '',
        isTelefon: data.isTelefon || '',
        adres: data.adres || '',
        il: data.il || '',
        ilce: data.ilce || '',
        acilDurumKisi: data.acilDurumKisi || '',
        acilDurumTelefon: data.acilDurumTelefon || '',
        kanGrubu: data.kanGrubu || '',
        kronikHastalik: data.kronikHastalik || '',
        bankaAdi: data.bankaAdi || '',
        iban: data.iban || '',
      });
      if (data.profilFoto) setPhotoPreview(data.profilFoto);
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
      if (id) {
        await personelService.update(parseInt(id), formData);
      } else {
        await personelService.create(formData);
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
            sx={{
              position: 'absolute',
              bottom: 10,
              right: 10,
              backgroundColor: 'primary.main',
              color: 'white',
              '&:hover': { backgroundColor: 'primary.dark' },
            }}
            component="label"
          >
            <PhotoCamera />
            <input
              type="file"
              hidden
              accept="image/*"
              onChange={handlePhotoChange}
            />
          </IconButton>
        </Box>
        <Typography variant="caption" color="text.secondary" display="block">
          Profil fotoğrafı yüklemek için kamera ikonuna tıklayın
        </Typography>
      </Paper>

      {/* Tabs */}
      <Paper>
        <Tabs
          value={tabValue}
          onChange={(_, v) => setTabValue(v)}
          variant="scrollable"
          scrollButtons="auto"
          sx={{ borderBottom: 1, borderColor: 'divider' }}
        >
          <Tab icon={<Person />} label="Genel Bilgiler" iconPosition="start" />
          <Tab icon={<ContactPhone />} label="İletişim" iconPosition="start" />
          <Tab icon={<Description />} label="Özlük" iconPosition="start" />
          <Tab icon={<School />} label="Eğitim" iconPosition="start" />
          <Tab icon={<Work />} label="İş Deneyimi" iconPosition="start" />
          <Tab icon={<CardMembership />} label="Sertifikalar" iconPosition="start" />
          <Tab icon={<HealthAndSafety />} label="Sağlık" iconPosition="start" />
          <Tab icon={<FamilyRestroom />} label="Aile" iconPosition="start" />
          <Tab icon={<AccountBalance />} label="Banka" iconPosition="start" />
        </Tabs>

        {/* Tab 0: Genel Bilgiler */}
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
                    value={formData.tcKimlik}
                    onChange={(e) => handleChange('tcKimlik', e.target.value)}
                    margin="normal"
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
                    onChange={(e) => handleChange('gorev', e.target.value)}
                    margin="normal"
                    required
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
                      value={formData.durum ? 'true' : 'false'}
                      onChange={(e) => handleChange('durum', e.target.value === 'true')}
                      label="Durum"
                    >
                      <MenuItem value="true">Aktif</MenuItem>
                      <MenuItem value="false">Pasif</MenuItem>
                    </Select>
                  </FormControl>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </TabPanel>

        {/* Tab 1: İletişim */}
        <TabPanel value={tabValue} index={1}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>İletişim Bilgileri</Typography>
              <Divider sx={{ mb: 2 }} />
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <TextField
                    fullWidth
                    label="E-posta"
                    type="email"
                    value={formData.email}
                    onChange={(e) => handleChange('email', e.target.value)}
                    margin="normal"
                  />
                  <TextField
                    fullWidth
                    label="Cep Telefonu"
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
                    label="Adres"
                    multiline
                    rows={3}
                    value={formData.adres}
                    onChange={(e) => handleChange('adres', e.target.value)}
                    margin="normal"
                  />
                  <Grid container spacing={2}>
                    <Grid item xs={6}>
                      <TextField
                        fullWidth
                        label="İl"
                        value={formData.il}
                        onChange={(e) => handleChange('il', e.target.value)}
                        margin="normal"
                      />
                    </Grid>
                    <Grid item xs={6}>
                      <TextField
                        fullWidth
                        label="İlçe"
                        value={formData.ilce}
                        onChange={(e) => handleChange('ilce', e.target.value)}
                        margin="normal"
                      />
                    </Grid>
                  </Grid>
                </Grid>
                <Grid item xs={12}>
                  <Typography variant="subtitle1" fontWeight="bold" sx={{ mt: 2, mb: 1 }}>
                    Acil Durum İletişim
                  </Typography>
                  <Grid container spacing={2}>
                    <Grid item xs={12} md={6}>
                      <TextField
                        fullWidth
                        label="Kişi Adı"
                        value={formData.acilDurumKisi}
                        onChange={(e) => handleChange('acilDurumKisi', e.target.value)}
                      />
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <TextField
                        fullWidth
                        label="Telefon"
                        value={formData.acilDurumTelefon}
                        onChange={(e) => handleChange('acilDurumTelefon', e.target.value)}
                      />
                    </Grid>
                  </Grid>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </TabPanel>

        {/* Tab 2: Özlük */}
        <TabPanel value={tabValue} index={2}>
          <Alert severity="info">
            Özlük bilgileri için formlar yakında eklenecek
          </Alert>
        </TabPanel>

        {/* Tab 3: Eğitim */}
        <TabPanel value={tabValue} index={3}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Eğitim Bilgileri</Typography>
            <Button variant="outlined" size="small" startIcon={<Add />}>
              Yeni Ekle
            </Button>
          </Box>
          <TableContainer component={Paper} variant="outlined">
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Okul Adı</TableCell>
                  <TableCell>Bölüm</TableCell>
                  <TableCell>Derece</TableCell>
                  <TableCell>Başlangıç</TableCell>
                  <TableCell>Bitiş</TableCell>
                  <TableCell>İşlemler</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                <TableRow>
                  <TableCell colSpan={6} align="center">
                    Henüz eğitim kaydı eklenmemiş
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </TabPanel>

        {/* Tab 4: İş Deneyimi */}
        <TabPanel value={tabValue} index={4}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">İş Deneyimi</Typography>
            <Button variant="outlined" size="small" startIcon={<Add />}>
              Yeni Ekle
            </Button>
          </Box>
          <TableContainer component={Paper} variant="outlined">
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Şirket</TableCell>
                  <TableCell>Pozisyon</TableCell>
                  <TableCell>Başlangıç</TableCell>
                  <TableCell>Bitiş</TableCell>
                  <TableCell>İşlemler</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                <TableRow>
                  <TableCell colSpan={5} align="center">
                    Henüz iş deneyimi kaydı eklenmemiş
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </TabPanel>

        {/* Tab 5: Sertifikalar */}
        <TabPanel value={tabValue} index={5}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Sertifikalar</Typography>
            <Button variant="outlined" size="small" startIcon={<Add />}>
              Yeni Ekle
            </Button>
          </Box>
          <Alert severity="info">
            Sertifika listesi buraya gelecek
          </Alert>
        </TabPanel>

        {/* Tab 6: Sağlık */}
        <TabPanel value={tabValue} index={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>Sağlık Bilgileri</Typography>
              <Divider sx={{ mb: 2 }} />
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth>
                    <InputLabel>Kan Grubu</InputLabel>
                    <Select
                      value={formData.kanGrubu}
                      onChange={(e) => handleChange('kanGrubu', e.target.value)}
                      label="Kan Grubu"
                    >
                      <MenuItem value="">Seçiniz</MenuItem>
                      <MenuItem value="A+">A Rh+</MenuItem>
                      <MenuItem value="A-">A Rh-</MenuItem>
                      <MenuItem value="B+">B Rh+</MenuItem>
                      <MenuItem value="B-">B Rh-</MenuItem>
                      <MenuItem value="AB+">AB Rh+</MenuItem>
                      <MenuItem value="AB-">AB Rh-</MenuItem>
                      <MenuItem value="0+">0 Rh+</MenuItem>
                      <MenuItem value="0-">0 Rh-</MenuItem>
                    </Select>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <TextField
                    fullWidth
                    label="Kronik Hastalık"
                    value={formData.kronikHastalik}
                    onChange={(e) => handleChange('kronikHastalik', e.target.value)}
                    placeholder="Varsa belirtiniz"
                  />
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </TabPanel>

        {/* Tab 7: Aile */}
        <TabPanel value={tabValue} index={7}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Aile Bireyleri</Typography>
            <Button variant="outlined" size="small" startIcon={<Add />}>
              Yeni Ekle
            </Button>
          </Box>
          <Alert severity="info">
            Aile bireyleri listesi buraya gelecek
          </Alert>
        </TabPanel>

        {/* Tab 8: Banka */}
        <TabPanel value={tabValue} index={8}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>Banka Hesap Bilgileri</Typography>
              <Divider sx={{ mb: 2 }} />
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <TextField
                    fullWidth
                    label="Banka Adı"
                    value={formData.bankaAdi}
                    onChange={(e) => handleChange('bankaAdi', e.target.value)}
                  />
                </Grid>
                <Grid item xs={12} md={6}>
                  <TextField
                    fullWidth
                    label="IBAN"
                    value={formData.iban}
                    onChange={(e) => handleChange('iban', e.target.value)}
                    placeholder="TR00 0000 0000 0000 0000 0000 00"
                  />
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </TabPanel>
      </Paper>

      {/* Kaydet Butonu (Alt) */}
      <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
        <Button
          variant="outlined"
          onClick={() => navigate('/personel')}
        >
          İptal
        </Button>
        <Button
          variant="contained"
          startIcon={<Save />}
          onClick={handleSubmit}
          disabled={loading}
        >
          Kaydet
        </Button>
      </Box>
    </Box>
  );
}

export default PersonelForm;
