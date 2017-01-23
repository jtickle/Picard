using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibEDJournal.State
{
    [Serializable]
    public class InventorySet : Dictionary<string, int>
    {
        public InventorySet() : base()
        {
        }

        public InventorySet(IDictionary<string, int> prev) : base(prev)
        {
        }

        public bool IsNegative
        {
            get
            {
                foreach (var val in Values)
                {
                    if (val < 0)
                        return true;
                }

                return false;
            }

            protected set { }
        }

        public bool IsZero
        {
            get
            {
                foreach (var val in Values)
                {
                    if (val != 0)
                        return false;
                }

                return true;
            }

            protected set { }
        }

        public void AddMat(string mat, int qty)
        {
            if (ContainsKey(mat))
                this[mat] += qty;
            else
                Add(mat, qty);
        }


        /// <summary>
        /// Get the value of a material if set, or zero otherwise
        /// </summary>
        /// <param name="Mat">The material name to look for</param>
        /// <returns></returns>
        public int GetMat(string Mat)
        {
            return ContainsKey(Mat) ? this[Mat] : 0;
        }

        public InventorySet Negate()
        {
            var Result = new InventorySet();
            foreach (var v in this)
            {
                Result.Add(v.Key, -v.Value);
            }
            return Result;
        }

        public bool ExactlyEquals(InventorySet other)
        {
            foreach(var key in Keys.Union(other.Keys))
            {
                // No match if either set is missing the key
                // or if the values don't match
                if (!Keys.Contains(key) || !other.Keys.Contains(key)
                    || this[key] != other[key])
                {
                    return false;
                }
            }

            return true;
        }

        public static InventorySet operator +(InventorySet left, KeyValuePair<string, int> right)
        {
            var Result = new InventorySet(left);

            if (left.ContainsKey(right.Key))
            {
                Result[right.Key] += right.Value;
            }
            else
            {
                Result.Add(right.Key, right.Value);
            }

            return Result;
        }

        public static InventorySet operator +(InventorySet left, InventorySet right)
        {
            var Result = new InventorySet();

            // Join together the set of keys from both and loop over them
            foreach (var key in left.Keys.Union(right.Keys))
            {
                var lHas = left.ContainsKey(key);
                var rHas = right.ContainsKey(key);

                if (lHas)
                {
                    Result.AddMat(key, left[key]);
                }

                if (rHas)
                {
                    Result.AddMat(key, right[key]);
                }
            }

            return Result;
        }

        public static InventorySet operator -(InventorySet left, InventorySet right)
        {
            return left + right.Negate();
        }
    }
}
