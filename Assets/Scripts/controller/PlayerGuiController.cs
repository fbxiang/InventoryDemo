using UnityEngine;
using System.Collections;

using UniInventory.Container;
using UniInventory.Items;
using UniInventory.Entity;

namespace UniInventory.Gui
{
    /// <summary>
    /// This class controls different gui and container components the player is interacting with
    /// </summary>
    [RequireComponent(typeof(ContainerSlots), typeof(ContainerHotbar), typeof(GuiHotbar))]
    [RequireComponent(typeof(GuiInventory), typeof(ContainerCursor))]
    public class PlayerGuiController : MonoBehaviour
    {
        public float itemSizeOnCursor;  // displayed item size when it is on the mouse cursor
        private GuiHotbar guiHotbar;  // the gui for the hotbar
        private ContainerHotbar containerHotbar; // the container for the hotbar

        private GuiInventory guiPlayerInventory;  // the gui for the inventory on player
        private ContainerSlots containerPlayerInventory;  // the container for the inventory on player

        private GuiInventory guiOtherInventory;  // the gui of the other inventory player opens
        private ContainerSlots containerOtherInventory;  // the container of the other inventory

        private ContainerCursor containerCursor;  // the container to store a single item stack on mouse cursor

        private EntityPlayer player;  // player itself

        public bool active;  // gui is active

        /// <summary>
        /// Initialize all components
        /// </summary>
        void Start()
        {
            guiHotbar = GetComponent<GuiHotbar>();
            containerHotbar = GetComponent<ContainerHotbar>();

            guiPlayerInventory = GetComponent<GuiInventory>();
            containerPlayerInventory = GetComponent<ContainerSlots>();

            containerCursor = GetComponent<ContainerCursor>();

            player = GetComponent<EntityPlayer>();
        }

        /// <summary>
        /// Set the other inventory the player interact with
        /// </summary>
        /// <param name="gui"></param>
        /// <param name="container"></param>
        public void setOther(GuiInventory gui, ContainerSlots container)
        {
            this.guiOtherInventory = gui;
            this.containerOtherInventory = container;
        }

        /// <summary>
        /// Reset the other inventory to nothing
        /// </summary>
        public void resetOther()
        {
            this.guiOtherInventory = null;
            this.containerOtherInventory = null;
        }

        /// <summary>
        /// Get the current container where player can interact with
        /// </summary>
        /// <param name="gui">output the gui</param>
        /// <param name="container">output the container</param>
        private void GetCurrent(out GuiSlotsBase gui, out ContainerSlots container)
        {
            gui = null; container = null;
            if (guiHotbar.CursorOverGui()) { gui = guiHotbar; container = containerHotbar;}
            if (guiPlayerInventory.CursorOverGui()) { gui = guiPlayerInventory; container = containerPlayerInventory; }
            if (guiOtherInventory != null && guiOtherInventory.CursorOverGui()) { gui = guiOtherInventory; container = containerOtherInventory; }
        }

        /// <summary>
        /// Draw stuff, handle mouse and keyboard events
        /// </summary>
        void OnGUI()
        {
            guiHotbar.UpdateDisplay(containerHotbar.ItemSlots, containerHotbar.FocusedSlotIndex);

            if (!active) return;

            guiPlayerInventory.UpdateDisplay(containerPlayerInventory.ItemSlots);

            if (guiOtherInventory!= null) {
                guiOtherInventory.UpdateDisplay(containerOtherInventory.ItemSlots);
            }

            GuiSlotsBase currentGui;
            ContainerSlots currentContainer;
            GetCurrent(out currentGui, out currentContainer);
            if (currentGui != null)
            {
                int slotIndex = currentGui.CursorOverSlot();
                if (slotIndex >= 0 && slotIndex < containerPlayerInventory.Capacity)
                {
                    GuiSlotsBase.DrawToolTip(currentContainer.GetItemStackAt(slotIndex));
                }
            }
            GuiSlotsBase.DrawItemStack(containerCursor.itemStack, Event.current.mousePosition.x, Event.current.mousePosition.y, itemSizeOnCursor * Screen.dpi);
        }

