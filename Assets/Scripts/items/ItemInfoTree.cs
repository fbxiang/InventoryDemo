using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Text;

namespace UniInventory.Items
{
    [System.Serializable]
    public struct IntBranch
    {
        public string key;
        public int value;
    }
    [System.Serializable]
    public struct StringBranch
    {
        public string key;
        [TextArea(1, 10)]
        public string value;
    }
    [System.Serializable]
    public struct DoubleBranch
    {
        public string key;
        public double value;
    }
    [System.Serializable]
    public struct IntArrayBranch
    {
        public string key;
        public List<int> value;
    }
    [System.Serializable]
    public struct TreeBranch
    {
        public string key;
        public InfoTreeInitializationObject value;
    }

    [System.Serializable]
    public class InfoTreeInitializationObject
    {
        // All these structures are for communication with the inspector
        public List<IntBranch> IntValues = new List<IntBranch>();
        public List<StringBranch> StringValues = new List<StringBranch>();
        public List<DoubleBranch> DoubleValues = new List<DoubleBranch>();
        public List<IntArrayBranch> IntArrayValues = new List<IntArrayBranch>();
        public List<TreeBranch> TreeValues = new List<TreeBranch>();

        /// <summary>
        /// Create the info tree from this struct
        /// </summary>
        /// <returns>the info tree</returns>
        public ItemInfoTree ToInfoTree()
        {
            ItemInfoTree tree = new ItemInfoTree();
            foreach (var kvpair in IntValues)
            {
                tree.WriteInt(kvpair.key, kvpair.value);
            }
            foreach (var kvpair in StringValues)
            {
                tree.WriteString(kvpair.key, kvpair.value);
            }
            foreach (var kvpair in DoubleValues)
            {
                tree.WriteDouble(kvpair.key, kvpair.value);
            }
            foreach (var kvpair in IntArrayValues)
            {
                tree.WriteIntArray(kvpair.key, kvpair.value.ToArray());
            }
            foreach (var kvpair in TreeValues)
            {
                tree.WriteTree(kvpair.key, kvpair.value.ToInfoTree());
            }
            return tree;
        }
    }

    /// <summary>
    /// Tree structure used to store item information
    /// </summary>
    [System.Serializable]
    public class ItemInfoTree : ICloneable, IXmlSerializable
    {
        public Dictionary<string, object> dictionary = new Dictionary<string, object>(); // dictionary used to store all 

        public ItemInfoTree() { }
        public object Clone()
        {
            return new ItemInfoTree(this);
        }
        public ItemInfoTree(ItemInfoTree other)
        {
            foreach (string key in other.dictionary.Keys)
            {
                object value = other.dictionary[key];
                if (value is ICloneable)
                {
                    dictionary[key] = ((ICloneable)value).Clone();
                }
                else
                {
                    dictionary[key] = value;
                }
            }
        }

        public void WriteString(string key, string value)
        {
            dictionary[key] = value;
        }

