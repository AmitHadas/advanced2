﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ImageServiceGui.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;

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
        private string m_vm_outpurDir;
        public string VM_OutputDir
        {
            get { return this.m_settingsModel.OutputDir; }
            set
            {
                this.m_settingsModel.OutputDir = value;
            }
        }
        private string m_vm_sourceName;
        public string VM_SourceName
        {
            get { return this.m_settingsModel.SourceName; }
            set
            {
                this.m_settingsModel.SourceName = value;
            }
        }

        private string m_vm_logName;
        public string VM_LogName
        {
            get { return this.m_settingsModel.LogName; }
            set
            {
                this.m_settingsModel.LogName = value;
            }
        }

        private string m_vm_thumbnailSize;
        public string VM_ThumbnailSize
        {
            get { return this.m_settingsModel.ThumbSize; }
            set
            {
                this.m_settingsModel.ThumbSize = value;
            }
        }
        private ObservableCollection<string> m_vm_handlersList;
        public ObservableCollection<string> VM_HandlersList
        {
            get { return m_settingsModel.HandlersList;}
            set { m_settingsModel.HandlersList = value; }
        }

        private string m_vm_selectedHandler;
        public string VM_SelectedHandler
        {
            get { return this.m_settingsModel.SelectedHandler; }
            set
            {
                this.m_settingsModel.SelectedHandler = value;
            }
        }
        public SettingsViewModel()
        {
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.m_settingsModel = new SettingsModel();
            m_settingsModel.PropertyChanged += PropertyChangedMethod;
            m_settingsModel.OutputDir = "noa";
            m_settingsModel.LogName = "amit";
            m_settingsModel.SourceName = "source";
            m_settingsModel.ThumbSize = "8";
            m_settingsModel.HandlersList = new ObservableCollection<string> { "hi", "there", "amit", "and", "noa" };

        }

        public SettingsModel SettingsModel
        {
            get { return this.m_settingsModel; }
            set
            {
                this.m_settingsModel = value;
            }
        }

        public ICommand RemoveCommand { get; private set; }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OnRemove(object obj)
        {
            for (int i = 0; i < VM_HandlersList.Count; i++)
            {
                if (VM_HandlersList[i].Equals(VM_SelectedHandler))
                {
                    this.VM_HandlersList.RemoveAt(i);
                    return;
                }
            }
        }
        private bool CanRemove(object obj)
        {
            return (!string.IsNullOrEmpty(VM_SelectedHandler));
        }
        private void PropertyChangedMethod(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
            NotifyPropertyChanged(e.PropertyName);
        }
    }
}