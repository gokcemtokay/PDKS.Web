import LoginPage from './pages/LoginPage';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';

// Uygulamam�z i�in basit bir tema olu�tural�m.
// Renkleri daha sonra istedi�iniz gibi de�i�tirebilirsiniz.
const theme = createTheme({
    palette: {
        primary: {
            main: '#1976d2', // Mavi tonu
        },
        secondary: {
            main: '#dc004e', // Pembe/k�rm�z� tonu
        },
    },
});

function App() {
    return (
        <ThemeProvider theme={theme}>
            {/* CssBaseline, t�m stilleri standart hale getirir */}
            <CssBaseline />
            <LoginPage />
        </ThemeProvider>
    );
}

export default App;