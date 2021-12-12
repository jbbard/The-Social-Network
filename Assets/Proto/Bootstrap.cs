using Unity.Entities;
using UnityEngine;

using Dummy.Archetypes;
using U_Components = Dummy.Components;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Archetypes.Initialize(entityManager);

        var player = entityManager.CreateEntity(Archetypes.s_player);
        entityManager.SetComponentData(player, new U_Components.Skills.EnergyMax { value = 50 });
        entityManager.SetComponentData(player, new U_Components.Skills.LibidoMax { value = 10 });
        entityManager.SetComponentData(player, new U_Components.Skills.Strength { value = 0 });

        var blueHouse = entityManager.CreateEntity(Archetypes.s_location);
        entityManager.SetComponentData(blueHouse, new U_Components.Base.Name { value = "Blue House" });

        var whiteHouse = entityManager.CreateEntity(Archetypes.s_location);
        entityManager.SetComponentData(whiteHouse, new U_Components.Base.Name { value = "White House" });

        var street = entityManager.CreateEntity(Archetypes.s_location);
        entityManager.SetComponentData(street, new U_Components.Base.Name { value = "Street" });
    }
}