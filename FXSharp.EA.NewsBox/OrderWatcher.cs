﻿using FXSharp.TradingPlatform.Exts;
using System;

namespace FXSharp.EA.NewsBox
{
    public class OrderWatcher
    {
        IOrderState state;
        public event EventHandler OrderClosed;
        private ExpiracyTimer expiracyTimer;
        private double expiredTime;
        private MagicBoxConfig mbConfig;

        public string Symbol { get; set; }

        public OrderWatcher(Order buyOrder, Order sellOrder, double expiredTime, MagicBoxConfig config)
        {
            mbConfig = config;
            AddOneCancelAnother(buyOrder, sellOrder);
            Symbol = buyOrder.Symbol;
            
            this.expiredTime = expiredTime;

            expiracyTimer = new ExpiracyTimer(expiredTime / 2);
            expiracyTimer.Expired += expiracyTimer_Expired;
        }

        void expiracyTimer_Expired(object sender, EventArgs e)
        {
            if (state == null) return;
            
            state.Cancel();
            expiracyTimer.Expired -= expiracyTimer_Expired;
        }

        internal void ManageOrder()
        {
            if (state == null) return;

            state.Manage();
        }

        private void AddOneCancelAnother(Order buyOrder, Order sellOrder)
        {
            state = new MagicBoxCreated(this, buyOrder, sellOrder, mbConfig);
        }

        // we should use event based for this
        internal void OrderRunning(Order order, ITrailingMethod trailing)
        {
            ResetExpiracy();

            state = new OrderAlreadyRunning(this, order, trailing);
        }

        private void ResetExpiracy()
        {
            expiracyTimer.Finish();
            expiracyTimer.Expired -= expiracyTimer_Expired;
            expiracyTimer = new ExpiracyTimer(expiredTime);
            expiracyTimer.Expired += expiracyTimer_Expired;
        }

        // we should use event based for this
        internal void MagicBoxCompleted()
        {
            // should create default state
            expiracyTimer.Finish();
            expiracyTimer.Expired -= expiracyTimer_Expired;

            state = null;
            OnOrderClosed();
        }

        private void OnOrderClosed()
        {
            if (OrderClosed == null) return;
            OrderClosed(this, EventArgs.Empty);
        }
    }
}
