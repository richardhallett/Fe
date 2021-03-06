﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Store a reference between a GraphicsResource and another core resource object.
    /// All core resource objects must implement the IDisposable interface to ensure proper cleanup.
    /// </summary>
    /// <typeparam name="T">An object you want to cache against the <see cref="GraphicsResource"/></typeparam>
    internal class ResourceCache<T> where T: IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCache{T}"/> class.
        /// </summary>
        /// <param name="size">The number of cached resources we can store.</param>
        internal ResourceCache(ushort size)
        {
            this.Size = size;

            this._handles = new Stack<ushort>(Enumerable.Range(0, size).Select(i => (ushort)i));
            this._resources = new T[size];
            this._cacheLookup = new Dictionary<ushort, WeakReference>();
            this._cacheCleanupQueue = new Queue<ushort>();
        }

        /// <summary>
        /// Gets the size of this cache.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        internal ushort Size { get; private set; }

        internal bool IsFull
        {
            get
            {
                return _handles.Count == 0;
            }
        }

        /// <summary>
        /// Adds the specified graphics resource to the cache, this sets the underlying core resource at the same time
        /// </summary>
        /// <param name="graphicsResource">The graphics resource.</param>
        /// <param name="coreResource">The core resource.</param>
        internal void Add(GraphicsResource graphicsResource, T coreResource)
        {
            // Get a free index for lookup
            if (_handles.Count == 0)
            {
                //Debug.WriteLine("Failed to create cached object of type " + this.Name);
                throw new Exception("Resource cache full of type " + this.GetType().ToString());
            }

            var handleIndex = _handles.Pop();

            // Store the underlying resource
            _resources[handleIndex] = coreResource;

            // Let our main object keep the index reference for later lookup.
            graphicsResource.ResourceIndex = handleIndex;

            // Underlying resource was passed in so we can say we're fully created
            graphicsResource.Created = true;

            // Add the reference of our resource to the underlying index handle.
            _cacheLookup.Add(handleIndex, new WeakReference(graphicsResource, false));
        }

        /// <summary>
        /// Adds the specified graphics resource but deferred for later full creation.
        /// </summary>
        /// <param name="graphicsResource">The graphics resource.</param>
        internal void Add(GraphicsResource graphicsResource)
        {
            // Get a free index for lookup
            if (_handles.Count == 0)
            {
                //Debug.WriteLine("Failed to create cached object of type " + this.Name);
                return;
            }

            var handleIndex = _handles.Pop();
            
            // Let our main object keep the index reference for later lookup.
            graphicsResource.ResourceIndex = handleIndex;

            // Add the reference of our resource to the underlying index handle.
            _cacheLookup.Add(handleIndex, new WeakReference(graphicsResource, false));
        }

        /// <summary>
        /// Sets the underlying resource. i.e. useful if it was just added for deferred creation.
        /// </summary>
        /// <param name="graphicsResource">The graphics resource.</param>
        /// <param name="coreResource">The core resource.</param>
        internal void SetResource(GraphicsResource graphicsResource, T coreResource)
        {
            // Store the underlying resource
            _resources[graphicsResource.ResourceIndex] = coreResource;
            
            // Underlying resource was passed in so we can say we're fully created
            graphicsResource.Created = true;
        }

        /// <summary>
        /// Gets the specified core resource from the cache.
        /// </summary>
        /// <param name="index">The index of the resource you want</param>
        /// <returns></returns>
        internal T Get(ushort index)
        {
            return _resources[index];
        }

        /// <summary>
        /// Gets the core resource from the cache via the key.
        /// </summary>
        /// <value>
        /// The core resource.
        /// </value>
        /// <param name="key">The key to use for the lookup</param>
        /// <returns></returns>
        internal T this[ushort key]
        {
            get
            {
                return Get(key);
            }
        }

        /// <summary>
        /// Cleans the resource cache up, i.e. any data references.
        /// </summary>
        /// <param name="force">if set to <c>true</c> force the resource to be entirely cleaned up, regardless if we have references</param>
        internal void Clean(bool force = false)
        {
            if (!force)
            {
                // Clean anything that is queued for cleaning
                while (this._cacheCleanupQueue.Count > 0)
                {
                    ushort handleIndex = this._cacheCleanupQueue.Dequeue();

                    if (_resources[handleIndex] != null)
                    {
                        _resources[handleIndex].Dispose();
                    }

                    _cacheLookup.Remove(handleIndex);

                    // Push the handle back on the stack as we can use it again.
                    _handles.Push(handleIndex);
                }

                // Look for stuff that needs to be cleaned.
                foreach (var cachedItem in this._cacheLookup)
                {
                    WeakReference resource = cachedItem.Value;
                    if (!resource.IsAlive)
                    {
                        _cacheCleanupQueue.Enqueue(cachedItem.Key);
                    }
                }
            }
            else
            {
                foreach (var index in this._cacheLookup.Keys)
                {
                    if (_resources[index] != null)
                    {
                        _resources[index].Dispose();
                    }
                }
            }
        }

        private Stack<ushort> _handles;
        private T[] _resources;
        private Dictionary<ushort, WeakReference> _cacheLookup;
        private Queue<ushort> _cacheCleanupQueue;
    }
}
