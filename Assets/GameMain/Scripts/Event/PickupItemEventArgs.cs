using GameFramework;
using GameFramework.Event;

namespace KSG
{
    public class PickupItemEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PickupItemEventArgs).GetHashCode();
        public override int Id => EventId;

        public int ItemEntityId
        {
            get;
            private set;
        }

        public int ItemId
        {
            get;
            private set;
        }

        public int Count
        {
            get;
            private set;
        }

        public int PickerEntityId
        {
            get;
            private set;
        }

        public PickupItemEventArgs()
        {
            ItemEntityId = 0;
            ItemId = 0;
            Count = 0;
            PickerEntityId = 0;
        }

        public static PickupItemEventArgs Create(int itemEntityId, int itemId, int count, int pickerEntityId)
        {
            PickupItemEventArgs args = ReferencePool.Acquire<PickupItemEventArgs>();
            args.ItemEntityId = itemEntityId;
            args.ItemId = itemId;
            args.Count = count;
            args.PickerEntityId = pickerEntityId;
            return args;
        }

        public override void Clear()
        {
            ItemEntityId = 0;
            ItemId = 0;
            Count = 0;
            PickerEntityId = 0;
        }
    }
}
