using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class UngroupButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(UngroupButton);

        public UngroupButton()
        {
            icon = EditorUtilities.GetIcon("ungroup");
            tooltip = "Ungroup selected objects";
            clicked += UngroupSelected;
        }

        private static void UngroupSelected()
        {
            var selected = Selection.transforms;
            if (selected == null || selected.Length == 0) return;

            // Iterate over a copy of selected array to avoid issues when deleting
            var selectedCopy = new Transform[selected.Length];
            selected.CopyTo(selectedCopy, 0);

            // Collect all moved child objects
            var movedChildren = new List<GameObject>();

            foreach (var t in selectedCopy)
            {
                if (t == null) continue;

                // Skip targets with no children
                if (t.childCount == 0) continue;

                var parent = t.parent; // original parent
                var siblingIndex = t.GetSiblingIndex(); // keep insertion order

                // Move children up to target's parent
                var children = new Transform[t.childCount];
                for (var i = 0; i < t.childCount; i++)
                    children[i] = t.GetChild(i);

                if (PrefabUtility.IsPartOfPrefabInstance(t.gameObject))
                {
                    foreach (var child in children)
                    {
                        
                    }
                }
                else
                {
                    foreach (var child in children)
                    {
                        Undo.SetTransformParent(child, parent, "Ungroup Selected");
                        child.SetSiblingIndex(siblingIndex++);
                        movedChildren.Add(child.gameObject);
                    }

                    Undo.DestroyObjectImmediate(t.gameObject);
                }
            }

            Selection.objects = movedChildren.ToArray();
        }
    }
}