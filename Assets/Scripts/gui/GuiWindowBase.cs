using UnityEngine;
using System.Collections;
using System;

namespace UniInventory.Gui
{
    public class GuiWindowBase : MonoBehaviour
    {
        public enum DrawFrom {START, END, CENTER}
        public enum AnchorAt {START, END, CENTER}

        public AnchorAt horizontalAnchor, verticalAnchor;
        public DrawFrom horizontalDrawFrom, verticalDrawFrom;

        public Vector2 windowSizeInches;
        public Vector2 windowOffsetInches;

        public GUISkin Skin;

        protected Vector2 windowSize, windowOffset;

        protected bool cursorOverGui = false;

        protected Vector2 startPosition;

        /// <summary>
        /// Get if the cursor is hovring over this GUI
        /// </summary>
        /// <returns></returns>
        public bool CursorOverGui()
        {
            return cursorOverGui;
        }

        /// <summary>
        /// Initialize gui pixel length info with inch length
        /// </summary>
        public virtual void Awake()
        {
            windowSize = windowSizeInches * Screen.dpi;
            windowOffset = windowOffsetInches * Screen.dpi;
            startPosition = GetStartPosition();
        }

        /// <summary>
        /// Draw the background of the window and update mouse info
        /// </summary>
        public virtual void DrawBackground()
        {
            GUI.skin = Skin;
            Rect guiRect = new Rect(GetStartPosition(), windowSize);
            GUI.Box(guiRect, "", Skin.GetStyle("background"));
            cursorOverGui = guiRect.Contains(Event.current.mousePosition);
        }

        /// <summary>
        /// Get the top left corner position of this window
        /// </summary>
        /// <returns></returns>
        private Vector2 GetStartPosition()
        {
            Vector2 anchor = GetAnchorPosition();
            Vector2 start = anchor;
            switch (horizontalDrawFrom)
            {
                case DrawFrom.START:
                    break;
                case DrawFrom.CENTER:
                    start = start - new Vector2(windowSize.x / 2, 0);
                    break;
                case DrawFrom.END:
                    start = start - new Vector2(windowSize.x, 0);
                    break;
            }
            switch (verticalDrawFrom)
            {
                case DrawFrom.START:
                    break;
                case DrawFrom.CENTER:
                    start = start - new Vector2(0, windowSize.y / 2);
                    break;
                case DrawFrom.END:
                    start = start - new Vector2(0, windowSize.y);
                    break;
            }
            return start + windowOffset;
        }

        /// <summary>
        /// calculate the anchor position
        /// </summary>
        /// <returns></returns>
        private Vector2 GetAnchorPosition()
        {
            float anchorX = 0, anchorY = 0;

            switch (horizontalAnchor)
            {
                case AnchorAt.START:
                    anchorX = 0;
                    break;
                case AnchorAt.CENTER:
                    anchorX = Screen.width / 2;
                    break;
                case AnchorAt.END:
                    anchorX = Screen.width;
                    break;
            }
            switch (verticalAnchor)
            {
                case AnchorAt.START:
                    anchorY = 0;
                    break;
                case AnchorAt.CENTER:
                    anchorY = Screen.height / 2;
                    break;
                case AnchorAt.END:
                    anchorY = Screen.height;
                    break;
            }
            return new Vector2(anchorX, anchorY);
        }
    }
}

