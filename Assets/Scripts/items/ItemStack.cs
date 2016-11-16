using UnityEngine;
using System.Collections;
using UniInventory.Registry;
using UniInventory.Entity;

namespace UniInventory.Items {
    [System.Serializable]
    public class ItemStack
    {
        public int stackSize; // the size of the stack

        public int itemId; // the id of the item the stack corresponds to

        public ItemInfoTree infoTree = new ItemInfoTree(); // the info tree to store stack specific information

        private float useTime;

        public ItemStack(Item item, int size=1) : this(item.id, size) { }

        public ItemStack(int id, int size=1)
        {
            itemId = id;
            stackSize = size;
            infoTree = new ItemInfoTree();

            Item item = GetItem();
            if (item == null)
            {
                stackSize = 0;
            }

            infoTree.WriteString("description", "");

            item.OnCreate(this);
        }


        /// <summary>
        /// Called by inventory to update item state
        /// </summary>
        public void Update(float deltaTime)
        {
            Item item = GetItem();
            if (item == null)
            {
                stackSize = 0; // mark to be destroyed
                return;
            }
            item.OnUpdate(this, deltaTime);
        }


        /// <summary>
        /// Called by inventory to destroy the item
        /// </summary>
        public void Destroy()
        {
            Item item = GetItem();
            if (item != null)
            {
                item.OnDestroy(this);
            }
            this.stackSize = 0; // destroy the stack
        }

        /// <summary>
        /// Test if some other item stack has the same properties as this one so they can be merged. 
        /// </summary>
        /// <param name="other">the item stack to test</param>
        /// <returns>true iff the other item stack has the same properties</returns>
        public bool mergeable(ItemStack other)
        {
            return this.itemId.Equals(other.itemId) && this.infoTree.Equals(other.infoTree);
        }

        /// <summary>
        /// Take some number of items from othe stack, return the remaing stack. Null if none remains.
        /// If the number is larger than stack size of the other stack. The stack will be merged.
        /// If the number will overflow the max stack size of this stack, take as many as possible.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public ItemStack takeNumberReturnRemain(ItemStack other, int number)
        {
            int maxStackSize = GetItem().maxStackSize;
            if (other.stackSize <= number)
                return mergeWith(other);
            
            if (this.stackSize + number > maxStackSize)
            {
                number = maxStackSize - this.stackSize;
            }
            this.stackSize += number;
            other.stackSize -= number;
            return other;
        }

        /// <summary>
        /// Called to merge the other stack with this stack. Only call it when the two stacks are mergeable.
        /// </summary>
        /// <param name="other">the stack to merge</param>
        /// <returns>null if the other stack is fully merged, remaining item stack otherwise</returns>
        public ItemStack mergeWith(ItemStack other)
        {
            int maxStackSize = GetItem().maxStackSize;
            int totalSize = other.stackSize + this.stackSize;
            if (totalSize <= maxStackSize)
            {
                this.stackSize = totalSize;
                return null;
            }
            this.stackSize = maxStackSize;
            other.stackSize = totalSize - maxStackSize;
            return other;
        }

        /// <summary>
        /// Split the item stack into two
        /// </summary>
        /// <param name="takeOut">the amount to take out from the stack</param>
        /// <returns>null if nothing is produced</returns>
        public ItemStack Split(int takeOut)
        {
            if (takeOut <= 0)
                return null; // split nothing out
            else if(takeOut > stackSize)
                takeOut = stackSize;

            ItemStack newStack = new ItemStack(this.itemId, takeOut);
            newStack.infoTree = new ItemInfoTree(infoTree);

            this.stackSize -= takeOut;

            return newStack;
        }

        /// <summary>
        /// Get the item object from registry
        /// </summary>
        /// <returns>the item corresponding to the item id</returns>
        public Item GetItem()
        {
            return ItemRegistry.Instance.GetItem(itemId);
        }

        /// <summary>
        /// Get the sprite of the current stack
        /// </summary>
        /// <returns>the sprite</returns>
        public Texture2D GetIcon()
        {
            return GetItem().GetIcon(this);
        }

        /// <summary>
        /// Get the description of this item
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetItem().GetDescription(this);
        }

        public void use(EntityLiving user, float deltaTime)
        {
            this.useTime += deltaTime;
            if (useTime >= GetItem().GetMaxUseTime(this))
            {
                this.useTime = 0;
                GetItem().OnUse(this, user);
            }
            else
                GetItem().OnUsing(this, user, deltaTime, useTime);
        }
        
        public void hold(EntityLiving user, float deltaTime)
        {
            this.useTime = 0;
        }
    }
}
