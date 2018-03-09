using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// </summary>
    public partial class StorageSecure
    {
        /// <summary>
        /// Filename for secure storage
        /// </summary>
        /// string readonly SSFILENAME
        public static string StorageSecureFilename
        {
            get;
            set;
        }

        /// <summary>
        /// Cache Key for secure storage
        /// </summary>
        /// const string CACHEKEY_KEY
        public static string StorageSecureCacheKey
        {
            get;
            set;
        }
    }
}
