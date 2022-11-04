using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using UnityEngine.UIElements;

namespace AIBehaviorTree.Utils
{
    internal class BehaviorTreeUtils
    {
        //find an asset with given name
        internal static T FindAsset<T>(string name) where T : UnityEngine.Object
        {
           
            var guids = AssetDatabase.FindAssets(name);
            if (guids.Length == 0)
            {
                Debug.Log("Failed to find asset guid for : " + name);
                return null;
            }
            else
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                //Debug.Log("Found asset guid for" + name);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }

        internal static string FindAssetPath(string name)
        {
            var guids = AssetDatabase.FindAssets(name);
            if (guids.Length == 0)
            {
                Debug.Log("Failed to find asset guid for : " + name);
                return null;
            }
            else
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return path;
            }
        }

        /// <summary>
        /// Get display name attribute from given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetDisplayName(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length > 0)
            {
                var nameAttribute = attributes[0] as DisplayNameAttribute;
                return nameAttribute.DisplayName;
            }

            return type.Name;
        }

        /// <summary>
        /// get catgory attribute from given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetCategory(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(CategoryAttribute), true);
            if (attributes.Length > 0)
            {
                var attribute = (attributes[0] as CategoryAttribute);
                return attribute.Category;
            }

            return "";
        }


        internal static string GetIcon(Type type)
        {

            //find icon attribute and set node icon
            var iconattributes = type.GetCustomAttributes(typeof(NodeIconAttribute), true);
            if (iconattributes.Length > 0)
            {
                var iconAttribute = iconattributes[0] as NodeIconAttribute;
                return iconAttribute.IconPath;

                
            }

            return "";
        }
    }
}
