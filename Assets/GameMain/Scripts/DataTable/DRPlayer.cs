//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2026-05-14 02:18:18.873
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace KSG
{
    /// <summary>
    /// 角色配置表1000。
    /// </summary>
    public class DRPlayer : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取角色编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取血量。
        /// </summary>
        public float HP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取移速。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取相机编号。
        /// </summary>
        public int CameraId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹编号。
        /// </summary>
        public int ProjectileEntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹类型。
        /// </summary>
        public string ProjectileType
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            HP = float.Parse(columnStrings[index++]);
            Speed = float.Parse(columnStrings[index++]);
            CameraId0 = int.Parse(columnStrings[index++]);
            ProjectileEntityId = int.Parse(columnStrings[index++]);
            ProjectileType = columnStrings[index++];

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    HP = binaryReader.ReadSingle();
                    Speed = binaryReader.ReadSingle();
                    CameraId0 = binaryReader.Read7BitEncodedInt32();
                    ProjectileEntityId = binaryReader.Read7BitEncodedInt32();
                    ProjectileType = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] m_CameraId = null;

        public int CameraIdCount
        {
            get
            {
                return m_CameraId.Length;
            }
        }

        public int GetCameraId(int id)
        {
            foreach (KeyValuePair<int, int> i in m_CameraId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetCameraId with invalid id '{0}'.", id.ToString()));
        }

        public int GetCameraIdAt(int index)
        {
            if (index < 0 || index >= m_CameraId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetCameraIdAt with invalid index '{0}'.", index.ToString()));
            }

            return m_CameraId[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_CameraId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, CameraId0),
            };
        }
    }
}
