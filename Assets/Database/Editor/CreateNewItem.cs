// Unity
using System;
using System.Collections.Generic;
using System.Linq;

// Third Party
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Database.Editor
{
    internal class CreateNewItem : OdinEditorWindow
    {
        public string displayName;

        [ReadOnly]
        public string id;

        [ValueDropdown("GetItemTypes")]
        public Type type;

        [EnableIf("IsValidItem"), Button]
        private void Create()
        {
            DatabaseEditorTools.CreateAndAddItem(displayName, type, Guid.Parse(id));
            id = Guid.NewGuid().ToString();
        }

        public static void ShowWindow()
        {
            var window = GetWindow<CreateNewItem>();
            window.id = Guid.NewGuid().ToString();
            window.Show();
        }

        private bool IsValidItem()
        {
            return !string.IsNullOrEmpty(displayName) && !Database.ContainItem(displayName) && type != null;
        }

        private IList<ValueDropdownItem<Type>> GetItemTypes()
        {
            var list = new List<ValueDropdownItem<Type>>();
            foreach (var t in DatabaseEditorTools.GetItemTypes())
                list.Add(new ValueDropdownItem<Type>(t.Name, t));
            return list;
        }
    }
}