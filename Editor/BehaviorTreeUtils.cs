using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

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
    }
}
