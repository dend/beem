using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Beem.ViewModels;

namespace Beem.Views
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (CoreViewModel.Instance.CurrentAppSettings.EnableAnalytics)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendView("AboutPage");
            }

            base.OnNavigatedTo(e);
        }

        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.Subject = "Beem Plus 1.9.0 Feedback";
            emailTask.To = "hilltopdev@outlook.com";
            emailTask.Show();
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }
    }
}