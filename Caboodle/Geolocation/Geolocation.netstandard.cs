﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Geolocation
    {
        static Task<Location> PlatformLastKnownLocationAsync() =>
            throw new NotImplementedInReferenceAssemblyException();

        static Task<Location> PlatformLocationAsync(GeolocationRequest request, CancellationToken? cancellationToken) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
