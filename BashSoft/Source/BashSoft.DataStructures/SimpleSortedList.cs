﻿namespace BashSoft.DataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Bytes2you.Validation;
    using Contracts;

    public class SimpleSortedList<T> : ISimpleOrderedBag<T>
        where T : IComparable<T>
    {
        private const int DefaultSize = 16;

        private T[] innerCollection;
        private int size;
        private readonly IComparer<T> comparison;

        public SimpleSortedList(int capacity, IComparer<T> comparison)
        {
            this.comparison = comparison;
            this.InitializeInnerCollection(capacity);
        }

        public SimpleSortedList(IComparer<T> comparison)
            : this(DefaultSize, comparison)
        {
        }

        public SimpleSortedList(int capacity)
            : this(capacity, Comparer<T>.Create((x, y) => x.CompareTo(y)))
        {
        }

        public SimpleSortedList()
            : this(DefaultSize)
        {
        }

        public int Size => this.size;

        public int Capacity => this.innerCollection.Length;

        public void Add(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }

            if (this.innerCollection.Length == this.Size)
            {
                this.Resize();
            }

            this.innerCollection[this.Size] = element;
            this.size++;
            Array.Sort(this.innerCollection, 0, this.Size, this.comparison);
        }

        public void AddAll(ICollection<T> collection)
        {
            Guard.WhenArgument(collection, "Collection").IsNull().Throw();

            if (this.Size + collection.Count >= this.innerCollection.Length)
            {
                this.MultiResize(collection);
            }

            foreach (var element in collection)
            {
                this.innerCollection[this.Size] = element;
                this.size++;
            }

            Array.Sort(this.innerCollection, 0, this.Size, this.comparison);
        }

        public bool Remove(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }

            var hasBeenRemoved = false;
            var indexOfRemovedElement = 0;

            for (var i = 0; i < this.Size; i++)
            {
                if (this.innerCollection[i].Equals(element))
                {
                    indexOfRemovedElement = i;
                    this.innerCollection[i] = default(T);
                    hasBeenRemoved = true;
                    break;
                }
            }

            if (hasBeenRemoved)
            {
                for (var i = indexOfRemovedElement; i < this.Size - 1; i++)
                {
                    this.innerCollection[i] = this.innerCollection[i + 1];
                }

                this.innerCollection[this.Size - 1] = default(T);
                this.size--;
            }

            return hasBeenRemoved;
        }

        public string JoinWith(string joiner)
        {
            var builder = new StringBuilder();

            foreach (var element in this)
            {
                builder.Append(element);
                builder.Append(joiner);
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - joiner.Length, joiner.Length);
            }

            return builder.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < this.Size; i++)
            {
                yield return this.innerCollection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void InitializeInnerCollection(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Capacity cannot be negative");
            }

            this.innerCollection = new T[capacity];
        }

        private void Resize()
        {
            var newCollection = new T[this.Size * 2];
            Array.Copy(this.innerCollection, newCollection, this.Size);
            this.innerCollection = newCollection;
        }

        private void MultiResize(ICollection<T> collection)
        {
            var newSize = this.innerCollection.Length * 2;

            while (this.Size + collection.Count >= newSize)
            {
                newSize *= 2;
            }

            var newCollection = new T[newSize];
            Array.Copy(this.innerCollection, newCollection, this.Size);
            this.innerCollection = newCollection;
        }
    }
}