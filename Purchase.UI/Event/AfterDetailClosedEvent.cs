using Prism.Events;

namespace Purchase.UI.Event
{
    public class AfterDetailClosedEvent : PubSubEvent<AfterDetailClosedDeletedEventArgs>
    {
        
    }

    public class AfterDetailClosedDeletedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
