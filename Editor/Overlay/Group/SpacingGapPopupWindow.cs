using System;
using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Group
{
    public class SpacingGapPopupWindow : PopupWindowContent
    {
        private static int sGapValue;
        
        private readonly string m_InputLabel;
        private readonly Action<float> m_Callback;

        public SpacingGapPopupWindow(string label, Action<float> callback)
        {
            m_InputLabel = label;
            m_Callback = callback;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(150, 60);
        }

        public override void OnGUI(Rect rect)
        {
            this.BeginCustomLabelWidth(80);
            sGapValue = EditorGUILayout.IntField(m_InputLabel, sGapValue);
            this.EndCustomLabelWidth();

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply"))
            {
                m_Callback?.Invoke(sGapValue);
                editorWindow.Close();
            }
        }
    }

    public static class PopupWindowContentExtensions
    {
        private static float sOldWidth = -1;

        public static void BeginCustomLabelWidth(this PopupWindowContent content, float width)
        {
            sOldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }

        public static void EndCustomLabelWidth(this PopupWindowContent content)
        {
            if (sOldWidth >= 0)
            {
                EditorGUIUtility.labelWidth = sOldWidth;
                sOldWidth = -1;
            }
        }
    }
}