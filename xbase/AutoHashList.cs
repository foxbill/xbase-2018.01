using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase
{
    public class AutoHashList<TKey,TValue>:Dictionary<TKey,TValue>
    {
        public void Put(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
                this[key] = value;
            else
                this.Add(key, value);
        }
    }
}
