using System;

namespace Twilight.ES.Tests.Unit
{
    public sealed class WithIdentityAggregate : Aggregate
    {
        public WithIdentityAggregate(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}
