using System;

namespace Citizens
{
    public static class SystemDateTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}
