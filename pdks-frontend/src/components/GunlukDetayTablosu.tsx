import React from 'react';
import {
  Table,
  TableHead,
  TableBody,
  TableRow,
  TableCell,
  Box,
  Typography,
  Chip,
  Tooltip,
  Paper,
} from '@mui/material';
import {
  Error as ErrorIcon,
  Schedule as ScheduleIcon,
} from '@mui/icons-material';
import { PuantajDetay } from '../types/puantaj.types';
import {
  formatTarih,
  formatSaat24,
  formatTimeSpan,
  formatDakika,
  getGunDurumuBadgeClass,
  getGunDurumuText,
} from '../utils/puantajUtils';

interface GunlukDetayTablosuProps {
  detaylar: PuantajDetay[];
  showVardiya?: boolean;
}

const GunlukDetayTablosu: React.FC<GunlukDetayTablosuProps> = ({
  detaylar,
  showVardiya = true,
}) => {
  const getBadgeColor = (durum: string): 'success' | 'info' | 'warning' | 'error' | 'default' => {
    const classMap: { [key: string]: 'success' | 'info' | 'warning' | 'error' | 'default' } = {
      success: 'success',
      info: 'info',
      warning: 'warning',
      danger: 'error',
      secondary: 'default',
      primary: 'info',
    };
    return classMap[getGunDurumuBadgeClass(durum)] || 'default';
  };

  const getRowBackgroundColor = (detay: PuantajDetay) => {
    if (detay.haftaSonuMu) return 'grey.50';
    if (detay.resmiTatilMi) return 'blue.50';
    if (detay.devamsizMi) return 'error.lighter';
    return 'white';
  };

  return (
    <Paper sx={{ overflow: 'hidden', border: 1, borderColor: 'divider', borderRadius: 2 }}>
      <Box sx={{ overflowX: 'auto' }}>
        <Table size="small">
          <TableHead>
            <TableRow sx={{ bgcolor: 'grey.50' }}>
              <TableCell sx={{ fontWeight: 'bold' }}>Tarih</TableCell>
              <TableCell sx={{ fontWeight: 'bold' }}>Gün</TableCell>
              {showVardiya && <TableCell sx={{ fontWeight: 'bold' }}>Vardiya</TableCell>}
              <TableCell sx={{ fontWeight: 'bold' }}>Planlanan</TableCell>
              <TableCell sx={{ fontWeight: 'bold' }}>Gerçekleşen</TableCell>
              <TableCell align="center" sx={{ fontWeight: 'bold' }}>Çalışma</TableCell>
              <TableCell align="center" sx={{ fontWeight: 'bold' }}>Fazla Mesai</TableCell>
              <TableCell align="center" sx={{ fontWeight: 'bold' }}>Durum</TableCell>
              <TableCell sx={{ fontWeight: 'bold' }}>Notlar</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {detaylar.map((detay) => (
              <TableRow
                key={detay.id}
                sx={{ 
                  bgcolor: getRowBackgroundColor(detay),
                  '&:hover': { bgcolor: 'action.hover' }
                }}
              >
                <TableCell sx={{ fontWeight: 'medium' }}>
                  {formatTarih(detay.tarih)}
                </TableCell>
                
                <TableCell>
                  <Typography variant="body2">{detay.gun}</Typography>
                </TableCell>

                {showVardiya && (
                  <TableCell>
                    <Typography variant="body2">{detay.vardiyaAdi || '-'}</Typography>
                  </TableCell>
                )}

                <TableCell>
                  {detay.planlananGiris && detay.planlananCikis ? (
                    <Typography variant="body2">
                      {formatTimeSpan(detay.planlananGiris)} -{' '}
                      {formatTimeSpan(detay.planlananCikis)}
                    </Typography>
                  ) : (
                    <Typography variant="body2" color="text.disabled">
                      -
                    </Typography>
                  )}
                </TableCell>

                <TableCell>
                  {detay.gerceklesenGiris && detay.gerceklesenCikis ? (
                    <Box>
                      <Typography variant="body2">
                        {formatSaat24(detay.gerceklesenGiris)} -{' '}
                        {formatSaat24(detay.gerceklesenCikis)}
                      </Typography>
                      {(detay.gecKaldiMi || detay.erkenCiktiMi) && (
                        <Box sx={{ mt: 0.5, display: 'flex', gap: 0.5, flexWrap: 'wrap' }}>
                          {detay.gecKaldiMi && (
                            <Tooltip title={`${detay.gecKalmaSuresi} dakika geç`}>
                              <Chip
                                icon={<ErrorIcon />}
                                label="Geç"
                                size="small"
                                color="error"
                                sx={{ height: 20, fontSize: '0.7rem' }}
                              />
                            </Tooltip>
                          )}
                          {detay.erkenCiktiMi && (
                            <Tooltip title={`${detay.erkenCikisSuresi} dakika erken`}>
                              <Chip
                                icon={<ScheduleIcon />}
                                label="Erken"
                                size="small"
                                color="warning"
                                sx={{ height: 20, fontSize: '0.7rem' }}
                              />
                            </Tooltip>
                          )}
                        </Box>
                      )}
                    </Box>
                  ) : (
                    <Typography variant="body2" color="text.disabled">
                      -
                    </Typography>
                  )}
                </TableCell>

                <TableCell align="center">
                  <Typography variant="body2" fontWeight="medium">
                    {detay.gerceklesenSure
                      ? formatDakika(detay.gerceklesenSure)
                      : '-'}
                  </Typography>
                  {detay.normalMesai && (
                    <Typography variant="caption" color="text.secondary">
                      Normal: {formatDakika(detay.normalMesai)}
                    </Typography>
                  )}
                </TableCell>

                <TableCell align="center">
                  {detay.fazlaMesai && detay.fazlaMesai > 0 ? (
                    <Chip
                      label={formatDakika(detay.fazlaMesai)}
                      color="success"
                      size="small"
                    />
                  ) : (
                    <Typography variant="body2" color="text.disabled">
                      -
                    </Typography>
                  )}
                </TableCell>

                <TableCell align="center">
                  <Chip
                    label={getGunDurumuText(detay.gunDurumu)}
                    color={getBadgeColor(detay.gunDurumu)}
                    size="small"
                  />
                  {detay.izinTuru && (
                    <Typography variant="caption" color="text.secondary" display="block" sx={{ mt: 0.5 }}>
                      {detay.izinTuru}
                    </Typography>
                  )}
                </TableCell>

                <TableCell>
                  {detay.notlar && (
                    <Tooltip title={detay.notlar}>
                      <Typography 
                        variant="caption" 
                        sx={{ 
                          maxWidth: 150,
                          overflow: 'hidden',
                          textOverflow: 'ellipsis',
                          whiteSpace: 'nowrap',
                          display: 'block'
                        }}
                      >
                        {detay.notlar}
                      </Typography>
                    </Tooltip>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>

        {detaylar.length === 0 && (
          <Box sx={{ p: 4, textAlign: 'center' }}>
            <Typography color="text.secondary">Günlük detay bulunamadı</Typography>
          </Box>
        )}
      </Box>
    </Paper>
  );
};

export default GunlukDetayTablosu;
