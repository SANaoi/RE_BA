using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace KSG
{
    public class EntityDataCamera : EntityData
    {
        public Vector3 DefaultLocalPosition { get; private set; }

        public static EntityDataCamera Create(int entityId, int typeId)
        {
            EntityDataCamera data = ReferencePool.Acquire<EntityDataCamera>();
            data.Id = entityId;
            data.TypeId = typeId;

            // 加载 DataTable
            IDataTable<DRCamera> dt = GameEntry.DataTable.GetDataTable<DRCamera>();
            DRCamera row = dt.GetDataRow(typeId);
            if (row != null)
            {
                data.DefaultLocalPosition = row.DefaultLocalPosition;
            }

            return data;
        }

        public override void Clear()
        {
            base.Clear();
            DefaultLocalPosition = Vector3.zero;
        }
    }
}