using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniInventory.Items;

namespace UniInventory.Container
{
    /// <summary>
    /// One Implementation of the container class. Store items in a linear fashion
    /// </summary>
    public class ContainerLinear : ContainerBase
    {
        public int Capacity;
        List<ItemStack> StoredStacks = new List<ItemStack>();

        /// <summary>
        /// total number if item stacks in the container
        /// </summary>
        public int Count
        {
            get
            {
                return StoredStacks.Count;
            }
        }

        protected override void InitializeContainer(List<ItemStackInfo> stacksInfo)
        {
            foreach (ItemStackInfo info in stacksInfo)
            {
                StoredStacks.Add(new ItemStack(info.itemId, info.stackSize));
            }
        }

        /// <summary>
        /// Try to add one item stack into the container
        /// </summary>
        /// <param name="newStack">the item stack to addparam>
        /// <returns>if the stack is only partially added, return the remaining item stack</returns>
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

        /// <summary>
        /// Get item stack with index
        /// </summary>
        /// <param name="index">the index of the array to get item stack</param>
        /// <returns>the item stack to get or null</returns>
        public ItemStack GetItemStackAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogError("[ContainerLinear] Item index " + index + " out of range, returning null.");
                return null;
            }
            return StoredStacks[index];
        }

        /// <summary>
        /// Remove the item stack at given index
        /// </summary>
        /// <param name="index">index to remove</param>
        /// <returns>the item stack removed</returns>
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


        /// <summary>
        /// get all item stacks
        /// </summary>
        /// <returns>a list of stacks</returns>
        public override List<ItemStack> GetItemStacks()
        {
            return StoredStacks;
        }

        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// My custom update function
        /// </summary>
        /// <param name="deltaTime">the delta time of this update</param>
        public override void Update(float deltaTime)
        {
            StoredStacks.ForEach(stack => stack.Update(deltaTime));
            StoredStacks.RemoveAll(stack => stack.stackSize == 0);
        }
    }
}
