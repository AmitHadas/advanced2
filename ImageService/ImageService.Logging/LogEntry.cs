using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// Class LogEntry.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class LogEntry : INotifyPropertyChanged
    {

        // implement the iINotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        /// <summary>
        /// The m type
        /// </summary>
        private string m_type;
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get { return this.m_type; }
            set
            {
                this.m_type = value;
                OnPropertyChanged("Type");
            }
        }
        /// <summary>
        /// The m information
        /// </summary>
        private string m_info;
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info
        {
            get { return this.m_info; }
            set
            {
                this.m_info = value;
                OnPropertyChanged("Info");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="info">The information.</param>
        public LogEntry(string type, string info)
        {
            this.m_type = type;
            this.m_info = info;
        }
    }
}
