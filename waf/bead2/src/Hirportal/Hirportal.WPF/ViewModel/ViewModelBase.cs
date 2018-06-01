using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hirportal.WPF.ViewModel
{
    /// <summary>
    /// Nézetmodell ősosztály típusa.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Tulajdonság változásának eseménye.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Üzenet küldésének eseménye.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageApplication;

        /// <summary>
        /// Nézetmodell ősosztály példányosítása.
        /// </summary>
        protected ViewModelBase() { }

        /// <summary>
        /// Tulajdonság változása ellenőrzéssel.
        /// </summary>
        /// <param name="propertyName">Tulajdonság neve.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Üzenet küldésének eseménykiváltása.
        /// </summary>
        /// <param name="message">Az üzenet.</param>
        protected void OnMessageApplication(String message)
        {
            MessageApplication?.Invoke(this, new MessageEventArgs(message));
        }
    }
}
