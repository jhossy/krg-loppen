using Krg.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Krg.Domain
{
    public class Registration
    {
        public Registration()
        {
            
        }

        public Registration(EventRegistration eventRegistration)
        {
            BringsTrailer = eventRegistration.BringsTrailer;
            Department = eventRegistration.Department;
            Email = eventRegistration.Email;
            EventDate = eventRegistration.EventDate;
            Name = eventRegistration.Name;
            NoOfAdults = eventRegistration.NoOfAdults;
            NoOfChildren = eventRegistration.NoOfChildren;
            PhoneNo = eventRegistration.PhoneNo;
            ShowName = eventRegistration.ShowName;
        }

        public DateTime EventDate { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public int NoOfAdults { get; set; }

        public int NoOfChildren { get; set; }

        public string PhoneNo { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool BringsTrailer { get; set; }

        public bool ShowName { get; set; }
    }
}
