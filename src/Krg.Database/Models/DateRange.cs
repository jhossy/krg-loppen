namespace Krg.Database.Models;

public class DateRange
{
    public DateOnly StartDate { get; }
    
    public DateOnly EndDate { get; }
    
    public DateRange(DateOnly start, DateOnly end)
    {
        if(end < start)
            throw new ArgumentException("end must be greater or equal to start");
        StartDate = start;
        EndDate = end;
    }
}