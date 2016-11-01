using UnityEngine;
using System.Collections;
using UniInventory.Registry;

namespace UniInventory.Items {
    [System.Serializable]
    public class Item
    {
        public string itemName = "item"; // (unlocalized) name of the item
        public int maxStackSize = 1; // max stack size of the item
        public int id; // globally unique id of the item

        public Item(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Called when a stack of this item is created
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        public virtual void OnCreate(ItemStack stack) { }

        /// <summary>
        /// Called when a stack of this item is destroyed
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        public virtual void OnDestroy(ItemStack stack) { }

        /// <summary>
        /// Called when a stack of this item is used by a itme user
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        /// <param name="user">item user</param>
        public virtual void OnUse(ItemStack stack, IItemUser user) { }

        /// <summary>
        /// Called when a stack of this item is being used
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        /// <param name="user">item user</param>
        public virtual void OnUsing(ItemStack stack, IItemUser user) { }

        /// <summary>
        /// Called whenever a stack of this item is updated
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        /// <param name="deltaTime">time elapsed from last update</param>
        public virtual void OnUpdate(ItemStack stack, float deltaTime) { }   
    }
}


