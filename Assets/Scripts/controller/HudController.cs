using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniInventory.Entity;
using UniInventory.Container;

namespace UniInventory.Gui
{

    /// <summary>
    /// This class controls the hud display of player info
    /// </summary>
    public class HudController : MonoBehaviour
    {
        public Text crossHair;

        EntityPlayer player;

        /// <summary>
        /// Initialize the variables
        /// </summary>
        void Start()
        {
            player = GetComponent<EntityPlayer>();
        }

        /// <summary>
        /// Draw hud components such as the cross hair
        /// </summary>
        void OnGUI()
        {
            if (player.LookObject != null && player.LookObject.GetComponent<ContainerSlots>() != null)
                setCrossHairActive(true);
            else
                setCrossHairActive(false);
        }

        /// <summary>
        /// change the active state of cross hair. Using different colors to represent state
        /// </summary>
        /// <param name="active"></param>
        void setCrossHairActive(bool active)
        {
            crossHair.color = active ? Color.green : Color.white;
        }
    }
}