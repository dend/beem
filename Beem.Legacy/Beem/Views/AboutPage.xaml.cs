using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Beem.Views
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.Subject = "Beem Plus 1.8.1 Feedback";
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