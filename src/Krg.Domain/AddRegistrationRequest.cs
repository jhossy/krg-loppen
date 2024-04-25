using System.ComponentModel.DataAnnotations;

namespace Krg.Domain
{
	public class AddRegistrationRequest
	{
        public int UmbracoNodeId { get; set; }
        public DateTime EventDate { get; set; }
        [Required(ErrorMessage = Constants.NameRequiredMessage)]
        public required string Name { get; set; }
        [EmailAddress]
		[Required(ErrorMessage = Constants.EmailRequiredMessage)]
		public required string Email { get; set; }
        public int NoOfAdults { get; set; } = 1;
        public int NoOfChildren { get; set; } = 1;
        public string? PhoneNo { get; set; }
        [Required(ErrorMessage = Constants.DepartmentRequiredMessage)]
        public required string Department { get; set; }
        public bool BringsTrailer { get; set; } = false;
        public bool ShowName { get; set; } = false;

    }
}
