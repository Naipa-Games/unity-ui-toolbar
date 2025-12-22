using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow
    {
        internal enum ZoomMode
        {
            Normal,
            Edit
        }
        
        private static readonly Color NormalBg = new(0f, 0f, 0f, 0.18f);
        private static readonly Color HoverBg = new(0.25f, 0.55f, 1f, 0.35f);

        private ZoomMode m_ZoomMode = ZoomMode.Normal;
        private Vector2 m_RightScroll;
        private PrefabEntry m_DragEntry;
        private readonly List<PrefabEntry> m_RemovedEntries = new();

        private void DrawRightPanel()
        {
            if (m_Config.categories.Count == 0)
            {
                DrawEmptyTips();
                return;
            }

            m_SelectedIndex = Mathf.Clamp(m_SelectedIndex, 0, m_Config.categories.Count - 1);
            var category = m_Config.categories[m_SelectedIndex];

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Category:", GUILayout.Width(60));
                    category.name = EditorGUILayout.TextField(category.name);
                    category.color = EditorGUILayout.ColorField(category.color, GUILayout.Width(60));

                    if (GUILayout.Button("Delete", EditorStyles.toolbarButton, GUILayout.Width(60)))
                    {
                        m_Config.Remove(m_SelectedIndex);
                        m_SelectedIndex = Mathf.Clamp(m_SelectedIndex, 0, m_Config.categories.Count - 1);
                    }
                }
                EditorGUILayout.EndHorizontal();

                DrawScaleSlider();

                m_RightScroll = EditorGUILayout.BeginScrollView(m_RightScroll);
                {
                    HandleAddPrefabEvent(category);

                    switch (m_ZoomMode)
                    {
                        case ZoomMode.Normal:
                            DrawNormalPrefabs(category);
                            break;
                        case ZoomMode.Edit:
                            DrawEditPrefabs(category);
                            break;
                        default:
                            Debug.LogError("Unknown Preview Mode: " + m_ZoomMode);
                            break;
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndScrollView();

                GUILayout.Box("Drag Prefabs or Folders to Add to Category", EditorStyles.helpBox);
            }
            EditorGUILayout.EndVertical();
        }

        private static void DrawEmptyTips()
        {
            GUILayout.Box("No Categories. Please add a category to get started.", EditorStyles.helpBox);
        }

        private void HandleEntryEvents(PrefabEntry entry, Rect rect, Rect previewRect)
        {
            var e = Event.current;
            var id = GUIUtility.GetControlID(FocusType.Passive);
            var hover = rect.Contains(e.mousePosition);

            switch (e.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    if (hover && e.button == 0)
                    {
                        GUIUtility.hotControl = id;
                        e.Use();
                        m_DragEntry = null;
                    }

                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.objectReferences = new Object[] { entry.prefab };
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        DragAndDrop.StartDrag(entry.prefab.name);
                        GUIUtility.hotControl = 0;
                        m_DragEntry = entry;
                        e.Use();
                    }

                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        e.Use();
                    }

                    break;

                case EventType.Repaint:
                    DrawBackground(rect, hover);
                    DrawPreview(previewRect, m_Renderer.GetPreview(entry.prefab));
                    break;
            }
        }

        private static void DrawLabel(Rect rect, float width, string text)
        {
            var maxSize = Mathf.CeilToInt(width / 8);
            if (text.Length > maxSize)
            {
                text = text[..(maxSize - 3)] + "...";
            }

            GUI.Label(rect, text, EditorStyles.centeredGreyMiniLabel);
        }

        private static void DrawBackground(Rect rect, bool hover)
        {
            EditorGUI.DrawRect(rect, hover ? HoverBg : NormalBg);
        }

        private static void DrawPreview(Rect rect, Texture tex)
        {
            if (tex)
                GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit);
            else
                GUI.Label(rect, "(No Preview)");
        }

        private void DrawScaleSlider()
        {
            var rect = GUILayoutUtility.GetLastRect();
            rect.x += rect.width - 90;
            rect.y = position.height - 25;
            rect.width = 80;

            EditorGUI.BeginChangeCheck();

            m_Config.cellSize = GUI.HorizontalSlider(rect, m_Config.cellSize,
                PrefabCollectionConfig.kMinCellSize,
                PrefabCollectionConfig.kMaxCellSize);

            if (EditorGUI.EndChangeCheck())
            {
                UpdateMode();
            }
        }

        private void UpdateMode()
        {
            const float max = PrefabCollectionConfig.kMaxCellSize - 3;
            m_ZoomMode = m_Config.cellSize > max ? ZoomMode.Edit : ZoomMode.Normal;
        }
    }
}