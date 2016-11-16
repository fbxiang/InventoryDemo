using UnityEngine;
using System.Collections;

using UniInventory.Container;
using UniInventory.Items;
using UniInventory.Entity;

namespace UniInventory.Gui
{
    [RequireComponent(typeof(ContainerSlots), typeof(ContainerHotbar), typeof(GuiHotbar))]
    [RequireComponent(typeof(GuiInventory), typeof(ContainerCursor))]
    public class PlayerGuiController : MonoBehaviour
    {
        public int itemSizeOnCursor;
        private GuiHotbar guiHotbar;
        private ContainerHotbar containerHotbar;

        private GuiInventory guiPlayerInventory;
        private ContainerSlots containerPlayerInventory;

        private GuiInventory guiOtherInventory;
        private ContainerSlots containerOtherInventory;

        private ContainerCursor containerCursor;

        public bool active;

        void Awake()
        {
            guiHotbar = GetComponent<GuiHotbar>();
            containerHotbar = GetComponent<ContainerHotbar>();

            guiPlayerInventory = GetComponent<GuiInventory>();
            containerPlayerInventory = GetComponent<ContainerSlots>();

            containerCursor = GetComponent<ContainerCursor>();
        }

        public void setOther(GuiInventory gui, ContainerSlots container)
        {
            this.guiOtherInventory = gui;
            this.containerOtherInventory = container;
        }

        public void resetOther()
        {
            this.guiOtherInventory = null;
            this.containerOtherInventory = null;
        }


        private void GetCurrent(out GuiSlotsBase gui, out ContainerSlots container)
        {
            gui = null; container = null;
            if (guiHotbar.CursorOverGui()) { gui = guiHotbar; container = containerHotbar;}
            if (guiPlayerInventory.CursorOverGui()) { gui = guiPlayerInventory; container = containerPlayerInventory; }
            if (guiOtherInventory != null && guiOtherInventory.CursorOverGui()) { gui = guiOtherInventory; container = containerOtherInventory; }
        }

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
            GuiSlotsBase.DrawItemStack(containerCursor.itemStack, Event.current.mousePosition.x, Event.current.mousePosition.y, itemSizeOnCursor);
        }

        private int GetNumberPressed()
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

        private int GetHotbarIndexFromNumber(int number)
        {
            return (number + 9) % 10;
        }

        private void changeHotbarFocus(int numberPressed)
        {
            if (numberPressed >= 0 && numberPressed < 10)
                GetComponent<ContainerHotbar>().FocusedSlotIndex = GetHotbarIndexFromNumber(numberPressed);
        }

        private void handleMouseClick(ContainerSlots currentContainer, int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < currentContainer.Capacity)
                {
                Slot slot = currentContainer.GetSlotAt(slotIndex);
                if (containerCursor.itemStack != null && slot.itemStack != null && slot.itemStack.mergeable(containerCursor.itemStack))
                {
                    containerCursor.itemStack = slot.itemStack.mergeWith(containerCursor.itemStack);
                }
                else
                {
                    ItemStack tempStack = containerCursor.itemStack;
                    containerCursor.itemStack = slot.itemStack;
                    slot.itemStack = tempStack;
                }
            }
        }

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

        private void setGuiOpenState(bool active)
        {
            if (!active)
                resetOther();
            this.active = active;
            Cursor.visible = active;
        }

        private void useFocusedItem()
        {
            if (containerHotbar.FocusedSlotIndex >= 0 && containerHotbar.FocusedSlotIndex < 10)
            {
                ItemStack stack = containerHotbar.GetItemStackAt(containerHotbar.FocusedSlotIndex);
                if (stack == null) return;
                stack.use(GetComponent<EntityPlayer>(), Time.deltaTime);
            }
        }

        private void holdFocusedItem()
        {
            if (containerHotbar.FocusedSlotIndex >= 0 && containerHotbar.FocusedSlotIndex < 10)
            {
                ItemStack stack = containerHotbar.GetItemStackAt(containerHotbar.FocusedSlotIndex);
                if (stack == null) return;
                stack.hold(GetComponent<EntityPlayer>(), Time.deltaTime);
            }
        }

        void Update()
        {
            EntityPlayer player = GetComponent<EntityPlayer>();
            if (player.Key.Down("inventory"))
                setGuiOpenState(!active);
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;

            int numberPressed = GetNumberPressed();
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

