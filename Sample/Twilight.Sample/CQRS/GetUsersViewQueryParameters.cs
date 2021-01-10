using System;

namespace Twilight.Sample.CQRS
{
    public record GetUsersViewQueryParameters(DateTimeOffset RegistrationDate);
}
