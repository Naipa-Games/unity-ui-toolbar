using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AlignCenterButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(AlignCenterButton);

        public AlignCenterButton()
        {
            icon = EditorUtilities.GetIcon("align_center");
            tooltip = "Align to the center";
            clicked += () => AlignUtilities.AlignSelectedRects(AlignMode.Center);
        }
    }
}