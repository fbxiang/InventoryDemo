using UnityEngine;
using System.Collections.Generic;


namespace UniInventory.Registry
{
    /// <summary>
    /// This class defines any ability to be used on an item stack
    /// </summary>
    public class Ability
    {
        public string abilityId;
        public string displayColor;
        public string description;
        public int priority;

        public Ability(string id, string color, int priority = 0, string description = "")
        {
            this.abilityId = id; this.displayColor = color; this.description = description; this.priority = priority;
        }

        /// <summary>
        /// Get the text to be displayed in the tooltip
        /// </summary>
        /// <returns></returns>
        public string GetDisplayText()
        {
            return abilityId;
        }
    }

    /// <summary>
    /// This class stores all the "abilities"
    /// </summary>
    public class AbilityRegistry
    {
        public static Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
        public static Ability Attack = RegisterAbility(new Ability("attack", "orange"));
        public static Ability Defence = RegisterAbility(new Ability("defence", "orange"));

        /// <summary>
        /// Register the ability and return it. It will replace the any ability with the same name
        /// </summary>
        /// <param name="ability">the ability to register</param>
        /// <returns>the ability passed in</returns>
        public static Ability RegisterAbility(Ability ability)
        {
            if (abilities.ContainsKey(ability.abilityId))
            {
                Debug.LogWarning("Replacing current ability with the same id: " + ability.abilityId);
            }
            abilities[ability.abilityId] = ability;
            return ability;
        }

        /// <summary>
        /// Get the text to display for the ability defined in ability class
        /// </summary>
        /// <param name="abilityId">id string of the ability</param>
        /// <returns></returns>
        public static string GetAbilityDisplayText(string abilityId)
        {
            return abilities.ContainsKey(abilityId) ? abilities[abilityId].GetDisplayText() : abilityId + "(Unknown Ability)";
        }

        /// <summary>
        /// Get the display color for the ability defined in ability class
        /// </summary>
        /// <param name="abilityId">id string of the ability</param>
        /// <returns></returns>
        public static string GetAbilityDisplayColor(string abilityId)
        {
            return abilities.ContainsKey(abilityId) ? abilities[abilityId].displayColor : "#00cccccc";
        }

        /// <summary>
        /// Get the priority for the ability to be displayed
        /// </summary>
        /// <param name="abilityId">id string of the ability</param>
        /// <returns></returns>
        public static int GetAbilityDisplayPriority(string abilityId)
        {
            return abilities.ContainsKey(abilityId) ? abilities[abilityId].priority : -1;
        }
    }
}