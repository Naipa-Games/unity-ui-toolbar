using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow
    {
        private void HandleAddPrefabEvent(PrefabCategory category)
        {
            var evt = Event.current;
            var dropRect = new Rect(0, 0, position.width - 150, position.height - 35);

            if (!dropRect.Contains(evt.mousePosition))
                return;

            if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform)
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    CollectObject(obj, category);
                }
            }

            evt.Use();
        }

        private static void CollectObject(Object obj, PrefabCategory cat)
        {
            if (!obj) return;

            var path = AssetDatabase.GetAssetPath(obj);

            if (AssetDatabase.IsValidFolder(path))
            {
                CollectFromFolder(path, cat);
                return;
            }

            TryAddPrefab(obj as GameObject, cat);
        }

        private static void CollectFromFolder(string folderPath, PrefabCategory cat)
        {
            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                TryAddPrefab(prefab, cat);
            }
        }

        private static void TryAddPrefab(GameObject prefab, PrefabCategory cat)
        {
            if (!prefab)
                return;

            if (PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.NotAPrefab)
                return;

            if (cat.prefabs.Any(p => p.prefab == prefab))
                return;

            cat.prefabs.Add(new PrefabEntry
            {
                prefab = prefab,
            });
        }
    }
}