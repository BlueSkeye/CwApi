using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwApi
{
    public class ExtensionDataHandler<T> : IDictionary<string, object>
    {
        private Dictionary<string, object> _container = new Dictionary<string, object>();
        public static Dictionary<Type, List<string>> _propertiesPerType =
            new Dictionary<Type, List<string>>();

        public ExtensionDataHandler()
        {
            if (!_propertiesPerType.ContainsKey(typeof(T))) {
                _propertiesPerType.Add(typeof(T), new List<string>());
            }
        }

        public object this[string key]
        {
            get => _container[key];
            set
            {
                List<string> knownProperties = _propertiesPerType[typeof(T)];
                if (!knownProperties.Contains(key)) {
                    Console.WriteLine("Found property '{0}' on type '{1}'",
                        key, typeof(T).FullName);
                    knownProperties.Add(key);
                }
                _container[key] = value;
            }
        }

        public ICollection<string> Keys => _container.Keys;

        public ICollection<object> Values => _container.Values;

        public int Count => _container.Count;

        public bool IsReadOnly => false;

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(string key, object value)
        {
            List<string> knownProperties = _propertiesPerType[typeof(T)];
            if (!knownProperties.Contains(key)) {
                Console.WriteLine("Found property '{0}' on type '{1}'",
                    key, typeof(T).FullName);
                knownProperties.Add(key);
            }
            _container.Add(key, value);
        }

        public void Clear()
        {
            _container.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _container.ContainsKey(item.Key);
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _container.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _container.Remove(item.Key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
        {
            return _container.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _container.GetEnumerator();
        }
    }
}
