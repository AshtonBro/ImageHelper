using AshtonBro.Panels.Data;
using System.Collections.Generic;

namespace AshtonBro.Panels
{
    public class ViewModel
    {
        private readonly List<SimpleItem> _list = new SimpleItemList();

        public List<SimpleItem> Items
        {
            get
            {
                return _list;
            }
        }
    }
}
