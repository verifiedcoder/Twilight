namespace Twilight.Sample.CQRS
{
    public sealed class RegisterUserCommandParameters
    {
        public RegisterUserCommandParameters(string forename, string surname)
        {
            Forename = forename;
            Surname = surname;
        }

        public string Forename { get; }

        public string Surname { get; }
    }
}
