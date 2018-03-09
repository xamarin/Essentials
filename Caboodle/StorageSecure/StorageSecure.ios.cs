using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Foundation;
using Security;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// </summary>
    public partial class StorageSecure
    {
        public static string Load(string filename)
        {
            var kc = new KeyChain();

            return kc.ValueForKey(filename);
        }

        public static void Save(string data, string filename)
        {
            var kc = new KeyChain();
            kc.SetValueForKey(data, filename);

            return;
        }
    }

    public class KeyChain
    {
        private static SecRecord ExistingRecordForKey(string key)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = StorageSecure.StorageSecureFilename,
                Label = key,
            };
        }

        public string ValueForKey(string key)
        {
            var record = ExistingRecordForKey(key);
            SecStatusCode resultCode;
            var match = SecKeyChain.QueryAsRecord(record, out resultCode);

            if (resultCode == SecStatusCode.Success)
                return NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
            else
                return string.Empty;
        }

        public void SetValueForKey(string value, string key)
        {
            var record = ExistingRecordForKey(key);
            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(ValueForKey(key)))
                    RemoveRecord(record);

                return;
            }

            // if the key already exists, remove it
            if (!string.IsNullOrEmpty(ValueForKey(key)))
                RemoveRecord(record);

            var result = SecKeyChain.Add(CreateRecordForNewKeyValue(key, value));
            if (result != SecStatusCode.Success)
            {
                throw new Exception($"Error adding record: {result}");
            }
        }

        private SecRecord CreateRecordForNewKeyValue(string key, string value)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = StorageSecure.StorageSecureFilename,
                Label = key,
                ValueData = NSData.FromString(value, NSStringEncoding.UTF8),
            };
        }

        private bool RemoveRecord(SecRecord record)
        {
            var result = SecKeyChain.Remove(record);
            if (result != SecStatusCode.Success)
            {
                throw new Exception(string.Format($"Error removing record: {result}"));
            }

            return true;
        }
    }
}
