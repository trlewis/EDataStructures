using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EDataStructuresTests
{
    using System.Collections.Generic;
    using System.Linq;
    using EDataStructures;

    [TestClass]
    public class MaxHeapTests
    {
        #region Content Order

        [TestMethod]
        public void InitializeOnly()
        {
            //whether the constructor that takes a list will operate correctly.
            var intList = new List<int> { 1, 3, -1, 5, -9, 10 };
            var heap = new MaxHeap<int>(intList);
            Assert.AreEqual(heap.RemoveRoot(), 10);
            Assert.AreEqual(heap.RemoveRoot(), 5);
            Assert.AreEqual(heap.RemoveRoot(), 3);
            Assert.AreEqual(heap.RemoveRoot(), 1);
            Assert.AreEqual(heap.RemoveRoot(), -1);
            Assert.AreEqual(heap.RemoveRoot(), -9);
        }

        [TestMethod]
        public void InitializeSomeAndAddGroup()
        {
            //add some with constructor, add another via group
            var intList = new List<int> { -1, 9, 3 };
            var heap = new MaxHeap<int>(intList);
            var moreInts = new List<int> { 0, -10, 2 };
            heap.AddMany(moreInts);
            Assert.AreEqual(heap.RemoveRoot(), 9);
            Assert.AreEqual(heap.RemoveRoot(), 3);
            Assert.AreEqual(heap.RemoveRoot(), 2);
            Assert.AreEqual(heap.RemoveRoot(), 0);
            Assert.AreEqual(heap.RemoveRoot(), -1);
            Assert.AreEqual(heap.RemoveRoot(), -10);
        }

        [TestMethod]
        public void InitializeSomeAndAddIndividually()
        {
            //initialize with a group, add more one at a time
            var intList = new List<int> { 2, 5, 3 };
            var heap = new MaxHeap<int>(intList);
            var moreInts = new List<int> { -20, 5, 8 };
            foreach (var i in moreInts)
                heap.Add(i);

            Assert.AreEqual(heap.RemoveRoot(), 8);
            Assert.AreEqual(heap.RemoveRoot(), 5);
            Assert.AreEqual(heap.RemoveRoot(), 5);
            Assert.AreEqual(heap.RemoveRoot(), 3);
            Assert.AreEqual(heap.RemoveRoot(), 2);
            Assert.AreEqual(heap.RemoveRoot(), -20);
        }

        [TestMethod]
        public void InitializeGroupRemoveSomeAddGroup()
        {
            var intList = new List<int> { -5, 3, -1, 8 };
            var heap = new MaxHeap<int>(intList);
            heap.RemoveRoot();
            heap.RemoveRoot(); //only (-1, -5) should remain

            var extraInts = new List<int> { 1, -22, 3, 5 };
            heap.AddMany(extraInts); // (5, 3, 1, -1, -5) 

            Assert.AreEqual(heap.RemoveRoot(), 5);
            Assert.AreEqual(heap.RemoveRoot(), 3);
            Assert.AreEqual(heap.RemoveRoot(), 1);
            Assert.AreEqual(heap.RemoveRoot(), -1);
            Assert.AreEqual(heap.RemoveRoot(), -5);
            Assert.AreEqual(heap.RemoveRoot(), -22);
        }

        [TestMethod]
        public void AddItemsInOrder()
        {
            //give values to the heap already in order
            var heap = new MaxHeap<int>();
            for (int i = 99; i >= 0; i--)
                heap.Add(i);
            for (int i = 99; i  >= 0; i--)
                Assert.AreEqual(i, heap.RemoveRoot());
        }

        [TestMethod]
        public void AddItemsInReverseOrder()
        {
            var heap = new MaxHeap<int>();
            for (int i = 0; i < 100; i++)
                heap.Add(i);
            for (int i = 99; i  >= 0; i--)
                Assert.AreEqual(i, heap.RemoveRoot());
        }

        #endregion Content Order

        #region HasItems Property

        [TestMethod]
        public void HasItemsAfterEmptied()
        {
            var intList = new List<int> { -33, 4, 1, -100 };
            var heap = new MaxHeap<int>(intList);
            for (int i = 0; i < 4; i++)
                heap.RemoveRoot();

            Assert.IsFalse(heap.HasItems);
        }

        [TestMethod]
        public void HasItemsInitializeEmpty()
        {
            var intList = new List<int>();
            var heap = new MaxHeap<int>(intList);
            Assert.IsFalse(heap.HasItems);
        }

        [TestMethod]
        public void HasItemsInitializedNonEmpty()
        {
            var intList = new List<int> { 2, 9, 4 };
            var heap = new MaxHeap<int>(intList);
            Assert.IsTrue(heap.HasItems);
        }

        [TestMethod]
        public void HasItemsInitializedEmptyAddGroup()
        {
            var heap = new MaxHeap<int>();
            var extraInts = new List<int> { 2, 1000, -80 };
            heap.AddMany(extraInts);
            Assert.IsTrue(heap.HasItems);
        }

        #endregion HasItems Property

        #region Count Property

        [TestMethod]
        public void CountInitializeEmpty()
        {
            var heap = new MaxHeap<int>();
            Assert.AreEqual(0, heap.Count);
        }

        [TestMethod]
        public void CountInitializeEmptyAddGroup()
        {
            var heap = new MaxHeap<int>();
            var intList = new List<int> { 1, 2, 3, -1 };
            heap.AddMany(intList);
            Assert.AreEqual(4, heap.Count);
        }

        [TestMethod]
        public void CountInitializeGroup()
        {
            var intList = new List<int> { -2, -1, 0 };
            var heap = new MaxHeap<int>(intList);
            Assert.AreEqual(3, heap.Count);
        }

        [TestMethod]
        public void CountInitializeRemoveAll()
        {
            var intList = new List<int> { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0 };
            var heap = new MaxHeap<int>(intList);
            for (int i = 0; i < 11; i++)
                heap.RemoveRoot();
            Assert.AreEqual(0, heap.Count);
        }

        [TestMethod]
        public void CountInitializeRemoveSome()
        {
            var intList = new List<int> { 4, 5, 8, 1, -9 };
            var heap = new MaxHeap<int>(intList); // (8, 5, 4, 1, -9)
            heap.RemoveRoot();
            heap.RemoveRoot();
            Assert.AreEqual(3, heap.Count);
        }

        #endregion Count Property

        #region PeekRoot

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Peek was allowed on empty heap")]
        public void PeekInitializeEmpty()
        {
            var heap = new MaxHeap<int>();
            heap.PeekRoot();
        }

        [TestMethod]
        public void PeekInitializeGroup()
        {
            var intList = new List<int> { 3, 4, 2, 9 };
            var heap = new MaxHeap<int>(intList);
            Assert.AreEqual(heap.PeekRoot(), 9);
            Assert.AreEqual(heap.PeekRoot(), 9);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Peek was allowed on empty heap")]
        public void PeekInitializeGroupThenEmpty()
        {
            var intList = new List<int> { 3, 9, -1, 20, 3 };
            var heap = new MaxHeap<int>(intList);
            for (int i = 0; i < 5; i++)
                heap.RemoveRoot();
            heap.PeekRoot();
        }

        [TestMethod]
        public void PeekInitializeEmptyAddGroup()
        {
            var heap = new MaxHeap<int>();
            var intList = new List<int> { 4, 8, -10, 2, 20, 15 };
            heap.AddMany(intList);
            Assert.AreEqual(heap.PeekRoot(), 20);
            Assert.AreEqual(heap.PeekRoot(), 20);
        }

        #endregion PeekRoot

        [TestMethod]
        public void InitializeManyItems()
        {
            // checks to see if it will expand to accomodate thousands of items
            var rand = new Random();
            var intList = new List<int>();
            const int count = 10000;
            for (int i = 0; i < count; i++)
                intList.Add(rand.Next(-count * 2, count * 2));
            var heap = new MaxHeap<int>(intList);
            Assert.AreEqual(count, heap.Count);
        }

        [TestMethod]
        public void InitializeEmptyAddManyItems()
        {
            const int count = 10000;
            var rand = new Random();
            var heap = new MaxHeap<int>();
            for (int i = 0; i < count; i++)
                heap.Add(rand.Next(-count * 2, count * 2));
            Assert.AreEqual(count, heap.Count);
        }

        private static List<int> GetRandomInts(int count, int floor, int ceiling)
        {
            var rand = new Random();
            var list = new List<int>();
            //make sure that all the elements aren't in order before returning
            var equal = true;
            while (equal)
            {
                for (int i = 0; i < count; i++)
                    list.Add(rand.Next(floor, ceiling));
                var ordered = list.OrderBy(i => i).ToList();
                equal = ordered.SequenceEqual(list);
            }
            return list;
        }
    }
}
