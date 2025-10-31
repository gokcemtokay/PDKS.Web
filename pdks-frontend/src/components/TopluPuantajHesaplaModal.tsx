import React, { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Radio,
  Select,
  MenuItem,
  TextField,
  Checkbox,
  ListItemText,
  CircularProgress,
  Alert,
  Stack,
  Box,
} from '@mui/material';
import { SelectChangeEvent } from '@mui/material';
import puantajService from '../services/puantajService';
import { TopluPuantajHesaplaRequest } from '../types/puantaj.types';

interface TopluPuantajHesaplaModalProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
  defaultYil: number;
  defaultAy: number;
  departmanlar?: Array<{ id: number; departmanAdi: string }>;
  personeller?: Array<{ id: number; adSoyad: string; sicilNo: string; departman: string }>;
}

const TopluPuantajHesaplaModal: React.FC<TopluPuantajHesaplaModalProps> = ({
  open,
  onClose,
  onSuccess,
  defaultYil,
  defaultAy,
  departmanlar = [],
  personeller = [],
}) => {
  const [hesaplaTuru, setHesaplaTuru] = useState<'tum' | 'departman' | 'personel'>('tum');
  const [departmanId, setDepartmanId] = useState<number>(0);
  const [personelIdler, setPersonelIdler] = useState<number[]>([]);
  const [yil, setYil] = useState<number>(defaultYil);
  const [ay, setAy] = useState<number>(defaultAy);
  
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleHesapla = async () => {
    try {
      setError(null);
      setLoading(true);

      const request: TopluPuantajHesaplaRequest = {
        yil,
        ay,
      };

      if (hesaplaTuru === 'tum') {
        request.tumPersonel = true;
      } else if (hesaplaTuru === 'departman') {
        if (!departmanId) {
          setError('Lütfen departman seçiniz');
          return;
        }
        request.departmanId = departmanId;
      } else if (hesaplaTuru === 'personel') {
        if (personelIdler.length === 0) {
          setError('Lütfen en az bir personel seçiniz');
          return;
        }
        request.personelIdler = personelIdler;
      }

      await puantajService.topluPuantajHesapla(request);
      onSuccess();
      handleClose();
    } catch (error: any) {
      console.error('Toplu puantaj hesaplama hatası:', error);
      setError(error.response?.data?.message || 'Puantaj hesaplanırken hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setHesaplaTuru('tum');
    setDepartmanId(0);
    setPersonelIdler([]);
    setError(null);
    onClose();
  };

  const handlePersonelChange = (event: SelectChangeEvent<number[]>) => {
    const value = event.target.value;
    setPersonelIdler(typeof value === 'string' ? value.split(',').map(Number) : value);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Toplu Puantaj Hesapla</DialogTitle>
      <DialogContent>
        <Stack spacing={3} sx={{ mt: 2 }}>
          {error && (
            <Alert severity="error" onClose={() => setError(null)}>
              {error}
            </Alert>
          )}

          {/* Dönem Seçimi */}
          <Box>
            <FormLabel>Dönem</FormLabel>
            <Stack direction="row" spacing={2} sx={{ mt: 1 }}>
              <TextField
                select
                label="Yıl"
                value={yil}
                onChange={(e) => setYil(Number(e.target.value))}
                fullWidth
              >
                {[2023, 2024, 2025, 2026].map((y) => (
                  <MenuItem key={y} value={y}>
                    {y}
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                select
                label="Ay"
                value={ay}
                onChange={(e) => setAy(Number(e.target.value))}
                fullWidth
              >
                {Array.from({ length: 12 }, (_, i) => i + 1).map((m) => (
                  <MenuItem key={m} value={m}>
                    {new Date(2000, m - 1).toLocaleDateString('tr-TR', { month: 'long' })}
                  </MenuItem>
                ))}
              </TextField>
            </Stack>
          </Box>

          {/* Hesaplama Türü */}
          <FormControl>
            <FormLabel>Hesaplama Türü</FormLabel>
            <RadioGroup
              value={hesaplaTuru}
              onChange={(e) => setHesaplaTuru(e.target.value as any)}
            >
              <FormControlLabel
                value="tum"
                control={<Radio />}
                label="Tüm Personel"
              />
              <FormControlLabel
                value="departman"
                control={<Radio />}
                label="Departman Bazında"
                disabled={departmanlar.length === 0}
              />
              <FormControlLabel
                value="personel"
                control={<Radio />}
                label="Seçili Personeller"
                disabled={personeller.length === 0}
              />
            </RadioGroup>
          </FormControl>

          {/* Departman Seçimi */}
          {hesaplaTuru === 'departman' && departmanlar.length > 0 && (
            <FormControl fullWidth>
              <FormLabel>Departman</FormLabel>
              <Select
                value={departmanId}
                onChange={(e) => setDepartmanId(Number(e.target.value))}
                displayEmpty
              >
                <MenuItem value={0}>Departman seçiniz</MenuItem>
                {departmanlar.map((dept) => (
                  <MenuItem key={dept.id} value={dept.id}>
                    {dept.departmanAdi}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          )}

          {/* Personel Seçimi */}
          {hesaplaTuru === 'personel' && personeller.length > 0 && (
            <FormControl fullWidth>
              <FormLabel>Personeller</FormLabel>
              <Select
                multiple
                value={personelIdler}
                onChange={handlePersonelChange}
                renderValue={(selected) => 
                  personeller
                    .filter(p => selected.includes(p.id))
                    .map(p => p.adSoyad)
                    .join(', ')
                }
                displayEmpty
              >
                <MenuItem disabled value="">
                  Personel seçiniz
                </MenuItem>
                {personeller.map((person) => (
                  <MenuItem key={person.id} value={person.id}>
                    <Checkbox checked={personelIdler.indexOf(person.id) > -1} />
                    <ListItemText 
                      primary={person.adSoyad}
                      secondary={`${person.sicilNo} - ${person.departman}`}
                    />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          )}
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={loading}>
          İptal
        </Button>
        <Button
          onClick={handleHesapla}
          variant="contained"
          disabled={loading}
          startIcon={loading ? <CircularProgress size={20} /> : null}
        >
          {loading ? 'Hesaplanıyor...' : 'Hesapla'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default TopluPuantajHesaplaModal;
