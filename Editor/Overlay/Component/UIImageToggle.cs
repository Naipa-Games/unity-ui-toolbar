using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIImageToggle : UIToolbarToggleBase
        {
            internal const string ID = SceneUIComponentToolbar.ID + "." + nameof(UIImageToggle);

            public UIImageToggle()
            {
                onIcon = EditorUtilities.GetIcon("ui_image");
                offIcon = EditorUtilities.GetIcon("ui_image");
                tooltip = "Create Image";
            }

            public override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Image");
            }
        }
    }
}