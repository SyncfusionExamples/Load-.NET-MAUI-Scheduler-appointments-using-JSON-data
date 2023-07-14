using Newtonsoft.Json;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SchedulerMAUI
{
    public class SchedulerViewModel : INotifyPropertyChanged
    {
        private bool showBusyIndicator;

        private ObservableCollection<Meeting> meetings;

        private List<Brush> colors;

        private List<JSONData> jsonDataCollection;
        /// <summary>
        /// Gets or sets meetings.
        /// </summary>
        public ObservableCollection<Meeting> Meetings
        {
            get
            {
                return this.meetings;
            }
            set
            {
                this.meetings = value;
                this.RaiseOnPropertyChanged("Meetings");
            }
        }
        public bool ShowBusyIndicator
        {
            get { return showBusyIndicator; }
            set
            {
                showBusyIndicator = value;
                this.RaiseOnPropertyChanged(nameof(ShowBusyIndicator));
            }
        }

        public ICommand QueryAppointmentsCommand { get; set; }

        public SchedulerViewModel()
        {
            this.QueryAppointmentsCommand = new Command<object>(LoadAppointments);
            colors = this.GetColors();
        }
        private async void LoadAppointments(object obj)
        {
            if (jsonDataCollection == null)
                await GetJsonData();
            Random random = new Random();
            var visibleDates = ((SchedulerQueryAppointmentsEventArgs)obj).VisibleDates;
            this.Meetings = new ObservableCollection<Meeting>();
            this.ShowBusyIndicator = true;

            DateTime visibleStartDate = visibleDates.FirstOrDefault();
            DateTime visibleEndDate = visibleDates.LastOrDefault();

            foreach (var data in jsonDataCollection)
            {
                DateTime startDate = Convert.ToDateTime(data.StartTime);
                DateTime endDate = Convert.ToDateTime(data.EndTime);

                if ((visibleStartDate <= startDate.Date && visibleEndDate >= startDate.Date) ||
                (visibleStartDate <= endDate.Date && visibleEndDate >= endDate.Date))
                {
                    Meetings.Add(new Meeting()
                    {
                        EventName = data.Subject,
                        From = startDate,
                        To = endDate,
                        Color = this.colors[random.Next(this.colors.Count)]
                });
                }
            }

            if (jsonDataCollection != null)
                await Task.Delay(1000);

            this.ShowBusyIndicator = false;
        }

        private async Task GetJsonData()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://services.syncfusion.com/js/production/api/schedule");
            jsonDataCollection = JsonConvert.DeserializeObject<List<JSONData>>(response);
        }

        private List<Brush> GetColors()
        {
            return new List<Brush>
            {
                Color.FromArgb("#FF8B1FA9"), Color.FromArgb("#FFD20100"), Color.FromArgb("#FFFC571D"), Color.FromArgb("#FF36B37B"), Color.FromArgb("#FF3D4FB5"),
                Color.FromArgb("#FF3D4FB5"), Color.FromArgb("#FF01A1EF"), Color.FromArgb("#FF0F8644"), Color.FromArgb("#FF00ABA9")
            };
        }

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Invoke method when property changed.
        /// </summary>
        /// <param name="propertyName">property name</param>
        private void RaiseOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
