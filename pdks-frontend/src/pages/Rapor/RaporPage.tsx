import { useState } from 'react';
import { Box, Typography, Paper, Button, Grid, TextField } from '@mui/material';

import { Download as DownloadIcon, Search as SearchIcon } from '@mui/icons-material';
import { data } from 'react-router-dom';

function RaporPage() {
  //const [baslangic, setBaslangic] = useState<Date | null>(null);
  //const [bitis, setBitis] = useState<Date | null>(null);
    const [formData, setFormData] = useState({
        baslangic: '',
        bitis: ''
    });
  return (
    <Box>
      <Typography variant="h4" fontWeight="bold" sx={{ mb: 3 }}>
        Raporlar
      </Typography>

      <Paper sx={{ p: 3, mb: 3 }}>
              <Grid container spacing={2} alignItems="center">

          <Grid item xs={12} md={3}>
                      <TextField
                          label="Başlangıç Tarihi"
                          type="date"
                          value={formData.baslangic}
                          onChange={(e) => setFormData({ ...formData, baslangic: e.target.value })}
                          InputLabelProps={{ shrink: true }}
            />
          </Grid>
          <Grid item xs={12} md={3}>
                      <TextField
                          label="Bitiş Tarihi"
                          type="date"
                          value={formData.bitis}
                          onChange={(e) => setFormData({ ...formData, bitis: e.target.value })}
                          InputLabelProps={{ shrink: true }}
            />
          </Grid>
          <Grid item xs={12} md={3}>
            <Button fullWidth variant="contained" startIcon={<SearchIcon />} sx={{ height: 56 }}>
              Rapor Oluştur
            </Button>
          </Grid>
          <Grid item xs={12} md={3}>
            <Button fullWidth variant="outlined" startIcon={<DownloadIcon />} sx={{ height: 56 }}>
              Excel İndir
            </Button>
          </Grid>
        </Grid>
      </Paper>

      <Grid container spacing={3}>
        {['Giriş-Çıkış Raporu', 'Geç Kalanlar Raporu', 'İzin Raporu', 'Aylık Puantaj'].map((rapor) => (
          <Grid item xs={12} md={6} key={rapor}>
            <Paper sx={{ p: 3, cursor: 'pointer', '&:hover': { bgcolor: 'action.hover' } }}>
              <Typography variant="h6">{rapor}</Typography>
              <Typography color="text.secondary" variant="body2" sx={{ mt: 1 }}>
                Detaylı {rapor.toLowerCase()} oluşturun
              </Typography>
            </Paper>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
}

export default RaporPage;
