using Prism.Events;

namespace Purchase.UI.Event
{
    class AfterSupplierSavedEvent:PubSubEvent<AfterSupplierSavedEventArgs>
    {
    }

    public class AfterSupplierSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
