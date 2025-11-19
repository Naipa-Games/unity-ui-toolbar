using System.Linq;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class UniformColumnSpacingButton : UniformSpacingButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UniformColumnSpacingButton);

        public UniformColumnSpacingButton()
        {
            icon = EditorUtilities.GetIcon("column_spacing");
            tooltip = "Uniform column spacing";
        }

        protected override string GetWindowTitle()
        {
            return "Col Spacing";
        }

        protected override void OnSpacingValueApply(float spacing)
        {
            var rects = EditorUtilities.GetSelectedRects()
                .OrderBy(r => r.localPosition.x)
                .ThenBy(r => r.GetSiblingIndex())
                .ToArray();
            if (rects.Length < 2)
                return;

            Undo.RecordObjects(rects.Cast<Object>().ToArray(), "Uniform Column Spacing");

            var first = rects[0];
            var prevRight = first.localPosition.x + first.rect.width * (1f - first.pivot.x);

            for (var i = 1; i < rects.Length; i++)
            {
                var rt = rects[i];
                var pos = rt.localPosition;

                var desiredLeft = prevRight + spacing;
                pos.x = desiredLeft + rt.rect.width * rt.pivot.x;
                rt.localPosition = pos;

                prevRight = pos.x + rt.rect.width * (1f - rt.pivot.x);
            }
        }
    }
}