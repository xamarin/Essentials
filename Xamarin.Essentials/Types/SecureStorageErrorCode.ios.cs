using Security;

namespace Xamarin.Essentials
{
    static class SecureStorageErrorCodeExtensions
    {
        internal static SecureStorageErrorCode ToErrorCode(this SecStatusCode statusCode)
        {
            switch (statusCode)
            {
                case SecStatusCode.Allocate:
                    return SecureStorageErrorCode.AllocateMemory;
                case SecStatusCode.AuthFailed:
                    return SecureStorageErrorCode.AuthenticationFailed;
                case SecStatusCode.BadReq:
                    return SecureStorageErrorCode.BadRequest;
                case SecStatusCode.Decode:
                    return SecureStorageErrorCode.Decode;
                case SecStatusCode.DuplicateItem:
                    return SecureStorageErrorCode.DuplicateItem;
                case SecStatusCode.InteractionNotAllowed:
                    return SecureStorageErrorCode.InteractionNotAllowed;
                case SecStatusCode.InternalComponent:
                    return SecureStorageErrorCode.InternalComponent;
                case SecStatusCode.InvalidKeyChain:
                    return SecureStorageErrorCode.InvalidKeyChain;
                case SecStatusCode.IO:
                    return SecureStorageErrorCode.IO;
                case SecStatusCode.ItemNotFound:
                    return SecureStorageErrorCode.ItemNotFound;
                case SecStatusCode.NoSuchKeyChain:
                    return SecureStorageErrorCode.NoSuchKeyChain;
                case SecStatusCode.NotAvailable:
                    return SecureStorageErrorCode.NotAvailable;
                case SecStatusCode.OpWr:
                    return SecureStorageErrorCode.FileAlreadyOpen;
                case SecStatusCode.Param:
                    return SecureStorageErrorCode.InvalidParameters;
                case SecStatusCode.ReadOnly:
                    return SecureStorageErrorCode.ReadOnly;
                case SecStatusCode.Unimplemented:
                    return SecureStorageErrorCode.NotImplemented;
                case SecStatusCode.UserCanceled:
                    return SecureStorageErrorCode.UserCanceled;
                case SecStatusCode.VerifyFailed:
                    return SecureStorageErrorCode.VerifyFailed;
            }
            return SecureStorageErrorCode.Other;
        }
    }
}
