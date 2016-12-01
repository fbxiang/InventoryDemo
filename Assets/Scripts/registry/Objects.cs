using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniInventory.Registry
{
    /// <summary>
    /// This class is used to load all sound resources
    /// </summary>
    class Objects
    {
        public static GameObject Ball = LoadGameObject("ball");
        public static GameObject BallStatic = LoadGameObject("ball_static");

        public static GameObject LoadGameObject(string filename)
        {
            return Resources.Load<GameObject>("prefabs/" + filename);
        }
    }
}
