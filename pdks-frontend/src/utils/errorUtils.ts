/**
 * errorUtils.ts - Tüm Formlarda Kullanılacak Hata Yönetimi
 * 
 * Konum: pdks-frontend/src/utils/errorUtils.ts
 */

export const parseApiError = (error: any): string => {
    console.error('API Error:', error);

    // 1. Response data varsa
    if (error.response?.data) {
        const data = error.response.data;

        // String mesaj (örn: "Internal Server Error: Bu TC Kimlik No zaten kayıtlı")
        if (typeof data === 'string') {
            return data.replace('Internal Server Error: ', '').replace(/^"(.*)"$/, '$1');
        }

        // Object içinde message varsa
        if (data.message) {
            return data.message;
        }

        // Validation errors (ModelState hatası)
        if (data.errors) {
            const errors = Object.entries(data.errors)
                .map(([field, messages]: [string, any]) => {
                    if (Array.isArray(messages)) {
                        return `${field}: ${messages.join(', ')}`;
                    }
                    return `${field}: ${messages}`;
                })
                .join('\n');
            return errors;
        }

        // Title varsa (ProblemDetails format)
        if (data.title) {
            return data.title;
        }
    }

    // 2. Response status'e göre
    if (error.response?.status) {
        switch (error.response.status) {
            case 400:
                return 'Geçersiz veri gönderildi. Lütfen formu kontrol edin.';
            case 401:
                return 'Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.';
            case 403:
                return 'Bu işlem için yetkiniz yok.';
            case 404:
                return 'İstenen kayıt bulunamadı.';
            case 409:
                return 'Bu kayıt zaten mevcut.';
            case 500:
                return 'Sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin.';
            default:
                return `Hata kodu: ${error.response.status}`;
        }
    }

    // 3. Network hatası
    if (error.code === 'ERR_NETWORK') {
        return 'Bağlantı hatası. Lütfen internet bağlantınızı kontrol edin.';
    }

    // 4. Timeout
    if (error.code === 'ECONNABORTED') {
        return 'İstek zaman aşımına uğradı. Lütfen tekrar deneyin.';
    }

    // 5. Genel hata mesajı
    return error.message || 'Bilinmeyen bir hata oluştu.';
};

/**
 * Hata gösterimi için yardımcı fonksiyon
 */
export const showError = (error: any, setError: (msg: string) => void) => {
    const errorMessage = parseApiError(error);
    setError(errorMessage);
    console.error('Error displayed to user:', errorMessage);
};
