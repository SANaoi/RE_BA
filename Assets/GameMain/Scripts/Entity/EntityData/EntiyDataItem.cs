using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class EntityDataItem : EntityData
    {
        [SerializeField]
        private int m_ItemId = 0;
        public EntityDataItem()
        {
            m_ItemId = 0;
        }

        public static EntityDataItem Create(int entityId, int typeId)
        {
            EntityDataItem data = ReferencePool.Acquire<EntityDataItem>();
            data.Id = entityId;
            data.TypeId = typeId;
            IDataTable<DRItem> dt = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem row = dt.GetDataRow(typeId);
            if (row != null)
            {
            }
            else
            {
                Log.Warning("Can not find Item id '{0}'.", typeId);
            }
            return data;
        }

        public override void Clear()
        {
            base.Clear();
            m_ItemId = 0;
        }
        /// <summary>
        /// 物品编号。
        /// </summary>
        public int ItemId
        {
            get
            {
                return m_ItemId;
            }
        }
    }
}