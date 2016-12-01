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
    class SoundEffects
    {
        public static AudioClip DefaultSound = LoadItemSound("default");

        public static AudioClip LoadItemSound(string filename)
        {
            return Resources.Load<AudioClip>("audio/items/" + filename);
        }

    }
}
