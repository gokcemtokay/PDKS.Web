import LoginPage from './pages/LoginPage';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';

// Uygulamamýz için basit bir tema oluþturalým.
// Renkleri daha sonra istediðiniz gibi deðiþtirebilirsiniz.
const theme = createTheme({
    palette: {
        primary: {
            main: '#1976d2', // Mavi tonu
        },
        secondary: {
            main: '#dc004e', // Pembe/kýrmýzý tonu
        },
    },
});

function App() {
    return (
        <ThemeProvider theme={theme}>
            {/* CssBaseline, tüm stilleri standart hale getirir */}
            <CssBaseline />
            <LoginPage />
        </ThemeProvider>
    );
}

export default App;