using System.ComponentModel.DataAnnotations.Schema;

namespace Krg.Database.Models
{
    public class EventRegistration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime UpdateTimeUtc { get; set; }

        public DateTime EventDate { get; set; }
        
        public string Name { get; set; }

        public string Department { get; set; }

        public int NoOfAdults { get; set; }

        public int NoOfChildren { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public bool BringsTrailer { get; set; }

        public bool ShowName { get; set; }
    }
}
