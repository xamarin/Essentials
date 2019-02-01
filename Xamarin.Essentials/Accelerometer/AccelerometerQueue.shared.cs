namespace Xamarin.Essentials
{
    // Detect if 3/4ths of the accelerometer events in the last half second are accelerating
    // this means we are free falling or shaking
    class AccelerometerQueue
    {
        readonly AccelerometerDataPool pool = new AccelerometerDataPool();

        // in nanoseconds
        readonly long maxWindowSize = 500_000_000;
        readonly long minWindowSize = 250_000_000;

        readonly int minQueueSize = 4;

        AccelerometerSample oldestSample;
        AccelerometerSample newestSample;
        int sampleCount;
        int acceleratingCount;

        internal void Add(long timestamp, bool accelerating)
        {
            Purge(timestamp - maxWindowSize);
            var added = pool.Acquire();
            added.Timestamp = timestamp;
            added.IsAccelerating = accelerating;
            added.Next = null;

            if (newestSample != null)
                newestSample.Next = added;

            newestSample = added;

            if (oldestSample == null)
                oldestSample = added;

            sampleCount++;

            if (accelerating)
                acceleratingCount++;
        }

        internal void Clear()
        {
            while (oldestSample != null)
            {
                var removed = oldestSample;
                oldestSample = removed.Next;
                pool.Release(removed);
            }
            newestSample = null;
            sampleCount = 0;
            acceleratingCount = 0;
        }

        void Purge(long cutoff)
        {
            while (sampleCount >= minQueueSize &&
                   oldestSample != null &&
                   cutoff - oldestSample.Timestamp > 0)
            {
                var removed = oldestSample;
                if (removed.IsAccelerating)
                    acceleratingCount--;

                sampleCount--;
                oldestSample = removed.Next;

                if (oldestSample == null)
                    newestSample = null;

                pool.Release(removed);
            }
        }

        // Returns true if we have enough samples to detect if we are shaking the device and that more than 3/4th of them are accelerating
        internal bool IsShaking => newestSample != null &&
                          oldestSample != null &&
                          newestSample.Timestamp - oldestSample.Timestamp >= minWindowSize &&
                          acceleratingCount >= (sampleCount >> 1) + (sampleCount >> 2);

        internal class AccelerometerSample
        {
            public long Timestamp { get; set; }

            public bool IsAccelerating { get; set; }

            public AccelerometerSample Next { get; set; }
        }

        internal class AccelerometerDataPool
        {
            AccelerometerSample head;

            internal AccelerometerSample Acquire() =>
                head?.Next ?? new AccelerometerSample();

            internal void Release(AccelerometerSample sample)
            {
                sample.Next = head;
                head = sample;
            }
        }
    }
}
