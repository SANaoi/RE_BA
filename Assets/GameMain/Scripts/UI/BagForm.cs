using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace KSG
{
    public class BagForm : UGuiFormEx
    {
        private ProcedureLevel m_ProcedureLevel = null;
        private Transform m_PlayerTab = null;
        private Transform m_ItemTab = null;
        private GameObject m_CharacterInfo = null;
        private GameObject m_ItemInfo = null;
        private Transform m_ItemContent = null;
        private Text m_PlayerHpText = null;
        private Text m_PlayerSpeedText = null;
        private GameObject m_ItemCellTemplate = null;
        private bool m_IsLoadingItemCellTemplate = false;
        private readonly List<ItemCellForm> m_ItemCells = new List<ItemCellForm>();

        private const int DefaultItemCellCapacity = 24;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            UIWindowController.RegisterForm(EnumUIForm.BagForm, true, true, true);
            m_ProcedureLevel = userData as ProcedureLevel;
            EnsureControls();
            Subscribe(PickupItemEventArgs.EventId, OnPickupItem);
            ShowCharacterInfo();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            UIWindowController.UnregisterForm(EnumUIForm.BagForm);
            base.OnClose(isShutdown, userData);
        }

        protected override void RefreshLocalization()
        {
            base.RefreshLocalization();
            RefreshCurrentView();
        }

        private void ShowCharacterInfo()
        {
            SetViewActive(true);
            RefreshCharacterInfo();
        }

        private void ShowItemInfo()
        {
            SetViewActive(false);
            RefreshItemInfo();
        }

        private void SetViewActive(bool showCharacterInfo)
        {
            if (m_CharacterInfo != null)
            {
                m_CharacterInfo.SetActive(showCharacterInfo);
            }

            if (m_ItemInfo != null)
            {
                m_ItemInfo.SetActive(!showCharacterInfo);
            }
        }

        private void RefreshCurrentView()
        {
            if (m_CharacterInfo != null && m_CharacterInfo.activeSelf)
            {
                RefreshCharacterInfo();
            }

            if (m_ItemInfo != null && m_ItemInfo.activeSelf)
            {
                RefreshItemInfo();
            }
        }

        private void RefreshCharacterInfo()
        {
            PlayerLogic player = m_ProcedureLevel != null ? m_ProcedureLevel.CurrentPlayer : null;
            EntityDataPlayer playerData = player != null ? player.playerData : null;

            if (m_PlayerHpText != null)
            {
                if (playerData == null)
                {
                    m_PlayerHpText.text = "HP --";
                }
                else
                {
                    m_PlayerHpText.text = Utility.Text.Format("HP {0}/{1}", player.HP.ToString("0"), playerData.MaxHP.ToString("0"));
                }
            }

            if (m_PlayerSpeedText != null)
            {
                m_PlayerSpeedText.text = playerData != null
                    ? Utility.Text.Format("Speed {0}", playerData.Speed.ToString("0.##"))
                    : "Speed --";
            }
        }

        private void RefreshItemInfo()
        {
            PlayerInventory inventory = m_ProcedureLevel != null ? m_ProcedureLevel.Inventory : null;
            IReadOnlyList<InventoryItemStack> items = inventory != null ? inventory.Items : null;
            int itemCount = items != null ? items.Count : 0;

            EnsureItemCells(DefaultItemCellCapacity);

            if (m_ItemCells.Count < DefaultItemCellCapacity)
            {
                return;
            }

            for (int i = 0; i < m_ItemCells.Count; i++)
            {
                InventoryItemStack stack = i < itemCount ? items[i] : null;
                m_ItemCells[i].Refresh(stack);
            }
        }

        private void EnsureControls()
        {
            m_PlayerTab = FindFirstTransform("Player", null);
            m_CharacterInfo = FindFirstTransform("CharacterInfo", null)?.gameObject;
            m_ItemInfo = FindFirstTransform("ItemInfo", HasScrollRectInChildren)?.gameObject;
            m_ItemTab = FindFirstTransform("ItemInfo", t => t.gameObject != m_ItemInfo);

            m_PlayerHpText = m_CharacterInfo != null ? FindFirstTransform(m_CharacterInfo.transform, "PlayerHP")?.GetComponent<Text>() : null;
            m_PlayerSpeedText = m_CharacterInfo != null ? FindFirstTransform(m_CharacterInfo.transform, "PlayerSpeed")?.GetComponent<Text>() : null;
            ScrollRect itemScrollRect = m_ItemInfo != null ? m_ItemInfo.GetComponentInChildren<ScrollRect>(true) : null;
            m_ItemContent = itemScrollRect != null ? itemScrollRect.content : null;

            BindButton(m_PlayerTab, ShowCharacterInfo);
            BindButton(m_ItemTab, ShowItemInfo);
        }

        private void BindButton(Transform target, UnityAction onClick)
        {
            if (target == null)
            {
                return;
            }

            Button button = target.gameObject.GetOrAddComponent<Button>();
            button.onClick.RemoveListener(onClick);
            button.onClick.AddListener(onClick);

            if (button.targetGraphic == null)
            {
                button.targetGraphic = target.GetComponentInChildren<Graphic>(true);
            }
        }

        private void EnsureItemCells(int itemCount)
        {
            if (m_ItemContent == null)
            {
                return;
            }

            HideStaticItemCellPlaceholders();

            if (m_ItemCellTemplate != null)
            {
                SyncItemCells(itemCount);
                return;
            }

            if (m_IsLoadingItemCellTemplate)
            {
                return;
            }

            m_IsLoadingItemCellTemplate = true;
            GameEntry.Resource.LoadAsset(
                AssetUtility.GetUIFormAsset("ItemCellForm"),
                typeof(GameObject),
                Constant.AssetPriority.UIFormAsset,
                new LoadAssetCallbacks(OnLoadItemCellTemplateSuccess, OnLoadItemCellTemplateFailure),
                DefaultItemCellCapacity);
        }

        private void OnLoadItemCellTemplateSuccess(string assetName, object asset, float duration, object userData)
        {
            m_IsLoadingItemCellTemplate = false;
            m_ItemCellTemplate = asset as GameObject;
            if (m_ItemCellTemplate == null)
            {
                Log.Error("Loaded item cell template '{0}' is invalid.", assetName);
                return;
            }

            int itemCount = userData is int ? (int)userData : 0;
            SyncItemCells(itemCount);
            RefreshItemInfo();
        }

        private void OnLoadItemCellTemplateFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            m_IsLoadingItemCellTemplate = false;
            Log.Error("Can not load item cell template '{0}', status '{1}', error message '{2}'.", assetName, status.ToString(), errorMessage);
        }

        private void SyncItemCells(int itemCount)
        {
            if (m_ItemContent == null || m_ItemCellTemplate == null)
            {
                return;
            }

            while (m_ItemCells.Count < itemCount)
            {
                int index = m_ItemCells.Count;
                GameObject itemCellObject = Instantiate(m_ItemCellTemplate, m_ItemContent, false);
                itemCellObject.name = Utility.Text.Format("ItemCell ({0})", index + 1);

                ItemCellForm itemCell = itemCellObject.GetComponent<ItemCellForm>();
                if (itemCell == null)
                {
                    itemCell = itemCellObject.AddComponent<ItemCellForm>();
                }

                m_ItemCells.Add(itemCell);
            }

            while (m_ItemCells.Count > itemCount)
            {
                int lastIndex = m_ItemCells.Count - 1;
                ItemCellForm itemCell = m_ItemCells[lastIndex];
                m_ItemCells.RemoveAt(lastIndex);

                if (itemCell != null)
                {
                    Destroy(itemCell.gameObject);
                }
            }
        }

        private void HideStaticItemCellPlaceholders()
        {
            if (m_ItemContent == null)
            {
                return;
            }

            for (int i = 0; i < m_ItemContent.childCount; i++)
            {
                Transform child = m_ItemContent.GetChild(i);
                if (IsItemCellName(child.name) && child.GetComponent<ItemCellForm>() == null)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        private void OnPickupItem(object sender, GameEventArgs e)
        {
            RefreshItemInfo();
        }

        private Transform FindFirstTransform(string targetName, Predicate<Transform> predicate)
        {
            return FindFirstTransform(transform, targetName, predicate);
        }

        private Transform FindFirstTransform(Transform root, string targetName, Predicate<Transform> predicate = null)
        {
            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name != targetName)
                {
                    continue;
                }

                if (predicate == null || predicate(transforms[i]))
                {
                    return transforms[i];
                }
            }

            return null;
        }

        private bool HasScrollRectInChildren(Transform target)
        {
            return target.GetComponentInChildren<ScrollRect>(true) != null;
        }

        private bool IsItemCellName(string objectName)
        {
            return objectName == "ItemCell" || objectName.StartsWith("ItemCell (");
        }

    }
}
