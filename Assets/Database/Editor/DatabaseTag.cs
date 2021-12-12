// Unity

using Database;
using UnityEngine;

// Third Party
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace Database.Editor
{
    internal class DatabaseTag : OdinMenuEditorWindow
    {
        public static void OpenWindow()
        {
            DatabaseEditorTools.Initialize();
            Database.Initialize();
            var window = GetWindow<DatabaseTag>();
            window.titleContent = new GUIContent("Tag");
            window.minSize = new Vector2(500, 300);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            DatabaseEditorTools.Initialize();
            foreach (var c in Database.GetTags())
                tree.Add(c.displayName, c);
            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("Create New"))
                    CreateNewTag.ShowWindow();

                if (MenuTree != null
                    && MenuTree.Selection != null
                    && MenuTree.Selection.SelectedValue != null
                    && MenuTree.Selection.SelectedValue is Tag item
                    && SirenixEditorGUI.ToolbarButton("Delete"))
                    DatabaseEditorTools.RemoveTag(item);
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}