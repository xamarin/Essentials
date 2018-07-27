using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Xamarin.Essentials.OnlineServices.Azure
{
    public static class BingSearchService
    {
        // Verify the endpoint URI.  At this writing, only one endpoint is used for Bing
        // search APIs.  In the future, regional endpoints may be available.  If you
        // encounter unexpected authorization errors, double-check this value against
        // the endpoint for your Bing Web search instance in your Azure dashboard.
        const string searchUriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        const string imageSearchUriBase = "https://api.cognitive.microsoft.com/bing/v7.0/images/search";
        const string newsSearchUriBase = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        const string videoSearchUriBase = "https://api.cognitive.microsoft.com/bing/v7.0/videos/search";

        static async Task<string> InternalSearchAsync (string uriBase, string subscriptionKey, string searchTerm)
        {
            var query = uriBase + "?q=" + Uri.EscapeDataString (searchTerm);
            var request = WebRequest.Create (query);
            request.Headers["Ocp-Apim-Subscription-Key"] = subscriptionKey;
            var response = await request.GetResponseAsync ();
            string json = new StreamReader (response.GetResponseStream ()).ReadToEnd ();

            return json;
        }

        /// <summary>
        /// Searchs the Web for pages matching the search term.
        /// </summary>
        /// <returns>The search results.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="searchTerm">Search term.</param>
        public static Task<string> SearchAsync (string subscriptionKey, string searchTerm) => InternalSearchAsync (searchUriBase, subscriptionKey, searchTerm);

        /// <summary>
        /// Searchs the Web for images matching the search term.
        /// <returns>The search results.</returns>
        /// <param name="subsciptionKey">Subsciption key.</param>
        /// <param name="searchTerm">Search term.</param>
        public static Task<string> SearchImagesAsync (string subsciptionKey, string searchTerm) => InternalSearchAsync (imageSearchUriBase, subsciptionKey, searchTerm);

        /// <summary>
        /// Searchs the Web for news matching the search term.
        /// </summary>
        /// <returns>The news async.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="searchTerm">Search term.</param>
        public static Task<string> SearchNewsAsync (string subscriptionKey, string searchTerm) => InternalSearchAsync (newsSearchUriBase, subscriptionKey, searchTerm);

        /// <summary>
        /// Searchs the Web for videos matching the search term.
        /// </summary>
        /// <returns>The videos async.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="searchTerm">Search term.</param>
        public static Task<string> SearchVideosAsync (string subscriptionKey, string searchTerm) => InternalSearchAsync (videoSearchUriBase, subscriptionKey, searchTerm);
    }
}