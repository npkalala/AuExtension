﻿using System;

namespace AuExtension.Extend.NetWork
{
    public class NetworkStatusChangedArgs : EventArgs
    {
        private bool isAvailable;

        /// <summary>
        /// Instantiate a new instance with the given availability.
        /// </summary>
        /// <param name="isAvailable"></param>

        public NetworkStatusChangedArgs(bool isAvailable)
        {
            this.isAvailable = isAvailable;
        }

        /// <summary>
        /// Gets a Boolean value indicating the current state of Internet connectivity.
        /// </summary>

        public bool IsAvailable
        {
            get { return isAvailable; }
        }
    }

    //********************************************************************************************
    // delegate NetworkStatusChangedHandler
    //********************************************************************************************

    /// <summary>
    /// Define the method signature for network status changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    public delegate void NetworkStatusChangedHandler(
        object sender, NetworkStatusChangedArgs e);
}