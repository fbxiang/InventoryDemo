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
        private ContainerSlots Container; // the container to display


        private GameObject[] InventorySlots; // all instantiated displayed slots
        private GameObject InventoryPanel; // the instantiated panel object

        private GuiState State = GuiState.CLOSED; // current state of the GUI

        public int SlotWidth = 32, SlotHeight = 32;
        public int SlotPadding = 4;
        public int SlotsPerRow = 10;

        public int itemSize = 16;

        public GUISkin Skin;
        
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
            if (Container == null)
            {
                State = GuiState.INVALID;
                Debug.LogError("[GuiInventory] missing reference to objects");
            }
        }

        private ItemStack GetItemStackAt(int index)
        {
            return Container.GetItemStackAt(index);
        }


        /// <summary>
        /// Get the sprite image representing the slot with index i
        /// </summary>
        /// <param name="index">index of the slot</param>
        /// <returns>the sprite at the index or null if no item is present</returns>
        private Texture2D GetIconAt(int index)
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
        /// Initial call to draw the entire inventory
        /// </summary>
        private void DrawInventory()
        {
            GUI.skin = Skin;
            int totalHeight = Screen.height;
            int totalWidth = 800;
            int startX = 0, startY = 0;
            GUI.Box(new Rect(startX, startY, totalWidth, totalHeight), "", Skin.GetStyle("background"));

            int slotsCount = Container.Capacity;
            int rowCount = (int)Mathf.Ceil(slotsCount / (float)SlotsPerRow);

            int slotsStartX = startX + (totalWidth - (SlotWidth * SlotsPerRow + SlotPadding * (SlotsPerRow - 1))) / 2;
            int slotsStartY = startY + totalHeight - 64 - (rowCount * SlotHeight + (rowCount - 1) * SlotPadding);

            DrawSlots(slotsStartX, slotsStartY, slotsCount);
        }

        /// <summary>
        /// Call to draw the inventory slots
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="slotsCount"></param>
        private void DrawSlots(int startX, int startY, int slotsCount)
        {
            int xStride = SlotWidth + SlotPadding;
            int yStride = SlotHeight + SlotPadding;

            int itemIndex = 0;
            for (int y = 0; y < SlotsPerRow; y++)
            {
                for (int x = 0; x < SlotsPerRow; x++)
                {
                    Rect slotRect = new Rect(startX + x * xStride, startY + y * yStride, SlotWidth, SlotHeight);

                    int itemPaddingWidth = (SlotWidth - itemSize) / 2;
                    int itemPaddingHeight = (SlotHeight - itemSize) / 2;

                    Rect itemRect = new Rect(startX + x * xStride + itemPaddingWidth, startY + y * yStride + itemPaddingHeight, itemSize, itemSize);

                    GUI.Box(slotRect, "", Skin.GetStyle("slot"));

                    Texture2D icon = GetIconAt(itemIndex);
                    if (icon != null)
                        GUI.DrawTexture(itemRect, icon);

                    int itemCount = GetItemCountAt(itemIndex);
                    if (itemCount > 0)
                    {
                        GUI.Label(new Rect(startX + x * xStride + SlotWidth / 2, startY + y * yStride + SlotHeight / 2, SlotWidth / 2, SlotHeight / 2), "" + itemCount);
                        if (slotRect.Contains(Event.current.mousePosition))
                        {
                            DrawToolTip(GetItemStackAt(itemIndex));
                        }
                    }

                    itemIndex += 1;
                    if (itemIndex == slotsCount)
                        return;
                }
            }
        }

        /// <summary>
        /// Called to draw the full tool tip of the item
        /// </summary>
        /// <param name="itemStack"></param>
        private void DrawToolTip(ItemStack itemStack)
        {
            // TODO: finish this function
            GUI.Box(new Rect(Event.current.mousePosition, new Vector2(200, 400)), itemStack.GetItem().itemName);
        }

        /// <summary>
        /// Called all the time to draw the GUI
        /// </summary>
        void OnGUI()
        {
            if (State == GuiState.OPEN)
            {
                DrawInventory();
            }
        }


        /// <summary>
        /// Update the inventory state
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
            }
        }
    }
}


