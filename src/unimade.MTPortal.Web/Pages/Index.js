// Main Index Page JavaScript
(function ($) {
    $(function () {
        // Initialize page components
        initPage();

        // Add smooth scrolling for anchor links
        $('a[href^="#"]').on('click', function (e) {
            e.preventDefault();
            $('html, body').animate({
                scrollTop: $($(this).attr('href')).offset().top - 70
            }, 500);
        });

        // Tenant card interactions
        $('.tenant-card').hover(
            function () {
                $(this).addClass('shadow');
            },
            function () {
                $(this).removeClass('shadow');
            }
        );
    });

    function initPage() {
        console.log('MTPortal Index Page initialized');

        // Add loading states for buttons
        $('abp-button[type="button"]').on('click', function () {
            var $button = $(this);
            var originalText = $button.text();

            $button.prop('disabled', true)
                .html('<i class="fas fa-spinner fa-spin me-2"></i>' + originalText);

            // Re-enable after 2 seconds (for demo)
            setTimeout(function () {
                $button.prop('disabled', false).text(originalText);
            }, 2000);
        });
    }

    // Public functions
    abp.modals.IndexPage = {
        refreshTenants: function () {
            abp.notify.info('Refreshing tenant list...');
            // Implement AJAX call to refresh tenants
            setTimeout(() => {
                abp.notify.success('Tenant list updated');
            }, 1000);
        },

        showSystemInfo: function () {
            abp.message.info('System is running smoothly!', 'System Status');
        }
    };
})(jQuery);

function showSettings() {
    abp.notify.info('Settings feature coming soon!');
}

function showReports() {
    abp.notify.info('Reports feature coming soon!');
}