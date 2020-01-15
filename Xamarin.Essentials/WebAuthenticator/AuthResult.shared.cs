using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin.Essentials
{
    public class AuthResult
    {
        public AuthResult()
        {
        }

        public AuthResult(Uri uri)
        {
            foreach (var kvp in WebUtils.ParseQueryString(uri.ToString()))
            {
                Properties[kvp.Key] = kvp.Value;
            }
        }

        public AuthResult(IDictionary<string, string> properties)
        {
            foreach (var kvp in properties)
                Properties[kvp.Key] = kvp.Value;
        }

        public DateTimeOffset Timestamp { get; set; } = new DateTimeOffset(DateTime.UtcNow);

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public void Put(string key, object value)
            => Properties[key] = value.ToString();

        public string Get(string key)
        {
            if (Properties.TryGetValue(key, out var v))
                return v;

            return default;
        }

        public int GetInt(string key, int defaultValue = default)
        {
            if (Properties.TryGetValue(key, out var v))
            {
                if (int.TryParse(v, out var i))
                    return i;
            }

            return defaultValue;
        }

        public virtual string AccessToken
            => Get("access_token");

        public string TokenType
            => Get("token_type");

        public string RefreshToken
            => Get("refresh_token");

        public virtual TimeSpan RefreshTokenExpiresIn
        {
            get
            {
                var refreshTokenExpiresIn = GetInt("refresh_token_expires_in", -1);

                if (refreshTokenExpiresIn > 0)
                    return TimeSpan.FromSeconds(refreshTokenExpiresIn);
                return TimeSpan.Zero;
            }
        }

        public virtual TimeSpan ExpiresIn
        {
            get
            {
                var expiresIn = GetInt("expires_in", -1);

                if (expiresIn >= 0)
                    return TimeSpan.FromSeconds(expiresIn);

                // Try the IdToken to see if it has 'exp' which is the id_token's expiry seconds since epoch
                // if (IdToken != null && IdToken.ContainsKey("exp") && int.TryParse(IdToken["exp"].ToString(), out var seconds))
                // return DateTimeOffset.FromUnixTimeSeconds(seconds) - DateTime.UtcNow;

                return TimeSpan.MinValue;
            }
        }

        public bool IsExpired
            => DateTime.UtcNow >= Timestamp + ExpiresIn;

        public bool IsRefreshExpired
            => RefreshTokenExpiresIn == TimeSpan.Zero || DateTime.UtcNow >= Timestamp + RefreshTokenExpiresIn;
    }
}
