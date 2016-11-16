using UnityEngine;
using System.Collections.Generic;
using UniInventory.Gui;
using UniInventory.Container;
namespace UniInventory.Entity
{
    public class KeyBinding
    {
        Dictionary<string, string> KeyRegistry = new Dictionary<string, string>();

        public void Register(string keyName, string keyValue)
        {
            KeyRegistry[keyName] = keyValue;
        }

        public bool Down(string key)
        {
            if (!KeyRegistry.ContainsKey(key)) return false;
            return Input.GetKeyDown(KeyRegistry[key]);
        }
        public bool Up(string key)
        {
            if (!KeyRegistry.ContainsKey(key)) return false;
            return Input.GetKeyUp(KeyRegistry[key]);
        }

        public bool Pressed(string key)
        {
            if (!KeyRegistry.ContainsKey(key)) return false;
            return Input.GetKey(KeyRegistry[key]);
        }
    }


    [RequireComponent(typeof(PlayerGuiController))]
    public class EntityPlayer : EntityLiving
    {
        public Camera cam;
        public PlayerGuiController guiController;

        public KeyBinding Key = new KeyBinding();
        void Awake()
        {
            guiController = GetComponent<PlayerGuiController>();

            Key.Register("inventory", "e");
            for (int i = 0; i < 10; i++)
            {
                Key.Register(i.ToString(), i.ToString());
            }
            Key.Register("use", "r");
        }


        void OnGUI()
        {
        }

        void Update()
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out hit, 20) && hit.transform.GetComponent<ContainerSlots>() != null)
            {
                ContainerSlots container = hit.transform.GetComponent<ContainerSlots>();
                GuiInventory gui = hit.transform.GetComponent<GuiInventory>();
                guiController.setOther(gui, container);
            }
        }
    }
}

