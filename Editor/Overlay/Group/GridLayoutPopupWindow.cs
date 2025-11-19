using System;
using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    public class GridLayoutOptions
    {
        public int Columns = 1;
        public int RowSpacing;
        public int ColumnSpacing;
    }

    public class GridLayoutPopupWindow : PopupWindowContent
    {
        private static readonly GridLayoutOptions kOptions = new();
        
        private readonly Action<GridLayoutOptions> m_OnApply;

        public GridLayoutPopupWindow(Action<GridLayoutOptions> callback)
        {
            m_OnApply = callback;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(150, 100);
        }

        public override void OnGUI(Rect rect)
        {
            this.BeginCustomLabelWidth(80);
            {
                kOptions.Columns = EditorGUILayout.IntField("Columns", kOptions.Columns);
                kOptions.RowSpacing = EditorGUILayout.IntField("Row Spacing", kOptions.RowSpacing);
                kOptions.ColumnSpacing = EditorGUILayout.IntField("Col Spacing", kOptions.ColumnSpacing);
            }
            this.EndCustomLabelWidth();

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply"))
            {
                m_OnApply?.Invoke(kOptions);
                editorWindow.Close();
            }
        }
    }
}