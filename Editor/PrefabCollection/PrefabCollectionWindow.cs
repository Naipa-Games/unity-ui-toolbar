using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Naipa.UIToolbar.Editor.PrefabCollection
{
    public partial class PrefabCollectionWindow : EditorWindow
    {
        public static void Open(Action<GameObject> onSelected = null)
        {
            var window = GetWindow<PrefabCollectionWindow>("Prefab Collector");
            window.minSize = new Vector2(555, 400);
            window.m_OnSelected = onSelected;
            window.Show();
        }

        private Action<GameObject> m_OnSelected;
        private PreviewRenderer m_Renderer;
        private PrefabCollectionConfig m_Config;
        private string m_SearchText;
        private int m_SelectedIndex = -1;

        private void OnEnable()
        {
            wantsMouseMove = true;
            SceneView.duringSceneGui += OnSceneGUI;

            InitRenderer();
            InitConfigs();
        }

        private void InitRenderer()
        {
            m_Renderer = new PreviewRenderer();
        }

        private void InitConfigs()
        {
            if (m_Config != null)
                return;

            var configs = EditorUtilities.FindAllScriptableObjects<PrefabCollectionConfig>();
            if (configs is not { Count: > 0 })
                return;

            m_Config = configs[0];
            m_SelectedIndex = 0;
        }

        private void OnDisable()
        {
            m_Renderer.Dispose();
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.MouseMove)
                Repaint();
            
            DrawConfigGUI();

            if (m_Config)
            {
                DrawPrefabsGUI();
            }
            else
            {
                DrawCreateNewConfigGUI();
            }
        }

        private void DrawConfigGUI()
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Config:", GUILayout.Width(40));
                m_Config = EditorGUILayout.ObjectField(m_Config, typeof(PrefabCollectionConfig), false) as
                    PrefabCollectionConfig;

                m_SearchText = EditorGUILayout.TextField(m_SearchText, EditorStyles.toolbarSearchField,
                    GUILayout.MaxWidth(150));

                if (GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    CreateConfig();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPrefabsGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawLeftPanel();
            DrawRightPanel();
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawCreateNewConfigGUI()
        {
            EditorGUILayout.HelpBox("No Prefab Collection Config found. Create one to get started",
                MessageType.Warning);
        }

        private void CreateConfig()
        {
            var configDirectory = Path.Combine(GetEditorRootPath(), "Configs");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            var config = CreateInstance(typeof(PrefabCollectionConfig));
            var filePath = Path.Combine(configDirectory, "PrefabCollectionConfig.asset");
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

            AssetDatabase.CreateAsset(config, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;
            m_Config = (PrefabCollectionConfig)config;
        }

        private static string GetEditorRootPath()
        {
            var guids = AssetDatabase.FindAssets("PrefabCollectionWindow t:Script");
            if (guids.Length <= 0) return "Assets/Plugins/Naipa/Editor/Configs";

            var scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            var pluginRoot = Path.GetDirectoryName(scriptPath);

            while (pluginRoot != null && !pluginRoot.EndsWith("Editor"))
            {
                pluginRoot = Path.GetDirectoryName(pluginRoot);
            }

            return pluginRoot;
        }
    }
}