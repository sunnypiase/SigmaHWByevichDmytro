﻿using System;
using System.Collections.Generic;
using Task11.Product.General;

namespace Task11.Product
{
    internal class DairyProductModel : FoodProductBase, IDairyProduct
    {
        #region Props

        #endregion

        #region Ctors
        public DairyProductModel() :
            this(default, default, default, default, default)
        { }

        public DairyProductModel(string name, double price, double weight, DateTime expirationTime, SortedDictionary<int, int> daysToExpirationAndPresentOfChange)
        {
            Name = name;
            Price = price;
            Weight = weight;
            ExpirationTime = expirationTime;

            _daysToExpirationAndPresentOfChange = new SortedDictionary<int, int>();

            foreach (var item in daysToExpirationAndPresentOfChange)
            {
                _daysToExpirationAndPresentOfChange.Add(item.Key, item.Value);
            }
        }

        public DairyProductModel(DairyProductModel other) :
            this(other.Name, other.Price, other.Weight, other.ExpirationTime, other._daysToExpirationAndPresentOfChange)
        { }
        #endregion

        #region Methods
        protected override double GetPriceByExpiration()
        {
            return base.GetPriceByExpiration();
        }

        public override void ChangePrice(int present)
        {
            base.ChangePrice(present);
        }

        public override int CompareTo(object obj)
        {
            return base.CompareTo(obj);
        }

        public override object Clone()
        {
            return new DairyProductModel(this);
        }

        #endregion

        #region ObjectOverrides
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
