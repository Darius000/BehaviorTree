using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace AIBehaviorTree
{
    [System.Serializable]
    public static class BehaviorTreeUtilities
    {
        [SerializeField]
        private static Texture2D scriptIcon = (EditorGUIUtility.IconContent("cs Script Icon").image) as Texture2D;

        [SerializeField]
        private static string m_LastCreationPath = "Assets";
       
        /// <summary>
        /// Creates a Condition Node c# File
        /// </summary>

        [MenuItem("Assets/Create/BehaviorTree/Create Behavior Nodes/Decorator C# Script", false, 89)]
        public static void CreateDecoratorNode()
        {
            string template_path = AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().m_ScriptTemplateDecorator);
            if (template_path.Length == 0)
            {
                Debug.LogWarning("Template not found in asset DataBase");
            }

            string name = ShowPathDialog("NewDecorator");
            CreateFromTemplate(name, template_path);
        }

        

        [MenuItem("Assets/Create/BehaviorTree/Create Behavior Nodes/BTTask C# Script", false, 89)]
        public static void CreateBTTaskNode()
        {
            string template_path = AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().m_ScriptTemplateTask);
            if (template_path.Length == 0)
            {
                Debug.LogWarning("Template not found in asset DataBase");
            }

            string name = ShowPathDialog("NewTask");
            CreateFromTemplate(name, template_path);
        }

        [MenuItem("Assets/Create/BehaviorTree/Create Behavior Nodes/Utility Action C# Script", false, 89)]
        public static void CreateUtilityActionNode()
        {
            string template_path = AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().m_ScriptTemplateAction);
            if (template_path.Length == 0)
            {
                Debug.LogWarning("Template not found in asset DataBase");
            }

            string name = ShowPathDialog("New Action");
            CreateFromTemplate(name, template_path);
        }

        [MenuItem("Assets/Create/BehaviorTree/Create Behavior Nodes/Utility Consideration C# Script", false, 89)]
        public static void CreateUtilityConsiderationNode()
        {
            string template_path = AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().m_ScriptTemplateConsideration);
            if (template_path.Length == 0)
            {
                Debug.LogWarning("Template not found in asset DataBase");
            }

            string name = ShowPathDialog("New Consideration");
            CreateFromTemplate(name, template_path);
        }

        private static string ShowPathDialog(string itemName)
        {
            var path = EditorUtility.SaveFilePanelInProject("Create New " + itemName, itemName, "cs", "", m_LastCreationPath);

            if (string.IsNullOrEmpty(path)) return null;

            m_LastCreationPath = Path.GetDirectoryName(path);

            Debug.Log(m_LastCreationPath);

            return path;
        }

        public static void CreateFromTemplate(string intiialName, string tempatePath)
        {
            if (string.IsNullOrEmpty(intiialName)) return;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateCodeFile>(), intiialName, scriptIcon, tempatePath);
        }

        //Inherits from EndNameAction, must ovverride EndNameAction.Action
        public class DoCreateCodeFile : UnityEditor.ProjectWindowCallback.EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                Object o = CreateScript(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        internal static UnityEngine.Object CreateScript(string pathName, string templatePath)
        {
            string className = Path.GetFileNameWithoutExtension(pathName).Replace(" ", string.Empty);

            string templateText = string.Empty;

            UTF8Encoding encoding = new UTF8Encoding(true, false);

            if(File.Exists(templatePath))
            {
                //read procedures
                StreamReader reader = new StreamReader(templatePath);
                templateText = reader.ReadToEnd();
                reader.Close();

                templateText = templateText.Replace("#SCRIPTNAME#", className);
                templateText = templateText.Replace("#NOTRIM#", string .Empty);

                //write procedures
                StreamWriter writer = new StreamWriter(Path.GetFullPath(pathName), false, encoding);
                writer.Write(templateText);
                writer.Close();

                AssetDatabase.ImportAsset(pathName);
                return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName);
            }
            else
            {
                Debug.LogError(string.Format("The Templare file was not found: {0}", templatePath));
                return null;
            }
        }
    }
}

