using System;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public abstract class EntityLogicBase : EntityLogic, IPause
    {
        [SerializeField]
        private EntityData m_entityData = null;
        private EventSubscriber m_eventSubscriber = null;
        protected bool pause = false;

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        public Animation CachedAnimation
        {
            get;
            private set;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CachedAnimation = GetComponent<Animation>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_entityData = userData as EntityData;
            if (m_entityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }

            Name = Utility.Text.Format("[Entity {0}]", Id);
            CachedTransform.localPosition = m_entityData.Position;
            CachedTransform.localRotation = m_entityData.Rotation;
            CachedTransform.localScale = Vector3.one;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            UnSubscribeAll();
            
            if (m_eventSubscriber != null)
            {
                ReferencePool.Release(m_eventSubscriber);
                m_eventSubscriber = null;
            }
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }


        protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (m_eventSubscriber == null)
                m_eventSubscriber = EventSubscriber.Create(this);

            m_eventSubscriber.Subscribe(id, handler);
        }

        protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (m_eventSubscriber != null)
                m_eventSubscriber.UnSubscribe(id, handler);
        }

        protected void UnSubscribeAll()
        {
            if (m_eventSubscriber != null)
                m_eventSubscriber.UnSubscribeAll();
        }

        public virtual void Pause()
        {
            pause = true;
        }

        public virtual void Resume()
        {
            pause = false;
        }
    }
}
