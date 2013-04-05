﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FXSharp.EA.NewsBox.Specs
{
    [TestFixture]
    public class When_decide_the_order_detail
    {
        [Test]
        public void Should_detect_collided_news()
        {
            DateTime time = DateTime.Now;

            var evtList = new List<EconomicEvent>
                {
                    new EconomicEvent {Currency = "USD", DateTime = time},
                    new EconomicEvent {Currency = "USD", DateTime = time}
                };

            IEnumerable<IGrouping<DateTime, EconomicEvent>> groups = evtList.GroupBy(x => x.DateTime);

            Assert.AreEqual(1, groups.Count());
            groups.First();

            foreach (var grp in groups)
            {
                Console.WriteLine(grp.Key);
                foreach (EconomicEvent item in grp)
                {
                    Console.WriteLine(item.Currency);
                }
            }
        }

        [Test]
        public void Should_detect_nominal_news()
        {
            var eEvnt = new EconomicEvent {Previous = "293"};

            Assert.IsFalse(eEvnt.IsSpeechOrMeeting);
        }

        [Test]
        public void Should_detect_speech_and_conf()
        {
            var eEvnt = new EconomicEvent {};

            Assert.IsTrue(eEvnt.IsSpeechOrMeeting);
        }
    }
}