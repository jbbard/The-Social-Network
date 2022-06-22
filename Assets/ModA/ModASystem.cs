using Unity.Entities;
using UnityEngine;

namespace ModA
{
    public partial class ModASystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.LogError("ModA");
        }

        protected override void OnUpdate()
        {
        }
    }
}