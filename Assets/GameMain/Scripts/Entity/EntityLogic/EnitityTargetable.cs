using System;

namespace KSG
{
    public abstract class EnitityTargetable : EntityLogicBase
    {
        protected float hp;
        public float HP
        {
            get { return hp; }
            set { hp = value; }
        }

        protected abstract float MaxHP { get;}

        public bool IsDead
        {
            get { return hp <= 0; }
        }

        public event Action<EnitityTargetable> OnDead;
        public event Action<EnitityTargetable> OnHidden;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);


        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            hp = MaxHP;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            
            OnHidden?.Invoke(this);

            OnHidden = null;
            OnDead = null;
        }

        public virtual void TakeDamage(float damage)
        {
            if (IsDead)
                return;
            hp -= damage;

            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }
        }

        protected virtual void Dead()
        {
            OnDead?.Invoke(this);
        }
    }
}
