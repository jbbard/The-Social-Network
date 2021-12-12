// Unity
using System;
using System.Collections.Generic;
using System.Linq;

// Third Party
using Sirenix.OdinInspector;

namespace Database
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class DatabaseItemDropdown : Attribute
    {
        private readonly Type m_type;
        private readonly string[] m_tags;

        protected DatabaseItemDropdown(Type _type, params string[] _tags)
        {
            m_type = _type;
            m_tags = _tags;
        }

#if UNITY_EDITOR
        private static IEnumerable<Item> FilterItems(Type _type, Item[] _items)
        {
            var result = Array.FindAll(_items, _i => _i.GetType() == _type);
            return result;
        }

        private IList<ValueDropdownItem<Item>> GetItems()
        {
            Database.Initialize();
            var items = Database.GetItems();
            return FilterItems(m_type, items)
                .Where(_i => _i.HasAllTags(m_tags))
                .Select(_x => new ValueDropdownItem<Item>(_x.name, _x))
                .OrderBy(_x => _x.Text)
                .ToList();
        }

        private static DatabaseItemDropdown GetAttribute(IEnumerable<Attribute> _properties)
        {
            foreach (var attribute in _properties)
            {
                if (attribute is DatabaseItemDropdown databaseItemDropdown)
                    return databaseItemDropdown;
            }
            return null;
        }
#endif
    }

    [IncludeMyAttributes]
    [ValueDropdown("@CitySimulation.GameResources.DataV2.DatabaseItemDropdown.GetAttribute($property.Attributes).GetItems()", IsUniqueList = true, DropdownWidth = 300)]
    public sealed class DatabaseUniqueItemDropdownAttribute : DatabaseItemDropdown
    {
        public DatabaseUniqueItemDropdownAttribute(Type _type, params string[] _tags) : base(_type, _tags)
        { }
    }

    [IncludeMyAttributes]
    [ValueDropdown("@CitySimulation.GameResources.DataV2.DatabaseItemDropdown.GetAttribute($property.Attributes).GetItems()", DropdownWidth = 300)]
    public sealed class DatabaseItemDropdownAttribute : DatabaseItemDropdown
    {
        public DatabaseItemDropdownAttribute(Type _type, params string[] _tags) : base(_type, _tags)
        { }
    }
}