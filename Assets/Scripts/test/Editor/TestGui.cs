using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniInventory.Container;
using UniInventory.Items;
using UniInventory.Registry;

namespace UniInventory.Testing
{

    class TestContainerSlots
    {
        [Test]
        public void TestAddGetItem()
        {
            ContainerSlots container = new ContainerSlots(5);
            Assert.AreEqual(5, container.Capacity);

            Assert.IsNull(container.GetItemStackAt(-1)); // invalid do not crash
            Assert.IsNull(container.GetItemStackAt(0)); // emtpy statck

            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10));
            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10));
            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10));
            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10));
            Assert.AreEqual(40, container.GetItemStackAt(0).stackSize);

            container.AddItemStack(new ItemStack(ItemRegistry.ItemDebug, 1));
            container.AddItemStack(new ItemStack(ItemRegistry.ItemDebug, 1));
            container.AddItemStack(new ItemStack(ItemRegistry.ItemDebug, 1));

            Assert.AreEqual(ItemRegistry.ItemDebug.id, container.GetItemStackAt(1).itemId);
            Assert.AreEqual(ItemRegistry.ItemDebug.id, container.GetItemStackAt(2).itemId);
            Assert.AreEqual(ItemRegistry.ItemDebug.id, container.GetItemStackAt(3).itemId);
            Assert.IsNull(container.GetItemStackAt(4));

            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 60));
            Assert.AreEqual(1, container.GetItemStackAt(4).stackSize);

            Assert.AreEqual(1, container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 99)).stackSize);
        }

        [Test]
        public void TestUpdate()
        {
            ContainerSlots container = new ContainerSlots(5);
            Assert.AreEqual(5, container.Capacity);

            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10));

            container.UpdateWith(5);

            container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10));

            Assert.IsNotNull(container.GetItemStackAt(1)); // the items differ

            container.UpdateWith(26);
            Assert.IsNull(container.GetItemStackAt(0));
            Assert.IsNotNull(container.GetItemStackAt(1)); // decay

            container.UpdateWith(8);
            Assert.IsNull(container.GetItemStackAt(1));
        }
    }
}
