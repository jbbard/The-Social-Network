// System
using System.Collections.Generic;

// Unity
using UnityEngine;

namespace Database
{
    public partial class Database
    {
        public const string c_databaseItemFolderName = "Database";

        public static List<Tag> tags;
        public static List<Item> items;
        public static Dictionary<System.Guid, Item> itemDictionaryId;
        public static Dictionary<string, Item> itemDictionaryName;
        public static Dictionary<System.Type, List<Item>> itemDictionaryType;

        public static void Initialize()
        {
            if (items == null)
            {
                items = new List<Item>(Resources.LoadAll<Item>(c_databaseItemFolderName));
                itemDictionaryId = new Dictionary<System.Guid, Item>(items.Count);
                itemDictionaryName = new Dictionary<string, Item>(items.Count);
                itemDictionaryType = new Dictionary<System.Type, List<Item>>();
                foreach (var item in items)
                {
                    itemDictionaryId[item.Id] = item;
                    itemDictionaryName[item.name] = item;
                    if (!itemDictionaryType.ContainsKey(item.GetType()))
                        itemDictionaryType.Add(item.GetType(), new List<Item> { item });
                    else
                        itemDictionaryType[item.GetType()].Add(item);
                }
            }
            if (tags == null)
                tags = new List<Tag>(Resources.LoadAll<Tag>(c_databaseItemFolderName));
        }
    }
}