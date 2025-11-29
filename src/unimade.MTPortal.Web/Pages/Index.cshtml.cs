using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using unimade.MTPortal.Roles;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TenantManagement;
using Volo.Abp.Users;
using Volo.Abp.MultiTenancy;
using unimade.MTPortal.Dashboards;

namespace unimade.MTPortal.Web.Pages;

public class IndexModel : MTPortalPageModel
{
    private readonly ITenantAppService _tenantAppService;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantStore _tenantStore;
    private readonly IDashboardAppService _dashboardAppService;

    public List<TenantDto> AvailableTenants { get; set; } = new();
    public bool IsAdmin { get; set; }
    public bool IsHostAdmin { get; set; }
    public bool IsTenantAdmin { get; set; }
    public bool IsHostUser { get; set; }
    public string WelcomeMessage { get; set; } = string.Empty;
    public SystemInfoDto SystemInfo { get; set; } = new();
    public bool IsUserAuthenticated => _currentUser.IsAuthenticated;
    public string UserName => _currentUser.UserName ?? string.Empty;

    public IndexModel(
        ITenantAppService tenantAppService, 
        ICurrentUser currentUser, 
        ITenantStore tenantStore,
        IDashboardAppService dashboardAppService)
    {
        _tenantAppService = tenantAppService;
        _currentUser = currentUser;
        _tenantStore = tenantStore;
        _dashboardAppService = dashboardAppService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        // Check if user is admin and determine type
        IsAdmin = _currentUser.IsInRole("admin");
        IsHostAdmin = IsAdmin && !CurrentTenant.IsAvailable;
        IsTenantAdmin = IsAdmin && CurrentTenant.IsAvailable;
        IsHostUser = !IsAdmin && !CurrentTenant.IsAvailable;

        if (!_currentUser.IsAuthenticated || !CurrentTenant.IsAvailable || IsAdmin)
        {
            await LoadDataAsync();
            return Page();
        }

        // Redirect authenticated non-admin users
        if (_currentUser.IsInRole(StaffRole.Name))
            return RedirectToPage("/Internal/Dashboard/Index");

        if (_currentUser.IsInRole(PublicRole.Name))
            return RedirectToPage("/External/Index");

        return RedirectToPage("/Account/AccessDenied");
    }

    private async Task LoadDataAsync()
    {
        if (_currentUser.IsAuthenticated && IsAdmin)
        {
            // Load admin-specific data
            WelcomeMessage = $"Welcome back, {_currentUser.Name ?? _currentUser.UserName}!";
            SystemInfo = await GetSystemInfoAsync();
        }
        else if (!_currentUser.IsAuthenticated || IsHostUser)
        {
            // Load tenants for unauthenticated users OR authenticated host non-admin users
            await LoadAvailableTenantsAsync();
        }
    }

    private async Task LoadAvailableTenantsAsync()
    {
        try
        {
            // For unauthenticated users, we need to get available tenants
            // Using ITenantStore to get tenant information directly
            var tenants = await _tenantAppService.GetListAsync(new GetTenantsInput
            {
                MaxResultCount = 100 // Reasonable limit for tenant selection
            });

            // Filter tenants that are available and accessible
            // In ABP, we check if tenants are active by trying to get their configuration
            var availableTenants = new List<TenantDto>();

            foreach (var tenant in tenants.Items)
            {
                try
                {
                    // Check if tenant configuration can be retrieved (indicates active tenant)
                    var tenantConfiguration = await _tenantStore.FindAsync(tenant.Name);
                    if (tenantConfiguration != null)
                    {
                        availableTenants.Add(tenant);
                    }
                }
                catch
                {
                    // Tenant is not accessible, skip it
                    continue;
                }
            }

            AvailableTenants = availableTenants;
        }
        catch (Exception ex)
        {
            // Log error and show empty tenant list
            Logger.LogWarning(ex, "Failed to load tenant list");
            AvailableTenants = new List<TenantDto>();
        }
    }

    private async Task<SystemInfoDto> GetSystemInfoAsync()
    {
        try
        {
            // For tenant admins, always show "Healthy" status
            var systemStatus = IsTenantAdmin ? "Healthy" : "Healthy";

            if (IsHostAdmin)
            {
                // Host admin sees tenant management
                var tenants = await _tenantAppService.GetListAsync(new GetTenantsInput());
                var activeTenants = tenants.Items.Count;

                return new SystemInfoDto
                {
                    TotalTenants = activeTenants,
                    ActiveUsers = await GetActivePublicUserCountAsync(),
                    SystemStatus = systemStatus,
                    LastUpdate = Clock.Now
                };
            }
            else if (IsTenantAdmin)
            {
                // Tenant admin sees announcements data
                var announcementCount = await GetAnnouncementCountAsync();

                return new SystemInfoDto
                {
                    TotalTenants = announcementCount, // Reusing TotalTenants property for announcements count
                    ActiveUsers = await GetActivePublicUserCountAsync(),
                    SystemStatus = systemStatus,
                    LastUpdate = Clock.Now
                };
            }

            // Fallback for other admin types
            return new SystemInfoDto
            {
                TotalTenants = 0,
                ActiveUsers = 0,
                SystemStatus = systemStatus,
                LastUpdate = Clock.Now
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to get system info");
            return new SystemInfoDto
            {
                TotalTenants = 0,
                ActiveUsers = 0,
                SystemStatus = "Healthy", // Always healthy for admins
                LastUpdate = Clock.Now
            };
        }
    }

    private async Task<int> GetActivePublicUserCountAsync()
    {
        return (int)(await _dashboardAppService.GetStatsAsync()).TotalPublicUsers;
    }

    private async Task<int> GetAnnouncementCountAsync()
    {
        return (int)(await _dashboardAppService.GetStatsAsync()).TotalAnnouncements;
    }
}

public class SystemInfoDto
{
    public int TotalTenants { get; set; }
    public int ActiveUsers { get; set; }
    public string SystemStatus { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; }
}