// System
using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEditor;
using UnityEngine;

// Third Party
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace Database.Editor
{
    internal class DatabaseWindow : OdinMenuEditorWindow
    {
        [MenuItem("Frog and Mouse/Database", priority = -10)]
        public static void OpenWindow()
        {
            DatabaseEditorTools.Initialize();
            var window = GetWindow<DatabaseWindow>();
            window.titleContent = new GUIContent("Database");
            window.minSize = new Vector2(1000, 700);
            window.WindowPadding = new Vector4(20, 20, 20, 20);
            window.Show();
        }

        private OdinMenuTree m_tree;

        protected override OdinMenuTree BuildMenuTree()
        {
            m_tree = new OdinMenuTree();
            DatabaseEditorTools.Initialize();
            UpdateMenuTree();
            return m_tree;
        }

        private string m_search;
        private List<int> m_typeSelected = new List<int>();
        private List<int> m_tagSelected = new List<int>();

        protected override void OnBeginDrawEditors()
        {
            DatabaseEditorTools.Initialize();
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                var types = DatabaseEditorTools.GetItemTypes().Select(_t => _t.Name).ToList();
                types.Insert(0, "None");
                if (SirenixEditorFields.Dropdown(new GUIContent(), m_typeSelected, types, false, GUILayout.Width(200)))
                {
                    if (m_typeSelected.Contains(0))
                        m_typeSelected.Clear();
                    UpdateMenuTree();
                }

                var tags = new List<string> { "Everything", "Nothing" };
                tags.AddRange(Database.GetTags().Select(_c => _c.displayName));
                if (SirenixEditorFields.Dropdown(new GUIContent(), m_tagSelected, tags, true, GUILayout.Width(200)))
                {
                    if (m_tagSelected.Contains(1))
                        m_tagSelected.Clear();
                    else if (m_tagSelected.Contains(0))
                    {
                        m_tagSelected.Clear();
                        for (var i = 0; i < Database.GetTags().Length; i++)
                            m_tagSelected.Add(i + 2);
                    }
                    UpdateMenuTree();
                }

                var s = SirenixEditorGUI.ToolbarSearchField(m_search);
                if (s != m_search)
                {
                    m_search = s.ToLower();
                    UpdateMenuTree();
                }

                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("Apply"))
                    AssetDatabase.SaveAssets();

                if (SirenixEditorGUI.ToolbarButton("Tag"))
                    DatabaseTag.OpenWindow();

                if (SirenixEditorGUI.ToolbarButton("Create New"))
                    CreateNewItem.ShowWindow();

                if (MenuTree != null
                    && MenuTree.Selection != null
                    && MenuTree.Selection.SelectedValue != null
                    && MenuTree.Selection.SelectedValue is Item item
                    && SirenixEditorGUI.ToolbarButton("Delete"))
                    DatabaseEditorTools.RemoveItem(item);
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void UpdateMenuTree()
        {
            m_tree.MenuItems.Clear();
            var ids = GetSelectedTags();
            var type = m_typeSelected.Count > 0 ? DatabaseEditorTools.GetItemTypes().ToArray()[m_typeSelected.First() - 1] : null;
            var items = Database.GetItems().ToList();
            items.Sort((_i1, _i2) => string.Compare(_i1.name, _i2.name, StringComparison.InvariantCulture));
            foreach (var item in items)
            {
                var valid = (string.IsNullOrEmpty(m_search) || item.name.ToLower().Contains(m_search))
                        && (ids.Length == 0 || item.HasAllTags(ids))
                        && (type == null || item.GetType() == type);

                if (valid)
                    m_tree.Add(item.name, item);
            }
        }

        private string[] GetSelectedTags()
        {
            var ids = new string[m_tagSelected.Count];

            for (var i = 0; i < m_tagSelected.Count; i++)
                ids[i] = Database.GetTags()[m_tagSelected[i] - 2].id;
            return ids;
        }
    }
}