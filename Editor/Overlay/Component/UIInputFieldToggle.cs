using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class UIInputFieldToggle : UIBaseToggle
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UIInputFieldToggle);
        
        public UIInputFieldToggle()
        {
            onIcon = EditorUtilities.GetIcon("ui_input_field");
            offIcon = EditorUtilities.GetIcon("ui_input_field");
            tooltip = "Create Input Field";
        }
        
        protected override GameObject GetPlacementObject()
        {
            return CreateUGUILegacyObject("Input Field", true);
        }
    }
}