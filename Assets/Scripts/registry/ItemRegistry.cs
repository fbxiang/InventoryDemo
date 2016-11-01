using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniInventory.Items;
using System;
using System.Reflection;
using System.Linq;

namespace UniInventory.Registry {

    public class ItemRegistry // : Singleton<ItemRegistry>
    {
        private static ItemRegistry instance = new ItemRegistry();
        public Dictionary<int, Item> items = new Dictionary<int, Item>();


        // item registry starts here
        public static Item ItemDebug = CreateAndRegisterItem<ItemDebug>(0);
        public static Item ItemRadioactive = CreateAndRegisterItem<ItemRadioactive>(1);

        static Item CreateAndRegisterItem<T>(int id) where T : Item
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(int) });
            Item newItem = (Item)ctor.Invoke(new object[] { id });
            Instance.RegisterItem(newItem);
            return newItem;
        }

        protected ItemRegistry() { }

        public void RegisterItem(Item item)
        {
            if (items.ContainsKey(item.id)) {
                Debug.LogError("[ItemRegistry] duplicate id");
                return;
            }
            items[item.id] = item;
        }

        public Item GetItem(int id)
        {
            if (id < 0 || id >= items.Count)
            {
                Debug.LogError("[ItemRegistry] Trying to get item with invalid Id: " + id);
                return null;
            }
            return items[id];
        }

        public static ItemRegistry Instance
        {
            get { return instance; }
        }
    }
}
