using UnityEngine;
using System.Collections;
using UniInventory.Registry;
using UniInventory.Entity;

namespace UniInventory.Items
{
    public class ItemBall : Item
    {
        public ItemBall(int id) : base(id)
        {
            maxStackSize = 128;
            itemName = "little ball";
            maxUseTime = 0.3f;
        }

        public override void OnCreate(ItemStack stack)
        {
            stack.infoTree.WriteString("description", "I am a ball you can throw.");
        }

        public override Texture2D GetIcon(ItemStack stack)
        {
            return Textures.BallTexture;
        }

        public override void OnUse(ItemStack stack, EntityLiving user)
        {
            Debug.Log("Item used");
            stack.stackSize -= 1;
        }

        public override void OnUsing(ItemStack stack, EntityLiving user, float DeltaTime, float TotalTime)
        {
            Debug.Log("Using time: " + TotalTime);
        }
    }
}