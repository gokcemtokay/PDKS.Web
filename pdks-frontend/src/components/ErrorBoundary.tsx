import { Component, ErrorInfo, ReactNode } from 'react';
import { Alert, Button, Box, Typography, Paper } from '@mui/material';
import { Error as ErrorIcon, Refresh as RefreshIcon } from '@mui/icons-material';

interface Props {
  children: ReactNode;
}

interface State {
  hasError: boolean;
  error: Error | null;
}

class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error('Error:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <Box sx={{ p: 4, display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh' }}>
          <Paper elevation={3} sx={{ p: 4, maxWidth: 600 }}>
            <Box sx={{ textAlign: 'center' }}>
              <ErrorIcon color="error" sx={{ fontSize: 80, mb: 2 }} />
              <Typography variant="h4" gutterBottom>Bir Hata Oluştu</Typography>
              <Typography color="text.secondary" sx={{ mb: 3 }}>
                Üzgünüz, beklenmeyen bir hata oluştu. Lütfen sayfayı yenileyin.
              </Typography>
              {this.state.error && (
                <Alert severity="error" sx={{ mb: 3, textAlign: 'left' }}>
                  {this.state.error.toString()}
                </Alert>
              )}
              <Button
                variant="contained"
                startIcon={<RefreshIcon />}
                onClick={() => window.location.reload()}
              >
                Sayfayı Yenile
              </Button>
            </Box>
          </Paper>
        </Box>
      );
    }

    return this.props.children;
  }
}

export default ErrorBoundary;
