using System;

namespace Code.Runtime.AI
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private readonly T[] _items;
        private int _count;
        public int Count => _count;

        public Heap(int maxSize)
        {
            _items = new T[maxSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = _count;
            _items[_count] = item;
            SortUp(item);
            _count++;
        }

        public T RemoveFirst()
        {
            T first = _items[0];
            _count--;
            _items[0] = _items[_count];
            _items[0].HeapIndex = 0;
            SortDown(_items[0]);
            return first;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }
        public bool Contains(T item)
        {
            return Equals(_items[item.HeapIndex], item);
        }

        private void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                if (childIndexLeft < _count)
                {
                    var swapIndex = childIndexLeft;
                    if (childIndexRight < _count)
                    {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0)
                    {
                        Swap(item, _items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while (true)
            {
                T parentItem = _items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                    Swap(item, parentItem);
                else
                    break;
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(T a, T b)
        {
            _items[a.HeapIndex] = b;
            _items[b.HeapIndex] = a;
            (a.HeapIndex, b.HeapIndex) = (b.HeapIndex, a.HeapIndex);
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}