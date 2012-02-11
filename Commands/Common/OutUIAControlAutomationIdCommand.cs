/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 01/12/2011
 * Time: 12:36 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of OutUIAControlAutomationIdCommand.
    /// </summary>
    [Cmdlet(VerbsData.Out, "UIAControlAutomationId")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class OutUIAControlAutomationIdCommand : OutCmdletBase
    {
        #region Constructor
        public OutUIAControlAutomationIdCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        #endregion Parameters
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            WriteObject(this, InputObject.Current.AutomationId);
            
        }
    }
}
