using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UISliderToggle : UIBaseToggle
        {
            internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UISliderToggle);

            public UISliderToggle()
            {
                icon = EditorUtilities.GetIcon("ui_slider");
                tooltip = "Create Slider";
            }

            protected override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Slider");
            }
        }
    }
}