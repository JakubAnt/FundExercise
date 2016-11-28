using System;

namespace Exercise.UI.ViewModel.EventHandlers
{
    public delegate void PropertyDeltaChangedEventHandler(object sender, PropertyDeltaChangedEventArgs e);

    public class PropertyDeltaChangedEventArgs : EventArgs
    {
        public PropertyDeltaChangedEventArgs(string propertyName, decimal newValue, decimal oldValue)
        {
            PropertyName = propertyName;
            PreviousValue = oldValue;
            NewValue = newValue;
        }
        public virtual string PropertyName { get; }

        public virtual decimal PreviousValue { get; }

        public virtual decimal NewValue { get; }

        public virtual decimal Delta => NewValue - PreviousValue;
    }
}