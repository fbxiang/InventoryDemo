using UnityEngine;
using System.Collections;
using UniInventory.Container;
using UniInventory.Items;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace UniInventory.Gui
{

    public class GuiInventory : GuiSlotsBase
    {
        public int SlotsPerRow = 10;

        private int cursorOverSlot;

        public override int CursorOverSlot()
        {
            return cursorOverSlot;
        }

        /// <summary>
        /// Initial call to draw the entire inventory
        /// </summary>
        public void UpdateDisplay(Slot[] slots)
        {
            DrawBackground();

            cursorOverSlot = -1;

            float totalHeight = windowSize.y;
            float totalWidth = windowSize.x;
            float startX = startPosition.x;
            float startY = startPosition.y;

            Rect guiRect = new Rect(startPosition, windowSize);
            GUI.Box(guiRect, "", Skin.GetStyle("background"));

            int slotsCount = slots.Length;
            int rowCount = (int)Mathf.Ceil(slotsCount / (float)SlotsPerRow);

            float slotsStartX = startX + (totalWidth - (slotWidth * SlotsPerRow + slotPadding * (SlotsPerRow - 1))) / 2;
            float slotsStartY = startY + totalHeight - 64 - (rowCount * slotHeight + (rowCount - 1) * slotPadding);

            DrawSlots(slots, slotsStartX, slotsStartY, slotsCount);
        }

        /// <summary>
        /// Call to draw the inventory slots
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="slotsCount"></param>
        private void DrawSlots(Slot[] slots, float startX, float startY, int slotsCount)
        {
            float xStride = slotWidth + slotPadding;
            float yStride = slotHeight + slotPadding;

            int slotIndex = 0;
            for (int y = 0; ; y++)
            {
                if (slotIndex == slotsCount) break;

                for (int x = 0; x < SlotsPerRow; x++)
                {
                    Rect slotRect = new Rect(startX + x * xStride, startY + y * yStride, slotWidth, slotHeight);

                    float itemPaddingWidth = (slotWidth - itemSize) / 2;
                    float itemPaddingHeight = (slotHeight - itemSize) / 2;

                    Rect itemRect = new Rect(startX + x * xStride + itemPaddingWidth, startY + y * yStride + itemPaddingHeight, itemSize, itemSize);

                    GUI.Box(slotRect, "", Skin.GetStyle("slot"));

                    DrawItemStack(GetItemStackAt(slots, slotIndex), startX + x * xStride + slotWidth/2, startY + y * yStride + slotHeight/2, itemSize);

                    if (slotRect.Contains(Event.current.mousePosition))
                    {
                        cursorOverSlot = slotIndex;
                    }

                    slotIndex += 1;
                    if (slotIndex == slotsCount)
                        break;
                }
            }
        }
    }
}


