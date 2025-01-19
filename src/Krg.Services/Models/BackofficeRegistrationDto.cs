using Krg.Domain.Models;
using Krg.Web.Extensions;

namespace Krg.Services.Models
{
    public class BackofficeRegistrationDto
    {
        public BackofficeRegistrationDto(Registration registration)
        {
            Id = registration.Id;
            EventDate = registration.EventDate.ToDkDate();
            UpdateDate = registration.UpdateTimeUtc.ToDkDate();
            Name = registration.Name;
            Department = registration.Department;
            NoOfAdults = registration.NoOfAdults;
            NoOfChildren = registration.NoOfChildren;
            PhoneNo = registration.PhoneNo;
            ShowName = registration.ShowName ? "Ja" : "Nej";
            Email = registration.Email;
            BringsTrailer = registration.BringsTrailer ? "Ja" : "Nej";
            IsCancelled = registration.IsCancelled ? "Ja" : "Nej";
        }

        public int Id { get; }

        public string EventDate { get; }

        public string UpdateDate { get; }

		public string Name { get; } = null!;

		public string Department { get; } = null!;

		public int NoOfAdults { get; }

		public int NoOfChildren { get; }

		public string PhoneNo { get; } = null!;

		public string Email { get; } = null!;

		public string BringsTrailer { get; }

		public string ShowName { get; }

        public string IsCancelled { get; }
	}
}
