﻿//using TradePlatform.MT4.Data;

using FXSharp.TradingPlatform.Exts;

namespace FXSharp.EA.FirstTest
{
    public class ModifyStopTest : EExpertAdvisor
    {
        private int count;
        private Order order;

        protected override int Init()
        {
            order = Buy(0.1, Bid - 100*Point, Bid + 500*Point);

            return 1;
        }

        protected override int DeInit()
        {
            return 1;
        }

        protected override int Start()
        {
            count++;

            if (order == null)
            {
                Init();
            }
            else
            {
                order.ModifyStopLoss(Ask - (count*100*Point));
            }

            if (order.CloseInProfit())
                order = null;
            return 1;
        }
    }
}