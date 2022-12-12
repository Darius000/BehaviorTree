using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEditor;
using System;

namespace AIBehaviorTree
{
    [CreateAssetMenu(menuName = "BehaviorTree/BlackBoard")]
    public class BlackBoard : ScriptableObject
    {
        [SerializeField, SerializeReference]
        //[HideInInspector]
        protected List<BlackBoardKey> m_Keys = new List<BlackBoardKey>();

        public Action<List<BlackBoardKey>> OnKeysChangedEvent;

        public Action<BlackBoardKey> OnKeyAddedEvent;

        public Action<int> OnKeyRemovedEvent;

        public Action<int> OnBlackBoardKeyUpdated;

        public BlackBoard Clone()
        {
            var bb = Instantiate(this);
            bb.m_Keys = m_Keys.ConvertAll(c => c.Clone());
            return bb;
        }

        public void SwitchKeys(int oldIndex, int newIndex)
        {
            var key1 = m_Keys[oldIndex];
            var key2 = m_Keys[newIndex];

            m_Keys[oldIndex] = key2;
            m_Keys[newIndex] = key1;

            OnKeysChangedEvent?.Invoke(m_Keys);
        }

        public BlackBoardKey GetKey(int index)
        {
            if(index < 0 || index >= m_Keys.Count) return null;

            return m_Keys[index];
        }

        public void AddNewKey(BlackBoardKey key)
        {
            key.ID = System.Guid.NewGuid().ToString();
            key.m_KeyName = BlackBoardKey.GetDefaultName();

            m_Keys.Add(key);

            //AssetDatabase.AddObjectToAsset(key, this);
            //AssetDatabase.SaveAssetIfDirty(this);
 
            OnKeyAddedEvent?.Invoke(key);
        }

        public void RemoveKey(int index)
        {
            OnKeyRemovedEvent?.Invoke(index);

            var key = m_Keys[index];

            m_Keys.Remove(key);

            //AssetDatabase.RemoveObjectFromAsset(key);
            //AssetDatabase.SaveAssetIfDirty(this);

            
        }


        public T GetKeyValue<T>(string name)
        {
            if(name == "(None)") return default(T);

            var key = GetKey(name);

            if (key != null) return key.GetValue<T>();

            return default(T);
        }

        public void SetKeyValue<T>(string name, T value)
        {
            var key = GetKey(name);
            if (key != null) key.SetValue(value);
        }

        public BlackBoardKey GetKey(string name)
        {
            return m_Keys.Find(x => x.m_KeyName == name);
        }

        public bool ContainsKey(string name)
        {
            var key = m_Keys.FirstOrDefault(x => x.m_KeyName == name) ;
            //Debug.Log(key);
            return key is not null;
        }

        public List<BlackBoardKey> GetAllKeys() { return m_Keys; }

        public int GetKeyCount() {  return m_Keys.Count; }

        
        /// <summary>
        /// Draws the keys properties
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index"></param>
        public void UpdateBlackBoardObject(SerializedObject obj, int index)
        {
            obj.Update();

            var property = obj.FindProperty("m_Keys");
            var elementProperty = property.GetArrayElementAtIndex(index);

            if(elementProperty == null) return;

            if(EditorGUILayout.PropertyField(elementProperty, true))
            {
                obj.ApplyModifiedProperties();
                OnBlackBoardKeyUpdated?.Invoke(index);
            }
           
        }
    }
}

