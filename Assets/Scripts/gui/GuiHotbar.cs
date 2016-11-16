using UnityEngine;
using System.Collections;
using System;
using UniInventory.Container;

namespace UniInventory.Gui
{
    public class GuiHotbar : GuiSlotsBase
    {   
        public readonly int numberOfSlots = 10;

        private bool cursorOverGui;
        private int cursorOverSlot;

        public override bool CursorOverGui()
        {
            return cursorOverGui;
        }

        public override int CursorOverSlot()
        {
            return cursorOverSlot;
        }

        public void UpdateDisplay(Slot[] slots, int focusIndex)
        {
            cursorOverGui = false;
            cursorOverSlot = -1;
            GUI.skin = Skin;
            int totalHeight = SlotHeight + 2 * SlotPadding;
            int totalWidth = SlotWidth * numberOfSlots + (numberOfSlots + 1) * SlotPadding;
            int startX = (Screen.width - totalWidth) / 2;
            int startY = Screen.height - totalHeight;
            Rect guiRect = new Rect(startX, startY, totalWidth, totalHeight);
            GUI.Box(guiRect, "", Skin.GetStyle("background"));
            cursorOverGui = guiRect.Contains(Event.current.mousePosition);
            for (int i = 0; i < numberOfSlots; i++)
            {
                int xStride = SlotWidth + SlotPadding;
                Rect slotRect = new Rect(startX + SlotPadding + i * (xStride), startY + SlotPadding, SlotWidth, SlotHeight);
                if (focusIndex == i)
                    GUI.Box(slotRect, "", Skin.GetStyle("focusedslot"));
                else
                    GUI.Box(slotRect, "", Skin.GetStyle("slot"));

                if (slotRect.Contains(Event.current.mousePosition))
                    cursorOverSlot = i;
                DrawItemStack(GetItemStackAt(slots, i), startX + SlotPadding + i * xStride + SlotWidth / 2, startY + SlotPadding + SlotHeight / 2, itemSize);
            }
        }
    }
}
