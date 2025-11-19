using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    public abstract class UniformSpacingButton : EditorToolbarButton
    {
        protected UniformSpacingButton()
        {
            clicked += OnClick;
        }

        private void OnClick()
        {
            PopupWindow.Show(this.GetScreenRect(), new SpacingGapPopupWindow(GetWindowTitle(), OnSpacingValueApply));
        }

        protected abstract string GetWindowTitle();
        protected abstract void OnSpacingValueApply(float spacing);
    }

    public static class EditorToolbarButtonExtensions
    {
        public static Rect GetScreenRect(this EditorToolbarButton button)
        {
            return new Rect(button.worldBound.x, button.worldBound.y + button.worldBound.height,
                button.worldBound.width, 0);
        }
    }
}