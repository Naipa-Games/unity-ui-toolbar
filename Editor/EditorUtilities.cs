using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Naipa.UIToolbar.Editor
{
    internal static class EditorUtilities
    {
        internal static Texture2D GetIcon(string name)
        {
            return Resources.Load<Texture2D>($"Icons/{name}");
        }

        internal static void DrawTexture(Rect rect, Color color)
        {
            var oldColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = oldColor;
        }

        internal static List<RectTransform> GetSelectedRects()
        {
            var list = new List<RectTransform>();

            foreach (var obj in Selection.transforms)
            {
                if (obj is RectTransform rect)
                    list.Add(rect);
            }

            return list;
        }

        internal static List<T> FindAllScriptableObjects<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(asset => asset != null)
                .ToList();
        }

        internal static void PlaceSceneObject(SceneView sceneView, Vector2 mousePos, GameObject instance)
        {
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y;

            var cam = sceneView.camera;
            var target = GetPlacementParent();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                target, mousePos, cam, out var localPoint);

            PlaceObjectAtPosition(target, instance, localPoint);
        }

        private static void PlaceObjectAtPosition(RectTransform parent, GameObject instance, Vector3 localPos)
        {
            if (!instance)
            {
                Debug.LogWarning("Placement object is null.");
                return;
            }

            Undo.RegisterCreatedObjectUndo(instance, "Place UI Object");
            instance.transform.SetParent(parent);
            instance.transform.localPosition = localPos;
        }

        private static RectTransform GetPlacementParent()
        {
            var selected = Selection.activeGameObject;
            if (selected && selected.GetComponent<RectTransform>())
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
    }
}