using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow
    {
        private void OnSceneGUI(SceneView sceneView)
        {
            var evt = Event.current;
            if (evt.type != EventType.DragPerform)
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            foreach (var item in DragAndDrop.objectReferences)
            {
                if (item != m_DragEntry.prefab)
                    continue;

                DragAndDrop.AcceptDrag();
                var instance = PrefabUtility.InstantiatePrefab(item as GameObject) as GameObject;
                EditorUtilities.PlaceSceneObject(sceneView, evt.mousePosition, instance);
                Selection.activeObject = instance;
                evt.Use();
                break;
            }
        }
    }
}