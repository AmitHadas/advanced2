using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ImageServiceGui.Model;

namespace ImageServiceGui.Views_Model
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        private SettingsModel m_settingsModel;

        public SettingsViewModel()
        {
            this.m_settingsModel = new SettingsModel();
            m_settingsModel.OutputDir = "noa";
            m_settingsModel.LogName = "amit";
            m_settingsModel.SourceName = "source";
            m_settingsModel.ThumbSize = "8";
            m_settingsModel.PropertyChanged +=
       delegate (Object sender, PropertyChangedEventArgs e) {
           NotifyPropertyChanged(e.PropertyName);
       };

            // לעשות רשימת הנדלרים
            // this.AllColors = new[] { "Red", "Blue", "Green" };
        }

        public SettingsModel SettingsModel
        {
            get { return this.m_settingsModel; }
            set
            {
                this.m_settingsModel = value;
            }
        }
    }
}
