using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIImageToggle : UIBaseToggle
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UIImageToggle);

            public UIImageToggle()
            {
                onIcon = EditorUtilities.GetIcon("ui_image");
                offIcon = EditorUtilities.GetIcon("ui_image");
                tooltip = "Create Image";
            }

            protected override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Image");
            }
        }
    }
}