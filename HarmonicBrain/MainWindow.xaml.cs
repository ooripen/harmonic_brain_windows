using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Forms; // For NotifyIcon (add reference to System.Windows.Forms)
using MessageBox = System.Windows.MessageBox;

namespace HarmonicBrain
{
    public partial class MainWindow : Window
    {
        private bool isActive = false;
        private string currentSpeed = "Medium";
        private AudioProcessor audioProcessor;
        private DispatcherTimer panTimer;
        private NotifyIcon trayIcon;
        
        // Panning cycle durations in seconds (example values)
        private readonly TimeSpan slowInterval = TimeSpan.FromSeconds(35);
        private readonly TimeSpan mediumInterval = TimeSpan.FromSeconds(12);
        private readonly TimeSpan fastInterval = TimeSpan.FromSeconds(7);
        
        public MainWindow()
        {
            InitializeComponent();
            audioProcessor = new AudioProcessor();
            InitializeTrayIcon();
            SetSpeed(currentSpeed);
        }
        
        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("Assets/HarmonicBrain.ico"), // Include an appropriate icon file
                Visible = true,
                Text = "Harmonic Brain"
            };
            trayIcon.DoubleClick += (s, e) => { this.Show(); this.WindowState = WindowState.Normal; };

            // Add context menu to tray icon
            var contextMenu = new System.Windows.Forms.ContextMenuStrip();
            contextMenu.Items.Add("Toggle Bilateral Stimulation", null, (s, e) => ToggleBilateralStimulation());
            contextMenu.Items.Add("Exit", null, (s, e) => { trayIcon.Visible = false; System.Windows.Application.Current.Shutdown(); });
            trayIcon.ContextMenuStrip = contextMenu;
        }
        
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleBilateralStimulation();
        }
        
        private void ToggleBilateralStimulation()
        {
            isActive = !isActive;
            if (isActive)
            {
                // Update UI
                StatusText.Text = "BILATERAL SOUND: On";
                // (Change button colors: e.g., blue background, white icon)
                ToggleButton.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4A90E2"));
                PowerIcon.Foreground = System.Windows.Media.Brushes.White;
                
                // Start audio processing
                audioProcessor.Start(currentSpeed);
            }
            else
            {
                StatusText.Text = "BILATERAL SOUND: Off";
                ToggleButton.Background = System.Windows.Media.Brushes.White;
                PowerIcon.Foreground = System.Windows.Media.Brushes.Gray;
                
                audioProcessor.Stop();
            }
        }
        
        private void SpeedButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                currentSpeed = button.Tag.ToString();
                SetSpeed(currentSpeed);
                // If active, update the running audio processor
                if (isActive)
                {
                    audioProcessor.UpdateSpeed(currentSpeed);
                }
            }
        }
        
        private void SetSpeed(string speed)
        {
            // Reset backgrounds for all speed buttons
            var inactiveBrush = System.Windows.Media.Brushes.Gray;
            SlowButton.Background = inactiveBrush;
            MediumButton.Background = inactiveBrush;
            FastButton.Background = inactiveBrush;
            
            // Highlight selected speed (blue background)
            var activeBrush = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4A90E2"));
            switch (speed)
            {
                case "Slow":
                    SlowButton.Background = activeBrush;
                    break;
                case "Medium":
                    MediumButton.Background = activeBrush;
                    break;
                case "Fast":
                    FastButton.Background = activeBrush;
                    break;
            }
        }
        
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
        
        private void UsageButton_Click(object sender, RoutedEventArgs e)
        {
            var usageWindow = new UsageWindow();
            usageWindow.Owner = this;
            usageWindow.ShowDialog();
        }
        
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Display a dropdown menu (or reuse the tray context menu)
            var menu = new System.Windows.Controls.ContextMenu();
            var aboutMenuItem = new System.Windows.Controls.MenuItem { Header = "About Harmonic Brain" };
            aboutMenuItem.Click += AboutButton_Click;
            var usageMenuItem = new System.Windows.Controls.MenuItem { Header = "Using Harmonic Brain" };
            usageMenuItem.Click += UsageButton_Click;
            var exitMenuItem = new System.Windows.Controls.MenuItem { Header = "Exit" };
            exitMenuItem.Click += (s, ev) => { this.Close(); };
            
            menu.Items.Add(aboutMenuItem);
            menu.Items.Add(usageMenuItem);
            menu.Items.Add(exitMenuItem);
            menu.IsOpen = true;
        }
        
        protected override void OnClosed(EventArgs e)
        {
            trayIcon.Visible = false;
            base.OnClosed(e);
        }
    }
}
