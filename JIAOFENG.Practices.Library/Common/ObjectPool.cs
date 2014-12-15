using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// Summary description for ObjectPool.
    /// </summary>
    public abstract class ObjectPool<ObjectType> : IDisposable where ObjectType : class
    {
        //using object pool
        private  Dictionary<ObjectType,long> usingObjects;
        //free object pool
        private Dictionary<ObjectType, long> freeOjbects;

        //thread lock for thread safe
        private SyncGuard sync;

        private const long DestroyInterval = 90 * 1000;//90 seconds
        private const long DestroyIntervalTicks = DestroyInterval * 10000;

        //to destroy free objects in pool every some time
        private Timer timer;
      
        public ObjectPool()
        {
            usingObjects = new Dictionary<ObjectType, long>();
            freeOjbects = new Dictionary<ObjectType, long>();

            sync = new SyncGuard(this);

            timer = new Timer(DestroyInterval);            
            timer.Elapsed += new ElapsedEventHandler(CollectGarbage);
            timer.Enabled = true;
        }

         /// <summary>        
        /// Releases unmanaged resources and performs other cleanup operations before the object    
        /// is reclaimed by garbage collection.        
        /// </summary>        
        ~ObjectPool()        
        {
            Dispose();        
        } 

        /// <summary>
        /// Create new object
        /// </summary>
        /// <returns></returns>
        protected abstract ObjectType Create();

        /// <summary>
        /// Varify if object is still valid
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        protected abstract bool Validate(ObjectType o);

        /// <summary>
        /// Release object 
        /// </summary>
        /// <param name="o"></param>
        protected abstract void Expire(ObjectType o);

        /// <summary>
        /// Get a object from pool
        /// </summary>
        /// <returns></returns>
        protected ObjectType GetObjectFromPool()
        {
            ObjectType found = null;
            sync.Lock();
            ObjectType[] keys = new ObjectType[freeOjbects.Keys.Count];
            freeOjbects.Keys.CopyTo(keys, 0);
            foreach (ObjectType o in keys)
            {
                if (Validate(o))
                {
                    freeOjbects.Remove(o);
                    usingObjects.Add(o, DateTime.Now.Ticks);
                    found = o;
                    break;
                }
                else
                {
                    freeOjbects.Remove(o);
                    Expire(o);
                }
            }

            if (found == null)
            {
                found = Create();
                usingObjects.Add(found, DateTime.Now.Ticks);
            }
            sync.Unlock();
            return found;
        }

        /// <summary>
        /// Return a object back to pool
        /// </summary>
        /// <param name="o"></param>
        protected void ReturnObjectToPool(ObjectType o)
        {
            if (o != null)
            {
                sync.Lock();
                usingObjects.Remove(o);
                freeOjbects.Add(o, DateTime.Now.Ticks);
                sync.Unlock();
            }
        }

        /// <summary>
        /// Realeas objects which are free for some time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void CollectGarbage(object sender, System.Timers.ElapsedEventArgs ea)
        {
            sync.Lock();
            ObjectType[] keys = new ObjectType[freeOjbects.Keys.Count];
            freeOjbects.Keys.CopyTo(keys, 0);
            foreach (ObjectType o in keys)
            {
                if ((DateTime.Now.Ticks - freeOjbects[o]) > DestroyIntervalTicks)
                {
                    freeOjbects.Remove(o);
                    Expire(o);
                }
            }

            keys = new ObjectType[usingObjects.Keys.Count];
            usingObjects.Keys.CopyTo(keys, 0);
            foreach (ObjectType o in keys)
            {
                if ((DateTime.Now.Ticks - usingObjects[o]) > DestroyIntervalTicks)
                {
                    usingObjects.Remove(o);
                    Expire(o);
                }
            }
            sync.Unlock();
        }

        /// <summary>
        /// Release all ojbects and destroy pool
        /// </summary>
        public void Dispose()
        {
            ObjectType[] keys = new ObjectType[freeOjbects.Keys.Count];
            freeOjbects.Keys.CopyTo(keys, 0);
            foreach (ObjectType o in keys)
            {
                freeOjbects.Remove(o);
                Expire(o);
            }

            keys = new ObjectType[usingObjects.Keys.Count];
            usingObjects.Keys.CopyTo(keys, 0);
            foreach (ObjectType o in keys)
            {
                usingObjects.Remove(o);
                Expire(o);
            }
        }
    }
}
