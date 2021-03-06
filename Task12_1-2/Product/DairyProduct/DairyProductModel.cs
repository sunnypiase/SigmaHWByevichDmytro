using System;
using System.Collections.Generic;
using System.Text;
using Task11.FileHandler;
using Task11.Product.General;

namespace Task11.Product
{
    internal class DairyProductModel : FoodProductBase, IDairyProduct, ITXTSerializer
    {
        #region Props

        #endregion
        #region Ctors
        public DairyProductModel() :
            this(default, default, default, default, default)

        { }

        public DairyProductModel(string name, double price, double weight, DateTime expirationTime, SortedDictionary<int, int> daysToExpirationAndPresentOfChange) :
            base(name, price, weight, expirationTime, daysToExpirationAndPresentOfChange)
        { }

        public DairyProductModel(DairyProductModel other) :
            this(other.Name, other.Price, other.Weight, other.ExpirationTime, other._daysToExpirationAndPresentOfChange)
        { }
        #endregion
        #region Methods
        public override object Clone()
        {
            return new DairyProductModel(this);
        }
        public string SerializeTxt()
        {
            StringBuilder sb = new();
            sb.Append('{');
            foreach (KeyValuePair<int, int> item in _daysToExpirationAndPresentOfChange)
            {
                sb.Append($"({item.Key} {item.Value})");
            }
            sb.Append('}');
            return $"<DairyProduct>;<Name: {Name}>;<Price: {_price}>;<Weight: {Weight}>;<ExpirationTime: {ExpirationTime:d}>;<DaysToExpirationAndPresentOfChange: {sb}>;";
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
