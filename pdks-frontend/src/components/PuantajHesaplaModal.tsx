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
  Checkbox,
  FormControlLabel,
  Stack,
  Typography,
  Snackbar,
  Alert,
} from '@mui/material';
import puantajService from '../services/puantajService';
import { getAyListesi, getYilListesi, getTurkceAyAdi } from '../utils/puantajUtils';

interface PuantajHesaplaModalProps {
  isOpen: boolean;
  onClose: () => void;
  personelId?: number;
  onSuccess?: () => void;
}

const PuantajHesaplaModal: React.FC<PuantajHesaplaModalProps> = ({
  isOpen,
  onClose,
  personelId,
  onSuccess,
}) => {
  const [yil, setYil] = useState<number>(new Date().getFullYear());
  const [ay, setAy] = useState<number>(new Date().getMonth() + 1);
  const [yenidenHesapla, setYenidenHesapla] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(false);
  const [snackbar, setSnackbar] = useState<{ open: boolean; message: string; severity: 'success' | 'error' }>({
    open: false,
    message: '',
    severity: 'success',
  });
  
  const ayListesi = getAyListesi();
  const yilListesi = getYilListesi();

  const handleHesapla = async () => {
    if (!personelId) {
      setSnackbar({
        open: true,
        message: 'Personel seçilmedi',
        severity: 'error',
      });
      return;
    }

    setLoading(true);
    try {
      await puantajService.hesaplaPuantaj({
        personelId,
        yil,
        ay,
        yenidenHesapla,
      });

      setSnackbar({
        open: true,
        message: `${getTurkceAyAdi(ay)} ${yil} puantajı hesaplandı`,
        severity: 'success',
      });

      onClose();
      onSuccess?.();
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.response?.data?.message || 'Puantaj hesaplanamadı',
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
        <DialogTitle>Puantaj Hesapla</DialogTitle>
        
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

            <FormControl>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={yenidenHesapla}
                    onChange={(e) => setYenidenHesapla(e.target.checked)}
                  />
                }
                label="Mevcut puantajı yeniden hesapla"
              />
              <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5, ml: 4 }}>
                Bu seçenek mevcut puantajı siler ve yeniden oluşturur
              </Typography>
            </FormControl>
          </Stack>
        </DialogContent>

        <DialogActions>
          <Button onClick={onClose}>
            İptal
          </Button>
          <Button
            variant="contained"
            onClick={handleHesapla}
            disabled={loading}
          >
            Hesapla
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

export default PuantajHesaplaModal;
