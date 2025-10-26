import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Box, Paper, Typography, Button, Tabs, Tab, Grid, Card, CardContent } from '@mui/material';
import { ArrowBack as ArrowBackIcon, Edit as EditIcon } from '@mui/icons-material';
import personelService, { PersonelDetay as PersonelDetayType } from '../../../services/personelService';

function PersonelDetay() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [personel, setPersonel] = useState<PersonelDetayType | null>(null);
  const [tabValue, setTabValue] = useState(0);

  useEffect(() => {
    if (id) loadPersonel();
  }, [id]);

  const loadPersonel = async () => {
    if (!id) return;
    try {
      const data = await personelService.getById(parseInt(id));
      setPersonel(data);
    } catch (error) {
      console.error('Personel yüklenemedi:', error);
    }
  };

  if (!personel) return <Typography>Yükleniyor...</Typography>;

  return (
    <Box>
      <Box sx={{ display: 'flex', alignItems: 'center', mb: 3, justifyContent: 'space-between' }}>
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/personel')} sx={{ mr: 2 }}>
            Geri
          </Button>
          <Typography variant="h4" fontWeight="bold">{personel.adSoyad}</Typography>
        </Box>
        <Button variant="contained" startIcon={<EditIcon />} onClick={() => navigate(`/personel/duzenle/${id}`)}>
          Düzenle
        </Button>
      </Box>

      <Paper sx={{ mb: 3 }}>
        <Tabs value={tabValue} onChange={(_, v) => setTabValue(v)}>
          <Tab label="Genel Bilgiler" />
          <Tab label="İletişim" />
          <Tab label="İş Bilgileri" />
        </Tabs>
      </Paper>

      {tabValue === 0 && (
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="subtitle2" color="text.secondary">Sicil No</Typography>
                <Typography variant="h6">{personel.sicilNo}</Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="subtitle2" color="text.secondary">TC Kimlik</Typography>
                <Typography variant="h6">{personel.tcKimlik || '-'}</Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      )}
    </Box>
  );
}

export default PersonelDetay;
