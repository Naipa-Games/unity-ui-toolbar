using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
                var target = GetPlacementParent();

                var mousePos = e.mousePosition;
                mousePos.y = sceneView.camera.pixelHeight - mousePos.y;

                var cam = sceneView.camera;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    target, mousePos, cam, out var localPoint);

                PlaceObjectAtPosition(target, localPoint);
                ClearPlacingMode();

                e.Use();
            }
        }

        private static RectTransform GetPlacementParent()
        {
            var selected = Selection.activeGameObject;
            if (selected != null && selected.GetComponent<RectTransform>() != null)
                return selected.transform as RectTransform;

            var canvases = Object.FindObjectsOfType<Canvas>();
            foreach (var c in canvases)
            {
                if (c.gameObject.activeInHierarchy && c.enabled)
                    return c.transform as RectTransform;
            }

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Undo.RegisterCreatedObjectUndo(canvasGo, "Create Canvas");
            return canvasGo.transform as RectTransform;
        }

        private static void PlaceObjectAtPosition(RectTransform parent, Vector3 localPos)
        {
            Debug.Log("Placing UI Object at position: " + localPos + " under parent: " + parent);

            var instance = sCurrentToggle.GetPlacementObject();
            if (instance == null)
            {
                Debug.LogWarning("Placement object is null.");
                return;
            }

            Undo.RegisterCreatedObjectUndo(instance, "Place UI Object");
            instance.transform.SetParent(parent);
            instance.transform.localPosition = localPos;
        }
    }
}