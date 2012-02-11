/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 12/2/2011
 * Time: 5:55 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of TranscriptCmdletBase.
    /// </summary>
    public class TranscriptCmdletBase : HasTimeoutCmdletBase
    {
        #region Constructor
        public TranscriptCmdletBase()
        {
            WriteCurrentPattern = false;
            NoClassInformation = false;
            NoScriptHeader = false;
            NoEvents = false;
            NoUI = false;
            Paused = false;
        }
        #endregion Constructor

        #region Parameters
        [Parameter(Mandatory=false)]
        internal new SwitchParameter PassThru { get; set; }
        
        
        [Parameter(Mandatory=false)]
        public SwitchParameter WriteCurrentPattern { get; set; }
        [Parameter(Mandatory=false)]
        public SwitchParameter NoClassInformation { get; set; }
        [Parameter(Mandatory=false)]
        public SwitchParameter NoEvents { get; set; }
        [Parameter(Mandatory=false)]
        public SwitchParameter NoScriptHeader { get; set; }
        [Parameter(Mandatory=false)]
        public SwitchParameter NoUI { get; set; }
        #endregion Parameters
        
        internal bool Paused { get; set; }
        
        internal protected System.Collections.ArrayList lastRecordedItem = 
            new System.Collections.ArrayList();
        // the list of all recorded controls' patterns
        internal protected System.Collections.ArrayList recordingPatterns = 
            new System.Collections.ArrayList();
        
        internal protected AutomationElement thePreviouslyUsedElement = null;
        
        internal void StopProcessing()
        {
            Global.GTranscript = false;
            EndProcessing();
        }
    }
}
