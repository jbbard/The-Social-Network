using Unity.Entities;

namespace Dummy.Archetypes
{
    public static class Archetypes
    {
        public static EntityArchetype s_character;
        public static EntityArchetype s_player;
        public static EntityArchetype s_location;

        private static readonly ComponentType[] c_player =
        {
            ComponentType.ReadWrite<Components.Condition.Energy>(),
            ComponentType.ReadWrite<Components.Condition.Libido>(),

            ComponentType.ReadWrite<Components.Skills.EnergyMax>(),
            ComponentType.ReadWrite<Components.Skills.LibidoMax>(),
            ComponentType.ReadWrite<Components.Skills.Strength>(),
        };

        private static readonly ComponentType[] c_character =
        {
            ComponentType.ReadWrite<Components.Condition.Energy>(),
            ComponentType.ReadWrite<Components.Condition.Libido>(),

            ComponentType.ReadWrite<Components.Skills.EnergyMax>(),
            ComponentType.ReadWrite<Components.Skills.LibidoMax>(),
            ComponentType.ReadWrite<Components.Skills.Strength>(),

            ComponentType.ReadWrite<Components.Relationship.Friendship>(),
            ComponentType.ReadWrite<Components.Relationship.Love>(),
        };

        private static readonly ComponentType[] c_location =
        {
            ComponentType.ReadWrite<Components.Base.Name>(),

            ComponentType.ReadWrite<Components.Location.Occupant>(),
            ComponentType.ReadWrite<Components.Location.Owners>(),
        };


        public static void Initialize(EntityManager entityManager)
        {
            s_player = entityManager.CreateArchetype(c_player);
            s_character = entityManager.CreateArchetype(c_character);
            s_location = entityManager.CreateArchetype(c_location);
        }
    }
}