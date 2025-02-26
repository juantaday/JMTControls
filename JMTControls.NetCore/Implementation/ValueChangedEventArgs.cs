namespace JMTControls.NetCore.Implementation
{
    using System;

    public class ValueChangedEventArgs : EventArgs
    {
        public decimal OldValue { get; }
        public decimal NewValue { get; }

        public ValueChangedEventArgs(decimal oldValue, decimal newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
