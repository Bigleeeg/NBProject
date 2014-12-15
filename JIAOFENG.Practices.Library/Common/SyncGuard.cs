using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

namespace JIAOFENG.Practices.Library.Common
{
    public sealed class SyncGuard : IDisposable
    {
        #region Private Members        
        private object target;        
        private bool locked;       
        #endregion

        #region Constructors        
        /// <summary>        
        /// Initializes a new instance of the <see cref="T:SyncGuard"/> class.        
        /// </summary>        
        /// <param name="target">The target.</param>        
        public SyncGuard(object target)        
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            this.target = target; 
            locked = false;                                                    
        }        
        /// <summary>        
        /// Releases unmanaged resources and performs other cleanup operations before the object    
        /// is reclaimed by garbage collection.        
        /// </summary>        
        ~SyncGuard()        
        {
            Dispose();        
        }        
        #endregion    
    
        #region Public Members       
        /// <summary>        
        /// Locks this instance.        
        /// </summary>        
        public void Lock()        
        {            
            Monitor.Enter(target);            
            locked = true;        
        }        
        /// <summary>        
        /// Unlocks this instance.        
        /// </summary>        
        public void Unlock()        
        {            
            Monitor.Exit(target);            
            locked = false;        
        }       
        /// <summary>        
        /// Removes the locks.        
        /// </summary>        
        public void Dispose()
        {            
            if (locked)            
            {                
                Unlock();            
            }        
        }        
        #endregion    
    }
}
