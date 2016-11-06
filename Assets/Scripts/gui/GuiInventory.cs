using UnityEngine;
using System.Collections;
using UniInventory.Container;
using UniInventory.Items;
using UnityEngine.UI;

namespace UniInventory.Gui
{
    enum GuiState
    {
        OPEN, CLOSED, INVALID
    }

    [RequireComponent(typeof(ContainerSlots))]
    public class GuiInventory : MonoBehaviour
    {
        public GameObject Canvas; // the canvas to draw on
        private ContainerSlots Container; // the container to display

        public GameObject InventorySlotPrefab; // inventory slot, containing image and text
        public GameObject InventoryPanelPrefab; // panel to add slots

        private GameObject[] InventorySlots; // all instantiated displayed slots
        private GameObject InventoryPanel; // the instantiated panel object

        private GuiState State = GuiState.CLOSED; // current state of the GUI
        
        /// <summary>
        /// Toggle GUI state between open and closed
        /// </summary>
        private void ToggleState() {
            if (State == GuiState.OPEN)
                State = GuiState.CLOSED;
            else if (State == GuiState.CLOSED)
                State = GuiState.OPEN;
        }

        /// <summary>
        /// Check to see if the GUI is valid to instantiate
        /// </summary>
        void Awake()
        {
            Container = GetComponent<ContainerSlots>(); // this should never fail
            if (Container == null || Canvas == null || InventoryPanelPrefab == null || InventorySlotPrefab == null)
            {
                State = GuiState.INVALID;
                Debug.LogError("[GuiInventory] missing reference to objects");
            }
        }
        /// <summary>
        /// Open and close the inventory panel. If not created, create and open.
        /// </summary>
        /// <param name="active">true to open, false to close</param>
        private void EnableInventoryPanel(bool active)
        {
            if (InventorySlots == null)
            {
                // create panel as child of canvas
                InventoryPanel = Instantiate(InventoryPanelPrefab);
                InventoryPanel.transform.SetParent(Canvas.transform);
                InventoryPanel.transform.localPosition = new Vector3(0, 0, 0);

                // create slots under panel
                InventorySlots = new GameObject[Container.Capacity];
                for (int i = 0; i < Container.Capacity; i++)
                {
                    InventorySlots[i] = Instantiate(InventorySlotPrefab);
                    InventorySlots[i].transform.SetParent(InventoryPanel.transform);
                }
            }
            InventoryPanel.SetActive(active);
        }

        /// <summary>
        /// Get the sprite image representing the slot with index i
        /// </summary>
        /// <param name="index">index of the slot</param>
        /// <returns>the sprite at the index or null if no item is present</returns>
        private Sprite GetSpriteAt(int index)
        {
            ItemStack stack = Container.GetItemStackAt(index);
            if (stack == null)
                return null;
            return stack.GetSprite();
        }

        /// <summary>
        /// Get the stack size to display for the item stack at index
        /// </summary>
        /// <param name="index">index of the slot</param>
        /// <returns>the stack size</returns>
        private int GetItemCountAt(int index)
        {
            ItemStack stack = Container.GetItemStackAt(index);
            if (stack == null)
                return 0;
            return stack.stackSize;
        }

        /// <summary>
        /// called to update the GUI display for each item
        /// </summary>
        private void UpdateSlotsDisplay()
        {
            // return if we have nothing to update
            if (InventorySlots == null)
                return;

            for (int i = 0; i < InventorySlots.Length; i++)
            {
                Sprite sprite = GetSpriteAt(i);
                int count = GetItemCountAt(i);

                GameObject itemImage = InventorySlots[i].transform.GetChild(0).gameObject;
                GameObject stackSizeText = InventorySlots[i].transform.GetChild(1).gameObject;
                if (sprite == null)
                {
                    itemImage.SetActive(false);
                    stackSizeText.SetActive(false);
                }
                else
                {
                    itemImage.SetActive(true);
                    itemImage.GetComponent<Image>().sprite = sprite;
                    stackSizeText.SetActive(true);
                    stackSizeText.GetComponent<Text>().text = count.ToString();
                }
            }

        }
        
        /// <summary>
        /// Update the inventory
        /// </summary>
        void Update()
        {
            if (State == GuiState.INVALID)
            {
                Debug.Log("[GuiInventory] trying to open invalid GUI");
                return;
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                ToggleState();
                if (State == GuiState.OPEN)
                    EnableInventoryPanel(true);
                else
                    EnableInventoryPanel(false);
            }

            if (State == GuiState.OPEN)
            {
                UpdateSlotsDisplay();
            }
        }
    }
}


