namespace EDataStructures
{
    using System;
    using System.Collections.Generic;

    public class MinHeap<T> : Heap<T> where T : IComparable
    {
        public MinHeap()
            : base(Comparer<T>.Default)
        { }

        public MinHeap(Comparer<T> comparer)
            : base(comparer)
        { }

        public MinHeap(IEnumerable<T> items)
            : base(items)
        { }

        public MinHeap(IEnumerable<T> items, Comparer<T> comparer)
            : base(items, comparer)
        { }

        protected override bool Dominates(int first, int second)
        {
            return this.Comparer.Compare(this.ElementAt(first), this.ElementAt(second)) < 0;
        }
    }

}
