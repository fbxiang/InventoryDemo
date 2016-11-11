using UnityEngine;
using System.Collections;
using UniInventory.Registry;

namespace UniInventory.Items
{
    public class ItemDebug : Item
    {
        public ItemDebug(int id) : base(id) { }

        public override void OnCreate(ItemStack stack)
        {
            stack.infoTree.WriteString("description", "This is only for debugging.");
        }
    }
}