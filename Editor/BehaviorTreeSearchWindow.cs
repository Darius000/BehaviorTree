using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AIBehaviorTree
{
    internal struct ListItem
    {
        public string displayName;
        public object userData;
    }
    //node search window for behaviour tree editor
    public class BehaviorTreeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private BehaviorTreeView m_BehaviourTreeView;
        private static Texture2D m_IndentationIcon;

        public void Initialize(BehaviorTreeView behaviourTreeView)
        {
            m_BehaviourTreeView = behaviourTreeView;

            if(m_IndentationIcon == null)
            {
                m_IndentationIcon = new Texture2D(1, 1);
                m_IndentationIcon.SetPixel(0, 0, Color.clear);
                m_IndentationIcon.Apply();
            }
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<ListItem> maingroup = new List<ListItem>();
            Dictionary<string, List<ListItem>> groups = new Dictionary<string, List<ListItem>>()
            {             
                { "Composites", new List<ListItem>() } ,
                { "Decorators", new List<ListItem>() },
                { "Tasks" , new List<ListItem>()}
            };

            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>() 
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
            };

           
            var types = TypeCache.GetTypesDerivedFrom<BTNode>();
            foreach (var type in types)
            {
                if (!type.IsAbstract)
                {
                    var displayName = type.Name;
                    var attributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                    if (attributes.Length > 0)
                    {
                        var nameAttribute = attributes[0] as DisplayNameAttribute;
                        displayName = nameAttribute.DisplayName;
                    }

                    var item = new ListItem() { displayName = displayName, userData = type };
                    if (type.IsSubclassOf(typeof(BTDecorator)))
                    {
                        groups["Decorators"].Add(item);
                    }
                    else if (type.IsSubclassOf(typeof(BTComposite)))
                    {
                        groups["Composites"].Add(item);
                    }
                    else if(type.IsSubclassOf(typeof(BTTaskNode)))
                    {
                        groups["Tasks"].Add(item);
                    }
                    else
                    {
                        maingroup.Add(item);
                    }

                }
            }

            foreach(var item in maingroup)
            {
                searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(item.displayName, m_IndentationIcon)) { level = 1, userData = item.userData });
            }

            foreach (var group in groups)
            {
                searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent(group.Key), 1));

                foreach(var list in group.Value)
                {
                    searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(list.displayName, m_IndentationIcon)) { level = 2, userData = list.userData});
                }
            }

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = m_BehaviourTreeView.GetLocalMousePosition(context.screenMousePosition, true);

            var nodeType = SearchTreeEntry.userData as Type;
            var node = m_BehaviourTreeView.CreateNode(nodeType, localMousePosition);
            if(node != null)
            {
                return true;
            }

            return false;
        }

        
    }
}
