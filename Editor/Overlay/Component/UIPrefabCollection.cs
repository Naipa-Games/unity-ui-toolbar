using Naipa.UIToolbar.Editor.PrefabCollection;
using UnityEditor;
using UnityEditor.Toolbars;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class UIPrefabCollection : EditorToolbarButton
    {
        internal const string ID = SceneUIComponentToolbar.ID + "." + nameof(UIPrefabCollection);

        public UIPrefabCollection()
        {
            icon = EditorUtilities.GetIcon("ui_box");
            tooltip = "Open Prefab Collection Window";
            clicked += OnOpenPrefabCollectionWindow;
        }

        private static void OnOpenPrefabCollectionWindow()
        {
            PrefabCollectionWindow.Open();
        }
    }
}