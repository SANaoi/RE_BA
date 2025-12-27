using GameFramework;
using UnityEditor;
using UnityEngine;

namespace KSG
{
    public class EntityDataFollower : EntityData
    {
        public Transform FollowTarget { get; set; }
        public Vector3 Offset { get; set; }
        public Vector3 Scale { get; set; }

        // TODO: Sound

        public EntityDataFollower() : base()
        {
            FollowTarget = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
        }
        public static EntityDataFollower Create(int entityId, int typeId, Transform follow, Vector3 offset, Vector3 scale, object userData = null)
        {
            EntityDataFollower entityData = ReferencePool.Acquire<EntityDataFollower>();
            entityData.Id = entityId;
            entityData.TypeId = typeId;
            entityData.FollowTarget = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.UserData = userData;
            return entityData;
        }

        // public static EntityDataFollower Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, object userData = null)
        // {
        //     EntityDataFollower entityData = ReferencePool.Acquire<EntityDataFollower>();
        //     entityData.FollowTarget = follow;
        //     entityData.Offset = offset;
        //     entityData.Scale = scale;
        //     // entityData.ShowSound = enumSound;
        //     entityData.UserData = userData;
        //     return entityData;
        // }

        // public static EntityDataFollower Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, Vector3 position, Quaternion rotation, object userData = null)
        // {
        //     EntityDataFollower entityData = ReferencePool.Acquire<EntityDataFollower>();
        //     entityData.FollowTarget = follow;
        //     entityData.Offset = offset;
        //     entityData.Scale = scale;
        //     // entityData.ShowSound = enumSound;
        //     entityData.Position = position;
        //     entityData.Rotation = rotation;
        //     entityData.UserData = userData;
        //     return entityData;
        // }

        public override void Clear()
        {
            FollowTarget = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
        }
    }
}