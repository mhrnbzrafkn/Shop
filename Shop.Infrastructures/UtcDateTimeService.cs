namespace Shop.Infrastructures
{
    public class UtcDateTimeService : DateTimeService
    {
        public DateTime Today => DateTime.Now.Date.ToUniversalTime();

        public DateTime Now => DateTime.Now.ToUniversalTime();
    }

    public interface DateTimeService
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
