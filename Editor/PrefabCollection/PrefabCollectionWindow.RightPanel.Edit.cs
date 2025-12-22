using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow
    {
        private void DrawEditPrefabs(PrefabCategory category)
        {
            m_RemovedEntries.Clear();

            for (var i = 0; i < category.prefabs.Count; i++)
            {
                DrawEditPrefabEntry(category, i, category.prefabs[i]);
            }

            if (m_RemovedEntries.Count <= 0)
                return;

            foreach (var entry in m_RemovedEntries)
            {
                category.prefabs.Remove(entry);
            }

            EditorUtility.SetDirty(m_Config);
            AssetDatabase.SaveAssets();
        }
        
        private void DrawEditPrefabEntry(PrefabCategory category, int index, PrefabEntry entry)
        {
            EditorGUILayout.BeginHorizontal("box");

            var rect = GUILayoutUtility.GetRect(m_Config.cellSize, m_Config.cellSize,
                GUILayout.Width(m_Config.cellSize),
                GUILayout.Height(m_Config.cellSize));

            HandleEntryEvents(entry, rect, rect);
            DrawEditData(category, index, entry);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawEditData(PrefabCategory category, int index, PrefabEntry entry)
        {
            EditorGUILayout.BeginVertical();
            {
                var originalLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 50;

                EditorGUI.BeginChangeCheck();
                var displayName = EditorGUILayout.TextField("Name", string.IsNullOrEmpty(entry.displayName)
                    ? entry.prefab.name
                    : entry.displayName);
                if (EditorGUI.EndChangeCheck())
                {
                    entry.displayName = displayName;
                    EditorUtility.SetDirty(m_Config);
                    AssetDatabase.SaveAssets();
                }

                entry.prefab =
                    (GameObject)EditorGUILayout.ObjectField("Prefab", entry.prefab, typeof(GameObject), false);

                EditorGUIUtility.labelWidth = originalLabelWidth;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.Width(20));
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_P4_DeletedLocal@2x"),
                        GUILayout.Height(20), GUILayout.Width(30)))
                {
                    m_RemovedEntries.Add(entry);
                }

                if (index > 0)
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("icon dropdown open@2x"),
                            GUILayout.Height(20), GUILayout.Width(30)))
                    {
                        category.MovePrefabUp(index);
                    }
                }

                if (index < category.prefabs.Count - 1)
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("icon dropdown@2x"),
                            GUILayout.Height(20), GUILayout.Width(30)))
                    {
                        category.MovePrefabDown(index);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}