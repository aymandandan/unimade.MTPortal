using Microsoft.AspNetCore.Builder;
using unimade.MTPortal;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("unimade.MTPortal.Web.csproj"); 
await builder.RunAbpModuleAsync<MTPortalWebTestModule>(applicationName: "unimade.MTPortal.Web");

public partial class Program
{
}
