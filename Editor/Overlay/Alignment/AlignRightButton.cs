using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AlignRightButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(AlignRightButton);

        public AlignRightButton()
        {
            icon = EditorUtilities.GetIcon("align_right");
            tooltip = "Align to the right";
            clicked += () => AlignUtilities.AlignSelectedRects(AlignMode.Right);
        }
    }
}