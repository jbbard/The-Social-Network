using Unity.Entities;

namespace Dummy.Components
{
    public interface IUpgradable : IComponentData {}

    namespace Base
    {
        public class Name : IComponentData
        {
            public string value;
        }
    }

    // Stats relative to player
    namespace Relationship
    {
        public struct Friendship : IComponentData
        {
            public int value;
        }

        public struct Love : IComponentData
        {
            public int value;
        }
    }

    namespace Condition
    {
        // sexual performance/desire
        public struct Libido : IComponentData
        {
            public int value;
        }

        public struct Energy : IComponentData
        {
            public int value;
        }
    }

    namespace Skills
    {
        public struct LibidoMax : IComponentData, IUpgradable
        {
            public int value;
        }

        public struct EnergyMax : IComponentData, IUpgradable
        {
            public int value;
        }

        public struct Strength : IComponentData, IUpgradable
        {
            public int value;
        }
    }

    namespace Location
    {
        public struct Resident : IBufferElementData
        {
            public Entity entity;
        }

        public struct Occupant : IBufferElementData
        {
            public Entity entity;
        }
    }

    // TODO Will be converted to database


    namespace Action
    {
        public enum E_ActionType
        {
            // 2 chars
            Talk,

            // Alone
            WatchPorn,
            Rest,
            Sleep,
        }

        public struct Action
        {
            public E_ActionType type;

            // Maybe multiple later
            public Entity author;
            public Entity target;
        }
    }
}