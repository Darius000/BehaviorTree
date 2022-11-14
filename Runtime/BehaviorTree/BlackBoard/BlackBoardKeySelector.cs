using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [System.Serializable]
    public class BlackBoardKeySelector 
    {
        [SerializeField]
        private string m_SelectedKeyName = "";

        ///Get the name of the selected key
        public string GetName() { return m_SelectedKeyName; }

        public virtual Type GetKeyType() { return null; }

        public bool IsValid()
        {
            return m_SelectedKeyName != null && m_SelectedKeyName != "" && m_SelectedKeyName != string.Empty;
        }
    }
}
