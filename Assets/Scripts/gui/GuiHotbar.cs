using UnityEngine;
using System.Collections;
using System;
using UniInventory.Container;

namespace UniInventory.Gui
{
    public class GuiHotbar : GuiSlotsBase
    {   
        public readonly int numberOfSlots = 10;

        private int cursorOverSlot;

        public override int CursorOverSlot()
        {
            return cursorOverSlot;
        }

        /// <summary>
        /// Update display with the info needed
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="focusIndex"></param>
        public void UpdateDisplay(Slot[] slots, int focusIndex)
        {
            DrawBackground();
            cursorOverSlot = -1;

            float slotsTotalHeight = slotHeight + 2 * slotPadding;
            float slotsTotalWidth = slotWidth * numberOfSlots + (numberOfSlots + 1) * slotPadding;

            Vector2 slotsStartPosition = windowSize / 2 - (new Vector2(slotsTotalWidth, slotsTotalHeight)) / 2 + startPosition;

            float startX = slotsStartPosition.x;
            float startY = slotsStartPosition.y;

            for (int i = 0; i < numberOfSlots; i++)
            {
                float xStride = slotWidth + slotPadding;
                Rect slotRect = new Rect(startX + slotPadding + i * (xStride), startY + slotPadding, slotWidth, slotHeight);
                if (focusIndex == i)
                    GUI.Box(slotRect, "", Skin.GetStyle("focusedslot"));
                else
                    GUI.Box(slotRect, "", Skin.GetStyle("slot"));

                if (slotRect.Contains(Event.current.mousePosition))
                    cursorOverSlot = i;
                DrawItemStack(GetItemStackAt(slots, i), startX + slotPadding + i * xStride + slotWidth / 2, startY + slotPadding + slotHeight / 2, itemSize);
            }
        }
    }
}
