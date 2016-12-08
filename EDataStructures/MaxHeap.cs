namespace EDataStructures
{
    using System;
    using System.Collections.Generic;

    public class MaxHeap<T> : Heap<T> where T : IComparable
    {
        public MaxHeap()
            : base(Comparer<T>.Default)
        { }

        public MaxHeap(Comparer<T> comparer)
            : base(comparer)
        { }

        public MaxHeap(IEnumerable<T> items)
            : base(items)
        { }

        public MaxHeap(IEnumerable<T> items, Comparer<T> comparer)
            : base(items, comparer)
        { }

        protected override bool Dominates(int first, int second)
        {
            return this.Comparer.Compare(this.ElementAt(first), this.ElementAt(second)) > 0;
        }
    }
}
