using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerMAUI
{
    public class Event : INotifyPropertyChanged
    {
        private DateTime from;
        private DateTime to;
        private bool isAllDay;
        private string eventName;
        private Brush background;
        public string location;

        /// <summary>
        /// Gets or sets the value to display the start date.
        /// </summary>
        public DateTime From
        {
            get
            { return this.from; }
            set
            {
                this.from = value;
                this.OnPropertyChanged(nameof(From));
            }
        }

        /// <summary>
        /// Gets or sets the value to display the end date.
        /// </summary>
        public DateTime To
        {
            get { return this.to; }
            set
            {
                this.to = value;
                this.OnPropertyChanged(nameof(To));
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the appointment is all-day or not.
        /// </summary>
        public bool IsAllDay
        {
            get { return this.isAllDay; }
            set
            {
                this.isAllDay = value;
                this.OnPropertyChanged(nameof(IsAllDay));
            }
        }

        /// <summary>
        /// Gets or sets the value to display the subject.
        /// </summary>
        public string EventName
        {
            get { return this.eventName; }
            set { this.eventName = value; }
        }

        /// <summary>
        /// Gets or sets the value to display the background.
        /// </summary>
        public Brush Background
        {
            get { return this.background; }
            set { this.background = value; }
        }

        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class JSONData
    {
        public string Subject { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
