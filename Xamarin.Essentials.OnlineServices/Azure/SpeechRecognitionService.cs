using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace Xamarin.Essentials.OnlineServices.Azure
{
    public static class SpeechRecognitionService
    {
        /// <summary>
        /// Recognizes the once async.
        /// </summary>
        /// <returns>The once async.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="serviceRegion">Service region.</param>
        public static async Task<string> RecognizeOnceAsync (string subscriptionKey, string serviceRegion)
        {
            var factory = SpeechFactory.FromSubscription (subscriptionKey, serviceRegion);
            using (var recognizer = factory.CreateSpeechRecognizer ()) {
                var result = await recognizer.RecognizeAsync ();

                return result.RecognitionStatus != RecognitionStatus.Recognized ? string.Empty : result.Text;
            }
        }
    }

}