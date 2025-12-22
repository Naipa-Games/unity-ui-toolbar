using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    namespace Naipa.UIToolbar.Editor.Overlay.Alignment
    {
        [EditorToolbarElement(ID, typeof(SceneView))]
        public class UIScrollViewToggle : EditorToolbarDropdownToggle, IUIToolbarToggle
        {
            internal const string ID = SceneUIComponentToolbar.ID + "." + nameof(UIScrollViewToggle);

            private const float kDefaultSpacing = 10f;
            private static readonly Vector2 kDefaultSize = new(240, 400);
            
            private enum Direction
            {
                Horizontal,
                Vertical,
                Grid
            }

            private Direction m_CurrentDirection = Direction.Horizontal;

            public UIScrollViewToggle()
            {
                icon = EditorUtilities.GetIcon("ui_list");
                tooltip = "Create Scroll View";
                this.RegisterValueChangedCallback(OnValueChanged);

                dropdownClicked += ShowDropdownMenu;
            }

            ~UIScrollViewToggle()
            {
                this.UnregisterValueChangedCallback(OnValueChanged);
                dropdownClicked -= ShowDropdownMenu;
            }

            private void ShowDropdownMenu()
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Grid Layout"), m_CurrentDirection == Direction.Grid,
                    () => SetDirection(Direction.Grid));
                menu.AddItem(new GUIContent("Horizontal Layout"), m_CurrentDirection == Direction.Horizontal,
                    () => SetDirection(Direction.Horizontal));
                menu.AddItem(new GUIContent("Vertical Layout"), m_CurrentDirection == Direction.Vertical,
                    () => SetDirection(Direction.Vertical));
                menu.ShowAsContext();
            }

            private void SetDirection(Direction dir)
            {
                m_CurrentDirection = dir;
            }

            private void OnValueChanged(ChangeEvent<bool> evt)
            {
                UIToolbarToggleEditor.OnToggleChanged(this, evt.newValue);
            }

            public void SetValue(bool enabled)
            {
                SetValueWithoutNotify(enabled);
            }

            public GameObject GetPlacementObject()
            {
                EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
                
                var go = Selection.activeGameObject;
                var rect = (RectTransform)go.transform;
                var scrollRect = go.GetComponent<ScrollRect>();
                var content = scrollRect.content;
                var sizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();

                switch (m_CurrentDirection)
                {
                    case Direction.Grid:
                        var grid = content.gameObject.AddComponent<GridLayoutGroup>();
                        grid.spacing = new Vector2(kDefaultSpacing, kDefaultSpacing);
                        rect.sizeDelta = kDefaultSize;
                        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                        break;
                    case Direction.Horizontal:
                        var horizontalLayout = content.gameObject.AddComponent<HorizontalLayoutGroup>();
                        horizontalLayout.spacing = kDefaultSpacing;
                        rect.sizeDelta = new Vector2(kDefaultSize.y, kDefaultSize.x - 100);
                        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                        break;
                    case Direction.Vertical:
                        var verticalLayout = content.gameObject.AddComponent<VerticalLayoutGroup>();
                        verticalLayout.spacing = kDefaultSpacing;
                        rect.sizeDelta = new Vector2(kDefaultSize.x - 100, kDefaultSize.y);
                        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                        break;
                }

                var template = new GameObject("Template");
                template.transform.SetParent(content.transform);
                template.AddComponent<Image>();

                return go;
            }
        }
    }
}