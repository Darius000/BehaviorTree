using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace AIBehaviorTree
{
    public class BehaviorTreeToolbar : Toolbar
    {
        public StyleColor ButtonStyleColor = new StyleColor(new Color(.2f, .2f, .6f));

        public Button SaveButton { get;}

        public Button MiniMapButton { get;}

        public AIListToolbarMenu AIListMenu { get; }

        public BehaviorTreeToolbar(Func<BehaviorTree> GetBehaviorTree)
        {
            AIListMenu = new AIListToolbarMenu(GetBehaviorTree) { name = "Current Scene AI", text = "Current Scene AI" };

            //menu bar buttons
            var blackboardButton = new Button() { text = "New Blackboard", style = { backgroundColor = ButtonStyleColor } };
            blackboardButton.clicked += () => { BlackBoardEditor.OpenWindow(); };


            var taskButton = new Button() { text = "New Task", style = { backgroundColor = ButtonStyleColor } };
            taskButton.clicked += BehaviorTreeUtilities.CreateBTTaskNode;

            var decoratorButton = new Button() { text = "New Decorator" };
            decoratorButton.clicked += BehaviorTreeUtilities.CreateDecoratorNode;

            MiniMapButton = new Button() { text = "MiniMap" };

            SaveButton = new Button() { text = "Save" };

            name = "Behavior Tree ToolBar";
            style.minHeight = 10.0f;
            Add(AIListMenu);
            Add(blackboardButton);
            Add(taskButton);
            Add(decoratorButton);
            Add(MiniMapButton);
            Add(SaveButton);
        }
    }
}
