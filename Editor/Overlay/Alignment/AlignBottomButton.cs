using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AlignBottomButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(AlignBottomButton);

        public AlignBottomButton()
        {
            icon = EditorUtilities.GetIcon("align_bottom");
            tooltip = "Align to the bottom";
            clicked += () => AlignUtilities.AlignSelectedRects(AlignMode.Bottom);
        }
    }
}