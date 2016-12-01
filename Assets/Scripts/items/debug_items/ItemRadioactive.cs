using UnityEngine;
using System.Collections;
using UniInventory.Registry;

using System.Linq;

namespace UniInventory.Items
{
    public class ItemRadioactive : Item
    {
        public ItemRadioactive(int id) : base(id)
        {
            this.maxStackSize = 99;
            this.itemName = "radioactive_substance";
        }

        public override void OnCreate(ItemStack stack)
        {
            stack.infoTree.WriteDouble("life", 30.0);
        }

        public override void OnUpdate(ItemStack stack, float deltaTime)
        {
            double newLife = stack.infoTree.ReadDouble("life") - deltaTime;
            if (newLife < 0)
            {
                stack.stackSize = 0; // mark for destroy
            }
            else
            {
                stack.infoTree.WriteDouble("life", newLife);
            }
        }

        public override Texture2D GetIcon(ItemStack stack)
        {
            Texture2D[] textures = Textures.radioTextureGroup;

            float rateOfChange = 0.1f;
            int currentFrame = (int) (Time.time / rateOfChange);
            int idx = currentFrame % textures.Length;

            return textures[idx];
        }

        public override string GetDescription(ItemStack stack)
        {
            double life = stack.infoTree.ReadDouble("life");
            return "Life remaining: " + life.ToString("0.00");
        }
    }
}