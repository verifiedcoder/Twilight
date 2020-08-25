using System;

namespace Twilight.Sample.Views
{
    public class UserView
    {
        public UserView(int viewId, int userId, string forename, string surname, string fullName, DateTimeOffset registrationDate)
        {
            ViewId = viewId;
            UserId = userId;
            Forename = forename;
            Surname = surname;
            FullName = fullName;
            RegistrationDate = registrationDate;
        }

        public int ViewId { get; }

        public int UserId { get; }

        public string Forename { get; }

        public string Surname { get; }

        public string FullName { get; }

        public DateTimeOffset RegistrationDate { get; }
    }
}
