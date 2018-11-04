using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace MP.WindowsServices.Common.ConfigurationHelper
{
    internal class ConfigElementCollection<T> : ConfigurationElementCollection,
                                              IList<T> where T : ConfigurationElement, IConfigurationCollectionElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element).Key;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException($"{typeof(T)} element");
            }

            BaseAdd(element, true);
        }

        public void Clear()
        {
            BaseClear();
        }

        public bool Contains(T element)
        {
            return !(IndexOf(element) < 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        public bool Remove(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException($"{typeof(T)} element");
            }

            BaseRemove(GetElementKey(element));

            return true;
        }

        public bool IsReadOnly => false;

        public int IndexOf(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException($"{typeof(T)} element");
            }

            return BaseIndexOf(element);
        }

        public void Insert(int index, T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException($"{typeof(T)} element");
            }

            BaseAdd(index, element);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public T this[int index]
        {
            get => (T)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }
    }
}
