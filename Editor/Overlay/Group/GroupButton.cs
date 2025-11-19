using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class GroupButton : EditorToolbarButton
    {
        internal const string ID = SceneUIAlignmentToolbar.ID + "." + nameof(GroupButton);

        public GroupButton()
        {
            icon = EditorUtilities.GetIcon("group");
            tooltip = "Group selected objects";
            clicked += OnClick;
        }

        private static void OnClick()
        {
#if UNITY_2021_3_OR_NEWER
            EditorApplication.ExecuteMenuItem("GameObject/Create Empty Parent");
#else
            var selection = Selection.gameObjects;
            if (selection.Length == 0)
            {
                return;
            }

            var group = new GameObject("New Group");
            Undo.RegisterCreatedObjectUndo(group, "Create Group");
            group.transform.SetParent(selection[0].transform.parent, false);

            var center = selection.Aggregate(Vector3.zero, (current, obj) => current + obj.transform.position);
            center /= selection.Length;
            group.transform.position = center;

            foreach (var obj in selection)
            {
                Undo.SetTransformParent(obj.transform, group.transform, "Group Objects");
            }

            Selection.activeGameObject = group;
            EditorGUIUtility.PingObject(group);
            InternalEditorUtility.SetIsInspectorExpanded(group, true);
#endif
        }
    }
}