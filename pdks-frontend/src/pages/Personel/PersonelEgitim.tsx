import { Box, Typography, Paper, Button } from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';

function PersonelEgitim() {
    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">
                    Personel Eğitim Bilgileri
                </Typography>
                <Button variant="contained" startIcon={<AddIcon />}>
                    Yeni Eğitim Ekle
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <Typography variant="h6" gutterBottom>
                    Bu sayfa geliştirilme aşamasındadır
                </Typography>
                <Typography color="text.secondary" paragraph>
                    Personel eğitim bilgileri (Üniversite, Lise, İlköğretim, Sertifikalar vb.) bu sayfada görüntülenecektir.
                </Typography>
            </Paper>
        </Box>
    );
}

export default PersonelEgitim;
