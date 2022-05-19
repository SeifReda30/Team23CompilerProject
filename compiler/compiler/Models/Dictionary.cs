using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace compiler.Models
{
    public class Dictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {

        public struct Entry
        {
            public TKey key;           // Key of entry
            public TValue value;         // Value of entry
        }
        private const int numberOfWords = 22;
        private int count=0;
        private Entry[] entries = new Entry[numberOfWords];


        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            entries[count].key = key;
            entries[count++].value = value;
        }
        public bool Contains(TKey key)
        {
            for (int i = 0; i < numberOfWords; i++)
            {
                if (entries[i].key.Equals(key))
                    return true;
            }
            return false;
        }

       
        public TValue getValue(TKey key)
        {
            for (int i = 0; i < numberOfWords; i++)
            {
                if (entries[i].key.Equals(key))
                {
                    return entries[i].value;
                }
            }
            return default;
        }
      

      
      

   

        public bool Remove(TKey key)
        {
            for (int i = 0; i < numberOfWords; i++)
            {
                if (entries[i].key.Equals(key))
                {
                    entries[i].key = default;
                    entries[i].value = default;
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < numberOfWords; i++)
            {
               entries[i].key = default;
               entries[i].value = default;
                
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }



    }
}