        /// <summary>
        /// Get the number key down event
        /// </summary>
        /// <returns>the number key pressed or -1 if no key pressed</returns>
        private int GetNumberKeyDown()
        {
            EntityPlayer player = GetComponent<EntityPlayer>();
            for (int i = 0; i < 10; i++)
            {
                if (player.Key.Down(i.ToString()))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Convert the number key to the index of slot in the hotbar
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int GetHotbarIndexFromNumber(int number)
        {
            return (number + 9) % 10;
        }

        /// <summary>
        /// Set the focus in hotbar to the number key
        /// </summary>
        /// <param name="numberKey"></param>
        private void changeHotbarFocus(int numberKey)
        {
            if (numberKey >= 0 && numberKey < 10)
                GetComponent<ContainerHotbar>().FocusedSlotIndex = GetHotbarIndexFromNumber(numberKey);
        }

        /// <summary>
        /// Handle the mouse click event when the gui is open
        /// </summary>
        /// <param name="currentContainer"></param>
        /// <param name="slotIndex"></param>
        private void handleMouseClick(ContainerSlots currentContainer, int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < currentContainer.Capacity)
                {
                Slot slot = currentContainer.GetSlotAt(slotIndex);
                if (containerCursor.itemStack != null && slot.itemStack != null && slot.itemStack.mergeable(containerCursor.itemStack))
                {
                    player.playerAudio.PlayOneShot(containerCursor.itemStack.GetClickSound());
                    containerCursor.itemStack = slot.itemStack.mergeWith(containerCursor.itemStack);
                }
                else
                {
                    if (containerCursor.itemStack != null || slot.itemStack != null)
                    {
                        player.playerAudio.PlayOneShot((containerCursor.itemStack != null? containerCursor.itemStack : slot.itemStack).GetClickSound());
                    }
                    ItemStack tempStack = containerCursor.itemStack;
                    containerCursor.itemStack = slot.itemStack;
                    slot.itemStack = tempStack;
                }
            }
        }

        /// <summary>
        /// Handle the mouse wheel event when gui is open
        /// </summary>
        /// <param name="currentContainer">current container</param>
        /// <param name="slotIndex">the slot mouse is over</param>
        private void handleMouseWheel(ContainerSlots currentContainer, int slotIndex)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (slotIndex >= 0 && slotIndex < currentContainer.Capacity)
                {
                    Slot slot = currentContainer.GetSlotAt(slotIndex);
                    if (containerCursor.itemStack == null && slot.itemStack != null)
                    {
                        containerCursor.itemStack = slot.itemStack.Split(1);
                    }
                    else if (containerCursor.itemStack != null && slot.itemStack != null && slot.itemStack.mergeable(containerCursor.itemStack))
                    {
                        slot.itemStack = containerCursor.itemStack.takeNumberReturnRemain(slot.itemStack, 1);
                    }
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (slotIndex >= 0 && slotIndex < currentContainer.Capacity)
                {
                    Slot slot = currentContainer.GetSlotAt(slotIndex);
                    if (containerCursor.itemStack != null && slot.IsEmpty())
                    {
                        slot.itemStack = containerCursor.itemStack.Split(1);
                    }
                    else if (containerCursor.itemStack != null && slot.itemStack != null && slot.itemStack.mergeable(containerCursor.itemStack))
                    {
                        containerCursor.itemStack = slot.itemStack.takeNumberReturnRemain(containerCursor.itemStack, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Open or close the gui
        /// </summary>
        /// <param name="open"></param>
        private void setGuiOpen(bool open)
        {
            if (!open)
                resetOther();
            else
            {
                if (player.LookObject != null && player.LookObject.GetComponent<ContainerSlots>() != null)
                {
                    containerOtherInventory = player.LookObject.GetComponent<ContainerSlots>();
                    guiOtherInventory = player.LookObject.GetComponent<GuiInventory>();
                }
            }
            this.active = open;
            Cursor.visible = open;
        }

        /// <summary>
        /// Called to notify the item stack that is is being used
        /// </summary>
        private void useFocusedItem()
        {
            if (containerHotbar.FocusedSlotIndex >= 0 && containerHotbar.FocusedSlotIndex < 10)
            {
                ItemStack stack = containerHotbar.GetItemStackAt(containerHotbar.FocusedSlotIndex);
                if (stack == null) return;
                stack.Use(GetComponent<EntityPlayer>(), Time.deltaTime);
            }
        }

        /// <summary>
        /// Called to notify the item stack that is is being held but not used
        /// </summary>
        private void holdFocusedItem()
        {
            if (containerHotbar.FocusedSlotIndex >= 0 && containerHotbar.FocusedSlotIndex < 10)
            {
                ItemStack stack = containerHotbar.GetItemStackAt(containerHotbar.FocusedSlotIndex);
                if (stack == null) return;
                stack.Hold(GetComponent<EntityPlayer>(), Time.deltaTime);
            }
        }

        /// <summary>
        /// update the events and call helper functions
        /// </summary>
        void Update()
        {
            EntityPlayer player = GetComponent<EntityPlayer>();

            if (player.Key.Down("close"))
                setGuiOpen(false);
            else if (player.Key.Down("inventory"))
                setGuiOpen(!active);
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;

            int numberPressed = GetNumberKeyDown();
            int slotIndexPressed = (numberPressed + 9) % 10;
            if (!active)
            {
                changeHotbarFocus(numberPressed);
                if (player.Key.Pressed("use"))
                    useFocusedItem();
                else
                    holdFocusedItem();
                return;
            }
            GuiSlotsBase currentGui;
            ContainerSlots currentContainer;
            GetCurrent(out currentGui, out currentContainer);
            if (currentContainer == null) return;
            int slotIndex = currentGui.CursorOverSlot();
            if (numberPressed >= 0 && numberPressed < 10 && slotIndex >= 0 && slotIndex < currentContainer.Capacity)
            {
                Slot slot1 = currentContainer.GetSlotAt(slotIndex);
                Slot slot2 = containerHotbar.GetSlotAt(slotIndexPressed);
                Slot.SwapItemStacks(slot1, slot2);
            }
            if (Input.GetMouseButtonDown(0))
                handleMouseClick(currentContainer, slotIndex);
            else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                handleMouseWheel(currentContainer, slotIndex);
        }
    }
}

