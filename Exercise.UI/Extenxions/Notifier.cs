using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Exercise.UI.ViewModel.EventHandlers;

namespace Exercise.UI.Extenxions
{
    // It could be also replaced with RealProxy or AoP for attribute based INotifyPropertyChanged implementation. 
    public static class Notifier
    {
        public static void Notify<TField>(
            this INotifyPropertyChanged sender, 
            PropertyChangedEventHandler handler, 
            ref TField field, 
            TField value, 
            [CallerMemberName] string propertyName = null)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (EqualityComparer<TField>.Default.Equals(field, value))
            {
                return;
            }
            field = value;
            OnPropertyChanged(sender, handler, propertyName);
        }

        public static void Notify<TSender>(
            this TSender sender,
            PropertyChangedEventHandler handler, 
            PropertyDeltaChangedEventHandler deltaHandler, 
            ref decimal field, 
            decimal value, 
            [CallerMemberName] string propertyName = null) 
            where TSender: INotifyPropertyDeltaChanged, INotifyPropertyChanged
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (EqualityComparer<decimal>.Default.Equals(field, value))
            {
                return;
            }
            var old = field;
            field = value;
            OnPropertyChanged(sender, handler, propertyName);
            OnPropertyDeltaChanged(sender, deltaHandler, value, old, propertyName);
        }

        private static void OnPropertyChanged(INotifyPropertyChanged sender, PropertyChangedEventHandler handler, string propertyName)
        {
            handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        private static void OnPropertyDeltaChanged(INotifyPropertyDeltaChanged sender, PropertyDeltaChangedEventHandler handler, decimal newValue, decimal oldValue, string propertyName)
        {
            handler?.Invoke(sender, new PropertyDeltaChangedEventArgs(propertyName, newValue, oldValue));
        }
    }
}