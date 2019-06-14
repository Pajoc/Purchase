using Prism.Events;

namespace Purchase.UI.Event
{
    public class AfterCollectionSavedEvent : PubSubEvent<AfterCollectionSavedEventArgs>
    {
        
    }

    public class AfterCollectionSavedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}
