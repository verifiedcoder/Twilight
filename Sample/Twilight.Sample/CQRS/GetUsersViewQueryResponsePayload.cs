using System.Collections.Generic;
using Twilight.Sample.Views;

namespace Twilight.Sample.CQRS
{
    public sealed class GetUsersViewQueryResponsePayload
    {
        public GetUsersViewQueryResponsePayload(IEnumerable<UserView> users) => UsersView = new List<UserView>(users);

        public List<UserView> UsersView { get; }
    }
}
