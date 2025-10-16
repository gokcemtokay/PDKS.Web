// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// ==================== SIDEBAR TOGGLE FUNCTIONALITY ====================
$(document).ready(function () {
    const sidebar = $('#sidebar');
    const content = $('#content');
    const overlay = $('#sidebarOverlay');
    const toggleBtn = $('#sidebarToggle');
    const collapseBtn = $('#sidebarCollapseBtn');

    // Toggle sidebar on mobile
    toggleBtn.on('click', function () {
        if (window.innerWidth < 992) {
            sidebar.toggleClass('active');
            overlay.toggleClass('active');
            $('body').toggleClass('overflow-hidden');
        } else {
            // Desktop: collapse sidebar
            sidebar.toggleClass('collapsed');
            content.toggleClass('expanded');

            // Save state to localStorage
            const isCollapsed = sidebar.hasClass('collapsed');
            localStorage.setItem('sidebarCollapsed', isCollapsed);
        }
    });

    // Close sidebar when clicking collapse button (mobile)
    collapseBtn.on('click', function () {
        closeSidebar();
    });

    // Close sidebar when clicking overlay (mobile)
    overlay.on('click', function () {
        closeSidebar();
    });

    // Close sidebar function
    function closeSidebar() {
        sidebar.removeClass('active');
        overlay.removeClass('active');
        $('body').removeClass('overflow-hidden');
    }

    // Handle window resize
    $(window).on('resize', function () {
        if (window.innerWidth >= 992) {
            // Desktop
            closeSidebar();
            $('body').removeClass('overflow-hidden');

            // Restore collapsed state from localStorage
            const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
            if (isCollapsed) {
                sidebar.addClass('collapsed');
                content.addClass('expanded');
            }
        } else {
            // Mobile
            sidebar.removeClass('collapsed');
            content.removeClass('expanded');
        }
    });

    // Initialize sidebar state on page load (desktop only)
    if (window.innerWidth >= 992) {
        const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
        if (isCollapsed) {
            sidebar.addClass('collapsed');
            content.addClass('expanded');
        }
    }

    // Close mobile menu when clicking on a menu item
    $('.sidebar ul li a:not([data-bs-toggle])').on('click', function () {
        if (window.innerWidth < 992) {
            closeSidebar();
        }
    });

    // Prevent body scroll when mobile menu is open
    sidebar.on('show.bs.collapse hide.bs.collapse', function () {
        if (window.innerWidth < 992) {
            $('body').toggleClass('overflow-hidden');
        }
    });
});

// ==================== NOTIFICATION AUTO HIDE ====================
$(document).ready(function () {
    // Auto hide alerts after 5 seconds
    $('.alert').each(function () {
        const alert = $(this);
        setTimeout(function () {
            alert.fadeOut('slow', function () {
                alert.remove();
            });
        }, 5000);
    });
});

// ==================== DATATABLE DEFAULT SETTINGS ====================
$.extend(true, $.fn.dataTable.defaults, {
    language: {
        url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json'
    },
    responsive: true,
    pageLength: 25,
    lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tümü"]],
    dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
        "<'row'<'col-sm-12'tr>>" +
        "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>"
});

// ==================== PREVENT BODY OVERFLOW ====================
$('body').on('overflow-hidden', function () {
    $('body').css('overflow', 'hidden');
});

// ==================== SMOOTH SCROLL ====================
$('a[href^="#"]').on('click', function (e) {
    e.preventDefault();
    const target = $(this.getAttribute('href'));
    if (target.length) {
        $('html, body').stop().animate({
            scrollTop: target.offset().top - 20
        }, 500);
    }
});