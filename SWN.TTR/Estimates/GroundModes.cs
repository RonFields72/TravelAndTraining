using System.Collections.Generic;

namespace SWN.TTR.Estimates
{
    public class GroundModes : IList<string>
    {
        protected GroundModes()
        {
        }

        public GroundModes(IList<string> groundModes)
        {
            this.contents = groundModes;
        }

        private IList<string> contents;
        private int count;

        #region IList<string> Members

        public int IndexOf(string item)
        {
            int itemIndex = -1;
            for (int i = 0; i < this.Count; ++i)
            {
                if (item.Equals(this.contents[i]))
                {
                    itemIndex = i;
                    break;
                }
            }
            return itemIndex;
        }

        public void Insert(int index, string item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public string this[int index]
        {
            get
            {
                return this.contents[index];
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region ICollection<string> Members

        public void Add(string item)
        {
            if (this.IndexOf(item) != -1)
                throw new System.InvalidOperationException("Ground mode already exists in collection.");
            else
            {
                this.contents.Add(item);
                this.count++;
            }
        }

        public void Clear()
        {
            this.contents.Clear();
            this.count = 0;
        }

        public bool Contains(string item)
        {
            foreach (string mode in this.contents)
                if (item.Equals(mode))
                    return true;
            return false;
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            int j = arrayIndex;
            for (int i = 0; i < this.Count; ++i)
            {
                array.SetValue(this.contents[i], j);
                ++j;
            }
        }

        public int Count
        {
            get { return this.count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(string item)
        {
            RemoveAt(IndexOf(item));
            return true;
        }

        #endregion

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string item in this.contents)
                yield return item;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}