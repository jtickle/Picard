using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picard
{
    using DeltaDict = Dictionary<string, int>;
    using IDeltaDict = IDictionary<string, int>;

    public static class DeltaTools
    {
        public static IDeltaDict Create()
        {
            return new DeltaDict();
        }

        public static IDeltaDict Add(IDeltaDict l, IDeltaDict r)
        {
            var Result = Create();

            var allKeys = l.Keys.Union(r.Keys);
            foreach(var key in allKeys)
            {
                var iHave = l.ContainsKey(key);
                var uHave = r.ContainsKey(key);

                if(iHave && uHave)
                {
                    Result.Add(key, l[key] + r[key]);
                }
                else if(iHave && !uHave)
                {
                    Result.Add(key, l[key]);
                }
                else if(!iHave && uHave)
                {
                    Result.Add(key, r[key]);
                }
                else
                {
                    Result.Add(key, 0);
                }
            }

            return Result;
        }

        internal static void AddMat(IDeltaDict d, string mat, int qty)
        {
            if(d.ContainsKey(mat))
            {
                d[mat] += qty;
            }
            else
            {
                d.Add(mat, qty);
            }
        }
    }
}
