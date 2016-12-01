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

        public static Texture2D[] radioTextureGroup = LoadItemTextureGroup("radioactive", 4);

        public static Texture2D LoadItemTexture(string filename)
        {
            return Resources.Load<Texture2D>("textures/items/"+filename);
        }

        public static Texture2D[] LoadItemTextureGroup(string filename, int n)
        {
            Texture2D[] textures = new Texture2D[n];
            for (int i = 1; i <= n; i++)
            {
                textures[i - 1] = Resources.Load<Texture2D>("textures/items/" + filename + "_" + i.ToString("D2"));
            }
            return textures;
        }
    }
}
