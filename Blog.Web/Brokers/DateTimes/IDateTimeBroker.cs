using System;

namespace Blog.Web.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetDateTimeOffset();
    }
}
