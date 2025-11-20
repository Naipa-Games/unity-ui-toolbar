using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIToggleToolbar : UIToolbarToggleBase
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UIToggleToolbar);

            public UIToggleToolbar()
            {
                icon = EditorUtilities.GetIcon("ui_toggle");
                tooltip = "Create Toggle";
            }

            public override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Toggle");
            }
        }
    }
}