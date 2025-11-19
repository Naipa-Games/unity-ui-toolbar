using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class GridLayoutButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(GridLayoutButton);

        public GridLayoutButton()
        {
            icon = EditorUtilities.GetIcon("grid");
            tooltip = "Grid layout";
            clicked += OnClick;
        }

        private void OnClick()
        {
            PopupWindow.Show(this.GetScreenRect(), new GridLayoutPopupWindow(OnApplyLayout));
        }

        private static void OnApplyLayout(GridLayoutOptions options)
        {
            var rects = EditorUtilities.GetSelectedRects();
            if (rects.Count < 1)
                return;

            var highestRoot = FindHighestRoot(rects);
            if (highestRoot == null)
            {
                Debug.LogError("Cannot find highest root!");
                return;
            }

            var targetRects = rects
                .Where(rt => rt != null && rt.parent == highestRoot)
                .ToList();

            if (targetRects.Count < 1)
                return;

            Undo.RecordObjects(targetRects.Cast<Object>().ToArray(), "Grid Layout");

            var targetInfos = targetRects.Select(rt => new RectObjectInfo(rt)).ToList();
            targetInfos = targetInfos
                .OrderBy(o => o.Bounds.min.x)
                .ThenByDescending(o => o.Bounds.min.y)
                .ToList();

            var startX = targetInfos.Min(o => o.Bounds.min.x);
            var startY = targetInfos.Max(o => o.Bounds.min.y);

            var currentColumn = 0;
            var currentX = startX;
            var currentY = startY;
            var rowHeight = 0f;

            foreach (var target in targetInfos)
            {
                if (currentColumn >= options.Columns)
                {
                    currentColumn = 0;
                    currentX = startX;

                    currentY -= rowHeight + options.RowSpacing;
                    rowHeight = 0;
                }

                var bounds = target.Bounds;
                
                // Calculate current bounds center (relative to parent)
                var currentCenter = new Vector2(bounds.x + bounds.width * 0.5f, bounds.y - bounds.height * 0.5f);

                // Compute offset from bounds center to current pivot (localPosition)
                var pivotPos = new Vector2(target.Root.localPosition.x, target.Root.localPosition.y);
                var centerToPivotOffset = currentCenter - pivotPos;

                // Compute desired bounds center (target grid position)
                var desiredCenter = new Vector2(currentX + bounds.width * 0.5f, currentY - bounds.height * 0.5f);

                // New pivot localPosition = desired center - offset
                var newLocalPos2D = desiredCenter - centerToPivotOffset;

                target.Root.localPosition = new Vector3(
                    newLocalPos2D.x,
                    newLocalPos2D.y,
                    target.Root.localPosition.z
                );

                currentX += bounds.width + options.ColumnSpacing;
                rowHeight = Mathf.Max(rowHeight, bounds.height);
                currentColumn++;
            }
        }

        private static RectTransform FindHighestRoot(List<RectTransform> objs)
        {
            var allParents = objs.Select(GetParentChain).ToList();

            var minLength = allParents.Min(list => list.Count);

            RectTransform highest = null;

            for (var i = 0; i < minLength; i++)
            {
                var candidate = allParents[0][i];
                if (allParents.All(list => list[i] == candidate))
                    highest = candidate;
                else
                    break;
            }

            return highest;
        }

        private static List<RectTransform> GetParentChain(RectTransform rt)
        {
            var list = new List<RectTransform>();
            while (rt != null)
            {
                list.Insert(0, rt);
                rt = rt.parent as RectTransform;
            }

            return list;
        }

        private class RectObjectInfo
        {
            public readonly RectTransform Root;
            public Rect Bounds;

            public RectObjectInfo(RectTransform root)
            {
                Root = root;
                Bounds = GetRectInParentSpace(root.parent as RectTransform, root);
            }

            private static Rect GetRectInParentSpace(RectTransform root, RectTransform target)
            {
                var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(root, target);
                Vector2 size = bounds.size;
                var center = bounds.center;
                var x = center.x - size.x * 0.5f;
                var y = center.y + size.y * 0.5f;
                return new Rect(x, y, size.x, size.y);
            }
        }
    }
}