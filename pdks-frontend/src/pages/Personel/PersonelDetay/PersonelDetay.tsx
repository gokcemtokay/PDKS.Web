import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box, Paper, Typography, Button, Tabs, Tab, Grid, Card, CardContent,
  Avatar, IconButton, Divider, Chip, Table, TableBody, TableCell,
  TableContainer, TableHead, TableRow, Dialog, DialogTitle, DialogContent,
  DialogActions, TextField, Alert,
} from '@mui/material';
import {
  ArrowBack, Edit, Save, Cancel, PhotoCamera, Phone, Email,
  LocationOn, Work, School, CardMembership, HealthAndSafety,
  FamilyRestroom, AccountBalance, Description, CalendarToday,
  Person, ContactPhone,
} from '@mui/icons-material';
import personelService from '../../../services/personelService';

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

function PersonelDetay() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [tabValue, setTabValue] = useState(0);
  const [personel, setPersonel] = useState<any>(null);
  const [loading, setLoading] = useState(false);
  const [photoDialog, setPhotoDialog] = useState(false);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  useEffect(() => {
    if (id) loadPersonel();
  }, [id]);

  const loadPersonel = async () => {
    if (!id) return;
    setLoading(true);
    try {
      const data = await personelService.getById(parseInt(id));
      setPersonel(data);
    } catch (error) {
      console.error('Personel yüklenemedi:', error);
    } finally {
      setLoading(false);
    }
  };

  const handlePhotoUpload = async () => {
    if (!selectedFile) return;
    // TODO: API'ye fotoğraf yükleme
    console.log('Fotoğraf yükleniyor:', selectedFile);
    setPhotoDialog(false);
  };

  if (loading) return <Typography>Yükleniyor...</Typography>;
  if (!personel) return <Typography>Personel bulunamadı</Typography>;

  return (
    <Box>
      {/* Header */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <Button startIcon={<ArrowBack />} onClick={() => navigate('/personel')} sx={{ mr: 2 }}>
            Geri
          </Button>
          <Typography variant="h4" fontWeight="bold">Personel Detayı</Typography>
        </Box>
        <Button variant="contained" startIcon={<Edit />} onClick={() => navigate(`/personel/duzenle/${id}`)}>
          Düzenle
        </Button>
      </Box>

      {/* Personel Özet Kartı */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={3} sx={{ textAlign: 'center' }}>
            <Box sx={{ position: 'relative', display: 'inline-block' }}>
              <Avatar
                              src={personel.profilResmi || ''}
                sx={{ width: 150, height: 150, mb: 2 }}
              >
                {personel.adSoyad?.[0]}
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
                size="small"
                onClick={() => setPhotoDialog(true)}
              >
                <PhotoCamera fontSize="small" />
              </IconButton>
            </Box>
            <Chip
              label={personel.durum ? 'Aktif' : 'Pasif'}
              color={personel.durum ? 'success' : 'error'}
              size="small"
            />
          </Grid>
          <Grid item xs={12} md={9}>
            <Typography variant="h5" fontWeight="bold" gutterBottom>
              {personel.adSoyad}
            </Typography>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <Person fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                  <Typography variant="body2" color="text.secondary">Sicil No:</Typography>
                  <Typography variant="body2" fontWeight="bold" sx={{ ml: 1 }}>{personel.sicilNo}</Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <Work fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                  <Typography variant="body2" color="text.secondary">Görev:</Typography>
                  <Typography variant="body2" fontWeight="bold" sx={{ ml: 1 }}>{personel.gorev}</Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <CalendarToday fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                  <Typography variant="body2" color="text.secondary">Giriş Tarihi:</Typography>
                  <Typography variant="body2" fontWeight="bold" sx={{ ml: 1 }}>
                    {personel.girisTarihi ? new Date(personel.girisTarihi).toLocaleDateString('tr-TR') : '-'}
                  </Typography>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <Email fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                  <Typography variant="body2" color="text.secondary">E-posta:</Typography>
                  <Typography variant="body2" fontWeight="bold" sx={{ ml: 1 }}>{personel.email || '-'}</Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <Phone fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                  <Typography variant="body2" color="text.secondary">Telefon:</Typography>
                  <Typography variant="body2" fontWeight="bold" sx={{ ml: 1 }}>{personel.telefon || '-'}</Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <LocationOn fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
                  <Typography variant="body2" color="text.secondary">Departman:</Typography>
                  <Typography variant="body2" fontWeight="bold" sx={{ ml: 1 }}>{personel.departmanAdi || '-'}</Typography>
                </Box>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </Paper>

      {/* Tabs */}
      <Paper sx={{ mb: 3 }}>
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
          <Tab icon={<Description />} label="Belgeler" iconPosition="start" />
        </Tabs>

        {/* Tab Panel 0: Genel Bilgiler */}
        <TabPanel value={tabValue} index={0}>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <Card variant="outlined">
                <CardContent>
                  <Typography variant="h6" gutterBottom>Kişisel Bilgiler</Typography>
                  <Divider sx={{ mb: 2 }} />
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">Ad Soyad</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.adSoyad}</Typography>
                  </Box>
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">TC Kimlik No</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.tcKimlik || '-'}</Typography>
                  </Box>
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">Doğum Tarihi</Typography>
                    <Typography variant="body1" fontWeight="bold">
                      {personel.dogumTarihi ? new Date(personel.dogumTarihi).toLocaleDateString('tr-TR') : '-'}
                    </Typography>
                  </Box>
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">Cinsiyet</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.cinsiyet || '-'}</Typography>
                  </Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary">Medeni Durum</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.medeniDurum || '-'}</Typography>
                  </Box>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} md={6}>
              <Card variant="outlined">
                <CardContent>
                  <Typography variant="h6" gutterBottom>İş Bilgileri</Typography>
                  <Divider sx={{ mb: 2 }} />
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">Sicil No</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.sicilNo}</Typography>
                  </Box>
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">Görev/Unvan</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.gorev}</Typography>
                  </Box>
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">Departman</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.departmanAdi || '-'}</Typography>
                  </Box>
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="body2" color="text.secondary">İşe Giriş Tarihi</Typography>
                    <Typography variant="body1" fontWeight="bold">
                      {personel.girisTarihi ? new Date(personel.girisTarihi).toLocaleDateString('tr-TR') : '-'}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary">Çalışma Süresi</Typography>
                    <Typography variant="body1" fontWeight="bold">
                      {personel.girisTarihi ? 
                        Math.floor((new Date().getTime() - new Date(personel.girisTarihi).getTime()) / (1000 * 60 * 60 * 24 * 365)) + ' yıl' 
                        : '-'}
                    </Typography>
                  </Box>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </TabPanel>

        {/* Tab Panel 1: İletişim */}
        <TabPanel value={tabValue} index={1}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>İletişim Bilgileri</Typography>
              <Divider sx={{ mb: 3 }} />
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <Box sx={{ mb: 3 }}>
                    <Typography variant="body2" color="text.secondary" gutterBottom>E-posta</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.email || '-'}</Typography>
                  </Box>
                  <Box sx={{ mb: 3 }}>
                    <Typography variant="body2" color="text.secondary" gutterBottom>Cep Telefonu</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.telefon || '-'}</Typography>
                  </Box>
                  <Box sx={{ mb: 3 }}>
                    <Typography variant="body2" color="text.secondary" gutterBottom>İş Telefonu</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.isTelefon || '-'}</Typography>
                  </Box>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Box sx={{ mb: 3 }}>
                    <Typography variant="body2" color="text.secondary" gutterBottom>Adres</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.adres || '-'}</Typography>
                  </Box>
                  <Box sx={{ mb: 3 }}>
                    <Typography variant="body2" color="text.secondary" gutterBottom>İl / İlçe</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.il || '-'} / {personel.ilce || '-'}</Typography>
                  </Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary" gutterBottom>Acil Durum İletişim</Typography>
                    <Typography variant="body1" fontWeight="bold">{personel.acilDurumKisi || '-'}</Typography>
                    <Typography variant="body2" color="text.secondary">{personel.acilDurumTelefon || '-'}</Typography>
                  </Box>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </TabPanel>

        {/* Tab Panel 2: Özlük */}
        <TabPanel value={tabValue} index={2}>
          <Alert severity="info" sx={{ mb: 2 }}>
            Özlük bilgileri henüz yüklenmedi. Düzenle butonuna tıklayarak bilgileri girebilirsiniz.
          </Alert>
        </TabPanel>

        {/* Tab Panel 3: Eğitim */}
        <TabPanel value={tabValue} index={3}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Eğitim Bilgileri</Typography>
            <Button variant="outlined" size="small" startIcon={<Edit />}>Yeni Ekle</Button>
          </Box>
          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell><strong>Okul Adı</strong></TableCell>
                  <TableCell><strong>Bölüm</strong></TableCell>
                  <TableCell><strong>Derece</strong></TableCell>
                  <TableCell><strong>Başlangıç</strong></TableCell>
                  <TableCell><strong>Bitiş</strong></TableCell>
                  <TableCell><strong>İşlemler</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                <TableRow>
                  <TableCell colSpan={6} align="center">
                    <Typography color="text.secondary">Henüz eğitim kaydı eklenmemiş</Typography>
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </TabPanel>

        {/* Tab Panel 4: İş Deneyimi */}
        <TabPanel value={tabValue} index={4}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">İş Deneyimi</Typography>
            <Button variant="outlined" size="small" startIcon={<Edit />}>Yeni Ekle</Button>
          </Box>
          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell><strong>Şirket</strong></TableCell>
                  <TableCell><strong>Pozisyon</strong></TableCell>
                  <TableCell><strong>Başlangıç</strong></TableCell>
                  <TableCell><strong>Bitiş</strong></TableCell>
                  <TableCell><strong>İşlemler</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                <TableRow>
                  <TableCell colSpan={5} align="center">
                    <Typography color="text.secondary">Henüz iş deneyimi kaydı eklenmemiş</Typography>
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </TabPanel>

        {/* Tab Panel 5: Sertifikalar */}
        <TabPanel value={tabValue} index={5}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Sertifikalar</Typography>
            <Button variant="outlined" size="small" startIcon={<Edit />}>Yeni Ekle</Button>
          </Box>
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <Alert severity="info">
                Henüz sertifika kaydı eklenmemiş. Yeni Ekle butonuna tıklayarak sertifika ekleyebilirsiniz.
              </Alert>
            </Grid>
          </Grid>
        </TabPanel>

        {/* Tab Panel 6: Sağlık */}
        <TabPanel value={tabValue} index={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>Sağlık Bilgileri</Typography>
              <Divider sx={{ mb: 2 }} />
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <Typography variant="body2" color="text.secondary">Kan Grubu</Typography>
                  <Typography variant="body1" fontWeight="bold">{personel.kanGrubu || '-'}</Typography>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Typography variant="body2" color="text.secondary">Kronik Hastalık</Typography>
                  <Typography variant="body1" fontWeight="bold">{personel.kronikHastalik || 'Yok'}</Typography>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </TabPanel>

        {/* Tab Panel 7: Aile */}
        <TabPanel value={tabValue} index={7}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Aile Bireyleri</Typography>
            <Button variant="outlined" size="small" startIcon={<Edit />}>Yeni Ekle</Button>
          </Box>
          <Alert severity="info">
            Henüz aile bireyi kaydı eklenmemiş.
          </Alert>
        </TabPanel>

        {/* Tab Panel 8: Banka */}
        <TabPanel value={tabValue} index={8}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>Banka Hesap Bilgileri</Typography>
              <Divider sx={{ mb: 2 }} />
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <Typography variant="body2" color="text.secondary">Banka Adı</Typography>
                  <Typography variant="body1" fontWeight="bold">{personel.bankaAdi || '-'}</Typography>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Typography variant="body2" color="text.secondary">IBAN</Typography>
                  <Typography variant="body1" fontWeight="bold">{personel.iban || '-'}</Typography>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </TabPanel>

        {/* Tab Panel 9: Belgeler */}
        <TabPanel value={tabValue} index={9}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Belgeler ve Dosyalar</Typography>
            <Button variant="outlined" size="small" startIcon={<Description />}>Dosya Yükle</Button>
          </Box>
          <Alert severity="info">
            Henüz dosya yüklenmemiş.
          </Alert>
        </TabPanel>
      </Paper>

      {/* Fotoğraf Yükleme Dialog */}
      <Dialog open={photoDialog} onClose={() => setPhotoDialog(false)} maxWidth="sm" fullWidth>
        <DialogTitle>Profil Fotoğrafı Yükle</DialogTitle>
        <DialogContent>
          <Box sx={{ textAlign: 'center', py: 3 }}>
            {selectedFile && (
              <Avatar
                src={URL.createObjectURL(selectedFile)}
                sx={{ width: 200, height: 200, mx: 'auto', mb: 2 }}
              />
            )}
            <Button variant="outlined" component="label" startIcon={<PhotoCamera />}>
              Fotoğraf Seç
              <input
                type="file"
                hidden
                accept="image/*"
                onChange={(e) => {
                  if (e.target.files && e.target.files[0]) {
                    setSelectedFile(e.target.files[0]);
                  }
                }}
              />
            </Button>
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setPhotoDialog(false)}>İptal</Button>
          <Button onClick={handlePhotoUpload} variant="contained" disabled={!selectedFile}>
            Yükle
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}

export default PersonelDetay;
