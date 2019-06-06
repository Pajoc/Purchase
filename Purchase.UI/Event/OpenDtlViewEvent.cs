using Prism.Events;

namespace Purchase.UI.Event
{
    public class OpenDtlViewEvent:PubSubEvent<OpenDtlViewEventArgs>
    {
        
    }

    public class OpenDtlViewEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }

    }
}
