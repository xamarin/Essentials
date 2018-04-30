using System.Collections.Generic;
using System.Diagnostics;

namespace Xamarin.Essentials
{
    internal static partial class TextToSpeechExtensions
    {
        public static List<string> Split(this string text, int max)
        {
            var parts = new List<string>();
            if (text.Length <= max)
            {
                // no need to split
                parts.Add(text);
            }
            else
            {
                var positionbegin = 0;
                var positionend = max;
                var position = positionbegin;

                var p = string.Empty;
                while (position != text.Length)
                {
                    while (positionend > positionbegin)
                    {
                        var ch = text[positionend];
                        if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch))
                        {
                            p = text.Substring(positionbegin, positionend - positionbegin);
                            break;
                        }
                        else if (positionend == positionbegin)
                        {
                            // no whitespace or punctuation found
                            // grab the whole buffer (max)
                            p = text.Substring(positionbegin, positionbegin + max);
                            break;
                        }

                        positionend--;
                    }
                    Debug.WriteLine($"p             = {p}");
                    Debug.WriteLine($"p.Length      = {p.Length}");
                    Debug.WriteLine($"positionbegin = {positionbegin}");
                    Debug.WriteLine($"positionend   = {positionend}");
                    Debug.WriteLine($"position      = {position}");

                    positionbegin = positionbegin + p.Length + 1;
                    positionend = positionbegin + max;
                    position = positionbegin;

                    Debug.WriteLine($"------------------------------");
                    Debug.WriteLine($"positionbegin = {positionbegin}");
                    Debug.WriteLine($"positionend   = {positionend}");
                    Debug.WriteLine($"position      = {position}");

                    parts.Add(p);
                }
            }

            return parts;
        }
    }
}
