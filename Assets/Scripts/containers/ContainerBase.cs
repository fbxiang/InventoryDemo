﻿using UnityEngine;
using UniInventory.Inventory;
using UniInventory.Items;
using System.Collections.Generic;

namespace UniInventory.Container
{

    [System.Serializable]
    public struct ItemStackInfo
    {
        public int itemId;
        public int stackSize;
        public InfoTreeInitializationObject infoTree;
    }


    public abstract class ContainerBase : MonoBehaviour, IInventory
    {
        public List<ItemStackInfo> StacksInfo;

        
        void Awake()
        {
            InitializeContainer(StacksInfo);
        }

        protected abstract void InitializeContainer(List<ItemStackInfo> stacksInfo);

        /// <summary>
        /// Get all stacks stored in the container
        /// </summary>
        /// <returns>a list of all stacks</returns>
        public abstract List<ItemStack> GetItemStacks();
        /// <summary>
        /// Try to add one item stack into the container
        /// </summary>
        /// <param name="stack">the item stack to addparam>
        /// <returns>if the stack is only partially added, return the remaining item stack</returns>
        public abstract ItemStack AddItemStack(ItemStack stack);

        public abstract void Update(float deltaTime);

        public virtual void Update()
        {
            Update(Time.deltaTime);
        }

    }
}
