/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 29.11.2011
 * Time: 14:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of HasTimeoutCmdletBase.
    /// </summary>
    public class HasTimeoutCmdletBase : HasControlInputCmdletBase
    {
        #region Constructor
        public HasTimeoutCmdletBase()
        {
            Wait = true;
            Timeout = Preferences.Timeout;
            Seconds = Timeout / 1000;
            OnErrorScreenShot = Preferences.OnErrorScreenShot;
            OnSuccessAction = null;
            OnErrorAction = null;
        }
        #endregion Constructor

        #region Parameters
        [Parameter(Mandatory=false)]
        internal SwitchParameter Wait { get; set; }
        [Alias("Milliseconds")]
        [Parameter(Mandatory=false)]
        public int Timeout { get; set; }
        [Parameter(Mandatory=false)]
        public int Seconds {
            get{ return Timeout / 1000; } 
            set{ Timeout = value * 1000; }
        }
        
        [Parameter(Mandatory=false)]
        public ScriptBlock[] OnSleepAction { get; set; }
        #endregion Parameters
    }
}
