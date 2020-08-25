namespace Twilight.Sample.CQRS
{
    public sealed class GetUserByIdQueryParameters
    {
        public GetUserByIdQueryParameters(int userId) => UserId = userId;

        public int UserId { get; }
    }
}
