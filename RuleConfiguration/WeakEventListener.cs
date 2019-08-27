/*
 * Copyright (C) 2011 by Autodesk, Inc. All Rights Reserved.
 *
 * By using this code, you are agreeing to the terms and conditions of
 * the License Agreement included in the documentation for this code.
 *
 * AUTODESK MAKES NO WARRANTIES, EXPRESS OR IMPLIED, AS TO THE
 * CORRECTNESS OF THIS CODE OR ANY DERIVATIVE WORKS WHICH INCORPORATE
 * IT. AUTODESK PROVIDES THE CODE ON AN "AS-IS" BASIS AND EXPLICITLY
 * DISCLAIMS ANY LIABILITY, INCLUDING CONSEQUENTIAL AND INCIDENTAL
 * DAMAGES FOR ERRORS, OMISSIONS, AND OTHER PROBLEMS IN THE CODE.
 *
 * Use, duplication, or disclosure by the U.S. Government is subject
 * to restrictions set forth in FAR 52.227-19 (Commercial Computer
 * Software Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
 * (Rights in Technical Data and Computer Software), as applicable.
 */

using System;
using System.Windows;


namespace RuleConfiguration
{
    /// <summary>
    /// A common weak event listener which can be used for different kinds of events.
    /// </summary>
    /// <typeparam name="TEventArgs">The EventArgs type for the event.</typeparam>
    /// <typeparam name="TAcceptableManager">The manager type this listener should take care of. Events from other manager type would be ignored.</typeparam>
    public class WeakEventListener<TEventArgs, TAcceptableManager> : IWeakEventListener where TEventArgs : EventArgs
                                                                                        where TAcceptableManager: WeakEventManager
    {
        EventHandler<TEventArgs> _realHander = null;


        /// <summary>
        /// Initializes a new instance of the WeakEventListener class.
        /// </summary>
        /// <param name="handler">The handler for the event.</param>
        public WeakEventListener(EventHandler<TEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler"); // NOXLATE
            }

            this._realHander = handler;
        }

        /// <summary>
        /// Receives events from the centralized event manager.
        /// </summary>
        /// <param name="managerType">The type of the WeakEventManager calling this method.</param>
        /// <param name="sender">Object that originated the event.</param>
        /// <param name="e">Event data.</param>
        /// <returns>
        /// true if the listener handled the event.
        /// false if it receives an event that it does not recognize or handle.
        /// </returns>
        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, Object sender, EventArgs e)
        {
            if (typeof(TAcceptableManager) != managerType)
                return false;

            TEventArgs realArgs = (TEventArgs)e;

            this._realHander(sender, realArgs);

            return true;
        }
    }
}
