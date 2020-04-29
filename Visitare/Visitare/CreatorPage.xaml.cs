using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage : ContentPage
    {
        public CreatorPage()
        {
            InitializeComponent();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(1.0)));
        }
        private void OnNewRouteClicked(object sender, EventArgs e)
        {

        }
        private void OnClearClicked(object sender, EventArgs e)
        {
            customMap.Pins.Clear();
            customMap.MapElements.Clear();
        }
        private async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(nazwaEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj nazwę punktu", "Ok");
                return;
            }

            CustomPin pin = new CustomPin
            {
                Type = PinType.SavedPin,
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                Label = nazwaEntry.Text,
                Address = opisEntry.Text,
                Name = "Xamarin",
                Url = "http://xamarin.com/about/",
                Question = zagadkaEntry.Text,
                Answer = odpowiedzEntry.Text
            };

            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                string pinName = ((CustomPin)s).Label;
                // string pytanie = ((CustomPin)s).Question;
                string opis = ((CustomPin)s).Address;
                // string odpowiedz = ((CustomPin)s).Answer;
                await DisplayAlert($"{pinName}", $"{opis}", "Quiz");
                // await DisplayAlert("Quiz", $"{pytanie}", "Przejdź do odpowiedzi");
                await Navigation.PushAsync(new QuestionPage(new Question()));

            };
            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);

            var json = JsonConvert.SerializeObject(new { X =  pin.Position.Latitude, Y = pin.Position.Longitude });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var result = await client.PostAsync("http://dearjean.ddns.net:44301/api/points", content);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Komunikat", "Dodanie puntku przebiegło pomyślnie", "Anuluj");
            }
            
        }
    }
}