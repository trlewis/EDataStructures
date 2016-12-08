namespace EDataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An abstract implementation of a priority queue. Whether it's a min heap or max heap or
    /// any other sort of heap is implemented in the inheriting class by defining the Dominates method.
    /// The bulk of this object came from Ohad Scneider on his StackOverflow response: http://stackoverflow.com/a/13776636
    /// </summary>
    /// <typeparam name="T">What type of object the heap contains. must be IComparable</typeparam>
    public abstract class Heap<T> where T : IComparable
    {
        private const string HeapEmptyMessage = "Heap is empty";
        private const int InitialSize = 33;

        private T[] _array = new T[InitialSize];
        private int _capacity = InitialSize;
        private int _tail;

        // ----------------------------
        // PROPERTIES
        // ----------------------------

        /// <summary>
        /// How many items are currently in the heap
        /// </summary>
        public int Count { get { return this._tail; } }

        /// <summary>
        /// Whether or not the heap has any items (if count is 1 or greater)
        /// </summary>
        public bool HasItems { get { return this._tail > 0; } }

        protected Comparer<T> Comparer { get; private set; }

        // ----------------------------
        // CONSTRUCTORS
        // ----------------------------

        protected Heap() : this(Comparer<T>.Default)
        { }

        protected Heap(Comparer<T> comparer) : this(Enumerable.Empty<T>(), comparer)
        { }

        protected Heap(IEnumerable<T> items) : this(items, Comparer<T>.Default)
        { }

        protected Heap(IEnumerable<T> items, Comparer<T> comparer)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.Comparer = comparer;
            this.AddMany(items);
        }

        // ----------------------------
        // ABSTRACT METHODS
        // ----------------------------

        /// <summary>
        /// Tests whether the item at location first should be closer to the top
        /// of the heap then the item at location second
        /// </summary>
        /// <param name="first">The location in the array of the first item to check.</param>
        /// <param name="second">The location in the array of the second item to check.</param>
        /// <returns>Whether the item at first should be higher on the heap than the item at second</returns>
        protected abstract bool Dominates(int first, int second);

        // ----------------------------
        // METHODS
        // ----------------------------

        /// <summary>
        /// Adds a single item to the heap. Rebalances heap.
        /// </summary>
        public void Add(T item)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            if (item == null)
                throw new ArgumentNullException("item");

            if (this._tail >= this._capacity)
                this.Grow();
            this._array[this._tail] = item;
            this._tail++;
            this.BubbleUp(this._tail - 1);
        }

        /// <summary>
        /// Adds the items to the heap one at a time. Making sure the heap is sorted
        /// properly
        /// </summary>
        /// <param name="items">The items to add to the heap.</param>
        public void AddMany(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (T item in items)
                this.Add(item);
        }

        /// <summary>
        /// Gets the root node without removing it from the heap.
        /// </summary>
        public T PeekRoot()
        {
            if (this._tail <= 0)
                throw new InvalidOperationException(HeapEmptyMessage);
            return this._array[0];
        }

        /// <summary>
        /// Removes the root item and returns it. Also moves the next item to the top.
        /// </summary>
        public T RemoveRoot()
        {
            if (this._tail <= 0)
                throw new InvalidOperationException(HeapEmptyMessage);

            T returnVal = this._array[0];
            this._array[0] = this._array[this._tail - 1];
            this._array[this._tail - 1] = returnVal;
            this._tail--;

            this.BubbleDown(0);
            return returnVal;
        }

        /// <summary>
        /// This is meant to be used for inheriting classes to get items at certain positions
        /// in the array. It exists so that inheriting classes cannot modify the array, only
        /// look at the contents.
        /// </summary>
        /// <param name="loc">The index of the item to get</param>
        protected T ElementAt(int loc)
        {
            return this._array[loc];
        }

        /// <summary>
        /// Given the location of the "parent node" this will try to swap the parent with the
        /// smallest child
        /// </summary>
        /// <param name="parentLoc">the location of the parent node</param>
        private void BubbleDown(int parentLoc)
        {
            while (!this.IsLeaf(parentLoc))
            {
                int minChild = this.LeftChild(parentLoc);
                if (minChild < this._tail - 1 && this.Dominates(this.RightChild(parentLoc), minChild))
                    minChild = this.RightChild(parentLoc);

                if (this.Dominates(parentLoc, minChild))
                    return;

                this.Swap(parentLoc, minChild);
                parentLoc = minChild;
            }
        }

        /// <summary>
        /// Tries to move the node at the index given up towards the top of the heap until
        /// it can be moved no further
        /// </summary>
        private void BubbleUp(int loc)
        {
            while (loc != 0 && this.Dominates(loc, this.Parent(loc)))
            {
                this.Swap(this.Parent(loc), loc);
                loc = this.Parent(loc);
            }
        }

        /// <summary>
        /// Doubles the capacity of the underlying array.
        /// </summary>
        private void Grow()
        {
            var newArr = new T[this._capacity * 2];
            for (int i = 0; i < this._capacity; i++)
                newArr[i] = this._array[i];
            this._array = newArr;
            this._capacity *= 2;
        }

        /// <summary>
        /// If the node at the index given is a leaf node or an internal node. Returns true
        /// if the node has no children.
        /// </summary>
        private bool IsLeaf(int loc)
        {
            return loc > (this._tail / 2) - 1;
        }

        /// <summary>
        /// Gets the index of the left child of the given parent index
        /// </summary>
        private int LeftChild(int parentLoc)
        {
            return parentLoc * 2 + 1;
        }

        /// <summary>
        /// Gets the index of the given child node index
        /// </summary>
        private int Parent(int childLoc)
        {
            return (childLoc - 1) / 2;
        }

        /// <summary>
        /// Gets the index of the right child of the given parent index
        /// </summary>
        private int RightChild(int parentLoc)
        {
            return parentLoc * 2 + 2;
        }

        /// <summary>
        /// Swaps the nodes at the given indices
        /// </summary>
        private void Swap(int a, int b)
        {
            T temp = this._array[a];
            this._array[a] = this._array[b];
            this._array[b] = temp;
        }
    }
}
