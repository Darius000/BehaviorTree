using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace AIBehaviorTree
{
    internal class ElementUtilities
    {
		public static Button CreateButton(string name, Action action = null, Color32 color = default(Color32))
		{
			var button = new Button(action) { text = name, style = { backgroundColor = new StyleColor(color) } };
			return button;
		}

		public static Toolbar CreateToolBar(string name, float height, VisualElement[] toolBarElements = null)
        {
            var toolbar = new Toolbar { name = name };
            toolbar.style.minHeight = height;

            if (toolBarElements != null)
            {
                foreach (VisualElement element in toolBarElements)
                {
                    element.style.minHeight = height;
                    toolbar.Add(element);
                }
            }

            return toolbar;
        }

        public static TabView CreateTabView(string name, Tab[] tabs)
        {
            var tabbedView = new TabView() { name = name , style = { flexGrow = 1 } };
            foreach(var tab in tabs)
            {
                tabbedView.AddTab(tab, false);  
            }
            if(tabs.Length > 0) tabbedView.Activate(tabs[0]);

            return tabbedView;
        }

        public static Tab CreateTab(string name, VisualElement child)
        {
            return new Tab(name, child);
        }


        public static  Label CreateLabel(string text, Color32 color)
        {
            return new Label { text = text, name = text , style = { backgroundColor = new StyleColor(color)} };
        }
    }
}
