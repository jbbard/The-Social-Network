// System
using System;
using System.Runtime.InteropServices;

// Unity
using Unity.Entities;

namespace Database
{
    [StructLayout(LayoutKind.Explicit)]
    public struct DataBaseID : IComponentData
    {
        [FieldOffset(0)]
        public Guid value;

        // Workaround to serialize Guid in SharedComponentData
        // Maybe fixed in upgrade of entity package
        [FieldOffset(0)]
        public Int64 _black;
        [FieldOffset(8)]
        public Int64 _magic;
        public DataBaseID(string _guid) : this()
        {
            value = Guid.Parse(_guid);
        }

        public static implicit operator DataBaseID(string _guid) => new DataBaseID(_guid);
        public static implicit operator DataBaseID(Guid _guid) => new DataBaseID { value = _guid };
        public static implicit operator string(DataBaseID _guid) => _guid.value.ToString();

        public T GetItem<T>() where T : Item
            => Database.GetItemById<T>(value);

        public bool HasDetail<T>() where T : Item
            => Database.GetItemById(value) is T;
    }
}