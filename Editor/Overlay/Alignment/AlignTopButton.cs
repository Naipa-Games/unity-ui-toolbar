using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class AlignTopButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(AlignTopButton);

        public AlignTopButton()
        {
            icon = EditorUtilities.GetIcon("align_top");
            tooltip = "Align to the top";
            clicked += () => AlignUtilities.AlignSelectedRects(AlignMode.Top);
        }
    }
}