namespace Database
{
    public static class ItemExtension
    {
        public static System.Guid[] Id(this Item[] _items)
        {
            var ids = new System.Guid[_items.Length];
            for (var i = 0; i < _items.Length; i++)
                ids[i] = _items[i].Id;
            return ids;
        }

        public static T[] To<T>(this Item[] _items) where T : Item
        {
            var array = new T[_items.Length];
            for (var i = 0; i < _items.Length; i++)
                if (_items[i] != null)
                    array[i] = _items[i].To<T>();
            return array;
        }
    }
}