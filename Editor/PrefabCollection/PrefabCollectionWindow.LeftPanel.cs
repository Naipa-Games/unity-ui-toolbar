using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow
    {
        private Vector2 m_LeftScroll;

        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(150f));
            m_LeftScroll = EditorGUILayout.BeginScrollView(m_LeftScroll);

            GUILayout.Label("Categories");

            for (var i = 0; i < m_Config.categories.Count; i++)
            {
                var category = m_Config.categories[i];
                var selected = i == m_SelectedIndex;
                var label = $"{category.name} ({category.prefabs.Count})";

                var rect = GUILayoutUtility.GetRect(
                    GUIContent.none,
                    EditorStyles.largeLabel,
                    GUILayout.Height(25),
                    GUILayout.ExpandWidth(true)
                );

                if (i == m_SelectedIndex)
                {
                    EditorGUI.DrawRect(rect, new Color(0.3f, 0.4f, 0.56f, 0.25f));
                }

                if (GUI.Toggle(rect, selected, label, EditorStyles.label))
                {
                    m_SelectedIndex = i;
                }

                rect = GUILayoutUtility.GetLastRect();
                rect.x -= 3;
                rect.width = 3;
                EditorUtilities.DrawTexture(rect, category.color);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("+ Add Category", EditorStyles.toolbarButton))
            {
                m_Config.NewCategory();
                m_SelectedIndex = m_Config.categories.Count - 1;
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}