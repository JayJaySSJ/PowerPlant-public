using System;

namespace PowerPlant.Domain
{
    public interface IDateProvider
    {
        DateTime Now { get; }
    }

    public class DateProvider : IDateProvider
    {
        public DateTime Now => DateTime.Now;
    }
}