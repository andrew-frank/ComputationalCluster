using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace VehicleRouting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DeportsTextBox.Text = "location_x: 10; location_y: 10; start: 0; end: 30; venicles: 2;" + Environment.NewLine;
            DeportsTextBox.Text += "location_x: 20; location_y: 20; start: 0; end: 20; venicles: 1;";

            VenicleInfoTextBox.Text = "Speed: 1; Capacity: 10;";

            RequestsTextBox.Text = "location_x: 1; location_y: 1; start: 0; end: 20; unload: 1; Size: 2;" + Environment.NewLine;
            RequestsTextBox.Text += "location_x: 5; location_y: 5; start: 0; end: 20; unload: 1; Size: 2;" + Environment.NewLine;
            RequestsTextBox.Text += "location_x: 25; location_y: 25; start: 0; end: 20; unload: 1; Size: 10;";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var deportsRegex = new Regex("location_x: (?<location_x>.+); location_y: (?<location_y>.+); start: (?<start>.+); end: (?<end>.+); venicles: (?<venicles>.+);");
            var deportsRegexResults = deportsRegex.Matches(DeportsTextBox.Text);
            var deports = new List<Deport>();
            for (var i = 0; i < deportsRegexResults.Count; i++)
            {
                deports.Add(new Deport
                {
                    Name = "D_" + (i + 1),
                    Location = new Point(
                        double.Parse(deportsRegexResults[i].Groups["location_x"].Value),
                        double.Parse(deportsRegexResults[i].Groups["location_y"].Value)),
                    Start = double.Parse(deportsRegexResults[i].Groups["start"].Value),
                    End = double.Parse(deportsRegexResults[i].Groups["end"].Value),
                    Venicles = int.Parse(deportsRegexResults[i].Groups["venicles"].Value)
                });
            }

            var venicleInfoRegex = new Regex("Speed: (?<Speed>.+); Capacity: (?<Capacity>.+);");
            var venicleInfoRegexResult = venicleInfoRegex.Match(VenicleInfoTextBox.Text);
            var venicleInfo = new VenicleInfo
            {
                Speed = double.Parse(venicleInfoRegexResult.Groups["Speed"].Value),
                Capacity = double.Parse(venicleInfoRegexResult.Groups["Capacity"].Value)
            };

            var requestsRegex = new Regex("location_x: (?<location_x>.+); location_y: (?<location_y>.+); start: (?<start>.+); end: (?<end>.+); unload: (?<unload>.+); Size: (?<Size>.+);");
            var requestsRegexResults = requestsRegex.Matches(RequestsTextBox.Text);
            var requests = new List<Request>();
            for (var i = 0; i < requestsRegexResults.Count; i++)
            {
                requests.Add(new Request
                {
                    Id = i + 1,
                    Location = new Point(
                        double.Parse(requestsRegexResults[i].Groups["location_x"].Value),
                        double.Parse(requestsRegexResults[i].Groups["location_y"].Value)),
                    Start = double.Parse(requestsRegexResults[i].Groups["start"].Value),
                    End = double.Parse(requestsRegexResults[i].Groups["end"].Value),
                    Unload = int.Parse(requestsRegexResults[i].Groups["unload"].Value),
                    Size = int.Parse(requestsRegexResults[i].Groups["Size"].Value)
                });
            }

            RoutesResultTextBox.Text = string.Empty;

            TryServe(deports, venicleInfo, requests);
        }

        private void TryServe(IList<Deport> deports, VenicleInfo venicleInfo, List<Request> requests)
        {
            try
            {
                var builder = new RouteBuilder(venicleInfo);
                var routes = builder.Build(deports, venicleInfo, requests).ToList();

                RoutesResultTextBox.Text += string.Format("Found {0} routes", routes.Count()) + Environment.NewLine + Environment.NewLine;
                foreach (var route in routes)
                {
                    RoutesResultTextBox.Text += string.Join(" -> " + Environment.NewLine, route.GetTimeTable().Select(checkPoint => "[Ar = " + checkPoint.ArrivalTime + " Loc = " + checkPoint.Location.X + " " + checkPoint.Location.Y + "]")) + Environment.NewLine;
                    RoutesResultTextBox.Text += "Venicle distance: " + route.GetTotalDistance() + Environment.NewLine;
                    RoutesResultTextBox.Text += Environment.NewLine;
                }

                RoutesResultTextBox.Text += "Total distance: " + routes.Sum(r => r.GetTotalDistance()) + Environment.NewLine;
                RoutesResultTextBox.Text += Environment.NewLine;
            }
            catch (ImpossibleRouteException exception)
            {
                RoutesResultTextBox.Text = "Can not serve requests:" + Environment.NewLine;
                foreach (var request in exception.ImpossibleRequests)
                {
                    RoutesResultTextBox.Text += "Loc = " + request.Location.X + " " + request.Location.Y + Environment.NewLine;
                }
                RoutesResultTextBox.Text += Environment.NewLine;

                RoutesResultTextBox.Text += "Routes without these requests:" + Environment.NewLine;
                TryServe(deports, venicleInfo, requests.Except(exception.ImpossibleRequests).ToList());
            }
            catch (Exception exception)
            {
                RoutesResultTextBox.Text = exception.Message;
            }
        }
    }
}
