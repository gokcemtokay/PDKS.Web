$(document).ready(function () {
    // ================================================
    // SIDEBAR TOGGLE (Desktop + Mobile)
    // ================================================

    // LocalStorage'dan sidebar durumunu yükle
    if (localStorage.getItem('sidebar-collapsed') === 'true') {
        $('.sidebar').addClass('collapsed');
        $('#content').addClass('expanded');
    }

    // Hamburger butonu - Toggle sidebar
    $('#sidebarToggle').on('click', function () {
        if ($(window).width() >= 992) {
            // Desktop: Collapse/Expand
            $('.sidebar').toggleClass('collapsed');
            $('#content').toggleClass('expanded');

            // Kaydet
            if ($('.sidebar').hasClass('collapsed')) {
                localStorage.setItem('sidebar-collapsed', 'true');
            } else {
                localStorage.removeItem('sidebar-collapsed');
            }
        } else {
            // Mobile: Show
            $('.sidebar').addClass('active');
            $('.sidebar-overlay').addClass('active');
            $('body').css('overflow', 'hidden');
        }
    });

    // Mobile: Close sidebar
    $('#sidebarCloseBtn, .sidebar-overlay').on('click', function () {
        $('.sidebar').removeClass('active');
        $('.sidebar-overlay').removeClass('active');
        $('body').css('overflow', 'auto');
    });

    // ================================================
    // DROPDOWN MENÜLER (Raporlar & Yönetim)
    // ================================================
    $('.dropdown-toggle').on('click', function (e) {
        e.preventDefault();

        // Sidebar collapsed ise çalıştırma
        if ($('.sidebar').hasClass('collapsed') && $(window).width() >= 992) {
            return false;
        }

        var submenuId = $(this).data('submenu') + 'Submenu';
        var $submenu = $('#' + submenuId);

        // Toggle
        $(this).toggleClass('active');
        $submenu.slideToggle(200);

        // Diğerlerini kapat
        $('.dropdown-toggle').not(this).removeClass('active');
        $('.submenu').not($submenu).slideUp(200);
    });

    // ================================================
    // ACTIVE MENU STATE
    // ================================================
    var currentPath = window.location.pathname;
    $('.sidebar a').each(function () {
        var href = $(this).attr('href');
        if (href && href !== '#' && currentPath.indexOf(href) === 0) {
            $(this).parent('li').addClass('active');

            // Parent dropdown'ı aç
            var $submenu = $(this).closest('.submenu');
            if ($submenu.length > 0) {
                $submenu.show();
                $submenu.prev('.dropdown-toggle').addClass('active');
            }
        }
    });

    // ================================================
    // MOBILE - AUTO CLOSE
    // ================================================
    $('.sidebar a:not(.dropdown-toggle)').on('click', function () {
        if ($(window).width() < 992) {
            $('.sidebar').removeClass('active');
            $('.sidebar-overlay').removeClass('active');
            $('body').css('overflow', 'auto');
        }
    });

    // ================================================
    // WINDOW RESIZE
    // ================================================
    $(window).on('resize', function () {
        if ($(window).width() >= 992) {
            $('.sidebar').removeClass('active');
            $('.sidebar-overlay').removeClass('active');
            $('body').css('overflow', 'auto');
        }
    });
});

// DataTables Türkçe
var datatablesTurkish = {
    "sDecimal": ",",
    "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
    "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
    "sInfoEmpty": "Kayıt yok",
    "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
    "sLengthMenu": "Sayfada _MENU_ kayıt göster",
    "sLoadingRecords": "Yükleniyor...",
    "sProcessing": "İşleniyor...",
    "sSearch": "Ara:",
    "sZeroRecords": "Eşleşen kayıt bulunamadı",
    "oPaginate": {
        "sFirst": "İlk",
        "sLast": "Son",
        "sNext": "Sonraki",
        "sPrevious": "Önceki"
    }
};