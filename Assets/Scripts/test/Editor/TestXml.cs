using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

using UniInventory.Items;
using UniInventory.Registry;
using UniInventory.Container;
using System.Xml;
using System.Text;
using System;

namespace UniInventory.Testing
{
    public class TestXml
    {
        [Test]
        public void TestBasic()
        {
            ItemInfoTree tree = new ItemInfoTree();
            ItemInfoTree subtree = new ItemInfoTree();
            subtree.WriteDouble("DOUBLE", 2.0);

            tree.WriteInt("INT", 1);
            tree.WriteDouble("DOUBLE", 1.0);
            tree.WriteIntArray("INTARRAY", new int[] { 1, 2, 3 });
            tree.WriteString("STRING", "XX");
            tree.WriteTree("TREE", subtree);

            StringBuilder builder = new StringBuilder();

            XmlWriter writer = XmlWriter.Create(builder);

            tree.WriteXml(writer);

            System.IO.StringReader stream = new System.IO.StringReader(builder.ToString());
            XmlReader reader = XmlReader.Create(stream);

            ItemInfoTree tree2 = new ItemInfoTree();
            tree2.ReadXml(reader);

            Assert.AreEqual(tree, tree2);
        }

    }
}
