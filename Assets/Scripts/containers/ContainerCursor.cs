using UnityEngine;
using System.Collections;
using UniInventory.Items;
using System;
using System.Collections.Generic;

namespace UniInventory.Container
{
    /// <summary>
    /// This class is a single slot container to store items temporarily (for drag and drop)
    /// </summary>
    public class ContainerCursor : ContainerBase
    {
        //TODO: test this class
        public ItemStack itemStack = null;        

        protected override void InitializeContainer(List<ItemStackInfo> stacksInfo)
        {
            if (stacksInfo.Count != 0)
            {
                itemStack = new ItemStack(stacksInfo[0].itemId, stacksInfo[0].stackSize);
                itemStack.infoTree.UpdateWith(stacksInfo[0].infoTree.ToInfoTree());
            }
        }

        public override List<ItemStack> GetItemStacks()
        {
            List<ItemStack> list = new List<ItemStack>();
            if (itemStack != null)
                list.Add(itemStack);
            return list;
        }

        public override ItemStack AddItemStack(ItemStack stack)
        {
            if (this.itemStack == null)
            {
                this.itemStack = stack;
                return null;
            }
            return stack;
        }


        public override void UpdateWith(float deltaTime)
        {
            if (this.itemStack != null)
            {
                itemStack.Update(deltaTime);
                if (itemStack.stackSize <= 0)
                    itemStack = null;
            }
        }
    }

}

