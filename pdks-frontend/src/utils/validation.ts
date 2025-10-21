// src/utils/validation.ts - YEN� DOSYA

import * as yup from 'yup';

// T�rk�e hata mesajlar�
yup.setLocale({
    mixed: {
        default: 'Ge�ersiz de�er',
        required: 'Bu alan zorunludur',
    },
    string: {
        email: 'Ge�erli bir e-posta adresi giriniz',
        min: 'En az ${min} karakter olmal�d�r',
        max: 'En fazla ${max} karakter olmal�d�r',
    },
    number: {
        min: 'En az ${min} olmal�d�r',
        max: 'En fazla ${max} olmal�d�r',
        positive: 'Pozitif bir say� olmal�d�r',
    },
});

// TC Kimlik No Validasyonu
export const validateTCKN = (tckn: string): boolean => {
    if (!tckn || tckn.length !== 11) return false;

    const digits = tckn.split('').map(Number);

    // �lk hane 0 olamaz
    if (digits[0] === 0) return false;

    // T�m haneler rakam olmal�
    if (digits.some(isNaN)) return false;

    // 10. hane kontrol�
    const sum10 = digits.slice(0, 9).reduce((sum, digit, i) => {
        return sum + digit * (10 - i);
    }, 0);

    if (sum10 % 11 !== digits[9]) return false;

    // 11. hane kontrol�
    const sum11 = digits.slice(0, 10).reduce((sum, digit) => sum + digit, 0);

    if (sum11 % 10 !== digits[10]) return false;

    return true;
};

// Telefon format� (5XXXXXXXXX)
export const validatePhone = (phone: string): boolean => {
    const cleaned = phone.replace(/\D/g, '');
    return /^5\d{9}$/.test(cleaned);
};

// Yup custom validators
yup.addMethod(yup.string, 'tckn', function (message = 'Ge�ersiz TC Kimlik No') {
    return this.test('tckn', message, function (value) {
        if (!value) return true; // Required kontrol� ayr� yap�lacak
        return validateTCKN(value);
    });
});

yup.addMethod(yup.string, 'phone', function (message = 'Ge�ersiz telefon numaras� (5XXXXXXXXX format�nda olmal�)') {
    return this.test('phone', message, function (value) {
        if (!value) return true;
        return validatePhone(value);
    });
});

// TypeScript i�in tip geni�letme
declare module 'yup' {
    interface StringSchema {
        tckn(message?: string): this;
        phone(message?: string): this;
    }
}

export default yup;