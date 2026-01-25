using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Baubit.Collections
{
    /// <summary>
    /// A threadsafe list allowing simultaneous reads and exclusive writes
    /// </summary>
    /// <typeparam name="T">The type of elements in the list</typeparam>
    public class ConcurrentList<T> : IList<T>
    {
        /// <inheritdoc/>
        public int Count
        {
            get
            {
                try
                {
                    lockSlim.EnterReadLock();
                    return store.Count;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    lockSlim.ExitReadLock();
                }
            }
        }

        /// <inheritdoc/>
        public virtual T this[int index]
        {
            get
            {
                try
                {
                    lockSlim.EnterReadLock();
                    return store[index];
                }
                catch
                {
                    throw;
                }
                finally
                {
                    lockSlim.ExitReadLock();
                }
            }

            set
            {
                try
                {
                    lockSlim.EnterWriteLock();
                    store[index] = value;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    lockSlim.ExitWriteLock();
                }
            }
        }

        public ConcurrentList()
        {

        }

        public ConcurrentList(IEnumerable<T> collection)
        {
            store = new List<T>(collection);
        }


        public bool IsReadOnly => false;

        private readonly List<T> store = new List<T>();
        private readonly ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual void Add(T item)
        {
            try
            {
                lockSlim.EnterWriteLock();
                store.Add(item);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            try
            {
                lockSlim.EnterWriteLock();
                store.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        protected T[] RemoveAndReturnAll()
        {
            try
            {
                lockSlim.EnterWriteLock();
                var items = store.ToArray();
                store.Clear();
                return items;
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            try
            {
                lockSlim.EnterReadLock();
                return store.Contains(item);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            try
            {
                lockSlim.EnterReadLock();
                store.CopyTo(array, arrayIndex);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            try
            {
                lockSlim.EnterReadLock();
                return store.ToArray().AsEnumerable().GetEnumerator();
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            try
            {
                lockSlim.EnterReadLock();
                return store.IndexOf(item);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        public virtual void Insert(int index, T item)
        {
            try
            {
                lockSlim.EnterWriteLock();
                store.Insert(index, item);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <inheritdoc/>
        public virtual bool Remove(T item)
        {
            try
            {
                lockSlim.EnterWriteLock();
                return store.Remove(item);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public virtual bool Remove(Func<IEnumerable<T>, T> selector, out T item)
        {
            try
            {
                lockSlim.EnterWriteLock();
                item = selector(store);
                if (item != null)
                {
                    store.Remove(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }


        /// <inheritdoc/>
        public virtual void RemoveAt(int index)
        {
            try
            {
                lockSlim.EnterWriteLock();
                store.RemoveAt(index);
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        protected T RemoveAtAndReturn(int index)
        {
            try
            {
                lockSlim.EnterWriteLock();
                var item = store[index];
                store.RemoveAt(index);
                return item;
            }
            catch
            {
                throw;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}