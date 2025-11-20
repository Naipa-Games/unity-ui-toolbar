using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIButtonToggle : UIToolbarToggleBase
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UIButtonToggle);

            public UIButtonToggle()
            {
                icon = EditorUtilities.GetIcon("ui_button");
                tooltip = "Create Button";
            }

            public override GameObject GetPlacementObject()
            {
                return CreateUGUILegacyObject("Button", true);
            }
        }
    }
}