using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Citizens.Tests
{
    public class TestsBase
    {
        protected readonly DateTime TestTodayDate = new DateTime(2016, 5, 15);

        [TestInitialize]
        public virtual void Initialize()
        {
            SystemDateTime.Now = () => TestTodayDate;
        }
    }
}