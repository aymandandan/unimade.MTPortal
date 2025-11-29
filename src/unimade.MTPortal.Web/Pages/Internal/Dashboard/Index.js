function createAnnouncement() {
    window.location.href = '/Internal/Announcements/Index';
}

function showSettings() {
    abp.notify.info('Settings feature coming soon!');
}

// Auto-refresh dashboard every 5 minutes
setInterval(() => {
    abp.notify.info('Refreshing dashboard data...');
    window.location.reload();
}, 300000);