using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System.Net;

namespace Xamarin.Essentials.OnlineServices.Azure
{
    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        readonly string subscriptionKey;

        public ApiKeyServiceClientCredentials (string subscriptionKey) => this.subscriptionKey = subscriptionKey;

        public override Task ProcessHttpRequestAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add ("Ocp-Apim-Subscription-Key", subscriptionKey);
            return base.ProcessHttpRequestAsync (request, cancellationToken);
        }
    }

    public static class TextAnalyticsService
    {
        static TextAnalyticsAPI GetTextAnalyticsAPI (string subscriptionKey, AzureRegions region) => new TextAnalyticsAPI (new ApiKeyServiceClientCredentials (subscriptionKey)) {
            AzureRegion = region
        };

        /// <summary>
        /// Detects the language async.
        /// </summary>
        /// <returns>The language async.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="region">Region.</param>
        /// <param name="inputText">Input text.</param>
        public static async Task<string> DetectLanguageAsync (string subscriptionKey, AzureRegions region, string inputText)
        {
            var result = await GetTextAnalyticsAPI (subscriptionKey, region)
                .DetectLanguageAsync (new BatchInput (
                    new List<Input> () { new Input (inputText) }
                ));
            return result?.Documents?[0].DetectedLanguages?[0].Name;
        }

        /// <summary>
        /// Extracts the key phrases async.
        /// </summary>
        /// <returns>The key phrases of the provided text.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="region">Region.</param>
        /// <param name="inputText">Input text.</param>
        /// <param name="language">Language.</param>
        public static async Task<IList<string>> ExtractKeyPhrasesAsync (string subscriptionKey, AzureRegions region, string inputText, string language)
        {
            var result = await GetTextAnalyticsAPI (subscriptionKey, region)
                .KeyPhrasesAsync (new MultiLanguageBatchInput (
                    new List<MultiLanguageInput> () { new MultiLanguageInput (language, "1", inputText) }
                ));
            return result?.Documents?[0].KeyPhrases;
        }

        /// <summary>
        /// Analyzes the sentiment async.
        /// </summary>
        /// <returns>The sentiment score.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="region">Region.</param>
        /// <param name="inputText">Input text.</param>
        /// <param name="language">Language.</param>
        public static async Task<double?> AnalyzeSentimentAsync (string subscriptionKey, AzureRegions region, string inputText, string language)
        {
            var result = await GetTextAnalyticsAPI (subscriptionKey, region)
                .SentimentAsync (new MultiLanguageBatchInput (
                    new List<MultiLanguageInput> () { new MultiLanguageInput (language, "1", inputText) }
                ));
            return result?.Documents?[0].Score;
        }
    }
}