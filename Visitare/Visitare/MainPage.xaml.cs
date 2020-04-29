using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Visitare
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(1.0)));
        }
        public MainPage(RoutePoints points)
        {
            InitializeComponent();
            foreach(Points tmp in points.routePoints)
            {
                CustomPin pin = new CustomPin
                {
                    Type = PinType.SavedPin,
                    Position = new Position(tmp.X, tmp.Y),
                    Label = tmp.Name,
                    Address = tmp.Description,
                    Name = "Xamarin",
                    Url = "http://xamarin.com/about/",
                    Question = "",
                    Answer = ""
                };

                if(String.IsNullOrWhiteSpace(tmp.Name))
                    pin.Label = "Name";

                pin.MarkerClicked += async (s, args) =>
                {
                    args.HideInfoWindow = true;
                    string pinName = ((CustomPin)s).Label;
                    // string pytanie = ((CustomPin)s).Question;
                    string opis = ((CustomPin)s).Address;
                    // string odpowiedz = ((CustomPin)s).Answer;
                    await DisplayAlert($"{pinName}", $"{opis}", "Ok");
                    // await DisplayAlert("Quiz", $"{pytanie}", "Przejdź do odpowiedzi");
                    //await Navigation.PushAsync(new QuestionPage(new Question()));

                };
                customMap.Pins.Add(pin);
            }

            if(points.routePoints.Count > 0)
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(points.routePoints[0].X, points.routePoints[0].Y), Distance.FromMiles(2.0)));
            else
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(2.0)));
        }
        private async void OnLogOut(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            customMap.Pins.Clear();
            customMap.MapElements.Clear();
        }

        private async void OnRoutesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RoutesPage());
        }

        private async void OnCreatorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreatorPage());
        }
    }
}
