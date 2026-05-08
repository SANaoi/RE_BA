using System;
using GameFramework;
using GameFramework.Event;

namespace KSG
{
    public class UGuiFormEx : UGuiForm
    {
        private EventSubscriber m_eventSubscriber;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Subscribe(LanguageChangedEventArgs.EventId, OnLanguageChanged);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            UnSubscribeAll();
            if (m_eventSubscriber != null)
            {
                ReferencePool.Release(m_eventSubscriber);
                m_eventSubscriber = null;
            }
        }

        protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (m_eventSubscriber == null)
            {
                m_eventSubscriber = EventSubscriber.Create(this);
            }

            m_eventSubscriber.Subscribe(id, handler);
        }

        protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (m_eventSubscriber != null)
            {
                m_eventSubscriber.UnSubscribe(id, handler);
            }
        }

        protected void UnSubscribeAll()
        {
            if (m_eventSubscriber != null)
            {
                m_eventSubscriber.UnSubscribeAll();
            }
        }

        protected virtual void OnLanguageChanged(object sender, GameEventArgs e)
        {
            RefreshLocalization();
        }
    }
}
