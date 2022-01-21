using Twilight.Samples.Common.Views;

namespace Twilight.Samples.Common.Features.GetUsersView;

public sealed record GetUsersViewQueryResponsePayload(IEnumerable<UserView> Users);
