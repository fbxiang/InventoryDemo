using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

using UniInventory.Items;
using UniInventory.Registry;
using UniInventory.Container;

namespace UniInventory.Testing
{
    public class TestItemInfoTree
    {
        [Test]
        public void TestReadWrite()
        {
            ItemInfoTree tree = new ItemInfoTree();

            tree.WriteString("key", "value");
            Assert.AreEqual(tree.ReadString("key"), "value");

            tree.WriteInt("key", 5);
            Assert.AreEqual(tree.ReadInt("key"), 5);

            tree.WriteDouble("key", 1.0);
            Assert.AreEqual(tree.ReadDouble("key"), 1.0);

            tree.WriteIntArray("key", new int[] { 1, 2, 3 });
            Assert.AreEqual(tree.ReadIntArray("key"), new int[] { 1, 2, 3 });

            Assert.IsNull(tree.ReadString("not key")); // not really a key!
            Assert.IsNull(tree.ReadString("key")); // wrong type!
        }

        [Test]
        public void TestReadWriteTree()
        {
            ItemInfoTree tree = new ItemInfoTree();
            tree.WriteInt("INT", 1);
            ItemInfoTree subtree = new ItemInfoTree();
            subtree.WriteInt("INT", 2);

            ItemInfoTree subtreecopy = new ItemInfoTree();
            subtreecopy.WriteInt("INT", 2);

            tree.WriteTree("TREE", subtree);

            Assert.AreEqual(tree.ReadTree("TREE"), subtreecopy);
        }

        [Test]
        public void TestTreeEqual()
        {
            // tree 1
            ItemInfoTree tree = new ItemInfoTree();
            ItemInfoTree subtree = new ItemInfoTree();
            subtree.WriteDouble("DOUBLE", 2.0);

            tree.WriteInt("INT", 1);
            tree.WriteDouble("DOUBLE", 1.0);
            tree.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            tree.WriteString("STRING", "XX");
            tree.WriteTree("TREE", subtree);

            // tree 2
            ItemInfoTree tree2 = new ItemInfoTree();
            ItemInfoTree subtree2 = new ItemInfoTree();
            subtree2.WriteDouble("DOUBLE", 2.0);

            tree2.WriteInt("INT", 1);
            tree2.WriteDouble("DOUBLE", 1.0);
            tree2.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            tree2.WriteString("STRING", "XX");
            tree2.WriteTree("TREE", subtree2);

            Assert.IsTrue(tree.Equals(tree2));

            tree2.WriteIntArray("INTARRAY", new int[] { 1, 2, 2 });
            Assert.IsFalse(tree.Equals(tree2));

            tree2.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            Assert.IsTrue(tree.Equals(tree2));

            subtree2.WriteInt("stuff", 2);
            Assert.IsFalse(tree.Equals(tree2));
        }

        [Test]
        public void TestClone()
        {
            ItemInfoTree tree = new ItemInfoTree();
            ItemInfoTree subtree = new ItemInfoTree();
            subtree.WriteDouble("DOUBLE", 2.0);

            tree.WriteInt("INT", 1);
            tree.WriteDouble("DOUBLE", 1.0);
            tree.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            tree.WriteString("STRING", "XX");
            tree.WriteTree("TREE", subtree);

            ItemInfoTree tree2 = new ItemInfoTree(tree);

            Assert.IsTrue(tree.Equals(tree2));
            Assert.AreNotSame(tree.ReadIntArray("INTARRAY"), tree2.ReadIntArray("INTARRAY"));
            Assert.AreNotSame(tree.ReadTree("TREE"), tree2.ReadTree("TREE"));
        }
    }

    public class TestItemRegistry
    {
        [Test]
        public void TestRegisteredItems()
        {
            Assert.IsTrue(ItemRegistry.Instance.items.Count == 3);
            Assert.IsNull(ItemRegistry.Instance.GetItem(-1)); // test do not crash
            Assert.IsTrue(ItemRegistry.Instance.GetItem(0) is Item);
        }
    }

    public class TestItemStack
    {
        [Test]
        public void TestConstructItemStack()
        {
            ItemStack stack = new ItemStack(ItemRegistry.ItemDebug, 1);
            Assert.AreEqual(1, stack.stackSize);
            Assert.AreEqual(ItemRegistry.ItemDebug.id, stack.itemId);
        }

        [Test]
        public void TestUpdate()
        {
            ItemStack stack = new ItemStack(ItemRegistry.ItemRadioactive, 20); // create a radioactive object
            double life1 = stack.infoTree.ReadDouble("life");
            stack.Update(0.5f);
            double life2 = stack.infoTree.ReadDouble("life");
            Assert.IsTrue(life1 > life2);
        }

        [Test]
        public void TestDestroy()
        {
            ItemStack stack = new ItemStack(ItemRegistry.ItemRadioactive, 20); // create a radioactive object
            stack.Destroy();
            Assert.AreEqual(0, stack.stackSize);
        }

