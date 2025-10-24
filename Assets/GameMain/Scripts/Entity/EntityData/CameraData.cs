using GameFramework.DataTable;
using UnityEngine;

namespace KSG
{
    public class CameraData : EntityData
    {
        private Vector3 m_defaultLocalPosition;
        public Vector3 DefaultLocalPosition
        {
            get
            {
                return m_defaultLocalPosition;
            }
        }
        public CameraData(int entityId, int typeId) : base(entityId, typeId)
        {
            IDataTable<DRCamera> dtCamera = GameEntry.DataTable.GetDataTable<DRCamera>();
            DRCamera drCamera = dtCamera.GetDataRow(TypeId);

            if (drCamera == null)
            {
                return;
            }
            m_defaultLocalPosition = drCamera.DefaultLocalPosition;
        }
    }
}