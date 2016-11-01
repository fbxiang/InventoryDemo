using UnityEngine;
using UniInventory.Inventory;
using UniInventory.Items;
using System.Collections.Generic;

namespace UniInventory.Container
{
    public abstract class ContainerBase : MonoBehaviour, IInventory
    {
        public abstract List<ItemStack> GetItemStacks();
        public abstract ItemStack AddItemStack(ItemStack stack);

    }
}
