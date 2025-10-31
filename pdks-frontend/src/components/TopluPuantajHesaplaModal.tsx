import React, { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  FormControl,
  FormLabel,
  Select,
  MenuItem,
  Radio,
  RadioGroup,
  FormControlLabel,
  Stack,
  LinearProgress,
  Typography,
  Box,
  Alert,
  Snackbar,
} from '@mui/material';
import puantajService from '../services/puantajService';
import { getAyListesi, getYilListesi } from '../utils/puantajUtils';

interface TopluPuantajHesaplaModalProps {
  isOpen: boolean;
  onClose: () => void;
  departmanlar?: { id: number; ad: string }[];
  onSuccess?: () => void;
}

const TopluPuantajHesaplaModal: React.FC<TopluPuantajHesaplaModalProps> = ({
  isOpen,
  onClose,
  departmanlar = [],
  onSuccess,
}) => {
  const [yil, setYil] = useState<number>(new Date().getFullYear());
  const [ay, setAy] = useState<number>(new Date().getMonth() + 1);
  const [secim, setSecim] = useState<string>('tumu');
  const [departmanId, setDepartmanId] = useState<number | undefined>();
  const [loading, setLoading] = useState<boolean>(false);
  const [snackbar, setSnackbar] = useState<{ open: boolean; message: string; severity: 'success' | 'error' }>({
    open: false,
    message: '',
    severity: 'success',
  });
  
  const ayListesi = getAyListesi();
  const yilListesi = getYilListesi();

  const handleHesapla = async () => {
    setLoading(true);
    try {
      const data: any = {
        yil,
        ay,
        tumPersonel: secim === 'tumu',
      };

      if (secim === 'departman' && departmanId) {
        data.departmanId = departmanId;
      }

      const result = await puantajService.topluPuantajHesapla(data);

      setSnackbar({
        open: true,
        message: result.message || 'Toplu puantaj başarıyla hesaplandı',
        severity: 'success',
      });

      onClose();
      onSuccess?.();
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.response?.data?.message || 'Toplu puantaj hesaplanamadı',
        severity: 'error',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  return (
    <>
      <Dialog open={isOpen} onClose={onClose} maxWidth="sm" fullWidth>
        <DialogTitle>Toplu Puantaj Hesapla</DialogTitle>
        
        <DialogContent>
          <Stack spacing={3} sx={{ mt: 1 }}>
            <FormControl fullWidth>
              <FormLabel>Yıl</FormLabel>
              <Select 
                value={yil} 
                onChange={(e) => setYil(Number(e.target.value))}
                size="small"
              >
                {yilListesi.map((y) => (
                  <MenuItem key={y} value={y}>
                    {y}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <FormControl fullWidth>
              <FormLabel>Ay</FormLabel>
              <Select 
                value={ay} 
                onChange={(e) => setAy(Number(e.target.value))}
                size="small"
              >
                {ayListesi.map((a) => (
                  <MenuItem key={a.value} value={a.value}>
                    {a.label}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <FormControl fullWidth>
              <FormLabel>Hesaplama Kapsamı</FormLabel>
              <RadioGroup value={secim} onChange={(e) => setSecim(e.target.value)}>
                <FormControlLabel value="tumu" control={<Radio />} label="Tüm Personel" />
                <FormControlLabel value="departman" control={<Radio />} label="Departman Bazlı" />
              </RadioGroup>
            </FormControl>

            {secim === 'departman' && (
              <FormControl fullWidth>
                <FormLabel>Departman</FormLabel>
                <Select
                  value={departmanId || ''}
                  onChange={(e) => setDepartmanId(Number(e.target.value))}
                  displayEmpty
                  size="small"
                >
                  <MenuItem value="" disabled>
                    Departman seçin
                  </MenuItem>
                  {departmanlar.map((dept) => (
                    <MenuItem key={dept.id} value={dept.id}>
                      {dept.ad}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            )}

            {loading && (
              <Box>
                <LinearProgress />
                <Typography variant="body2" color="text.secondary" align="center" sx={{ mt: 1 }}>
                  Puantajlar hesaplanıyor...
                </Typography>
              </Box>
            )}
          </Stack>
        </DialogContent>

        <DialogActions>
          <Button onClick={onClose} disabled={loading}>
            İptal
          </Button>
          <Button
            variant="contained"
            onClick={handleHesapla}
            disabled={loading || (secim === 'departman' && !departmanId)}
          >
            Toplu Hesapla
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar
        open={snackbar.open}
        autoHideDuration={5000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <Alert onClose={handleCloseSnackbar} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </>
  );
};

export default TopluPuantajHesaplaModal;
