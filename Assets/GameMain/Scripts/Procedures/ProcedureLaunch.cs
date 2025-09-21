using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class ProcedureLaunch : ProcedureBase
    {
        private Dictionary<string, bool> m_LoadedFlag = new();

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("Init Game.");

            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.DataNode.GetOrAddNode(Constant.ProcedureRunningData.NextSceneName).SetData<VarString>("Menu");
            GameEntry.DataNode.GetOrAddNode(Constant.ProcedureRunningData.TransitionalMessage).SetData<VarString>("Loading.");
            GameEntry.DataNode.GetOrAddNode(Constant.ProcedureRunningData.CanChangeProcedure).SetData<VarBoolean>(false);
            m_LoadedFlag.Clear();
            PreloadResources();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            IEnumerator<bool> iter = m_LoadedFlag.Values.GetEnumerator();
            while (iter.MoveNext())
            {
                if (!iter.Current)
                {
                    Log.Info("Data table loading...");
                    return;
                }
            }
            Log.Info("All data table loaded.");
            //所有资源加载就绪，进入场景切换流程
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

        }

        private void PreloadResources()
        {
            // LoadDataTable("Sound");
            // LoadDataTable("Tools");
            LoadDataTable("Player");
            LoadDataTable("Entity");
            LoadDataTable("Scene");
            LoadDataTable("UIForm");
            // LoadDataTable("Camera");
        }
        private void LoadDataTable(string dataTableName)
        {
            string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, false);
            m_LoadedFlag.Add(dataTableAssetName, false);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
        }
        /// <summary>
        /// 如果匹配，它会将资源表名称在 m_LoadedFlag 字典中的状态设置为 true，并输出成功加载的日志信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DataTableAssetName] = true;

            Log.Info("Load dictionary '{0}' OK.", ne.DataTableAssetName);
        }
    }
}
