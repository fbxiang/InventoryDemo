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

        private bool cursorOverGui;
        private int cursorOverSlot;

        public enum Anchor {LEFT, RIGHT };
        public Anchor anchor;

        public override bool CursorOverGui()
        {
            return cursorOverGui;
        }

        public override int CursorOverSlot()
        {
            return cursorOverSlot;
        }

        /// <summary>
        /// Initial call to draw the entire inventory
        /// </summary>
        public void UpdateDisplay(Slot[] slots)
        {
            cursorOverGui = false;
            cursorOverSlot = -1;

            GUI.skin = Skin;
            int totalHeight = Screen.height;
            int totalWidth = 800;
            int startX = anchor == Anchor.LEFT ? 0 : Screen.width - totalWidth, startY = 0;
            Rect guiRect = new Rect(startX, startY, totalWidth, totalHeight);
            GUI.Box(guiRect, "", Skin.GetStyle("background"));
            cursorOverGui = guiRect.Contains(Event.current.mousePosition);

            int slotsCount = slots.Length;
            int rowCount = (int)Mathf.Ceil(slotsCount / (float)SlotsPerRow);

            int slotsStartX = startX + (totalWidth - (SlotWidth * SlotsPerRow + SlotPadding * (SlotsPerRow - 1))) / 2;
            int slotsStartY = startY + totalHeight - 64 - (rowCount * SlotHeight + (rowCount - 1) * SlotPadding);

            DrawSlots(slots, slotsStartX, slotsStartY, slotsCount);
        }

        /// <summary>
        /// Call to draw the inventory slots
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="slotsCount"></param>
        private void DrawSlots(Slot[] slots, int startX, int startY, int slotsCount)
        {
            int xStride = SlotWidth + SlotPadding;
            int yStride = SlotHeight + SlotPadding;

            int slotIndex = 0;
            for (int y = 0; ; y++)
            {
                if (slotIndex == slotsCount) break;

                for (int x = 0; x < SlotsPerRow; x++)
                {
                    Rect slotRect = new Rect(startX + x * xStride, startY + y * yStride, SlotWidth, SlotHeight);

                    int itemPaddingWidth = (SlotWidth - itemSize) / 2;
                    int itemPaddingHeight = (SlotHeight - itemSize) / 2;

                    Rect itemRect = new Rect(startX + x * xStride + itemPaddingWidth, startY + y * yStride + itemPaddingHeight, itemSize, itemSize);

                    GUI.Box(slotRect, "", Skin.GetStyle("slot"));

                    DrawItemStack(GetItemStackAt(slots, slotIndex), startX + x * xStride + SlotWidth/2, startY + y * yStride + SlotHeight/2, itemSize);

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


