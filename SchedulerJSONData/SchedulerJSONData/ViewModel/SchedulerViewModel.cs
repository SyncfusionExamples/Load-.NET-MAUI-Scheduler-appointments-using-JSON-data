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
        }
        private async void LoadAppointments(object obj)
        {
            if (jsonDataCollection == null)
                await GetJsonData();

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
