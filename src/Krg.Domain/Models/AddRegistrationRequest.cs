﻿using System.ComponentModel.DataAnnotations;

namespace Krg.Domain.Models
{
    public class AddRegistrationRequest
    {
        public DateTime EventDate { get; set; }
        
        [Required(ErrorMessage = Constants.NameRequiredMessage)]        
        public required string Name { get; set; }
        
        [EmailAddress(ErrorMessage = Constants.EmailInvalidFormat)]
        [Required(ErrorMessage = Constants.EmailRequiredMessage)]
        public required string Email { get; set; }
        
        public int NoOfAdults { get; set; } = 1;
        
        public int NoOfChildren { get; set; } = 1;
        
        [Required(ErrorMessage = Constants.PhoneNoRequiredMessage)]
        public required string PhoneNo { get; set; }
        
        [Required(ErrorMessage = Constants.DepartmentRequiredMessage)]
        public required string Department { get; set; }
        
        public bool BringsTrailer { get; set; } = false;
        
        public bool ShowName { get; set; } = false;

        public required string ContactName { get; set; }

        public required string ContactEmail { get; set; }

        public required string ContactPhone { get; set; }

	}
}
