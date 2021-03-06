using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task11.FileHandler;

namespace Task11
{
    internal class ProductStorage<T> : IList<T>, ITXTSerializer
        where T : IProduct
    {
        #region Props
        private List<T> _products;
        /// <summary>
        /// перевіряє чи підпадає під задані умови продукт перед його додаванням
        /// </summary>
        public event Predicate<T> OnProductPreAddFaceControl;
        /// <summary>
        /// у випадку, коли продукт не підпадає під умови додавання викликається ця подія, повинна записувати лог у файл
        /// </summary>
        public event Action<string> OnBadProductLogger;
        public double Pirice => _products.Select(product => product.Price).Sum();
        public double MaxPrice => _products.Select(product => product.Price).Max();
        #endregion
        #region Ctors
        public ProductStorage()
        {
            _products = new List<T>();
        }
        public ProductStorage(IEnumerable<T> products) : this()
        {
            foreach (T product in products)
            {
                if (OnProductPreAddFaceControl?.Invoke(product) ?? true)
                {
                    _products.Add((T)product.Clone());
                }
                else
                {
                    if (product is ITXTSerializer serializerProduct)
                    {
                        OnBadProductLogger?.Invoke(serializerProduct.SerializeTxt() + "<Describe : Продукт не підпадає під умови додавання>;");
                    }
                    else
                    {
                        OnBadProductLogger?.Invoke("<NonTXTSerializableProduct>;" + product.ToString() + "<Describe : Продукт не підпадає під умови додавання>;");
                    }
                }
            }
        }
        #endregion
        #region IList
        public T this[int index]
        {
            get => (T)_products[index].Clone();
            set => _products[index] = (T)value.Clone();
        }

        public int Count => _products.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (OnProductPreAddFaceControl?.Invoke(item) ?? true)
            {
                _products.Add((T)item.Clone());
            }
            else
            {
                if (item is ITXTSerializer serializerProduct)
                {
                    OnBadProductLogger?.Invoke(serializerProduct.SerializeTxt() + "<Describe : Продукт не підпадає під умови додавання>;");
                }
                else
                {
                    OnBadProductLogger?.Invoke("<NonTXTSerializableProduct>;" + item.ToString() + "<Describe : Продукт не підпадає під умови додавання>;");
                }
            }
        }

        public void Clear()
        {
            _products.Clear();
        }

        public bool Contains(T item)
        {
            return _products.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _products.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _products.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _products.GetEnumerator();
        }
        public int IndexOf(T item)
        {
            return _products.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (OnProductPreAddFaceControl?.Invoke(item) ?? true)
            {
                _products.Insert(index, (T)item.Clone());
            }
            else
            {
                if (item is ITXTSerializer serializerProduct)
                {
                    OnBadProductLogger?.Invoke(serializerProduct.SerializeTxt() + "<Describe : Продукт не підпадає під умови додавання>;");
                }
                else
                {
                    OnBadProductLogger?.Invoke("<NonTXTSerializableProduct>;" + item.ToString() + "<Describe : Продукт не підпадає під умови додавання>;");
                }
            }
        }

        public bool Remove(T item)
        {
            return (_products.Remove(item));
        }

        public void RemoveAt(int index)
        {
            _products.RemoveAt(index);
        }

        #endregion
        #region Methods
        public IEnumerable<G> GetAll<G>() where G : T
        {
            foreach (T item in _products)
            {
                if (item is G result)
                {
                    yield return result;
                }
            }
        }
        public IEnumerable<G> GetAll<G>(Predicate<G> predicate) where G : T
        {
            foreach (T item in _products)
            {
                if (item is G result && predicate(result))
                {
                    yield return result;
                }
            }
        }
        public void Sort()
        {
            _products.Sort();
        }
        public void Sort(IComparer<T> comparer)
        {
            _products.Sort(comparer);
        }

        public string SerializeTxt()
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in _products)
            {
                if (item is ITXTSerializer serializer)
                {
                    sb.AppendLine(serializer.SerializeTxt());
                }
                else
                {
                    sb.AppendLine(item.ToString());
                }
            }
            return sb.ToString();
        }

        #endregion
        #region ObjectOverrides
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (IProduct product in _products)
            {
                sb.AppendLine(product.ToString());
            }
            return sb.ToString();
        }



        #endregion

    }
}
