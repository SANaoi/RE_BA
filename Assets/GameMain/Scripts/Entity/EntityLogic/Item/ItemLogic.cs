namespace KSG
{
    public class ItemLogic : EntityLogicBase
    {
        private const float PickupRadius = 1.25f;

        private EntityDataItem m_ItemData = null;
        private bool m_IsPicked = false;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ItemData = userData as EntityDataItem;
            if (m_ItemData == null)
            {
                return;
            }

            m_IsPicked = false;
            EnsurePickupTrigger();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            m_ItemData = null;
            m_IsPicked = false;
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (m_IsPicked || m_ItemData == null)
            {
                return;
            }

            PlayerLogic player = other.GetComponentInParent<PlayerLogic>();
            if (player == null)
            {
                return;
            }

            m_IsPicked = true;
            GameEntry.Event.Fire(this, PickupItemEventArgs.Create(Entity.Id, m_ItemData.ItemId, 1, player.Id));
        }

        private void EnsurePickupTrigger()
        {
            UnityEngine.SphereCollider pickupCollider = GetComponent<UnityEngine.SphereCollider>();
            if (pickupCollider == null)
            {
                pickupCollider = gameObject.AddComponent<UnityEngine.SphereCollider>();
            }

            pickupCollider.isTrigger = true;
            pickupCollider.radius = PickupRadius;
        }
    }
}
