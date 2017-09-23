using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
namespace AudioKetab
{
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            flowlistview.FlowColumnMinWidth = App.ScreenWidth;

        }
        async void back_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {


            }
        }

        void Flowlistview_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as NotificatonModel.OtherNotification;
            if(item.notification_type.Equals("add new song"))
            {
                
            }
            else
            {
                Navigation.PushModalAsync(new UserDetailsPage(Convert.ToInt32(item.users_id) ,StaticDataModel.CurrentContext));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            flowlistview.FlowItemTapped+= Flowlistview_FlowItemTapped;

            GetNotificationList().Wait();

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            flowlistview.FlowItemTapped -= Flowlistview_FlowItemTapped;
        }
        private async Task GetNotificationList()
        {
            List<NotificatonModel.OtherNotification> list = null;

            StaticMethods.ShowLoader();
            Task.Factory.StartNew(
                    // tasks allow you to use the lambda syntax to pass wor
                    () =>
                    {
                        list = WebService.GetAllNotificaton();
                    }).ContinueWith(
                    t =>
                    {
                        if (list != null)
                        {
                           
                            flowlistview.FlowItemsSource = list;
                    if(list.Count==0)
                    
                    {
                        DisplayAlert("Alert","No Notification found!","Ok");
                    }}


                    }, TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
    }
}
