using System.IO;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// </summary>
    public partial class StorageSecure
    {
        public static string Load(string filename)
        {
            string content = null;
            try
            {
#if !NETSTANDARD1_0
                if (File.Exists(filename))
                    return File.ReadAllText(filename);
#endif
            }
            catch
            {
                throw;
            }

            return content;
        }

        public static void Save(string data, string filename)
        {
#if !NETSTANDARD1_0
            File.WriteAllText(filename, data);
#endif

        }
    }
}
