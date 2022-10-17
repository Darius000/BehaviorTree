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
    public class TabView : VisualElement
    {
        private List<Tab> m_Tabs = new List<Tab>();
        private Tab m_ActiveTab = null;
        private VisualElement m_Toolbar = null;

        public TabView()
        {

            m_Toolbar = new VisualElement()
            {
                name = "Tabs Container",
                style = {
                paddingBottom = 0,
                marginBottom = 0,
                alignItems = Align.FlexStart,
                flexDirection = FlexDirection.Row,
                borderBottomColor = new StyleColor(new Color32(110, 110, 110, 255)),
                borderBottomWidth = 1
            }
            };

            contentContainer = new VisualElement() { name = "CurrentTabContent" };
            contentContainer.style.flexGrow = 1;
            contentContainer.style.paddingBottom = 0;
            contentContainer.style.paddingLeft = 0;
            contentContainer.style.paddingRight = 0;
            contentContainer.style.paddingTop = 0;

            hierarchy.Add(m_Toolbar);
            hierarchy.Add(contentContainer);
        }

        public override VisualElement contentContainer { get; }


        public void Activate(Tab button)
        {
            if (m_ActiveTab != null) m_ActiveTab.Deselect();

            m_ActiveTab = button;
            contentContainer.Clear();
            contentContainer.Add(button.Target);

            button.Target.StretchToParentSize();
        }

        public void AddTab(Tab tabButton, bool activate)
        {
            tabButton.OnSelect += Activate;
            tabButton.tabIndex = m_Tabs.Count();
            m_Tabs.Add(tabButton);
            m_Toolbar.Add(tabButton);

            if (activate)
            {
                Activate(tabButton);
            }
        }

        public void RemoveTab(Tab tabButton)
        {
            m_Tabs.Remove(tabButton);
            m_Toolbar.Remove(tabButton);
        }

        public new class UxmlFactory : UxmlFactory<TabView, VisualElement.UxmlTraits> { }
    }
}