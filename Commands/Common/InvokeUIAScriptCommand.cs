/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 19/01/2012
 * Time: 10:04 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Collections;
using System.Collections.ObjectModel;

namespace UIAutomation.Commands.Common
{
    /// <summary>
    /// Description of InvokeUIAScriptCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAScript")]
    internal class InvokeUIAScriptCommand : HasTimeoutCmdletBase
    {
        #region Constructor
        public InvokeUIAScriptCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=true)]
        public ScriptBlock[] ScriptBlock { get; set; }
        #endregion Parameters
        
        protected override void BeginProcessing()
        {
            // RunScriptBlocks(this);
        }
    }
}
