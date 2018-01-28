using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIBaseElement : MonoBehaviour
    {
        ///////////////////////////////////////////////////////////////////
        /// Interaction Delegate
        ///////////////////////////////////////////////////////////////////

        public delegate void OnClickDelegate(object data);
        protected OnClickDelegate onClickDelegate;

        ///////////////////////////////////////////////////////////////////
        /// Private Member Variables
        ///////////////////////////////////////////////////////////////////

        protected object mData = null;

        ///////////////////////////////////////////////////////////////////
        /// MonoBehaviour Implementation
        ///////////////////////////////////////////////////////////////////

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        ///////////////////////////////////////////////////////////////////
        /// UIBaseElement Implementation
        ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Set up any initial data or state information.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Validate the element. Base class just returns true.
        /// </summary>
        /// <returns></returns>
        protected virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// Destory anything that keeps memory, remove any callbacks, whatever.
        /// </summary>
        public virtual void Shutdown()
        {
        }

        /// <summary>
        /// Sets the element data
        /// </summary>
        /// <param name="data">generic object data</param>
        public virtual void SetData(object data)
        {
            mData = data;
        }

        /// <summary>
        /// Add an OnClick listener to recieve element data.
        /// </summary>
        /// <param name="callback"></param>
        public void AddOnClickDelegate(OnClickDelegate callback)
        {
            if(callback != null)
            {
                // This ensures we never have duplicates, and it does nothing if the callback is not already in the delegate invocation list.
                onClickDelegate -= callback;
                onClickDelegate += callback;
            }
        }

        /// <summary>
        /// Removes an OnClick listener from the delegate.
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveOnClickDelegate(OnClickDelegate callback)
        {
            if (callback != null)
            {
                onClickDelegate -= callback;
            }
        }

        /// <summary>
        /// Callback to use with Unity's OnClick Event
        /// </summary>
        public virtual void OnClick()
        {
            Debug.LogFormat("Recieved OnClick() event on object: {0}", name);

            if(onClickDelegate != null)
            {
                onClickDelegate(mData);
            }
        }

        /// <summary>
        /// Called when this element is destroyed
        /// </summary>
        public virtual void OnDestroy()
        {
            if(onClickDelegate != null)
            {
                Delegate[] invocationList = onClickDelegate.GetInvocationList();

                for(int i = 0; i < invocationList.Length; ++i)
                {
                    invocationList[i] = null;
                }
            }
            onClickDelegate = null;
        }
    }
}