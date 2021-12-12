// System
using System.Collections.Generic;

namespace Database
{
    public partial class Database
    {
        public enum E_Search
        {
            All = 0,
            Any,
            None
        }

        public static Tag[] GetTags() => tags.ToArray();
        public static Item[] GetItems() => items.ToArray();

        public static bool ContainTag(string _id)
        {
            foreach (var t in tags)
                if (t.id == _id)
                    return true;
            return false;
        }

        public static bool ContainItem(System.Guid _id) => itemDictionaryId.ContainsKey(_id);

        public static bool ContainItem(string _name) => itemDictionaryName.ContainsKey(_name);

        public static Item GetItemById(System.Guid _id, params string[] _tags)
        {
            if (!ContainItem(_id))
                return null;
            var item = itemDictionaryId[_id];
            return item.HasAllTags(_tags) ? item : null;
        }

        public static Item GetItemByName(string _name, params string[] _tags)
        {
            if (!ContainItem(_name))
                return null;
            var item = itemDictionaryName[_name];
            return item.HasAllTags(_tags) ? item : null;
        }

        public static T GetItemByName<T>(string _name, params string[] _tags) where T : Item
        {
            var item = GetItemByName(_name, _tags);
            if (item != null && item is T i)
                return i;
            return null;
        }

        public static T GetItemById<T>(System.Guid _id, params string[] _tags) where T : Item
        {
            var item = GetItemById(_id, _tags);
            if (item != null && item is T i)
                return i;
            return null;
        }

        public static T[] GetItemByIds<T>(System.Guid[] _ids, params string[] _tags) where T : Item
        {
            var array = new T[_ids.Length];
            for (var i = 0; i < _ids.Length; i++)
                array[i] = GetItemById<T>(_ids[i], _tags);
            return array;
        }

        public static Item GetFirstItemByType(System.Type _type, params string[] _tags)
        {
            if (itemDictionaryType.ContainsKey(_type))
            {
                foreach (var item in itemDictionaryType[_type])
                    if (item.HasAllTags(_tags))
                        return item;
            }
            return null;
        }

        public static Item[] GetItemsByType(System.Type _type, params string[] _tags)
        {
            var list = new List<Item>();
            if (itemDictionaryType.ContainsKey(_type))
            {
                foreach (var item in itemDictionaryType[_type])
                    if (item.HasAllTags(_tags))
                        list.Add(item);
            }
            return list.ToArray();
        }

        public static T GetFirstItemByType<T>(params string[] _tags) where T : Item
        {
            if (itemDictionaryType.ContainsKey(typeof(T)))
            {
                foreach (var item in itemDictionaryType[typeof(T)])
                    if (item.HasAllTags(_tags))
                        return item as T;
            }
            return null;
        }

        public static T[] GetItemsByType<T>(E_Search _type = E_Search.All, params string[] _tags)
            where T : Item
        {
            var list = new List<T>();
            if (itemDictionaryType.ContainsKey(typeof(T)))
            {
                switch (_type)
                {
                    case E_Search.All:
                        foreach (var item in itemDictionaryType[typeof(T)])
                            if (item.HasAllTags(_tags))
                                list.Add(item as T);
                        break;
                    case E_Search.Any:
                        foreach (var item in itemDictionaryType[typeof(T)])
                            if (item.HasAnyTags(_tags))
                                list.Add(item as T);
                        break;
                    case E_Search.None:
                        foreach (var item in itemDictionaryType[typeof(T)])
                            if (!item.HasAnyTags(_tags))
                                list.Add(item as T);
                        break;
                }
            }
            list.Sort((T _item, T _other) => string.Compare(_item.name, _other.name));
            return list.ToArray();
        }
    }
}