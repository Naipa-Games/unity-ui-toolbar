using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIScrollbarToggle : UIToolbarToggleBase
        {
            internal const string ID = SceneUIComponentToolbar.ID + "." + nameof(UIScrollbarToggle);

            public UIScrollbarToggle()
            {
                icon = EditorUtilities.GetIcon("ui_scrollbar");
                tooltip = "Create Scrollbar";
            }

            public override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Scrollbar");
            }
        }
    }
}