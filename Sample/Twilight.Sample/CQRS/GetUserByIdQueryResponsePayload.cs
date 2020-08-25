namespace Twilight.Sample.CQRS
{
    public sealed class GetUserByIdQueryResponsePayload
    {
        public GetUserByIdQueryResponsePayload(int userId, string forename, string surname)
        {
            UserId = userId;
            Forename = forename;
            Surname = surname;
        }

        public int UserId { get; }

        public string Forename { get; }

        public string Surname { get; }
    }
}
