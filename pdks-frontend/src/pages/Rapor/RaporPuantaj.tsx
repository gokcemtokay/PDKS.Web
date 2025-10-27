/**
 * PDKS Frontend - Hızlı Sayfa Oluşturma Template'i
 * 
 * Aşağıdaki sayfaları bu template'i kullanarak oluşturun:
 * - PersonelEgitim.tsx
 * - PersonelSertifika.tsx
 * - PersonelSaglik.tsx
 * - PersonelDeneyim.tsx
 * - PuantajGecKalanlar.tsx
 * - PuantajErkenCikanlar.tsx
 * - RaporPersonel.tsx
 * - RaporIzin.tsx
 * - RaporPuantaj.tsx
 * - RaporMasraf.tsx
 * - ParametrePage.tsx
 */

import { Box, Typography, Paper, Button } from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';

function RaporPuantaj() {
    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4" fontWeight="bold">
                    Sayfa Başlığı
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                >
                    Yeni Ekle
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <Typography variant="h6" gutterBottom>
                    Bu sayfa geliştirilme aşamasındadır
                </Typography>
                <Typography color="text.secondary" paragraph>
                    Sayfa içeriği yakında eklenecektir. API entegrasyonu ve veri görselleştirme işlemleri devam etmektedir.
                </Typography>
            </Paper>
        </Box>
    );
}

export default RaporPuantaj;

/**
 * KULLANIM ÖRNEĞİ:
 * 
 * 1. Bu dosyayı kopyalayın
 * 2. Dosya adını değiştirin (örn: PersonelEgitim.tsx)
 * 3. Function adını değiştirin (örn: PersonelEgitim)
 * 4. "Sayfa Başlığı" kısmını uygun başlıkla değiştirin
 * 5. Export adını güncelleyin
 * 
 * Örnek:
 * 
 * function PersonelEgitim() {
 *     return (
 *         <Box>
 *             <Typography variant="h4" fontWeight="bold">
 *                 Personel Eğitim Bilgileri
 *             </Typography>
 *             ...
 *         </Box>
 *     );
 * }
 * 
 * export default PersonelEgitim;
 */
