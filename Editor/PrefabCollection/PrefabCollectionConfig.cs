using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    [CreateAssetMenu(fileName = "PrefabCollectionConfig",
        menuName = "Naipa/UIToolbar/Prefab Collection Config",
        order = 1)]
    public class PrefabCollectionConfig : ScriptableObject
    {
        public const float kMinCellSize = 60;
        public const float kMaxCellSize = 90;

        public float cellSize = kMinCellSize;
        public List<PrefabCategory> categories = new();

        public void OnEnable()
        {
            if (categories.Count == 0)
            {
                NewCategory();
            }
        }

        public void NewCategory()
        {
            categories.Add(new PrefabCategory
            {
                name = "New Category",
                color = new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    1f
                )
            });
        }

        public void Remove(int index)
        {
            if (index >= 0 && index < categories.Count)
            {
                categories.RemoveAt(index);
            }
        }
    }

    [Serializable]
    public class PrefabCategory
    {
        public string name;
        public Color color = Color.yellow;
        public List<PrefabEntry> prefabs = new();

        public void AddPrefab(GameObject prefab)
        {
            if (!prefab) return;

            prefabs.Add(new PrefabEntry
            {
                prefab = prefab,
                displayName = prefab.name
            });
        }

        public void MovePrefabUp(int index)
        {
            if (index <= 0 || index >= prefabs.Count) return;
            (prefabs[index - 1], prefabs[index]) = (prefabs[index], prefabs[index - 1]);
        }

        public void MovePrefabDown(int index)
        {
            if (index < 0 || index >= prefabs.Count - 1) return;
            (prefabs[index + 1], prefabs[index]) = (prefabs[index], prefabs[index + 1]);
        }
    }

    [Serializable]
    public class PrefabEntry
    {
        public GameObject prefab;
        public string displayName;
        public string Name => string.IsNullOrEmpty(displayName) ? prefab.name : displayName;
    }
}