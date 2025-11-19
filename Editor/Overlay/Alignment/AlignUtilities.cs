using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Alignment
{
    public enum AlignMode
    {
        Left,
        Center,
        Right,
        Top,
        Middle,
        Bottom
    }

    internal static class AlignUtilities
    {
        public static void AlignSelectedRects(AlignMode mode)
        {
            var rects = EditorUtilities.GetSelectedRects();
            switch (rects.Count)
            {
                case 0:
                    return;
                case 1:
                    AlignToParent(rects[0], mode);
                    break;
                default:
                    AlignToFirstSelected(rects.ToArray(), mode);
                    break;
            }
        }

        /// <summary>
        /// Align a single RectTransform to its parent (handles arbitrary anchor configurations)
        /// </summary>
        private static void AlignToParent(RectTransform rect, AlignMode mode)
        {
            if (rect == null)
                return;

            var parent = rect.parent as RectTransform;
            if (parent == null)
                return;

            Undo.RecordObject(rect, "Align To Parent");

            var pW = parent.rect.width;
            var pH = parent.rect.height;
            var pLeft = -pW * parent.pivot.x;
            var pRight = pW * (1 - parent.pivot.x);
            var pTop = pH * (1 - parent.pivot.y);
            var pBottom = -pH * parent.pivot.y;

            // Get rect corners in parent local coordinates (world -> parent local)
            var worldCorners = new Vector3[4];
            rect.GetWorldCorners(worldCorners);
            var parentLocalCorners = new Vector3[4];
            for (var i = 0; i < 4; i++)
            {
                parentLocalCorners[i] = parent.InverseTransformPoint(worldCorners[i]);
            }

            // corners order: 0 = bottom-left, 1 = top-left, 2 = top-right, 3 = bottom-right
            var rectLeft = Mathf.Min(parentLocalCorners[0].x, parentLocalCorners[1].x);
            var rectRight = Mathf.Max(parentLocalCorners[2].x, parentLocalCorners[3].x);
            var rectTop = Mathf.Max(parentLocalCorners[1].y, parentLocalCorners[2].y);
            var rectBottom = Mathf.Min(parentLocalCorners[0].y, parentLocalCorners[3].y);
            var rectCenterX = (rectLeft + rectRight) * 0.5f;
            var rectCenterY = (rectTop + rectBottom) * 0.5f;

            // Calculate delta (in parent local space) = target edge position - current edge position
            var deltaX = 0f;
            var deltaY = 0f;
            switch (mode)
            {
                case AlignMode.Left:
                    deltaX = pLeft - rectLeft;
                    break;
                case AlignMode.Center:
                    deltaX = -rectCenterX;
                    break;
                case AlignMode.Right:
                    deltaX = pRight - rectRight;
                    break;
                case AlignMode.Top:
                    deltaY = pTop - rectTop;
                    break;
                case AlignMode.Middle:
                    deltaY = -rectCenterY;
                    break;
                case AlignMode.Bottom:
                    deltaY = pBottom - rectBottom;
                    break;
            }

            // Horizontal alignment: handle stretching (anchors.x)
            if (!Mathf.Approximately(deltaX, 0f))
            {
                var horizStretched = !Mathf.Approximately(rect.anchorMin.x, rect.anchorMax.x);
                if (horizStretched)
                {
                    // Horizontal stretch: shift both offsetMin.x and offsetMax.x without changing size
                    var oldOffsetMin = rect.offsetMin;
                    var oldOffsetMax = rect.offsetMax;
                    // deltaX is in parent local space; offsets share the same coordinate space
                    rect.offsetMin = new Vector2(oldOffsetMin.x + deltaX, oldOffsetMin.y);
                    rect.offsetMax = new Vector2(oldOffsetMax.x + deltaX, oldOffsetMax.y);
                }
                else
                {
                    // Non-stretched: modify anchoredPosition.x
                    var oldAP = rect.anchoredPosition;
                    rect.anchoredPosition = new Vector2(oldAP.x + deltaX, oldAP.y);
                }
            }

            // Vertical alignment: handle stretching (anchors.y)
            if (!Mathf.Approximately(deltaY, 0f))
            {
                var vertStretched = !Mathf.Approximately(rect.anchorMin.y, rect.anchorMax.y);
                if (vertStretched)
                {
                    var oldOffsetMin = rect.offsetMin;
                    var oldOffsetMax = rect.offsetMax;
                    rect.offsetMin = new Vector2(oldOffsetMin.x, oldOffsetMin.y + deltaY);
                    rect.offsetMax = new Vector2(oldOffsetMax.x, oldOffsetMax.y + deltaY);
                }
                else
                {
                    var oldAP = rect.anchoredPosition;
                    rect.anchoredPosition = new Vector2(oldAP.x, oldAP.y + deltaY);
                }
            }

            EditorUtility.SetDirty(rect);
        }

        /// <summary>
        /// Multi-selection alignment:
        /// Align all other selected RectTransforms relative to the first selected one.
        /// If the targets are under different parents, alignment is done visually via world-space conversion.
        /// </summary>
        private static void AlignToFirstSelected(RectTransform[] rects, AlignMode mode)
        {
            if (rects == null || rects.Length == 0)
                return;

            var baseRect = rects[0];
            var baseParent = baseRect.parent;
            if (baseParent == null)
            {
                Debug.LogWarning("Base rect has no parent.");
                return;
            }

            Undo.RecordObjects(rects.Cast<Object>().ToArray(), "Align To First Selected");

            var baseWorldCorners = new Vector3[4];
            baseRect.GetWorldCorners(baseWorldCorners);

            var baseParentLocalCorners = new Vector3[4];
            var baseParentRect = baseRect.parent as RectTransform;
            if (baseParentRect == null)
            {
                Debug.LogWarning("Base rect's parent is not a RectTransform.");
                return;
            }

            for (var i = 0; i < 4; i++)
            {
                baseParentLocalCorners[i] = baseParentRect.InverseTransformPoint(baseWorldCorners[i]);
            }

            var baseLeft = Mathf.Min(baseParentLocalCorners[0].x, baseParentLocalCorners[1].x);
            var baseRight = Mathf.Max(baseParentLocalCorners[2].x, baseParentLocalCorners[3].x);
            var baseTop = Mathf.Max(baseParentLocalCorners[1].y, baseParentLocalCorners[2].y);
            var baseBottom = Mathf.Min(baseParentLocalCorners[0].y, baseParentLocalCorners[3].y);
            var baseCenterX = (baseLeft + baseRight) * 0.5f;
            var baseCenterY = (baseTop + baseBottom) * 0.5f;

            for (var i = 1; i < rects.Length; i++)
            {
                var rect = rects[i];
                if (rect == null)
                    continue;

                var parent = rect.parent as RectTransform;
                if (parent == null)
                    continue;

                // Get current rect bounds in its parent's local space
                var worldCorners = new Vector3[4];
                rect.GetWorldCorners(worldCorners);

                var parentLocalCorners = new Vector3[4];
                for (var k = 0; k < 4; k++)
                    parentLocalCorners[k] = parent.InverseTransformPoint(worldCorners[k]);

                var rLeft = Mathf.Min(parentLocalCorners[0].x, parentLocalCorners[1].x);
                var rRight = Mathf.Max(parentLocalCorners[2].x, parentLocalCorners[3].x);
                var rTop = Mathf.Max(parentLocalCorners[1].y, parentLocalCorners[2].y);
                var rBottom = Mathf.Min(parentLocalCorners[0].y, parentLocalCorners[3].y);
                var rCenterX = (rLeft + rRight) * 0.5f;
                var rCenterY = (rTop + rBottom) * 0.5f;

                // Convert the baseRect reference coordinate (edge/center) from its parent space -> world -> target rect's parent local space
                Vector3 baseRefWorld;
                Vector3 targetCoordInParentLocal;

                switch (mode)
                {
                    case AlignMode.Left:
                        // baseLeft 是 baseParent 本地坐标 -> 转为 world -> 转为 rect.parent local
                        baseRefWorld = baseParentRect.TransformPoint(new Vector3(baseLeft, 0f, 0f));
                        targetCoordInParentLocal = parent.InverseTransformPoint(baseRefWorld);
                    {
                        var desiredLeft = targetCoordInParentLocal.x;
                        var deltaX = desiredLeft - rLeft;
                        // apply to rect same as AlignToParent logic
                        var horizStretched = !Mathf.Approximately(rect.anchorMin.x, rect.anchorMax.x);
                        if (horizStretched)
                        {
                            rect.offsetMin = new Vector2(rect.offsetMin.x + deltaX, rect.offsetMin.y);
                            rect.offsetMax = new Vector2(rect.offsetMax.x + deltaX, rect.offsetMax.y);
                        }
                        else
                        {
                            rect.anchoredPosition =
                                new Vector2(rect.anchoredPosition.x + deltaX, rect.anchoredPosition.y);
                        }
                    }
                        break;

                    case AlignMode.Center:
                        baseRefWorld = baseParentRect.TransformPoint(new Vector3(baseCenterX, 0f, 0f));
                        targetCoordInParentLocal = parent.InverseTransformPoint(baseRefWorld);
                    {
                        var desiredCenterX = targetCoordInParentLocal.x;
                        var deltaX = desiredCenterX - rCenterX;
                        var horizStretched = !Mathf.Approximately(rect.anchorMin.x, rect.anchorMax.x);
                        if (horizStretched)
                        {
                            rect.offsetMin = new Vector2(rect.offsetMin.x + deltaX, rect.offsetMin.y);
                            rect.offsetMax = new Vector2(rect.offsetMax.x + deltaX, rect.offsetMax.y);
                        }
                        else
                        {
                            rect.anchoredPosition =
                                new Vector2(rect.anchoredPosition.x + deltaX, rect.anchoredPosition.y);
                        }
                    }
                        break;

                    case AlignMode.Right:
                        baseRefWorld = baseParentRect.TransformPoint(new Vector3(baseRight, 0f, 0f));
                        targetCoordInParentLocal = parent.InverseTransformPoint(baseRefWorld);
                    {
                        var desiredRight = targetCoordInParentLocal.x;
                        var deltaX = desiredRight - rRight;
                        var horizStretched = !Mathf.Approximately(rect.anchorMin.x, rect.anchorMax.x);
                        if (horizStretched)
                        {
                            rect.offsetMin = new Vector2(rect.offsetMin.x + deltaX, rect.offsetMin.y);
                            rect.offsetMax = new Vector2(rect.offsetMax.x + deltaX, rect.offsetMax.y);
                        }
                        else
                        {
                            rect.anchoredPosition =
                                new Vector2(rect.anchoredPosition.x + deltaX, rect.anchoredPosition.y);
                        }
                    }
                        break;

                    case AlignMode.Top:
                        baseRefWorld = baseParentRect.TransformPoint(new Vector3(0f, baseTop, 0f));
                        targetCoordInParentLocal = parent.InverseTransformPoint(baseRefWorld);
                    {
                        var desiredTop = targetCoordInParentLocal.y;
                        var deltaY = desiredTop - rTop;
                        var vertStretched = !Mathf.Approximately(rect.anchorMin.y, rect.anchorMax.y);
                        if (vertStretched)
                        {
                            rect.offsetMin = new Vector2(rect.offsetMin.x, rect.offsetMin.y + deltaY);
                            rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y + deltaY);
                        }
                        else
                        {
                            rect.anchoredPosition =
                                new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + deltaY);
                        }
                    }
                        break;

                    case AlignMode.Middle:
                        baseRefWorld = baseParentRect.TransformPoint(new Vector3(0f, baseCenterY, 0f));
                        targetCoordInParentLocal = parent.InverseTransformPoint(baseRefWorld);
                    {
                        var desiredCenterY = targetCoordInParentLocal.y;
                        var deltaY = desiredCenterY - rCenterY;
                        var vertStretched = !Mathf.Approximately(rect.anchorMin.y, rect.anchorMax.y);
                        if (vertStretched)
                        {
                            rect.offsetMin = new Vector2(rect.offsetMin.x, rect.offsetMin.y + deltaY);
                            rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y + deltaY);
                        }
                        else
                        {
                            rect.anchoredPosition =
                                new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + deltaY);
                        }
                    }
                        break;

                    case AlignMode.Bottom:
                        baseRefWorld = baseParentRect.TransformPoint(new Vector3(0f, baseBottom, 0f));
                        targetCoordInParentLocal = parent.InverseTransformPoint(baseRefWorld);
                    {
                        var desiredBottom = targetCoordInParentLocal.y;
                        var deltaY = desiredBottom - rBottom;
                        var vertStretched = !Mathf.Approximately(rect.anchorMin.y, rect.anchorMax.y);
                        if (vertStretched)
                        {
                            rect.offsetMin = new Vector2(rect.offsetMin.x, rect.offsetMin.y + deltaY);
                            rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y + deltaY);
                        }
                        else
                        {
                            rect.anchoredPosition =
                                new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + deltaY);
                        }
                    }
                        break;
                }

                EditorUtility.SetDirty(rect);
            }
        }
    }
}