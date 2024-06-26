using System;
using System.Collections.Generic;

namespace TreeCollections.Tree.ItemTree.EntityTree.EntityDefinition;

internal class AliasComparer<TItem, TName> : IEqualityComparer<TItem>
{
    private readonly Func<TItem, TName> getName;
    private readonly IEqualityComparer<TName> nameComparer;
           
    public AliasComparer(Func<TItem, TName> getName, IEqualityComparer<TName> nameComparer)
    {
            this.getName = getName;
            this.nameComparer = nameComparer;
        } 

    public bool Equals(TItem x, TItem y)
    {
            return nameComparer.Equals(getName(x), getName(y));
        }

    public int GetHashCode(TItem obj)
    {
            return nameComparer.GetHashCode(getName(obj));
        }
}