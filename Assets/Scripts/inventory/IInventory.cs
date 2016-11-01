using UnityEngine;
using System.Collections.Generic;
using UniInventory.Items;

namespace UniInventory.Inventory
{
    public interface IInventory
    {

        /// <summary>
        /// Get all item stacks stored
        /// </summary>
        /// <returns>a list of item stacks Stored in this inventory</returns>
        List<ItemStack> GetItemStacks();


        /// <summary>
        /// Try to add a item stack to the inventory
        /// </summary>
        /// <param name="stack">the stack to add</param>
        /// <returns>null if all are added, otherwise the remaining item stack</returns>
        ItemStack AddItemStack(ItemStack stack);
    }
}
