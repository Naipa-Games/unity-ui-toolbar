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

            var prev = rects[0];
            var prevBottom = prev.localPosition.y - prev.rect.height * prev.pivot.y;

            for (var i = 1; i < rects.Length; i++)
            {
                var rt = rects[i];
                var pos = rt.localPosition;

                var desiredTop = prevBottom - spacing;
                pos.y = desiredTop - rt.rect.height * (1 - rt.pivot.y);
                rt.localPosition = pos;

                prevBottom = pos.y - rt.rect.height * rt.pivot.y;
            }
        }
    }
}