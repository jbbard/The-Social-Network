// System
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;

// Unity
using UnityEditor;
using UnityEngine;

namespace Database.Editor
{
    public static class DatabaseEditorTools
    {
        private const string c_rootPath = "Assets/Resources";
        private const string c_databaseItemFolderPath = c_rootPath + "/" + Database.c_databaseItemFolderName;

        public static void Initialize()
        {
            if (!Directory.Exists(c_databaseItemFolderPath))
                AssetDatabase.CreateFolder(c_rootPath, Database.c_databaseItemFolderName);
            var items = new List<Item>(Resources.LoadAll<Item>(Database.c_databaseItemFolderName));
            var tags = new List<Tag>(Resources.LoadAll<Tag>(Database.c_databaseItemFolderName));
            if (Database.items == null || Database.items.Count != items.Count)
            {
                Database.items = items;
                Database.itemDictionaryId = new Dictionary<Guid, Item>(items.Count);
                Database.itemDictionaryName = new Dictionary<string, Item>(items.Count);
                Database.itemDictionaryType = new Dictionary<Type, List<Item>>();
                foreach (var item in items)
                {
                    Database.itemDictionaryId[item.Id] = item;
                    Database.itemDictionaryName[item.name] = item;
                    if (!Database.itemDictionaryType.ContainsKey(item.GetType()))
                        Database.itemDictionaryType.Add(item.GetType(), new List<Item> { item });
                    else
                        Database.itemDictionaryType[item.GetType()].Add(item);
                }
            }
            if (Database.tags == null || Database.tags.Count != tags.Count)
                Database.tags = tags;
        }

        public static T CreateAndAddItem<T>(string _displayName, Guid _id = default) where T : Item
        {
            if (_id == Guid.Empty)
                _id = Guid.NewGuid();

            var instance = ScriptableObject.CreateInstance<T>();
            instance.OnCreate(_id, _displayName);

            AddItem(instance, typeof(T).Name);
            return instance;
        }

        public static Item CreateAndAddItem(string _displayName, Type _type, Guid _id = default)
        {
            if (_id == Guid.Empty)
                _id = Guid.NewGuid();

            var instance = ScriptableObject.CreateInstance(_type) as Item;
            instance.OnCreate(_id, _displayName);

            AddItem(instance, _type.Name);
            return instance;
        }

        public static void AddItem(Item _item, string _type)
        {
            Initialize();
            Database.items.Add(_item);
            Database.itemDictionaryId.Add(_item.Id, _item);
            Database.itemDictionaryName.Add(_item.name, _item);
            if (!Database.itemDictionaryType.ContainsKey(_item.GetType()))
                Database.itemDictionaryType.Add(_item.GetType(), new List<Item> { _item });
            else
                Database.itemDictionaryType[_item.GetType()].Add(_item);

            var path = Path.Combine(c_databaseItemFolderPath, _type);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(_item, $"{c_databaseItemFolderPath}/{_type}/{_item.Id}.asset");
        }

        public static void RemoveItem(Item _item)
        {
            Initialize();
            Database.items.Remove(_item);
            Database.itemDictionaryId.Remove(_item.Id);
            Database.itemDictionaryName.Remove(_item.name);
            if (Database.itemDictionaryType.ContainsKey(_item.GetType()))
                Database.itemDictionaryType[_item.GetType()].Remove(_item);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_item));
        }

        public static Tag CreateAndAddTag(string _displayName, string _id)
        {
            var instance = ScriptableObject.CreateInstance<Tag>();
            instance.displayName = _displayName;
            instance.id = _id;
            AddTag(instance);
            return instance;
        }

        public static void AddTag(Tag _tag)
        {
            Initialize();
            Database.tags.Add(_tag);
            AssetDatabase.CreateAsset(_tag, $"{c_databaseItemFolderPath}/{_tag.id}.asset");
        }

        public static void RemoveTag(Tag _tag, bool _skipEditorConfirmation = false)
        {
            Initialize();
            if (!IsTagUsed(_tag)
                || _skipEditorConfirmation
                || EditorUtility.DisplayDialog(
                    "Delete tag",
                    "This tag is used in database items, are you sure to delete it?",
                    "Yes",
                    "No"))
            {
                foreach (var i in Database.items)
                    if (i.tags.Contains(_tag.id))
                        i.RemoveTag(_tag.id);
                Database.tags.Remove(_tag);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_tag));
            }
        }

        public static bool IsTagUsed(Tag _tag)
        {
            foreach (var i in Database.items)
                if (i.tags.Contains(_tag.id))
                    return true;
            return false;
        }

        public static IEnumerable<Type> GetItemTypes()
        {
            return System.Reflection.Assembly
                .GetAssembly(typeof(Item))
                .GetTypes()
                .Where(_t => _t.IsSubclassOf(typeof(Item)));
        }
    }
}