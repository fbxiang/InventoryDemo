using UnityEngine;
using System.Collections;
using UniInventory.Registry;
using UniInventory.Entity;
using gameplay;

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

            GameObject ball = Object.Instantiate(Objects.BallStatic);
            ball.transform.position = user.Position;
            ball.GetComponent<Rigidbody>().velocity = user.LookVector;
            ball.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV(0, 1, 0.1f, 0.4f, 0.9f, 1f));
            ball.SetActive(true);
        }

        public override void OnUsing(ItemStack stack, EntityLiving user, float DeltaTime, float TotalTime)
        {
            Debug.Log("Using time: " + TotalTime);
        }
    }
}