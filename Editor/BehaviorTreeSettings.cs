using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace AIBehaviorTree
{
    //Settings asset
    class BehaviorTreeSettings : ScriptableObject
    {
        [SerializeField]
        internal VisualTreeAsset m_NodeUXML;

        [SerializeField]
        internal VisualTreeAsset m_PortUXML;

        [SerializeField]
        internal TextAsset m_ScriptTemplateTask;

        [SerializeField]
        internal TextAsset m_ScriptTemplateDecorator;

        [SerializeField]
        internal Texture2D m_BlackBoardKeyIcon;

        private static BehaviorTreeSettings FindSettings()
        {
            var guids = AssetDatabase.FindAssets("t:BehaviorTreeSettings");
            if(guids.Length > 0)
            {
                //Debug.LogWarning($"Found muliple settings, using the first");
            }

            if (guids.Length == 0) return null;

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<BehaviorTreeSettings>(path);
        }

        internal static BehaviorTreeSettings GetOrCreateSettings()
        {
            var settings = FindSettings();

            if(settings == null)
            {
                settings = ScriptableObject.CreateInstance<BehaviorTreeSettings>();
                settings.m_NodeUXML = Utils.BehaviorTreeUtils.FindAsset<VisualTreeAsset>("NodeView");
                settings.m_PortUXML = Utils.BehaviorTreeUtils.FindAsset<VisualTreeAsset>("PortView");
                settings.m_ScriptTemplateTask = Utils.BehaviorTreeUtils.FindAsset<TextAsset>("BTTask_Template");
                settings.m_ScriptTemplateDecorator = Utils.BehaviorTreeUtils.FindAsset<TextAsset>("BTDecorator_Template");
                settings.m_BlackBoardKeyIcon = Utils.BehaviorTreeUtils.FindAsset<Texture2D>("Capsule");
                AssetDatabase.CreateAsset(settings, "Assets");
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    class BehaviourTreeSettingsProvider : SettingsProvider
    {
        private SerializedObject m_Settings;

        class Styles
        {
            public static GUIContent nodeUXMLTemplatePath = new GUIContent("Node UXML");
            public static GUIContent portUXMLTemplatePath = new GUIContent("Port UXML");
            public static GUIContent taskNodeTemplate = new GUIContent("New Task Node Template");
            public static GUIContent decoratorNodeTemplate = new GUIContent("New Decorator Node Template");
            public static GUIContent blackBoardKeyIcon = new GUIContent("BlackBoard Key Icon");
        }

  
        public BehaviourTreeSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope) { }


        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_Settings = BehaviorTreeSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            
            EditorGUILayout.PropertyField(m_Settings.FindProperty("m_NodeUXML"), Styles.nodeUXMLTemplatePath);
            EditorGUILayout.PropertyField(m_Settings.FindProperty("m_PortUXML"), Styles.portUXMLTemplatePath);
            EditorGUILayout.PropertyField(m_Settings.FindProperty("m_ScriptTemplateTask"), Styles.taskNodeTemplate);
            EditorGUILayout.PropertyField(m_Settings.FindProperty("m_ScriptTemplateDecorator"), Styles.decoratorNodeTemplate);
            EditorGUILayout.PropertyField(m_Settings.FindProperty("m_BlackBoardKeyIcon"), Styles.blackBoardKeyIcon);

            m_Settings.ApplyModifiedPropertiesWithoutUndo();
        }

        //register provider
        [SettingsProvider]
        public static SettingsProvider CreateBehaviourTreeSettingsProvider()
        {
            var provider = new BehaviourTreeSettingsProvider("Project/BehaviorTreeSettings", SettingsScope.Project);

            provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
            return provider;
        }
    }
}