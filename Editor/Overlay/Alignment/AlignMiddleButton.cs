using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AlignMiddleButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(AlignMiddleButton);

        public AlignMiddleButton()
        {
            icon = EditorUtilities.GetIcon("align_middle");
            tooltip = "Align to the middle";
            clicked += () => AlignUtilities.AlignSelectedRects(AlignMode.Middle);
        }
    }
}