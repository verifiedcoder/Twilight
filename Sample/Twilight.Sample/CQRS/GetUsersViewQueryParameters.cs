using System;

namespace Twilight.Sample.CQRS
{
    public sealed class GetUsersViewQueryParameters
    {
        public GetUsersViewQueryParameters(DateTimeOffset registrationDate) => RegistrationDate = registrationDate;

        public DateTimeOffset RegistrationDate { get; }
    }
}
