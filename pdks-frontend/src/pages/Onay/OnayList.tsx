import { useEffect, useState } from 'react';
import { Box, Typography, Button, Chip, IconButton } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Check as CheckIcon, Close as CloseIcon } from '@mui/icons-material';
import onayService, { OnayKaydi } from '../../services/onayService';

function OnayList() {
    const [onaylar, setOnaylar] = useState<OnayKaydi[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await onayService.getBekleyenOnaylar();
            setOnaylar(data);
        } catch (error: any) {
            console.error('Onaylar yüklenemedi:', error);
            // 👇 BACKEND'DEN GELEN HATA MESAJINI GÖR
            console.error('Response data:', error.response?.data);
            console.error('Response status:', error.response?.status);
            console.error('Response headers:', error.response?.headers);

            // 👇 Kullanıcıya göster
            alert(`Hata: ${JSON.stringify(error.response?.data)}`);
        } finally {
            setLoading(false);
        }
    };

    const handleOnayla = async (onayDetayId: number) => {
        try {
            await onayService.onayla(onayDetayId);
            alert('Talep onaylandı!');
            loadData();
        } catch (error) {
            alert('Onaylama başarısız!');
        }
    };

    const handleReddet = async (onayDetayId: number) => {
        try {
            await onayService.reddet(onayDetayId);
            alert('Talep reddedildi!');
            loadData();
        } catch (error) {
            alert('Reddetme başarısız!');
        }
    };

    const columns: GridColDef[] = [
        {
            field: 'modulTipi',
            headerName: 'Modül',
            width: 120,
            renderCell: (params) => <Chip label={params.value} size="small" />,
        },
        { field: 'talepEdenKisi', headerName: 'Talep Eden', width: 200 },
        { field: 'adimAdi', headerName: 'Adım', width: 150 },
        {
            field: 'talepTarihi',
            headerName: 'Talep Tarihi',
            width: 150,
            valueFormatter: (value) => new Date(value).toLocaleDateString('tr-TR'),
        },
        { field: 'beklemeSuresi', headerName: 'Bekleyen Gün', width: 120 },
        {
            field: 'actions',
            headerName: 'İşlemler',
            width: 150,
            renderCell: (params) => (
                <Box>
                    <IconButton size="small" color="success" onClick={() => handleOnayla(params.row.onayKaydiId)}>
                        <CheckIcon />
                    </IconButton>
                    <IconButton size="small" color="error" onClick={() => handleReddet(params.row.onayKaydiId)}>
                        <CloseIcon />
                    </IconButton>
                </Box>
            ),
        },
    ];

    return (
        <Box>
            <Typography variant="h4" fontWeight="bold" sx={{ mb: 3 }}>
                Bekleyen Onaylar
            </Typography>

            <Box sx={{ height: 600, width: '100%' }}>
                <DataGrid rows={onaylar} columns={columns} loading={loading} getRowId={(row) => row.onayKaydiId} pageSizeOptions={[10, 25, 50]} />
            </Box>
        </Box>
    );
}

export default OnayList;