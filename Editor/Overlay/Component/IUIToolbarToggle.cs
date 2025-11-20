using UnityEngine;

namespace Naipa.UIToolbar.Editor.Overlay.Component
{
    public interface IUIToolbarToggle
    {
        void SetValue(bool enabled);
        GameObject GetPlacementObject();
    }
}