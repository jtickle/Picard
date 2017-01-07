using System.Collections.Generic;
using System.Linq;

namespace Picard.Lib
{
    using DeltaDict = Dictionary<string, int>;
    using IDeltaDict = IDictionary<string, int>;

    /// <summary>
    /// Tools for operating upon a dictionary of materials and deltas.
    /// </summary>
    public static class DeltaTools
    {
        /// <summary>
        /// Creates a new, empty delta dictionary
        /// </summary>
        /// <returns>
        /// A new, empty delta dictionary
        /// </returns>
        public static IDeltaDict Create()
        {
            return new DeltaDict();
        }

        /// <summary>
        /// Adds together two delta dictionaries
        /// </summary>
        /// <param name="l">The first dict to add</param>
        /// <param name="r">The second dict to add</param>
        /// <returns>A dictionary containing the strings from l and r and the sum of their counts</returns>
        public static IDeltaDict Add(IDeltaDict l, IDeltaDict r)
        {
            var Result = Create();

            // Join together the set of keys from both and loop over them
            foreach(var key in l.Keys.Union(r.Keys))
            {
                var iHave = l.ContainsKey(key);
                var uHave = r.ContainsKey(key);

                if(iHave && uHave)
                {
                    AddMat(Result, key, l[key] + r[key]);
                }
                else if(iHave && !uHave)
                {
                    AddMat(Result, key, l[key]);
                }
                else if(!iHave && uHave)
                {
                    AddMat(Result, key, r[key]);
                }
                else
                {
                    AddMat(Result, key, 0);
                }
            }

            return Result;
        }

        public static IDeltaDict ZeroMinus(IDeltaDict d)
        {
            var Result = Create();
            foreach (var v in d)
            {
                Result.Add(v.Key, -v.Value);
            }
            return Result;
        }

        public static IDeltaDict Subtract(IDeltaDict l, IDeltaDict r)
        {
            return Add(l, ZeroMinus(r));
        }

        /// <summary>
        /// Adds a material to a delta dictionary
        /// </summary>
        /// <param name="d">The dictionary to which to add</param>
        /// <param name="mat">The Inara.cz material name</param>
        /// <param name="qty">The quantity</param>
        public static void AddMat(IDeltaDict d, string mat, int qty)
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

        public static bool IsZero(IDeltaDict d)
        {
            foreach(var val in d.Values)
            {
                if(val != 0)
                    return false;
            }

            return true;
        }

        public static bool IsNegative(IDeltaDict d)
        {
            foreach(var val in d.Values)
            {
                if (val < 0)
                    return true;
            }

            return false;
        }

        public static bool IsExactlyEqual(IDeltaDict a, IDeltaDict b)
        {
            bool currentMatch = true;

            foreach (var key in a.Keys.Union(b.Keys))
            {
                // No match if either set is missing the key
                if (!a.Keys.Contains(key))
                {
                    currentMatch = false;
                }
                else if (!b.Keys.Contains(key))
                {
                    currentMatch = false;
                }
                // And no match if they are both set but the values are different
                else if (a[key] != b[key])
                {
                    currentMatch = false;
                }
            }

            return currentMatch;
        }
    }
}
