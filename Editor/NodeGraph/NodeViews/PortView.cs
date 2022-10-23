using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;

namespace AIBehaviorTree
{
    public class PortView : Port
    {
        public int Index { get; set; }

        protected PortView(int index, Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) 
            : base(portOrientation, portDirection, portCapacity, type)
        {
            Index = index;
        }

        public static Port Create<TEdge>(int index , Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
        {
            return new PortView(index, orientation, direction, capacity, type) { };     
        }
    }
}
