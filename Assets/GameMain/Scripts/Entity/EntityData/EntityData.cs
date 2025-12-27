//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using GameFramework;
using UnityEngine;

namespace KSG
{
    [Serializable]
    public class EntityData : IReference
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_TypeId;

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        [SerializeField]
        private Quaternion m_Rotation = Quaternion.identity;

        public object UserData
        {
            get;
            protected set;
        }

        public EntityData()
        {
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            UserData = null;
        }

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId
        {
            get
            {
                return m_TypeId;
            }
            set
            {
                m_TypeId = value;
            }
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }
        
        public static EntityData Create(int entityId, int typeId, Vector3 pos, Quaternion rotation, object userData = null)
        {
            EntityData entityData = ReferencePool.Acquire<EntityData>();
            entityData.Id = entityId;
            entityData.TypeId = typeId;
            entityData.Position = pos;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }
        public virtual void Clear()
        {
            m_Id = 0;
            m_TypeId = 0;
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            UserData = null;
        }
    }
}
