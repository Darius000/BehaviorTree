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
        public AIListToolbarMenu()
        {

        }

        public Action<BehaviorTreeComponent> OnAISelectedEvent;



        /// <summary>
        /// finds all the associated ai in the scene
        /// </summary>
        /// <param name="type"></param>
        public void PopulateList(System.Type type)
        {
            var ai = GameObject.FindObjectsOfType<BehaviorTreeComponent>();


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
            var component = action.userData as BehaviorTreeComponent;

            Debug.Log(component.gameObject.name);

            if (OnAISelectedEvent != null)
            {
                OnAISelectedEvent.Invoke(component);
            }
        }
    }
}