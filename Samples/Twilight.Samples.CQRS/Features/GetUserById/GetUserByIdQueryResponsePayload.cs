namespace Twilight.Samples.CQRS.Features.GetUserById;

public sealed record GetUserByIdQueryResponsePayload(int UserId, string Forename, string Surname);
