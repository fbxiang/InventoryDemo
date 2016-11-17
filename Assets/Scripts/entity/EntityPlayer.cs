﻿using UnityEngine;
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


    [RequireComponent(typeof(PlayerGuiController), typeof(HudController))]
    public class EntityPlayer : EntityLiving
    {
        public Camera cam;  // main camera
        PlayerGuiController guiController;  // the gui controller

        public GameObject LookObject { get; private set; }  // the object the player is looking at

        public KeyBinding Key = new KeyBinding();  // key binding of current player

        /// <summary>
        /// Initialize variables and key bindings
        /// </summary>
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


        /// <summary>
        /// Update the object the player is looking at
        /// </summary>
        void Update()
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out hit, 20))
            {
                LookObject = hit.transform.gameObject;
            }
            else
            {
                LookObject = null;
            }
        }
    }
}