        [Test]
        public void TestMergeable()
        {
            ItemInfoTree tree = new ItemInfoTree();
            ItemInfoTree subtree = new ItemInfoTree();
            subtree.WriteDouble("DOUBLE", 2.0);

            tree.WriteInt("INT", 1);
            tree.WriteDouble("DOUBLE", 1.0);
            tree.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            tree.WriteString("STRING", "XX");
            tree.WriteTree("TREE", subtree);

            // tree 2
            ItemInfoTree tree2 = new ItemInfoTree();
            ItemInfoTree subtree2 = new ItemInfoTree();
            subtree2.WriteDouble("DOUBLE", 2.0);

            tree2.WriteInt("INT", 1);
            tree2.WriteDouble("DOUBLE", 1.0);
            tree2.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            tree2.WriteString("STRING", "XX");
            tree2.WriteTree("TREE", subtree2);

            ItemStack stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 13);
            ItemStack stack2 = new ItemStack(ItemRegistry.ItemRadioactive, 1);
            ItemStack stack3 = new ItemStack(ItemRegistry.ItemRadioactive, 1);
            ItemStack stack4 = new ItemStack(ItemRegistry.ItemDebug, 1);
            stack3.Update(0.3f);
            Assert.IsTrue(stack1.mergeable(stack2));
            Assert.IsTrue(stack2.mergeable(stack1));
            Assert.IsFalse(stack1.mergeable(stack3));
            Assert.IsFalse(stack4.mergeable(stack1));
        }

        public void TestTake()
        {
            ItemStack stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 98);
            ItemStack stack2 = new ItemStack(ItemRegistry.ItemRadioactive, 1);
            Assert.IsNull(stack1.takeNumberReturnRemain(stack2, 1));
            Assert.AreEqual(99, stack1.stackSize);

            stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 50);
            stack2 = new ItemStack(ItemRegistry.ItemRadioactive, 52);

            Assert.AreEqual(3, stack1.takeNumberReturnRemain(stack2, 53).stackSize);
            Assert.AreEqual(99, stack1.stackSize);

            stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 50);
            stack2 = new ItemStack(ItemRegistry.ItemRadioactive, 50);

            Assert.AreEqual(3, stack1.takeNumberReturnRemain(stack2, 50).stackSize);
            Assert.AreEqual(99, stack1.stackSize);
        }

        [Test]
        public void TestMerge()
        {
            ItemStack stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 98);
            ItemStack stack2 = new ItemStack(ItemRegistry.ItemRadioactive, 1);
            Assert.IsNull(stack1.mergeWith(stack2));
            Assert.AreEqual(99, stack1.stackSize);

            stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 50);
            stack2 = new ItemStack(ItemRegistry.ItemRadioactive, 50);

            Assert.AreEqual(1, stack1.mergeWith(stack2).stackSize);
            Assert.AreEqual(99, stack1.stackSize);
        }

        [Test]
        public void TestSplit()
        {
            ItemStack stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 13);
            ItemStack stack2 = stack1.Split(14);
            Assert.AreEqual(0, stack1.stackSize);
            Assert.AreEqual(13, stack2.stackSize);

            stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 13);
            stack2 = stack1.Split(0);
            Assert.IsNull(stack2);

            stack1 = new ItemStack(ItemRegistry.ItemRadioactive, 13);
            ItemInfoTree tree = stack1.infoTree;
            stack2 = stack1.Split(5);

            Assert.AreEqual(tree, stack1.infoTree);
            Assert.AreEqual(8, stack1.stackSize);
            Assert.AreEqual(5, stack2.stackSize);
            Assert.IsTrue(stack1.mergeable(stack2));

            stack2.Update(0.4f);
            Assert.IsFalse(stack1.mergeable(stack2));
        }

    }

    public class TestContainerLinear
    {
        [Test]
        public void TestAddGetRemoveStack()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<ContainerLinear>();
            ContainerLinear container = obj.GetComponent<ContainerLinear>();
            container.Capacity = 2;
            Assert.AreEqual(0, container.Count);
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10)));
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 10)));
            Assert.AreEqual(20, container.GetItemStackAt(0).stackSize);
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemDebug, 1)));
            Assert.AreEqual(ItemRegistry.ItemDebug.id, container.GetItemStackAt(1).itemId);
            Assert.AreEqual(2, container.Count);
            Assert.AreEqual(1, container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 80)).stackSize);
            Assert.AreEqual(20, container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 20)).stackSize);

            Assert.AreEqual(99, container.RemoveItemStackAt(0).stackSize);
            Assert.AreEqual(1, container.Count);
        }

        [Test]
        public void TestUpdate()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<ContainerLinear>();
            ContainerLinear container = obj.GetComponent<ContainerLinear>();

            container.Capacity = 5;
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 99)));
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 99)));
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 99)));
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 99)));
            Assert.IsNull(container.AddItemStack(new ItemStack(ItemRegistry.ItemRadioactive, 30)));

            container.UpdateWith(10f);
            Assert.AreEqual(20.0, container.GetItemStackAt(0).infoTree.ReadDouble("life")); // life decreased
            container.UpdateWith(30f);
            Assert.AreEqual(0, container.Count); // properly removed

        }


    }

}
