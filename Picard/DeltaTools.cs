﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picard
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

        /// <summary>
        /// Adds a material to a delta dictionary
        /// </summary>
        /// <param name="d">The dictionary to which to add</param>
        /// <param name="mat">The Inara.cz material name</param>
        /// <param name="qty">The quantity</param>
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
