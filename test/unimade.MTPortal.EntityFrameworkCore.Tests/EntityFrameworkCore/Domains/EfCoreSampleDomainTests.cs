using unimade.MTPortal.Samples;
using Xunit;

namespace unimade.MTPortal.EntityFrameworkCore.Domains;

[Collection(MTPortalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MTPortalEntityFrameworkCoreTestModule>
{

}
