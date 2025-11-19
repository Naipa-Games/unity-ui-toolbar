using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIScrollbarToggle : UIBaseToggle
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UIScrollbarToggle);

            public UIScrollbarToggle()
            {
                icon = EditorUtilities.GetIcon("ui_scrollbar");
                tooltip = "Create Scrollbar";
            }

            protected override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Scrollbar");
            }
        }
    }
}