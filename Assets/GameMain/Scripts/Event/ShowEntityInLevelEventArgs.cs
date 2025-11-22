using System;
using System.Numerics;
using GameFramework;
using GameFramework.Event;

namespace KSG
{
    public class ShowEntityInLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowEntityInLevelEventArgs).GetHashCode();
        public override int Id => EventId;
        public Type type
        {
            get;
            private set;
        }
        public int EntityId
        {
            get;
            private set;
        }
        public Action<Entity> ShowSuccess
        {
            get;
            private set;
        }

        public EntityData entityData
        {
            get;
            private set;
        }

        public ShowEntityInLevelEventArgs()
        {
            EntityId = -1;
            type = null;
            ShowSuccess = null;
            entityData = null;
        }

        public static ShowEntityInLevelEventArgs Create(
            int entityId,
            Type entityType,
            Action<Entity> showSuccess,
            EntityData entityData,
            object userData = null)
        {
            ShowEntityInLevelEventArgs ShowEntityInLevelEventArgs = ReferencePool.Acquire<ShowEntityInLevelEventArgs>();
            ShowEntityInLevelEventArgs.EntityId = entityId;
            ShowEntityInLevelEventArgs.type = entityType;
            ShowEntityInLevelEventArgs.ShowSuccess = showSuccess;
            ShowEntityInLevelEventArgs.entityData = entityData;
            return ShowEntityInLevelEventArgs;
        }
        public override void Clear()
        {
            EntityId = -1;
            type = null;
            ShowSuccess = null;
            entityData = null;
        }
    }
}