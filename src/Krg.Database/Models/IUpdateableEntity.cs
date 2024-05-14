namespace Krg.Database.Models
{
	public interface IUpdateableEntity
    {
		DateTime UpdateTimeUtc { get; set; }
	}
}
