using System.Timers;
using System;
using Timer = System.Timers.Timer;
using NAudio.CoreAudioApi;  // Requires NAudio NuGet package
// using NAudio.Wave; // Uncomment if you use specific NAudio classes

namespace HarmonicBrain
{
    public class AudioProcessor
    {
        private Timer panTimer;
        private double currentPan = 0; // -1 = full left, +1 = full right
        private bool panDirection = true; // true for rightward, false for leftward
        
        // Define intervals for different speeds (in milliseconds)
        private readonly int slowInterval = 35000;
        private readonly int mediumInterval = 12000;
        private readonly int fastInterval = 7000;
        
        private int currentInterval = 12000;  // default to medium
        
        public AudioProcessor()
        {
            panTimer = new Timer();
            panTimer.Elapsed += PanTimer_Elapsed;
            panTimer.AutoReset = true;
        }
        
        public void Start(string speed)
        {
            UpdateSpeed(speed);
            // Start or configure audio capture/processing here
            // For example, initialize WASAPI loopback capture if using NAudio.
            
            panTimer.Start();
        }
        
        public void Stop()
        {
            panTimer.Stop();
            // Reset audio routing to normal
            ResetAudioPan();
        }
        
        public void UpdateSpeed(string speed)
        {
            switch (speed)
            {
                case "Slow":
                    currentInterval = slowInterval;
                    break;
                case "Medium":
                    currentInterval = mediumInterval;
                    break;
                case "Fast":
                    currentInterval = fastInterval;
                    break;
            }
            panTimer.Interval = currentInterval;
        }
        
        private void PanTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Alternate pan values:
            // When pan is applied to one side: one ear is full (or nearly full) and the other drops to 5%
            // Here we simulate by toggling a pan value between -1 and +1.
            currentPan = panDirection ? 1 : -1;
            panDirection = !panDirection;
            
            // Apply the panning effect to the system audio:
            ApplyPan(currentPan);
        }
        
        private void ApplyPan(double pan)
        {
            // This is a placeholder.
            // You would use NAudio’s APIs to adjust the channel volumes.
            // For instance, if integrating with a WASAPI loopback and an output device, you’d
            // create a mixer or DSP effect to set left/right volumes accordingly.
            // Example (pseudocode):
            //   leftVolume = pan > 0 ? 1.0 : 0.05;
            //   rightVolume = pan > 0 ? 0.05 : 1.0;
            // Then apply these values to the audio output.
        }
        
        private void ResetAudioPan()
        {
            // Reset audio to normal (equal volumes on both channels).
        }
    }
}
