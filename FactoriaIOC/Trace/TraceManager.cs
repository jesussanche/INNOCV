﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FactoriaIOC.Trace
{
    public class TraceManager : ITraceManager
    {
        #region Members

        TraceSource source;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public TraceManager()
        {
            // Create default source
            source = new TraceSource("NLayerApp");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Trace internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to trace</param>
        /// <param name="message">Message of event</param>
        void TraceInternal(TraceEventType eventType, string message)
        {
            if (source != null)
            {
                try
                {
                    source.TraceEvent(eventType, (int)eventType, message);
                }
                catch (SecurityException)
                {
                    //Cannot access to file listener or cannot have
                    //privileges to write in event log
                    //do not propagete this :-(
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        /// <param name="operationName"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2135:SecurityRuleSetLevel2MethodsShouldNotBeProtectedWithLinkDemandsFxCopRule"),
        SecurityPermission(SecurityAction.LinkDemand)]
        public void TraceStartLogicalOperation(string operationName)
        {
            if (String.IsNullOrEmpty(operationName))
                throw new ArgumentNullException("operationName", "operationName cannot be null");

            System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            System.Diagnostics.Trace.CorrelationManager.StartLogicalOperation(operationName);
        }


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2135:SecurityRuleSetLevel2MethodsShouldNotBeProtectedWithLinkDemandsFxCopRule"),
        SecurityPermission(SecurityAction.LinkDemand)]
        public void TraceStopLogicalOperation()
        {
            try
            {
                System.Diagnostics.Trace.CorrelationManager.StopLogicalOperation();
            }
            catch (InvalidOperationException)
            {
                //stack empty
            }
        }


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        public void TraceStart()
        {
            TraceInternal(TraceEventType.Start, "Trace start");
        }


        /// <summary>
        ///<see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        public void TraceStop()
        {

            TraceInternal(TraceEventType.Stop, "Trace stop");

        }


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/></param>
        public void TraceInfo(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message", "message cannot be empty");

            TraceInternal(TraceEventType.Information, message);

        }


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/></param>
        public void TraceWarning(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message", "message cannot be empty");

            TraceInternal(TraceEventType.Warning, message);

        }


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/></param>
        public void TraceError(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message", "message cannot be empty");

            TraceInternal(TraceEventType.Error, message);

        }


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/>
        /// </summary>
        /// <param name="message"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Logging.ITraceManager"/></param>
        public void TraceCritical(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message", "message cannot be empty");

            TraceInternal(TraceEventType.Critical, message);
        }

        #endregion
    }
}
