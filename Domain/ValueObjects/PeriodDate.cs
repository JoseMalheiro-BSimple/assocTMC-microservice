namespace Domain.ValueObjects;
public class PeriodDate
{
    public DateOnly InitDate { get; set; }
    public DateOnly FinalDate { get; set; }

    public PeriodDate() { }

    public PeriodDate(DateOnly InitDate, DateOnly FinalDate)
    {
        if (InitDate > FinalDate)
            throw new ArgumentException("Invalid Arguments");
        this.InitDate = InitDate;
        this.FinalDate = FinalDate;
    }
}

