// src/utils/validation.ts - YENÝ DOSYA

import * as yup from 'yup';

// Türkçe hata mesajlarý
yup.setLocale({
    mixed: {
        default: 'Geçersiz deðer',
        required: 'Bu alan zorunludur',
    },
    string: {
        email: 'Geçerli bir e-posta adresi giriniz',
        min: 'En az ${min} karakter olmalýdýr',
        max: 'En fazla ${max} karakter olmalýdýr',
    },
    number: {
        min: 'En az ${min} olmalýdýr',
        max: 'En fazla ${max} olmalýdýr',
        positive: 'Pozitif bir sayý olmalýdýr',
    },
});

// TC Kimlik No Validasyonu
export const validateTCKN = (tckn: string): boolean => {
    if (!tckn || tckn.length !== 11) return false;

    const digits = tckn.split('').map(Number);

    // Ýlk hane 0 olamaz
    if (digits[0] === 0) return false;

    // Tüm haneler rakam olmalý
    if (digits.some(isNaN)) return false;

    // 10. hane kontrolü
    const sum10 = digits.slice(0, 9).reduce((sum, digit, i) => {
        return sum + digit * (10 - i);
    }, 0);

    if (sum10 % 11 !== digits[9]) return false;

    // 11. hane kontrolü
    const sum11 = digits.slice(0, 10).reduce((sum, digit) => sum + digit, 0);

    if (sum11 % 10 !== digits[10]) return false;

    return true;
};

// Telefon formatý (5XXXXXXXXX)
export const validatePhone = (phone: string): boolean => {
    const cleaned = phone.replace(/\D/g, '');
    return /^5\d{9}$/.test(cleaned);
};

// Yup custom validators
yup.addMethod(yup.string, 'tckn', function (message = 'Geçersiz TC Kimlik No') {
    return this.test('tckn', message, function (value) {
        if (!value) return true; // Required kontrolü ayrý yapýlacak
        return validateTCKN(value);
    });
});

yup.addMethod(yup.string, 'phone', function (message = 'Geçersiz telefon numarasý (5XXXXXXXXX formatýnda olmalý)') {
    return this.test('phone', message, function (value) {
        if (!value) return true;
        return validatePhone(value);
    });
});

// TypeScript için tip geniþletme
declare module 'yup' {
    interface StringSchema {
        tckn(message?: string): this;
        phone(message?: string): this;
    }
}

export default yup;