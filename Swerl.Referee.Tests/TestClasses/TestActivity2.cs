﻿using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Tests.TestClasses
{
    public class TestActivity2 : TypedActivity
    {
        public string Id { get; set; }

        public TestActivity2(string id)
        {
            Id = id;
        }
    }
}
