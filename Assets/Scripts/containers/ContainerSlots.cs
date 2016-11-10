using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniInventory.Items;

namespace UniInventory.Container
{
    /// <summary>
    /// The class representing a inventory slot that stores an item stack
    /// </summary>
    class Slot
    {
        public ItemStack itemStack;

        /// <summary>
        /// Check if the slot is empty
        /// </summary>
        /// <returns>true if it is empty</returns>
        public bool IsEmpty()
        {
            return itemStack == null;
        }

        /// <summary>
        /// Call this function to update the contained item stack with time
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (itemStack != null)
            {
                itemStack.Update(deltaTime);
                if (itemStack.stackSize == 0)
                    itemStack = null;
            }
        }
    }
    public class ContainerSlots : ContainerBase
    {
        public int Capacity = 10; // max capacity

        private Slot[] ItemSlots; // Item slots used to store item stacks

        public ContainerSlots(int capacity=10)
        {
            this.Capacity = capacity;
            ItemSlots = new Slot[Capacity];
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i] = new Slot();
            }
        }

        /// <summary>
        /// Initialization at awake. Create the desired container.
        /// </summary>
        /// <param name="stacksInfo">information about the stacks</param>
        protected override void InitializeContainer(List<ItemStackInfo> stacksInfo)
        {
            ItemSlots = new Slot[Capacity];
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i] = new Slot();
            }

            for (int i = 0; i < Math.Min(stacksInfo.Count, Capacity); i++)
            {
                ItemSlots[i].itemStack = new ItemStack(stacksInfo[i].itemId, stacksInfo[i].stackSize);
                ItemSlots[i].itemStack.infoTree.UpdateWith(stacksInfo[i].infoTree.ToInfoTree());
            }
        }

        /// <summary>
        /// Get the item stack at index
        /// </summary>
        /// <param name="index">index of the item stack</param>
        /// <returns>item stack or null</returns>
        public ItemStack GetItemStackAt(int index)
        {
            if (index < 0 || index > Capacity)
            {
                Debug.LogWarning("[ContainerSlots] index out of bound");
                return null;
            }
            return ItemSlots[index].itemStack;
        }

        /// <summary>
        /// Find am empty slot with the smallest index
        /// </summary>
        /// <returns>the empty slot or null if all are filled</returns>
        private Slot FindFirstEmptySlot()
        {
            foreach (Slot slot in ItemSlots)
            {
                if (slot.IsEmpty())
                    return slot;
            }
            return null;
        }

        /// <summary>
        /// Add an item stack to the inventory
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public override ItemStack AddItemStack(ItemStack stack)
        {
            ItemStack remainder = stack;
            foreach (Slot slot in ItemSlots)
            {
                if (!slot.IsEmpty() && slot.itemStack.mergeable(stack))
                {
                    remainder = slot.itemStack.mergeWith(stack);
                    if (remainder == null) return null;
                }
            }
            Slot emptySlot = FindFirstEmptySlot();
            if (emptySlot == null)
                return remainder;
            emptySlot.itemStack = remainder;
            return null;
        }

        /// <summary>
        /// Get all item stacks
        /// </summary>
        /// <returns></returns>
        public override List<ItemStack> GetItemStacks()
        {
            List<ItemStack> stacks = new List<ItemStack>();
            foreach (Slot slot in ItemSlots)
            {
                if (!slot.IsEmpty())
                {
                    stacks.Add(slot.itemStack);
                }
            }
            return stacks;
        }

        /// <summary>
        /// Called every frame to update information
        /// </summary>
        public override void Update()
        {
            Update(Time.deltaTime);
        }

        /// <summary>
        /// Update the inventory with time
        /// </summary>
        /// <param name="deltaTime">time</param>
        public override void Update(float deltaTime)
        {
            foreach (Slot slot in ItemSlots)
            {
                slot.Update(deltaTime);
            }
        }
    }
}

