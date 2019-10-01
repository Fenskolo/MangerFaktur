using System;
using System.ComponentModel.Design;

namespace ManagerFaktur
{
    public class SymbolCollectionEditor : CollectionEditor
    {
        public SymbolCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            Symbol item = new Symbol();
            item = (Symbol)value;

            return base.GetDisplayText(string.Format("{0}, {1}", item.FirstString, item.LastString));
        }
    }
}
