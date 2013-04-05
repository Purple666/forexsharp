﻿//using TradePlatform.MT4.Data;

using System;
using System.Threading;
using FXSharp.TradingPlatform.Exts;
using TradePlatform.MT4.SDK.API;

namespace FXSharp.EA.FirstTest
{
    public class MyExpertAdvisor : EExpertAdvisor
    {
        private QuoteBeat beat;
        private int count = 0;

        private bool isInitialize;
        private Order order;
        private double threshold = 3;

        protected override int Init()
        {
            beat = new QuoteBeat();
            beat.Add(new Quote(Bid, Ask));

            isInitialize = true;

            return 1;
        }

        protected override int DeInit()
        {
            return 1;
        }

        protected override int Start()
        {
            if (!isInitialize)
            {
                Init();
            }

            // should introduce event and directly buy or sell 
            // should create new object for decision making. 

            while (true)
            {
                RefreshRates();

                beat.Add(new Quote(Bid, Ask));

                PrintReport(beat);

                if (beat.LastDelta >= threshold)
                {
                    if (IsNoOpenOrder())
                    {
                        Console.WriteLine("No Open Order");
                        order = Buy(1);

                        //order.ChangeStopLossInPoints(100);
                        order.ChangeTakeProfitInPoints(40);
                    }
                    else
                    {
                        if (IsAlreadyOpenBuy()) return 1;

                        order.Close();

                        order = Buy(1);
                        //order.ChangeStopLossInPoints(100);
                        order.ChangeTakeProfitInPoints(40);
                    }
                }
                else if (Math.Abs(beat.LastDelta) >= threshold)
                {
                    if (IsNoOpenOrder())
                    {
                        Console.WriteLine("No Open Order");
                        order = Sell(1);
                        //order.ChangeStopLossInPoints(100);
                        order.ChangeTakeProfitInPoints(40);
                    }
                    else
                    {
                        if (IsAlreadyOpenSell()) return 1;

                        order.Close();
                        order = Sell(1);
                        //order.ChangeStopLossInPoints(100);
                        order.ChangeTakeProfitInPoints(40);
                    }
                }

                Thread.Sleep(500);
            }


            return 1;
        }

        private void PrintReport(QuoteBeat beat)
        {
            Console.WriteLine("Delta Bid : {0}, Ask : {1}", beat.DeltaBid, beat.DeltaAsk);
            Console.WriteLine("Max Up : {0}, Down : {1}", beat.MaxUp, beat.MaxDown);
        }

        private bool IsAlreadyOpenSell()
        {
            return order.OrderType == ORDER_TYPE.OP_SELL;
        }

        private bool IsAlreadyOpenBuy()
        {
            return order.OrderType == ORDER_TYPE.OP_BUY;
        }

        private bool IsNoOpenOrder()
        {
            return order == null || !order.IsOpen;
        }
    }
}