using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picard
{
    /// <summary>
    /// This only exists because I need to get the same kind of data out of both
    /// possible main application forms
    /// </summary>
    public interface IGetData
    {
        /// <summary>
        /// Get the Deltas that the user has approved of
        /// </summary>
        /// <returns>
        /// Materials Dictionary of user-approved materials deltas
        /// </returns>
        IDictionary<string, int> GetDeltas();

        IDictionary<string, int> GetTotals();

        /// <summary>
        /// Determine if the State should be Saved
        /// </summary>
        /// <returns>
        /// For instance, if the user has made any changes.
        /// </returns>
        bool ShouldSave();
    }
}
