using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UISliderToggle : UIToolbarToggleBase
        {
            internal const string ID = SceneUIComponentToolbar.ID + "." + nameof(UISliderToggle);

            public UISliderToggle()
            {
                icon = EditorUtilities.GetIcon("ui_slider");
                tooltip = "Create Slider";
            }

            public override GameObject GetPlacementObject()
            {
                return CreateUGUIObject("Slider");
            }
        }
    }
}