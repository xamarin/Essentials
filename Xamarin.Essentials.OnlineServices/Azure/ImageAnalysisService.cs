using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Xamarin.Essentials.OnlineServices.Azure
{
    public static class ImageAnalysisService
    {
        static ComputerVisionAPI GetComputerVisionAPI (string subscriptionKey, AzureRegions region) => new ComputerVisionAPI (
                new ApiKeyServiceClientCredentials (subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { })
        {
            AzureRegion = region
        };

        /// <summary>
        /// Analyzes the image file async.
        /// </summary>
        /// <returns>The image file async.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="region">Region.</param>
        /// <param name="imageFilePath">Image file path.</param>
        /// <param name="features">Features.</param>
        public static async Task<ImageAnalysis> AnalyzeImageFileAsync (string subscriptionKey, AzureRegions region, string imageFilePath, IList<VisualFeatureTypes> features)
        {
            if (!File.Exists (imageFilePath)) {
                return null;
            }

            using (Stream imageStream = File.OpenRead (imageFilePath)) {
                return await GetComputerVisionAPI (subscriptionKey, region)
                    .AnalyzeImageInStreamAsync (imageStream, features);
            }
        }

        /// <summary>
        /// Analyzes the image URLA sync.
        /// </summary>
        /// <returns>The image URLA sync.</returns>
        /// <param name="subscriptionKey">Subscription key.</param>
        /// <param name="region">Region.</param>
        /// <param name="imageUrl">Image URL.</param>
        /// <param name="features">Features.</param>
        public static async Task<ImageAnalysis> AnalyzeImageURLAsync (string subscriptionKey, AzureRegions region, string imageUrl, IList<VisualFeatureTypes> features)
        {
            if (!Uri.IsWellFormedUriString (imageUrl, UriKind.Absolute)) {
                return null;
            }

            return await GetComputerVisionAPI (subscriptionKey, region)
                .AnalyzeImageAsync (imageUrl, features);
        }
    }
}