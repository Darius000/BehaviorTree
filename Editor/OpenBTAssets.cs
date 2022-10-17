using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace AIBehaviorTree
{
    public static class OpenBTAssets 
    {
        [OnOpenAssetAttribute(1)]
        public static bool OpenAssetWindow(int instanceID, int line)
        {
            if (Application.isPlaying) return false;

            BehaviorTree t = EditorUtility.InstanceIDToObject(instanceID) as BehaviorTree;

            if (t)
            {
                BehaviorTreeEditor.OpenWindowAsset(t);

                return true;
            }

            return false;
        }

        [OnOpenAssetAttribute(2)]
        public static bool OpenAsset(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID) as BlackBoard;

            if (asset == null) return false;

            BlackBoardEditor.OpenBlackBoardAsset(asset);

            return true;

        }
    }
}
