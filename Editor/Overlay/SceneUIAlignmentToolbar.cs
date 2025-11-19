using System;
using Naipa.UIToolbar.Editor.Overlay.Alignment;
using Naipa.UIToolbar.Editor.Overlay.Group;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay
{
    [Overlay(typeof(SceneView), ID, DisplayTitle, "", true), Icon(IconName)]
    public class SceneUIAlignmentToolbar : ToolbarOverlay
    {
        /// <summary> The unique ID of this overlay. </summary>
        internal const string ID = "Naipa." + nameof(SceneUIAlignmentToolbar);

        /// <summary> The display title of this overlay. </summary>
        internal const string DisplayTitle = "UI Alignment Toolbar";

        /// <summary> The icon name of this overlay. </summary>
        internal const string IconName = "UnityEditor.SceneHierarchyWindow";

        /// <summary> The version of this overlay. </summary>
        internal static readonly Version Version = new(1, 0);

        /// <summary> Creates a new instance of <see cref="SceneUIAlignmentToolbar" />. </summary>
        private SceneUIAlignmentToolbar() : base(
            GroupButton.ID,
            UngroupButton.ID,
            AlignLeftButton.ID,
            AlignCenterButton.ID,
            AlignRightButton.ID,
            AlignTopButton.ID,
            AlignMiddleButton.ID,
            AlignBottomButton.ID,
            UniformRowSpacingButton.ID,
            UniformColumnSpacingButton.ID,
            GridLayoutButton.ID
        )
        {
        }
    }
}