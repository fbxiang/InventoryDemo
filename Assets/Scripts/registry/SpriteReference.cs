using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniInventory.Registry
{
    [System.Serializable]
    public struct SpriteInfo
    {
        public string Key;
        public Sprite Value;
    }

    /// <summary>
    /// Initialize sprite reference at awake
    /// </summary>
    public class SpriteReference : MonoBehaviour
    {
        public List<SpriteInfo> SpritesInfo; // communicate with the inspector

        private static Dictionary<string, Sprite> database = null; // store all sprites

        /// <summary>
        /// Initialize the database
        /// </summary>
        void Awake()
        {
            if (database == null)
                database = new Dictionary<string, Sprite>();
            foreach (SpriteInfo info in SpritesInfo)
            {
                database[info.Key] = info.Value;
            }
        }

        /// <summary>
        /// Get stored sprite with key
        /// </summary>
        /// <param name="key">the name of the sprite</param>
        /// <returns>the sprite</returns>
        public static Sprite GetSprite(string key)
        {
            if (database.ContainsKey(key))
                return database[key];
            Debug.LogWarning("[SpriteReference] Trying to get non-existing sprite.");
            return database["default"];
        }

    }

}