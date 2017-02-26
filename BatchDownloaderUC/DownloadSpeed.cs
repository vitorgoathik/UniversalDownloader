using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BatchDownloaderUC
{
    internal class DownloadSpeed
    {
        private Stopwatch watch;
        private long NumCounts = 0;
        private int PrevBytes = 0;
        private double[] DataPoints;

        private long CurrentBytesReceived = 0;

        internal double Speed { get; set; }
        internal string SpeedInUnit => Speed > 0 ? Functions.ConvertSizeToUnit((long)Math.Round(Speed)) : "0";

        internal double ElapsedTimeInSeconds { get; set; }

        private System.Timers.Timer ticker = new System.Timers.Timer(1000);

        internal DownloadSpeed(WebClient webClient, int maxPoints = 5)
        {
            watch = new System.Diagnostics.Stopwatch();

            DataPoints = new double[maxPoints];

            webClient.DownloadProgressChanged += (sender, e) =>
            {
                int curBytes = (int)(e.BytesReceived - PrevBytes);
                if (curBytes < 0)
                    curBytes = (int)e.BytesReceived;

                CurrentBytesReceived += curBytes;

                PrevBytes = (int)e.BytesReceived;
            };

            ticker.Elapsed += (sender, e) =>
            {
                double dataPoint = (double)CurrentBytesReceived / watch.ElapsedMilliseconds;
                DataPoints[NumCounts++ % maxPoints] = dataPoint;
                Speed = DataPoints.Average()*1024;
                CurrentBytesReceived = 0;
                ElapsedTimeInSeconds += 1;
                watch.Restart();
            };
        }

        internal void Stop()
        {
            ElapsedTimeInSeconds = 0;
            watch.Stop();
            ticker.Stop();
        }

        internal void Start()
        {
            watch.Start();
            ticker.Start();
        }

        internal void Reset()
        {
            CurrentBytesReceived = 0;
            PrevBytes = 0;
            ElapsedTimeInSeconds = 0;
            watch.Restart();
            ticker.Start();
        }
    }
}
