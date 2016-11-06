using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniInventory.Items;

namespace UniInventory.Container
{

    class Slot
    {
        public ItemStack Stack;

        public bool Empty()
        {
            return Stack == null;
        }
    }
    public class ContainerSlots : ContainerBase
    {
        public int Capacity = 10; // max capacity

        private Slot[] ItemSlots; // Item slots used to store item stacks

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
                ItemSlots[i].Stack = new ItemStack(stacksInfo[i].itemId, stacksInfo[i].stackSize);
                ItemSlots[i].Stack.infoTree.UpdateWith(stacksInfo[i].infoTree.ToInfoTree());
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
            return ItemSlots[index].Stack;
        }

        /// <summary>
        /// Find am empty slot with the smallest index
        /// </summary>
        /// <returns>the empty slot or null if all are filled</returns>
        private Slot FindFirstEmptySlot()
        {
            foreach (Slot slot in ItemSlots)
            {
                if (slot.Empty())
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
                if (!slot.Empty() && slot.Stack.mergeable(stack))
                {
                    remainder = slot.Stack.mergeWith(stack);
                    if (remainder == null) return null;
                }
            }
            Slot emptySlot = FindFirstEmptySlot();
            if (emptySlot == null)
                return remainder;
            emptySlot.Stack = remainder;
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
                if (!slot.Empty())
                {
                    stacks.Add(slot.Stack);
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

        public override void Update(float deltaTime)
        {
            foreach (Slot slot in ItemSlots)
            {
                if (!slot.Empty())
                {
                    slot.Stack.Update(deltaTime);
                    if (slot.Stack.stackSize == 0)
                        slot.Stack = null;
                }
            }
        }
    }
}

