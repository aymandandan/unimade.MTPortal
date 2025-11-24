using Xunit;

namespace unimade.MTPortal.EntityFrameworkCore;

[CollectionDefinition(MTPortalTestConsts.CollectionDefinitionName)]
public class MTPortalEntityFrameworkCoreCollection : ICollectionFixture<MTPortalEntityFrameworkCoreFixture>
{

}
