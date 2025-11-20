using System;
using Naipa.UIToolbar.Editor.Overlay.Component;
using Naipa.UIToolbar.Editor.Overlay.Component.Naipa.UIToolbar.Editor.Overlay.Alignment;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay
{
    [Overlay(typeof(SceneView), ID, DisplayTitle, "", true), Icon(IconName)]
    public class SceneUIComponentToolbar : ToolbarOverlay
    {
        /// <summary> The unique ID of this overlay. </summary>
        internal const string ID = "Naipa." + nameof(SceneUIComponentToolbar);

        /// <summary> The display title of this overlay. </summary>
        internal const string DisplayTitle = "UI Component Toolbar";

        /// <summary> The icon name of this overlay. </summary>
        internal const string IconName = "UnityEditor.SceneHierarchyWindow";

        /// <summary> The version of this overlay. </summary>
        internal static readonly Version Version = new(1, 0);

        /// <summary> Creates a new instance of <see cref="SceneUIComponentToolbar" />. </summary>
        private SceneUIComponentToolbar() : base(
            UIImageToggle.ID,
            UITextToggle.ID,
            UIInputFieldToggle.ID,
            UIButtonToggle.ID,
            UIToggleToolbar.ID,
            UISliderToggle.ID,
            UIScrollbarToggle.ID,
            UIScrollViewToggle.ID
        )
        {
        }
    }
}