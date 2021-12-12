// System
using System.Collections.Generic;

// Unity
using UnityEngine;

// Third Party
using Sirenix.OdinInspector;

namespace Database
{
    public abstract class Item : ScriptableObject
    {
        [ReadOnly, BoxGroup("General", order: -10000)]
        [SerializeField]
        private string id;

        [BoxGroup("General")]
        public new string name;

        [BoxGroup("General")]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowPaging = false, ShowItemCount = false)]
        [ValueDropdown("GetTags", IsUniqueList = true, DropdownWidth = 200)]
        public List<string> tags;

        private System.Guid m_guid;
        public System.Guid Id
        {
            get
            {
                if (m_guid == System.Guid.Empty)
                    m_guid = System.Guid.Parse(id);
                return m_guid;
            }
        }

        public string Name => name;

#if UNITY_EDITOR
        public virtual void OnCreate(System.Guid _guid, string _name)
        {
            id = _guid.ToString();
            name = _name;
            tags = new List<string>();
        }

        public void AddTag(string _id)
        {
            if (Database.ContainTag(_id))
                tags.Add(_id);
        }

        public void RemoveTag(string _id)
        {
            tags.Remove(_id);
        }

        private IList<ValueDropdownItem<string>> GetTags()
        {
            var list = new List<ValueDropdownItem<string>>();
            foreach (var c in Database.GetTags())
                list.Add(new ValueDropdownItem<string>(c.displayName, c.id));
            return list;
        }
#endif

        public T To<T>() where T : Item => this as T;

        public bool HasTag(string _tag)
        {
            if (string.IsNullOrWhiteSpace(_tag))
                return false;

            return tags.Contains(_tag);
        }

        public bool HasAllTags(params string[] _tags)
        {
            if (_tags == null || _tags.Length == 0)
                return true;

            foreach (var category in tags)
                if (System.Array.IndexOf(_tags, category) == -1)
                    return false;
            foreach (var i in _tags)
                if (!tags.Contains(i))
                    return false;
            return true;
        }

        public bool HasAnyTags(params string[] _tags)
        {
            if (_tags == null || _tags.Length == 0)
                return true;

            foreach (var category in tags)
                if (System.Array.IndexOf(_tags, category) == -1)
                    return false;
            foreach (var i in _tags)
                if (tags.Contains(i))
                    return true;
            return false;
        }
    }
}