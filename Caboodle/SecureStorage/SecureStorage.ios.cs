using System;
using System.Threading.Tasks;

using Foundation;
using Security;

namespace Microsoft.Caboodle
{
    public static partial class SecureStorage
    {
        static Task<string> PlatformGetAsync(string key)
        {
            var kc = new KeyChain();

            return Task.FromResult(kc.ValueForKey(key));
        }

        static Task PlatformSetAsync(string key, string data)
        {
            var kc = new KeyChain();
            kc.SetValueForKey(data, key);

            return Task.CompletedTask;
        }
    }

    class KeyChain
    {
        static SecRecord ExistingRecordForKey(string key)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = key,
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
                throw new Exception($"Error adding record: {result}");
        }

        SecRecord CreateRecordForNewKeyValue(string key, string value)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = key,
                Label = key,
                ValueData = NSData.FromString(value, NSStringEncoding.UTF8),
            };
        }

        bool RemoveRecord(SecRecord record)
        {
            var result = SecKeyChain.Remove(record);
            if (result != SecStatusCode.Success)
                throw new Exception(string.Format($"Error removing record: {result}"));

            return true;
        }
    }
}
