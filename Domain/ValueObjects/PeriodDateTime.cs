namespace Domain.ValueObjects;
public class PeriodDateTime
{
    public DateTime _initDate { get; set; }
    public DateTime _finalDate { get; set; }

    public PeriodDateTime(DateTime initDate, DateTime finalDate)
    {
        if (initDate < finalDate) { 
            _initDate = initDate;
            _finalDate = finalDate;
        }
        else
            throw new ArgumentException("Invalid Arguments");
    }

    public PeriodDateTime(){ }

    public PeriodDateTime(PeriodDate periodDate) : this(
        periodDate.InitDate.ToDateTime(TimeOnly.MinValue),
        periodDate.FinalDate.ToDateTime(TimeOnly.MinValue))
    {
    }
}
