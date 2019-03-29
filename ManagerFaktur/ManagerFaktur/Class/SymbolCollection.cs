using System.Collections;

namespace ManagerFaktur
{
    public class SymbolCollection : CollectionBase
    {
        public Symbol this[int index] {get => (Symbol)List[index]; }

        public void Add(Symbol emp)
        {
            List.Add(emp);
        }

        public void Remove(Symbol emp)
        {
            List.Remove(emp);
        }
    }    
}
