using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;

namespace AIBehaviorTree
{
    public class BlackBoardKeyView : VisualElement
    {

        public static Texture2D Icon { get; private set; } = BehaviorTreeSettings.GetOrCreateSettings().m_BlackBoardKeyIcon;

        public string Title { get; set; }

        public string Description { get; set; }

        public VisualElement m_KeyInfoContainer { get; private set; }


        public BlackBoardKeyView()
        {
            CreateView();
        }

        public BlackBoardKeyView(BlackBoardKey blackboardkey)
        {
            CreateView();
            Update(blackboardkey);
        }

        private Label CreateLabel(string text)
        {
            var label = new Label(text);
            label.name = "Name";
            label.style.flexGrow = 1;
            label.style.paddingBottom = 5f;
            label.style.paddingLeft = 5f;
            label.style.paddingRight = 5f;
            label.style.paddingTop = 5f;

            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.fontSize = 12;

            label.style.unityFontStyleAndWeight = FontStyle.Bold;

            return label;
        }

        private VisualElement CreateIcon(Texture2D texture)
        {
            var icon = new VisualElement();
            icon.name = "Icon";
            icon.style.backgroundImage = texture;
            icon.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
            icon.style.maxWidth = 50.0f;
            icon.style.flexGrow = 1;

            return icon;
        }

        public void Update(BlackBoardKey key)
        {
            var namelabel = m_KeyInfoContainer.Q<Label>("Name");
            if (namelabel != null) namelabel.text = key?.m_KeyName;

            this.tooltip = key?.m_Description;

            var iconlabel = this.Q<VisualElement>("Icon");
            iconlabel.style.unityBackgroundImageTintColor = (StyleColor)key?.GetKeyColor();

        }

        protected virtual void CreateView()
        {
            m_KeyInfoContainer = new VisualElement();
            m_KeyInfoContainer.style.flexDirection = FlexDirection.Row;
            m_KeyInfoContainer.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;

            var icon = CreateIcon(Icon);
            var label = CreateLabel("");


            m_KeyInfoContainer.Add(icon);
            m_KeyInfoContainer.Add(label);


            this.Add(m_KeyInfoContainer);

        }
    }
}
