using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bootstrap : MonoBehaviour
{
    [Button]
    public static void LoadModDLLs() {
        Debug.Log("Load mod DLLs");

        var systems = new List<Type>();
        var assemblies = Directory.GetFiles(Application.streamingAssetsPath + "/Mods", "*.dll", SearchOption.AllDirectories);

        try {
            foreach (var dll in assemblies) {
                var assembly = Assembly.LoadFile(dll);
                Debug.Log($"Load {dll}");

                var types = assembly.GetTypes();
                foreach (var type in types) {
                    if (typeof(ComponentSystemBase).IsAssignableFrom(type))
                        systems.Add(type);
                }
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }

        Debug.Log("Add mod systems to world");
        for (int i = 0; i < systems.Count; i++)
        {
            Debug.Log($"{systems[i].Name} added");
            World.DefaultGameObjectInjectionWorld.CreateSystem(systems[i]);
        }
    }

    private void Start()
    {
        LoadModDLLs();
    }
}