using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

namespace AIBehaviorTree
{

    public class AIListToolbarMenu : ToolbarMenu
    {
        public Func<BehaviorTree> GetBehaviorTree { get; set; }


        public Action<AIController> OnAISelectedEvent;


        public AIListToolbarMenu(Func<BehaviorTree> GetBehaviorTree)
        {
            this.GetBehaviorTree = GetBehaviorTree;

            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            if(EditorApplication.isPlaying)
            {
                PopulateList();
            }
        }

        ~AIListToolbarMenu()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnPlayModeChanged(PlayModeStateChange evt)
        {
            if(evt == PlayModeStateChange.EnteredEditMode)
            {
                ClearList();

            }
            
            if(evt == PlayModeStateChange.EnteredPlayMode)
            {
                PopulateList();
            }

            Debug.Log("Play Mode Changed to " + evt);
        }

        /// <summary>
        /// finds all the associated ai in the scene
        /// </summary>
        /// <param name="type"></param>
        public void PopulateList()
        {
            var type = GetBehaviorTree?.Invoke().GetType();
            var ai = GameObject.FindObjectsOfType<AIController>();


            foreach (var component in ai)
            {
                if (component.Tree.GetType() != type) continue;

                menu.AppendAction(component.gameObject.name, SelectGameObjectFromDropDown,
                    (DropdownMenuAction) => DropdownMenuAction.Status.Normal, component);
            }
        }

        /// <summary>
        /// clears the active ai list in the scene
        /// </summary>
        public void ClearList()
        {
            menu.MenuItems().Clear();
        }

        private void SelectGameObjectFromDropDown(DropdownMenuAction action)
        {
            var component = action.userData as AIController;

            Debug.Log(component.gameObject.name);

            if (OnAISelectedEvent != null)
            {
                OnAISelectedEvent.Invoke(component);
            }
        }
    }
}