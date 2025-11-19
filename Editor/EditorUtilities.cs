using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor
{
    internal static class EditorUtilities
    {
        internal static Texture2D GetIcon(string name)
        {
            return Resources.Load<Texture2D>($"Icons/{name}");
        }
        
        public static List<RectTransform> GetSelectedRects()
        {
            var list = new List<RectTransform>();

            foreach (var obj in Selection.transforms)
            {
                if (obj is RectTransform rect)
                    list.Add(rect);
            }

            return list;
        }
    }
}