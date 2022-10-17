using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AIBehaviorTree
{
  
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        

        public EdgeConnectorListener()
        {
            
        }
       
        public void OnDrop(GraphView graphView, Edge edge)
        {
            //Debug.Log("Dropped Edge!");
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            //Debug.Log("Dropped outside port!");
          
        }       


    }
}
