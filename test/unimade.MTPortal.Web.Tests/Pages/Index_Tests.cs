using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace unimade.MTPortal.Pages;

[Collection(MTPortalTestConsts.CollectionDefinitionName)]
public class Index_Tests : MTPortalWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