        public string ReadString(string key)
        {
            try
            {
                return (string)dictionary[key];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void WriteInt(string key, int value)
        {
            dictionary[key] = value;
        }

        public int ReadInt(string key)
        {
            try
            {
                return (int)dictionary[key];
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public void WriteDouble(string key, double value)
        {
            dictionary[key] = value;
        }

        public double ReadDouble(string key)
        {
            try
            {
                return (double)dictionary[key];
            }
            catch (Exception e)
            {
                return 0.0;
            }
        }

        public void WriteIntArray(string key, int[] value)
        {
            dictionary[key] = value;
        }

        public int[] ReadIntArray(string key)
        {
            try
            {
                return (int[])dictionary[key];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void WriteTree(string key, ItemInfoTree tree)
        {
            dictionary[key] = tree;
        }

        public ItemInfoTree ReadTree(string key)
        {
            try
            {
                return (ItemInfoTree)dictionary[key];
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Update the current tree with some other info tree
        /// </summary>
        /// <param name="other"></param>
        public void UpdateWith(ItemInfoTree other)
        {
            other.dictionary.ToList().ForEach(x => this.dictionary[x.Key] = x.Value);
        }


        /// <summary>
        /// Two trees are equal if they have exactly the same elements. (Arrays are compared with sequence euqal)
        /// </summary>
        /// <param name="obj">other object(tree)</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ItemInfoTree))
                return false;
            ItemInfoTree otherTree = (ItemInfoTree)obj;
            if (this.dictionary.Count != otherTree.dictionary.Count)
                return false;
            foreach (string key in this.dictionary.Keys)
            {
                if (!otherTree.dictionary.ContainsKey(key))
                    return false;
                object value1 = otherTree.dictionary[key];
                object value2 = this.dictionary[key];

                if (value1 is int[] && value2 is int[] && !Enumerable.SequenceEqual((int[])value1, (int[])value2))
                {
                    return false;
                }
                else if (!(value1 is int[]) && !value1.Equals(value2))
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Although this is implemented, info trees are not designed to be user as keys. Please do not use.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (string key in this.dictionary.Keys)
            {
                hash += key.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// the required function for serialization
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            throw null;
        }

        /// <summary>
        /// read tree properties from xml file
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            Console.WriteLine("start read");
            reader.MoveToContent();
            reader.ReadStartElement("infoTree");
            while (reader.IsStartElement())
            {
                Console.WriteLine("type: " + reader.GetAttribute("type") + " key: " + reader.GetAttribute("key"));
                string key = reader.GetAttribute("key");
                string type = reader.GetAttribute("type");
                reader.ReadStartElement();

                switch (type)
                {
                    case "int":
                        int intVal = reader.ReadContentAsInt();
                        WriteInt(key, intVal);
                        Console.WriteLine(intVal);
                        break;
                    case "text":
                        string value = reader.ReadContentAsString();
                        WriteString(key, value);
                        Console.WriteLine(value);
                        break;
                    case "double":
                        double doubleVal = reader.ReadContentAsDouble();
                        WriteDouble(key, doubleVal);
                        Console.WriteLine(doubleVal);
                        break;
                    case "intArray":
                        List<int> intList = new List<int>();
                        while (reader.IsStartElement())
                        {
                            reader.ReadStartElement("value");
                            int elem = reader.ReadContentAsInt();
                            Console.WriteLine(elem);
                            intList.Add(elem);
                            reader.ReadEndElement();
                        }
                        WriteIntArray(key, intList.ToArray());
                        break;
                    case "tree":
                        ItemInfoTree subTree = new ItemInfoTree();
                        subTree.ReadXml(reader);
                        WriteTree(key, subTree);
                        break;
                }
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Write all tree properties to xml file
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("infoTree");

            foreach (var kvpair in dictionary)
            {
                string typeName = getTypeString(kvpair.Value);
                writer.WriteStartElement("item");
                writer.WriteAttributeString("type", typeName);
                writer.WriteAttributeString("key", kvpair.Key);

                switch (typeName)
                {
                    case "intArray":
                        foreach (int x in (int[])kvpair.Value)
                        {
                            writer.WriteStartElement("value");
                            writer.WriteString(x.ToString());
                            writer.WriteEndElement();
                        }
                        break;
                    case "tree":
                        StringBuilder builder = new StringBuilder();
                        XmlWriter subTreeWriter = XmlWriter.Create(builder);
                        ((ItemInfoTree)kvpair.Value).WriteXml(writer);
                        break;
                    default:
                        writer.WriteString(kvpair.Value.ToString());
                        break;
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Helper function to get the string represented value type given the value object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static String getTypeString(object value)
        {
            if (value.GetType().Equals(typeof(int)))
            {
                return "int";
            }
            else if (value.GetType().Equals(typeof(string)))
            {
                return "text";
            }
            else if (value.GetType().Equals(typeof(double)))
            {
                return "double";
            }
            else if (value.GetType().Equals(typeof(int[])))
            {
                return "intArray";
            }
            return "tree";
        }
    }
}
