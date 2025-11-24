using unimade.MTPortal.Samples;
using Xunit;

namespace unimade.MTPortal.EntityFrameworkCore.Applications;

[Collection(MTPortalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MTPortalEntityFrameworkCoreTestModule>
{

}
