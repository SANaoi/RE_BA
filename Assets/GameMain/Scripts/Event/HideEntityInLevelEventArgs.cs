using GameFramework;
using GameFramework.Event;

namespace KSG
{
    public class HideEntityInLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideEntityInLevelEventArgs).GetHashCode();
        public override int Id => EventId;
        
        public int EntityId { get; private set; }

        public HideEntityInLevelEventArgs()
        {
            EntityId = -1;
        }
        public override void Clear()
        {
            EntityId = -1;
        }
        public static HideEntityInLevelEventArgs Create(int entityId, object userData = null)
        {
            HideEntityInLevelEventArgs HideEntityInLevelEventArgs = ReferencePool.Acquire<HideEntityInLevelEventArgs>();
            HideEntityInLevelEventArgs.EntityId = entityId;
            return HideEntityInLevelEventArgs;
        }

    }
}