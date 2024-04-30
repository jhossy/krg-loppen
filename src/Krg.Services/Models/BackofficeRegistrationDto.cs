using Krg.Domain.Models;
using Krg.Web.Extensions;

namespace Krg.Domain
{
    public class BackofficeRegistrationDto
    {
        public BackofficeRegistrationDto(Registration registration)
        {
            EventDate = registration.EventDate.ToDkDate();
            Name = registration.Name;
            Department = registration.Department;
            NoOfAdults = registration.NoOfAdults;
            NoOfChildren = registration.NoOfChildren;
            PhoneNo = registration.PhoneNo;
            ShowName = registration.ShowName ? "Ja" : "Nej";
            Email = registration.Email;
            BringsTrailer = registration.BringsTrailer ? "Ja" : "Nej";
        }

        public string EventDate { get; }

		public string Name { get; } = null!;

		public string Department { get; } = null!;

		public int NoOfAdults { get; }

		public int NoOfChildren { get; }

		public string PhoneNo { get; } = null!;

		public string Email { get; } = null!;

		public string BringsTrailer { get; }

		public string ShowName { get; }
	}
}
