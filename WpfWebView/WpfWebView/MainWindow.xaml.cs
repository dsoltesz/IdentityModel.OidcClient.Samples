using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfWebView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcClient _oidcClient = null;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Start;
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

       

        public async void Start(object sender, RoutedEventArgs e)
        {

            //OKTA WORKS
            //var options = new OidcClientOptions()
            //{
            //    Authority = "https://dev-158000.okta.com/",
            //    ClientId = "0oap3ktastxKA81BJ4x6",
            //    Scope = "openid profile email",
            //    RedirectUri = "http://localhost:8080/authorization-code/callback",
            //    Browser = new WpfEmbeddedBrowser(),
            //    ClientSecret = "UoYWsWn1-Tiwp2NItHVup46iDCwZVolDGTW5sHs3"
            //};

            //MyID
            var options = new OidcClientOptions()
            {
                Authority = "https://idp.myid.disney.com",
                ClientId = "VPSlocalhost",
                Scope = "openid profile email",
                RedirectUri = "http://localhost:11375/",
                Browser = new WpfEmbeddedBrowser(),
                ClientSecret = "BZ3PAQBqXFVb1LLiKZTDIWLJzvvlEUpYCDWQ6yV4LMmhHiwoOV"
            };

            _oidcClient = new OidcClient(options);

            LoginResult result;
            try
            {
                result = await _oidcClient.LoginAsync();
            }
            catch (Exception ex)
            {
                Message.Text = $"Unexpected Error: {ex.Message}";
                return;
            }

            if (result.IsError)
            {
                Message.Text = result.Error == "UserCancel" ? "The sign-in window was closed before authorization was completed." : result.Error;
            }
            else
            {
                var name = result.User.Identity.Name;
                Message.Text = $"Hello {name}";
            }
        }
    }
}
