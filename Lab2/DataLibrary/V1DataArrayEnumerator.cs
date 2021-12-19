using System;
using System.Collections;
using System.Collections.Generic;

namespace DataLibrary
{
    public class V1DataArrayEnumerator : IEnumerator<DataItem>
    {
        private int index = -1;
        private readonly V1DataArray array;
        private DataItem dataItem = default;

        public V1DataArrayEnumerator(V1DataArray array) => this.array = array;

        public DataItem Current { get => dataItem; }

        public bool MoveNext()
        {
            if (index >= array.Count || ++index >= array.Count)
            {
                return false;
            }
            else
            {
                dataItem = array.ItemInGrid(index / array.ColNumber, index % array.ColNumber);
                return true;
            }
        }

        public void Reset() { index = -1; }

        object IEnumerator.Current { get => Current; }
        void IDisposable.Dispose() { }
    }
}
