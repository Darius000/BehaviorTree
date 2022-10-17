using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace AIBehaviorTree
{
    public class Tab : VisualElement
    {
        public VisualElement visualRootElement { get; }

        private Color m_DefaultColor = new Color(.3f, .3f, .3f, 1.0f);

        private Color m_ActiveColor = new Color(.3f, .3f, 1.0f, 1.0f);

        public Tab()
        {

        }
        public Tab(string text, VisualElement target)
        {
            name = text;


            visualRootElement = new VisualElement()
            {
                name = "Tab Content",
                style =
            {

                paddingBottom = 0,
                paddingLeft = 5,
                paddingTop = 0,
                paddingRight = 5,
                borderTopWidth = 1,
                borderBottomWidth = 1,
                borderRightWidth = 1,
                borderLeftWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                flexGrow = 1,
                backgroundColor = new StyleColor(new Color32(40, 40, 40, 255))
            }
            };

            visualRootElement.Add(new Label(text) { name = "Tab Label" });
            visualRootElement.RegisterCallback<MouseDownEvent>(ExecuteDefaultActionAtTarget);

            Target = target;

            TargetId = text;

            hierarchy.Add(visualRootElement);

            SetBorderColor(m_DefaultColor);
        }

        private void SetBorderColor(Color color)
        {
            visualRootElement.style.borderBottomColor = color;
            visualRootElement.style.borderTopColor = color;
            visualRootElement.style.borderRightColor = color;
            visualRootElement.style.borderLeftColor = color;
        }

        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);

            if (evt.eventTypeId == MouseDownEvent.TypeId())
            {
                Select();
            }
        }

        public string TargetId { get; }
        public VisualElement Target { get; set; }

        public event Action<Tab> OnSelect;

        public void Deselect()
        {
            SetBorderColor(m_DefaultColor);
        }
        public void Select()
        {
            SetBorderColor(m_ActiveColor);
            OnSelect.Invoke(this);
        }
    }
}