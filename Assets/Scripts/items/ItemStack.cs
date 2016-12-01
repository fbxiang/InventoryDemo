using UnityEngine;
using System.Collections.Generic;
using UniInventory.Registry;
using UniInventory.Entity;
using System.Linq;

namespace UniInventory.Items {
    [System.Serializable]
    public class ItemStack
    {
        public int stackSize; // the size of the stack

        public int itemId; // the id of the item the stack corresponds to

        public ItemInfoTree infoTree = new ItemInfoTree(); // the info tree to store stack specific information

        public float useTime;

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

        public AudioClip GetClickSound()
        {
            return GetItem().GetClickSound(this);
        }

        /// <summary>
        /// Get the description of this item
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetItem().GetDescription(this);
        }

        /// <summary>
        /// Get the acutal title to be displayed (rich text)
        /// </summary>
        /// <returns></returns>
        public string GetDisplayTitle()
        {
            string changedName = infoTree.ReadString("title");
            if (changedName != null) return changedName;
            else return GetItem().itemName;
        }

        /// <summary>
        /// Get the subtitle to be displayed under title (rich text)
        /// </summary>
        /// <returns></returns>
        public string GetDisplaySubtitle()
        {
            string subtitle = infoTree.ReadString("subtitle");
            return subtitle != null ? subtitle : "";
        }

        /// <summary>
        /// Get the ids and powers of all attached abilities
        /// </summary>
        /// <returns>a list of key value pair representing the id and power of the ability</returns>
        public List<KeyValuePair<string, int>> GetAbilityIdPowers()
        {
            ItemInfoTree abilityTree = infoTree.ReadTree("abilities");
            if (abilityTree == null) return new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> abilities = abilityTree.dictionary
                .Where(kvp => kvp.Value is int)
                .Select(kvp=>new KeyValuePair<string, int>(kvp.Key, (int)kvp.Value)).ToList();
            return abilities;
        }


    /// <summary>
    /// Called by the controller to notify the item stack that it has been used for a certain amount of time
    /// </summary>
    /// <param name="user">user of thte item</param>
    /// <param name="deltaTime">the user time from last update</param>
    public void Use(EntityLiving user, float deltaTime)
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
        
        /// <summary>
        /// Called by controller to notify the item stack that it has been hold for a certain amount of time
        /// </summary>
        /// <param name="user">holder of the item</param>
        /// <param name="deltaTime">the hold time of from last update</param>
        public void Hold(EntityLiving user, float deltaTime)
        {
            this.useTime = 0;
        }
    }
}
