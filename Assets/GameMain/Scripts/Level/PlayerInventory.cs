using System.Collections.Generic;

namespace KSG
{
    public sealed class InventoryItemStack
    {
        public int ItemId
        {
            get;
            private set;
        }

        public int Count
        {
            get;
            private set;
        }

        public InventoryItemStack(int itemId, int count)
        {
            ItemId = itemId;
            Count = count;
        }

        public void Add(int count)
        {
            Count += count;
        }
    }

    public sealed class PlayerInventory
    {
        private readonly List<InventoryItemStack> m_Items = new List<InventoryItemStack>();

        public IReadOnlyList<InventoryItemStack> Items
        {
            get
            {
                return m_Items;
            }
        }

        public void AddItem(int itemId, int count)
        {
            if (itemId <= 0 || count <= 0)
            {
                return;
            }

            InventoryItemStack stack = GetStack(itemId);
            if (stack != null)
            {
                stack.Add(count);
                return;
            }

            m_Items.Add(new InventoryItemStack(itemId, count));
        }

        public int GetItemCount(int itemId)
        {
            InventoryItemStack stack = GetStack(itemId);
            return stack != null ? stack.Count : 0;
        }

        public void Clear()
        {
            m_Items.Clear();
        }

        private InventoryItemStack GetStack(int itemId)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].ItemId == itemId)
                {
                    return m_Items[i];
                }
            }

            return null;
        }
    }
}
