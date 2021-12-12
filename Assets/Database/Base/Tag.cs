// Unity
using UnityEngine;

namespace Database
{
    public class Tag : ScriptableObject
    {
        [Sirenix.OdinInspector.ReadOnly]
        public string displayName;

        [Sirenix.OdinInspector.ReadOnly]
        public string id;
    }
}