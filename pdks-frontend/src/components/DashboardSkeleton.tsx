// src/components/DashboardSkeleton.tsx - YENÝ DOSYA
import { Grid, Card, CardContent, Skeleton, Box } from '@mui/material';

function DashboardSkeleton() {
    return (
        <Box>
            <Skeleton variant="text" width={200} height={40} sx={{ mb: 3 }} />
            
            <Grid container spacing={3}>
                {[1, 2, 3, 4].map((item) => (
                    <Grid item xs={12} sm={6} md={3} key={item}>
                        <Card sx={{ height: '100%' }}>
                            <CardContent>
                                <Box display="flex" justifyContent="space-between" alignItems="center">
                                    <Box sx={{ flex: 1 }}>
                                        <Skeleton variant="text" width={80} height={50} />
                                        <Skeleton variant="text" width={120} height={30} />
                                    </Box>
                                    <Skeleton variant="circular" width={40} height={40} />
                                </Box>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>

            <Grid container spacing={3} sx={{ mt: 2 }}>
                <Grid item xs={12}>
                    <Card>
                        <CardContent>
                            <Skeleton variant="text" width={200} height={30} sx={{ mb: 2 }} />
                            <Skeleton variant="rectangular" height={200} />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        </Box>
    );
}

export default DashboardSkeleton;