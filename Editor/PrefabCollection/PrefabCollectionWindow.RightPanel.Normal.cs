using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow
    {
        private const float kTableSpacing = 5f;

        private void DrawNormalPrefabs(PrefabCategory category)
        {
            var width = position.width - 170;
            var col = Mathf.Min(Mathf.FloorToInt(width / m_Config.cellSize), category.prefabs.Count);
            var colWidth = category.prefabs.Count == 1 ? m_Config.cellSize : (width - (col - 1) * kTableSpacing) / col;
            var index = 0;

            GUILayout.Space(kTableSpacing);

            while (index < category.prefabs.Count)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    for (var i = 0; i < col && index < category.prefabs.Count; i++, index++)
                    {
                        var entry = category.prefabs[index];

                        DrawNormalEntry(entry, colWidth);

                        if (i < col - 1)
                            GUILayout.Space(kTableSpacing);
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(kTableSpacing);
            }
        }

        private void DrawNormalEntry(PrefabEntry entry, float entrySize)
        {
            var offset = (m_Config.cellSize - entrySize) / 2f;
            var rect = GUILayoutUtility.GetRect(entrySize, entrySize + 15);
            var bgRect = new Rect(rect.x, rect.y, rect.width, rect.height - 15);
            var previewRect = new Rect(rect.x - offset, rect.y - offset, m_Config.cellSize, m_Config.cellSize);

            HandleEntryEvents(entry, bgRect, previewRect);

            if (Event.current.type != EventType.Repaint)
                return;

            var textRect = new Rect(rect.x + 5, rect.y + entrySize + 2, entrySize - 10, 10);
            DrawLabel(textRect, entrySize, entry.Name);
        }
    }
}