using System;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ShowEntityInLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowEntityInLevelEventArgs).GetHashCode();
        public override int Id => EventId;
        public int EntityId
        {
            get;
            private set;
        }

        public Type type
        {
            get;
            private set;
        }
        public string entityGroup
        {
            get;
            private set;
        }

        public string entityfolder
        {
            get;
            private set;
        }

        public int priority
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
            type = null;
            entityGroup = null;
            entityfolder = null;
            priority = 0;
            ShowSuccess = null;
            entityData = null;
        }

        public static ShowEntityInLevelEventArgs Create
        (
            Type entityType,
            string entityGroup,
            string entityfolder,
            int priority,
            Action<Entity> showSuccess,
            EntityData entityData,
            object userData = null
        )
        {
            ShowEntityInLevelEventArgs ShowEntityInLevelEventArgs = ReferencePool.Acquire<ShowEntityInLevelEventArgs>();
            ShowEntityInLevelEventArgs.type = entityType;
            ShowEntityInLevelEventArgs.entityGroup = entityGroup;
            ShowEntityInLevelEventArgs.entityfolder = entityfolder;
            ShowEntityInLevelEventArgs.priority = priority;
            ShowEntityInLevelEventArgs.ShowSuccess = showSuccess;
            ShowEntityInLevelEventArgs.entityData = entityData;
            return ShowEntityInLevelEventArgs;
        }
        public override void Clear()
        {
            type = null;
            entityGroup = null;
            entityfolder = null;
            priority = 0;
            ShowSuccess = null;
            entityData = null;
        }
    }
}