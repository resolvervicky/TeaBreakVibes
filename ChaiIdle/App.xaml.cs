using System;
using System.Windows;

namespace ChaiIdle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Da, main app logic ba! Hide main window, show overlay when idle
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Da, software rendering mode ba! Fixes video in transparent windows
            System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            
            base.OnStartup(e);
            Logger.Log("App started with Software Rendering Mode - No more black videos ba!");
        }
    }
}
