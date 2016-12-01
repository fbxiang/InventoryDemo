using UnityEngine;
using System.Collections;
using UniInventory.Registry;
using UniInventory.Entity;

namespace UniInventory.Items {
    [System.Serializable]
    public class Item
    {
        public string itemName = "item"; // (unlocalized) name of the item
        public int maxStackSize = 1; // max stack size of the item
        public int id; // globally unique id of the item
        public float maxUseTime = 1;

        public virtual float GetMaxUseTime(ItemStack stack)
        {
            return maxUseTime;
        }


        public virtual void SetMaxUseTime(float time)
        {
            maxUseTime = time;
        }

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
        public virtual void OnUse(ItemStack stack, EntityLiving user) { }

        /// <summary>
        /// Called when a stack of this item is being used
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        /// <param name="user">item user</param>
        public virtual void OnUsing(ItemStack stack, EntityLiving user, float DeltaTime, float TotalTime) { }

        /// <summary>
        /// Called whenever a stack of this item is updated
        /// </summary>
        /// <param name="stack">the stack of the item</param>
        /// <param name="deltaTime">time elapsed from last update</param>
        public virtual void OnUpdate(ItemStack stack, float deltaTime) { }

        /// <summary>
        /// Get the sprite image of the item
        /// </summary>
        /// <param name="stack">the specific item stack</param>
        /// <returns>the sprite</returns>
        public virtual Texture2D GetIcon(ItemStack stack) { return Textures.DefaultTexture; }

        /// <summary>
        /// Get the sound of this object when picked up or placed down
        /// </summary>
        /// <param name="stack">the item stack</param>
        /// <returns>the sound</returns>
        public virtual AudioClip GetClickSound(ItemStack stack)
        {
            return SoundEffects.DefaultSound;
        }

        /// <summary>
        /// Get the description displayed on the tool tip
        /// </summary>
        /// <param name="stack">the item stack containing this item</param>
        /// <returns>the string description (rich text)</returns>
        public virtual string GetDescription(ItemStack stack)
        {
            string description = stack.infoTree.ReadString("description");
            if (description == null) return "";
            return description;
        }
    }
}


