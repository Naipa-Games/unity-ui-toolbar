using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIToggleToggle : UIBaseToggle
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UIToggleToggle);

            public UIToggleToggle()
            {
                icon = EditorUtilities.GetIcon("ui_toggle");
                tooltip = "Create Toggle";
            }

            protected override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Toggle");
            }
        }
    }
}