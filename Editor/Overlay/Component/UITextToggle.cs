using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UITextToggle : UIBaseToggle
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UITextToggle);

            public UITextToggle()
            {
                icon = EditorUtilities.GetIcon("ui_text");
                tooltip = "Create Text";
            }

            protected override GameObject GetPlacementObject()
            {
                return CreateUGUILegacyObject("Text", true);
            }
        }
    }
}