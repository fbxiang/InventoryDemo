using UnityEngine;
using System.Collections.Generic;
using UniInventory.Items;
using UniInventory.Container;

namespace UniInventory.Gui
{
    public abstract class GuiSlotsBase : MonoBehaviour
    {
        public int SlotWidth = 32, SlotHeight = 32;
        public int SlotPadding = 4;
        public int itemSize;

        public GUISkin Skin;

        protected ItemStack GetItemStackAt(Slot[] slots, int index)
        {
            return slots[index].itemStack;
        }

        /// <summary>
        /// Draw item stack centring at x, y with item size
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="itemSize"></param>
        public static void DrawItemStack(ItemStack stack, float centerX, float centerY, float itemSize)
        {
            if (stack == null) return;
            Texture2D icon = stack.GetIcon();
            int itemCount = stack.stackSize;

            float startX = centerX - itemSize / 2;
            float startY = centerY - itemSize / 2;

            GUI.DrawTexture(new Rect(startX, startY, itemSize, itemSize), icon);

            Vector2 size = GUI.skin.GetStyle("label").CalcSize(new GUIContent("" + itemCount));
            Vector2 labelStart = new Vector2(centerX + itemSize / 2, centerY + itemSize / 2) - size *2 / 3;

            GUI.Label(new Rect(labelStart, size), "" + itemCount);

        }

        /// <summary>
        /// Get if the cursor is hovring over this GUI
        /// </summary>
        /// <returns></returns>
        public abstract bool CursorOverGui();

        /// <summary>
        /// Get the slot index the mouse is currently hovering over, -1 if mouse is elsewhere
        /// </summary>
        /// <returns></returns>
        public abstract int CursorOverSlot();

        /// <summary>
        /// Called to draw the full tool tip of the item
        /// </summary>
        /// <param name="itemStack"></param>
        public static void DrawToolTip(ItemStack itemStack)
        {
            if (itemStack == null) return;
            string name = "<color=#ffffffff>" + itemStack.GetItem().itemName + "</color>";
            string description = "<color=#ccccccff>" + itemStack.GetDescription() + "</color>";

            TooltipBuilder builder = new TooltipBuilder();
            builder.AddText(name, "itemtitle");
            builder.AddText(description, "description");
            builder.buildToolTip();
        }
    }

    /// <summary>
    /// This class builds up the tooltip of an item.
    /// </summary>
    public class TooltipBuilder
    {
        /// <summary>
        /// Storing all text required for the tooltip
        /// </summary>
        struct TooltipComponent
        {
            public string text;
            public GUIStyle style;
        }

        public float marginLeft = 10, marginRight = 50, marginTop = 10, marginBottom = 10;
        public float margin { set { marginLeft = marginRight = marginTop = marginBottom = value; } }

        private List<TooltipComponent> components = new List<TooltipComponent>();

        /// <summary>
        /// Add a block of text to the tooltip
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        public void AddText(string text, string style = "label")
        {
            TooltipComponent newComponent = new TooltipComponent();
            newComponent.text = text;
            newComponent.style = GUI.skin.GetStyle(style);
            components.Add(newComponent);
        }

        /// <summary>
        /// Build up the tooltip and display in the GUI.
        /// </summary>
        public void buildToolTip()
        {
            float width = 0, height = 0;
            foreach (TooltipComponent component in components)
            {
                if (component.style.wordWrap) continue; // if it is willing to wrap, we do nothing
                float minWidth, maxWidth;
                component.style.CalcMinMaxWidth(new GUIContent(component.text), out minWidth, out maxWidth);
                width = Mathf.Max(width, minWidth);
            }
            width += marginLeft + marginRight;
            width = Mathf.Max(200, width);

            foreach (TooltipComponent component in components)
            {
                height += component.style.CalcHeight(new GUIContent(component.text), width - marginLeft - marginRight);
            }
            height += marginBottom + marginTop;

            Vector2 mousePosition = Event.current.mousePosition;

            float startX = mousePosition.x + width >= Screen.width ? Screen.width - width : mousePosition.x;
            float startY = mousePosition.y + height >= Screen.height ? Screen.height - height : mousePosition.y;

            GUI.Box(new Rect(startX, startY, width, Mathf.Max(height, 200)), "", GUI.skin.GetStyle("tooltip"));

            GUILayout.BeginArea(new Rect(startX + marginLeft, startY + marginTop, width - marginLeft - marginRight, height - marginTop - marginBottom));
            GUILayout.BeginVertical();
            foreach (TooltipComponent component in components)
            {
                GUILayout.Label(component.text, component.style);
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}

