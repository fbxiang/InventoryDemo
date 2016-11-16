using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniInventory.Registry
{
    class Textures
    {
        public static Texture2D DefaultTexture = LoadItemTexture("default");
        public static Texture2D RadioTexture = LoadItemTexture("radioactive");
        public static Texture2D BallTexture = LoadItemTexture("ball");

        public static Texture2D LoadItemTexture(string filename)
        {
            return Resources.Load<Texture2D>("textures/items/"+filename);
        }
    }
}
