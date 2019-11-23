﻿using System;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Recorder
    {
        public static Task<bool> PlatformCanRecordAudio
        => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task PlatformRecordAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task<RecordedAudio> PlatformStopAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}