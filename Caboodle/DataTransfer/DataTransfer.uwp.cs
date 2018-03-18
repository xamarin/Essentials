using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace Microsoft.Caboodle
{
    public static partial class DataTransfer
    {
        static ShareTextRequest shareRequest;

        public static Task ShowShareUI(ShareTextRequest request)
        {
            shareRequest = request;

            var dataTransferManager = DataTransferManager.GetForCurrentView();

            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(ShareTextHandler);

            DataTransferManager.ShowShareUI();

            return Task.CompletedTask;
        }

        static void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var request = e.Request;

            request.Data.Properties.Title = shareRequest.Title ?? DeviceInfo.AppName;

            if (!string.IsNullOrWhiteSpace(shareRequest.Text))
            {
                request.Data.SetText(shareRequest.Text);
            }

            if (!string.IsNullOrWhiteSpace(shareRequest.Uri))
            {
                request.Data.SetWebLink(new Uri(shareRequest.Uri));
            }

            shareRequest = null;
        }
    }
}
