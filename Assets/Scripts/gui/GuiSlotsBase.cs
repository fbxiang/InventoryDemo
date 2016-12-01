using UnityEngine;
using System.Collections.Generic;
using UniInventory.Items;
using UniInventory.Container;
using UniInventory.Registry;
using System.Linq;

namespace UniInventory.Gui
{
    public abstract class GuiSlotsBase : GuiWindowBase
    {
        public float slotWidthInches = 0.5f, slotHeightInches = 0.5f;
        public float slotPaddingInches = 0.05f;
        public float itemSizeInches = 0.4f;

        protected float slotWidth, slotHeight;
        protected float slotPadding;
        protected float itemSize;

        public override void Awake()
        {
            base.Awake();
            slotWidth = slotWidthInches * Screen.dpi;
            slotHeight = slotHeightInches * Screen.dpi;
            slotPadding = slotPaddingInches * Screen.dpi;
            itemSize = itemSizeInches * Screen.dpi;
        }


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
            Vector2 labelStart = new Vector2(centerX + itemSize / 2, centerY + itemSize / 2) - size * 2 / 3;

            GUI.Label(new Rect(labelStart, size), "" + itemCount);

        }

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
            string description = itemStack.GetDescription();

            TooltipBuilder builder = new TooltipBuilder();
            builder.AddText(itemStack.GetDisplayTitle(), "itemtitle");
            builder.AddText(itemStack.GetDisplaySubtitle(), "itemSubtitle");
            builder.AddText(description, "description");

            List<KeyValuePair<string, int>> abilityPriorityList = new List<KeyValuePair<string, int>>();
            foreach (var kvp in itemStack.GetAbilityIdPowers())
            {
                string id = kvp.Key; int power = kvp.Value;
                string text = AbilityRegistry.GetAbilityDisplayText(id);
                string color = AbilityRegistry.GetAbilityDisplayColor(id);
                int priority = AbilityRegistry.GetAbilityDisplayPriority(id);
                string displayText = "<color=" + color + ">" + text + " " + power + "</color>";
                abilityPriorityList.Add(new KeyValuePair<string, int>(displayText, priority));
            }

            foreach (var kvp in abilityPriorityList.OrderBy(kvp => -kvp.Value))
            {
                builder.AddText(kvp.Key, "ability");
            }
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
        /// <param name="text">nullable text, null would have no effect</param>
        /// <param name="style"></param>
        public void AddText(string text, string style = "label")
        {
            if (text == null) return;
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

            GUI.Box(new Rect(startX, startY, width, height), "", GUI.skin.GetStyle("tooltip"));

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

