using Dummy.Components;
using Unity.Entities;
using UnityEngine;

namespace Dummy.Systems
{
    // TODO convert to system group
    public class Upgradable : SystemBase
    {
        // NativeListEntityQuery
        private System.Collections.Generic.List<EntityQuery> m_queryList;

        protected override void OnCreate()
        {
            // Create queries for each type that implement
            m_queryList = new System.Collections.Generic.List<EntityQuery>();
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                    if (types[j].GetInterface(nameof(IUpgradable)) != null)
                        m_queryList.Add(GetEntityQuery(ComponentType.ReadWrite(types[j])));
            }

            // Other option
            // Create a IForEach
            // Generate Code for each type that implement the interface
        }

        protected override void OnUpdate()
        {
            
        }
    }
}