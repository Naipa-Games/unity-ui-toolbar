using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AlignLeftButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(AlignLeftButton);

        public AlignLeftButton()
        {
            icon = EditorUtilities.GetIcon("align_left");
            tooltip = "Align to the left";
            clicked += () => AlignUtilities.AlignSelectedRects(AlignMode.Left);
        }
    }
}