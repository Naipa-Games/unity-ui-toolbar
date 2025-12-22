using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    public static class UIToolbarToggleEditor
    {
        private static IUIToolbarToggle sCurrentToggle;

        public static void OnToggleChanged(IUIToolbarToggle target, bool value)
        {
            if (sCurrentToggle != target)
            {
                sCurrentToggle?.SetValue(false);
                sCurrentToggle = target;
            }

            if (value)
            {
                StartPlacingMode();
            }
            else
            {
                StopPlacingMode();
            }

            SceneView.RepaintAll();
        }

        private static void StartPlacingMode()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void StopPlacingMode()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private static void ClearPlacingMode()
        {
            sCurrentToggle.SetValue(false);
            sCurrentToggle = null;
            StopPlacingMode();
            SceneView.RepaintAll();
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), MouseCursor.ArrowPlus);

            var e = Event.current;

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            {
                if (sCurrentToggle != null)
                {
                    ClearPlacingMode();
                    e.Use();
                    return;
                }
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                EditorUtilities.PlaceSceneObject(sceneView, e.mousePosition, sCurrentToggle?.GetPlacementObject());
                ClearPlacingMode();
                e.Use();
            }
        }
    }
}