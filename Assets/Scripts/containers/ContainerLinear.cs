using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniInventory.Items;

namespace UniInventory.Container
{

    [System.Serializable]
    public struct ItemStackInfo
    {
        public int itemId;
        public int stackSize;
    }

    public class ContainerLinear : ContainerBase
    {
        public int Capacity;
        public List<ItemStackInfo> StacksInfo; // used to communicate with the inspector

        List<ItemStack> StoredStacks = new List<ItemStack>();

        public int Count
        {
            get
            {
                return StoredStacks.Count;
            }
        }
        
        void Awake()
        {
            foreach (ItemStackInfo info in StacksInfo)
            {
                StoredStacks.Add(new ItemStack(info.itemId, info.stackSize));
            }
        }

        public override ItemStack AddItemStack(ItemStack newStack)
        {
            foreach (ItemStack stack in StoredStacks)
            {
                if (stack.mergeable(newStack))
                {
                    newStack = stack.mergeWith(newStack);
                    if (newStack == null) return null; // we merged everything
                }
            }
            if (Count < Capacity)
            {
                StoredStacks.Add(newStack);
                return null;
            }
            return newStack;
        }

        public ItemStack GetItemStackAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogError("[ContainerLinear] Item index " + index + " out of range, returning null.");
                return null;
            }
            return StoredStacks[index];
        }

        public ItemStack RemoveItemStackAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogError("[ContainerLinear] Item index " + index + " out of range, failed to remove.");
                return null;
            }
            ItemStack stackRemoved = StoredStacks[index];
            StoredStacks.RemoveAt(index);
            return stackRemoved;
        }


        public override List<ItemStack> GetItemStacks()
        {
            return StoredStacks;
        }

        public void Update()
        {
            StoredStacks.ForEach(stack => stack.Update(Time.deltaTime));
            StoredStacks.RemoveAll(stack => stack.stackSize == 0);
        }

        public void Update(float deltaTime)
        {
            StoredStacks.ForEach(stack => stack.Update(deltaTime));
            StoredStacks.RemoveAll(stack => stack.stackSize == 0);
        }
    }
}
