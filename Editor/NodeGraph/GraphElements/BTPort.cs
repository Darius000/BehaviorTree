using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;
using UnityEditor;

namespace AIBehaviorTree
{
    public class BTPort : Port
    {
        protected BTPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            VisualTreeAsset visualTreeAsset = BehaviorTreeSettings.GetOrCreateSettings().m_PortUXML;
            visualTreeAsset?.CloneTree(this);

            this.styleSheets.Clear();
        }

        public override bool showInMiniMap => true;

        public static new BTPort Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            //DefaultEdgeConnectorListener listener = new Port.DefaultEdgeConnectorListener();
            BTPort port = new BTPort(orientation, direction, capacity, type)
            {
                //m_EdgeConnector = new EdgeConnector<TEdge>(listener)
            };
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }
    }
}
