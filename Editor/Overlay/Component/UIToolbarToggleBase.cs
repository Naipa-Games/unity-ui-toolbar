using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    public abstract class UIToolbarToggleBase : EditorToolbarToggle, IUIToolbarToggle
    {
        private static UIToolbarToggleBase sCurrent;

        protected UIToolbarToggleBase()
        {
            this.RegisterValueChangedCallback(OnValueChanged);
        }

        ~UIToolbarToggleBase()
        {
            this.UnregisterValueChangedCallback(OnValueChanged);
        }

        public abstract GameObject GetPlacementObject();

        public void SetValue(bool enabled)
        {
            SetValueWithoutNotify(enabled);
        }

        protected static GameObject CreateUGUIObject(string menuPath)
        {
            EditorApplication.ExecuteMenuItem($"GameObject/UI/{menuPath}");
            return Selection.activeGameObject;
        }

        protected static GameObject CreateUGUILegacyObject(string menuPath, bool useTextMeshPro = false)
        {
            if (useTextMeshPro && System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro") != null)
            {
                if (EditorApplication.ExecuteMenuItem($"GameObject/UI/{menuPath} - TextMeshPro"))
                    return Selection.activeGameObject;
            }

#if UNITY_2021_1_OR_NEWER
            EditorApplication.ExecuteMenuItem($"GameObject/UI/Legacy/{menuPath}");
#else
            EditorApplication.ExecuteMenuItem($"GameObject/UI/{menuPath}");
#endif
            return Selection.activeGameObject;
        }

        private void OnValueChanged(ChangeEvent<bool> evt)
        {
            UIToolbarToggleEditor.OnToggleChanged(this, evt.newValue);
        }
    }
}