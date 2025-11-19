using System.Linq;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class UniformRowSpacingButton : UniformSpacingButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UniformRowSpacingButton);

        public UniformRowSpacingButton()
        {
            icon = EditorUtilities.GetIcon("row_spacing");
            tooltip = "Uniform row spacing";
        }

        protected override string GetWindowTitle()
        {
            return "Row Spacing";
        }

        protected override void OnSpacingValueApply(float spacing)
        {
            var rects = EditorUtilities.GetSelectedRects()
                .OrderByDescending(r => r.localPosition.y)
                .ThenBy(r => r.GetSiblingIndex())
                .ToArray();
            if (rects.Length < 2)
                return;

            Undo.RecordObjects(rects.Cast<Object>().ToArray(), "Uniform Row Spacing");

            var currentY = rects[0].localPosition.y;

            foreach (var rt in rects)
            {
                var height = rt.rect.height;
                var pos = rt.localPosition;
                pos.y = currentY;
                rt.localPosition = pos;
                currentY -= height + spacing;
            }
        }
    }
}