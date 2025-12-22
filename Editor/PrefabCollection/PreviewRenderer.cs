using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public class PreviewRenderer
    {
        private const int kPreviewSize = 256;
        private const int kPreviewLayer = 31;

        private Camera m_Camera;
        private Canvas m_Canvas;
        private RenderTexture m_RT;
        private readonly Dictionary<GameObject, Texture2D> m_Cache = new();

        public PreviewRenderer()
        {
            CreateEnvironment();
        }

        private void CreateEnvironment()
        {
            var camGo = new GameObject("UGUI_PreviewCamera")
            {
                hideFlags = HideFlags.HideAndDontSave,
                layer = kPreviewLayer
            };

            m_Camera = camGo.AddComponent<Camera>();
            m_Camera.orthographic = true;
            m_Camera.orthographicSize = kPreviewSize / 2f;
            m_Camera.clearFlags = CameraClearFlags.SolidColor;
            m_Camera.backgroundColor = Color.clear;
            m_Camera.cullingMask = 1 << kPreviewLayer;

            // RenderTexture
            m_RT = new RenderTexture(kPreviewSize, kPreviewSize, 16, RenderTextureFormat.ARGB32)
            {
                hideFlags = HideFlags.HideAndDontSave,
                useMipMap = false,
                autoGenerateMips = false
            };
            m_Camera.targetTexture = m_RT;

            var canvasGo = new GameObject("UGUI_PreviewCanvas")
            {
                hideFlags = HideFlags.HideAndDontSave,
                layer = kPreviewLayer
            };

            m_Canvas = canvasGo.AddComponent<Canvas>();
            m_Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_Canvas.worldCamera = m_Camera;
            m_Canvas.planeDistance = 100;

            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();
        }

        public void Dispose()
        {
            Object.DestroyImmediate(m_Camera.gameObject);
            Object.DestroyImmediate(m_Canvas.gameObject);
            m_RT.Release();
            Object.DestroyImmediate(m_RT);
        }

        public Texture2D GetPreview(GameObject prefab)
        {
            if (!prefab)
                return null;

            if (!prefab.TryGetComponent<RectTransform>(out _))
            {
                return AssetPreview.GetAssetPreview(prefab) ?? AssetPreview.GetMiniThumbnail(prefab);
            }

            if (m_Cache.TryGetValue(prefab, out var cached))
                return cached;

            var tex = Render(prefab);
            m_Cache[prefab] = tex;
            return tex;
        }

        private Texture2D Render(GameObject prefab)
        {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.hideFlags = HideFlags.HideAndDontSave;
            instance.transform.SetParent(m_Canvas.transform, false);
            SetLayerRecursively(instance.transform, kPreviewLayer);

            var rootRect = instance.GetComponent<RectTransform>();

            rootRect.anchorMin = rootRect.anchorMax = new Vector2(0.5f, 0.5f);
            rootRect.pivot = new Vector2(0.5f, 0.5f);
            rootRect.anchoredPosition = Vector2.zero;
            rootRect.localScale = Vector3.one;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);

            Canvas.ForceUpdateCanvases();

            var bounds = CalculateUIBounds(instance);

            if (bounds.size == Vector3.zero)
                bounds.size = Vector3.one * 100f;

            var width = bounds.size.x;
            var height = bounds.size.y;

            var scale = Mathf.Min(
                kPreviewSize / width,
                kPreviewSize / height
            );

            scale *= 0.9f;
            rootRect.localScale = Vector3.one * scale;

            rootRect.anchoredPosition = Vector2.zero;

            Canvas.ForceUpdateCanvases();

            m_Camera.Render();

            RenderTexture.active = m_RT;
            var tex = new Texture2D(kPreviewSize, kPreviewSize, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, kPreviewSize, kPreviewSize), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            Object.DestroyImmediate(instance);
            return tex;
        }

        private static Bounds CalculateUIBounds(GameObject root)
        {
            var graphics = root.GetComponentsInChildren<Graphic>(true);

            var hasBounds = false;
            var bounds = new Bounds();

            foreach (var g in graphics)
            {
                var rect = g.rectTransform;
                var corners = new Vector3[4];
                rect.GetWorldCorners(corners);

                for (var i = 0; i < 4; i++)
                {
                    if (!hasBounds)
                    {
                        bounds = new Bounds(corners[i], Vector3.zero);
                        hasBounds = true;
                    }
                    else
                    {
                        bounds.Encapsulate(corners[i]);
                    }
                }
            }

            return bounds;
        }

        private static void SetLayerRecursively(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (var i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i), layer);
        }
    }
}