namespace Exercise.UI.ViewModel.EventHandlers
{
    public interface INotifyPropertyDeltaChanged
    {
        event PropertyDeltaChangedEventHandler PropertyDeltaChanged;
    }
}