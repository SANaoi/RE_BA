using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using Unity.Mathematics;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class LevelControl : IReference
    {
        private Dictionary<int, Entity> m_dicSerialEntity;
        private Dictionary<int, Action<Entity>> m_dicCallback;

        private bool pause = false;
        public LevelControl()
        {
            m_dicSerialEntity = new Dictionary<int, Entity>();
            m_dicCallback = new Dictionary<int, Action<Entity>>();
        }
        public void OnEnter()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create
            (
                typeof(PlayerLogic),
                "Player", 
                "Player", 
                Constant.AssetPriority.PlayerAsset,
                (entity) => {GameEntry.UI.OpenUIForm(EnumUIForm.CrosshairForm); },
                EntityDataPlayer.Create(GameEntry.Entity.GenerateSerialId(), (int)EnumEntity.Momoi_Original)
            ));
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create
            (
                typeof(ItemLogic),
                "Item", 
                "Item", 
                Constant.AssetPriority.ItemAsset,
                (entity) => { },
                EntityDataItem.Create(GameEntry.Entity.GenerateSerialId(), (int)EnumEntity.Envelope)
            ));
        }

        public void OnUpdate()
        {

        }

        public void Quick()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

        }
        public void Resume()
        {
            pause = false;

        }
        public Entity GetEntity<T>(int serialId)
        {
            Entity entity = null;
            if (m_dicSerialEntity.TryGetValue(serialId, out entity))
            {
                return m_dicSerialEntity[serialId];
            }
            return null;
        }
        public IEnumerable<Entity> GetAllEntities()
        {
            return m_dicSerialEntity.Values;
        }

        public int ShowEntity
        (
            Type entityType,
            string entityGroup,
            string entityfolder,
            int priority,
            Action<Entity> showSuccess,
            EntityData entityData
        )
        {
            // serialId == entityData.Id;
            m_dicCallback.Add(entityData.Id, showSuccess);
            EntityExtension.ShowEntity
            (
                GameEntry.Entity,
                entityType,
                entityGroup,
                entityfolder,
                priority,
                entityData
            );

            return entityData.Id;
        }
        public void HideEntity(int serialId)
        {
            Entity entity = null;
            if (!m_dicSerialEntity.TryGetValue(serialId, out entity))
            {
                Log.Error("Can find entity('serial id:{0}') ", serialId);
            }
            m_dicSerialEntity.Remove(serialId);
            m_dicCallback.Remove(serialId);
            Entity[] entities = GameEntry.Entity.GetChildEntities(entity);
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    //若Child Entity由这个Loader对象托管，则由此Loader释放
                    if (m_dicSerialEntity.ContainsKey(item.Id))
                    {
                        HideEntity(item);
                    }
                    else//若Child Entity不由这个Loader对象托管，则从Parent Entity脱离
                        GameEntry.Entity.DetachEntity(item);
                }
            }
            GameEntry.Entity.HideEntity(entity);
        }
        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            HideEntity(entity.Id);
        }
        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne == null)
            {
                return;
            }

            Action<Entity> callback;
            if (!m_dicCallback.TryGetValue(ne.Entity.Id, out callback))
            {
                return;
            }

            m_dicSerialEntity.Add(ne.Entity.Id, ne.Entity);
            callback?.Invoke(ne.Entity);
        }
        public static LevelControl Create()
        {
            LevelControl levelControl = ReferencePool.Acquire<LevelControl>();
            return levelControl;
        }
        public void Clear()
        {
            m_dicSerialEntity.Clear();
            m_dicCallback.Clear();
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        }
    }
}