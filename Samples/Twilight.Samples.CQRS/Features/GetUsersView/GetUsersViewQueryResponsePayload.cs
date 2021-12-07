using Twilight.Samples.CQRS.Views;

namespace Twilight.Samples.CQRS.Features.GetUsersView;

public sealed record GetUsersViewQueryResponsePayload(IEnumerable<UserView> Users);