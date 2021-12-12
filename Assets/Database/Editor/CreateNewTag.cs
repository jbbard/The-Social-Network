// Third Party
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Database.Editor
{
    internal class CreateNewTag : OdinEditorWindow
    {
        [OnValueChanged("OnNameChange")]
        public string displayName;

        [ReadOnly]
        public string id;

        [EnableIf("IsValidTag"), Button]
        private void Create()
        {
            DatabaseEditorTools.CreateAndAddTag(displayName, id);
            Close();
        }

        public static void ShowWindow()
        {
            GetWindow<CreateNewTag>().Show();
        }

        private bool IsValidTag()
        {
            return !string.IsNullOrEmpty(displayName) && !Database.ContainTag(id);
        }

        private void OnNameChange()
        {
            id = displayName.ToLower().Replace(" ", "_");
        }
    }
}