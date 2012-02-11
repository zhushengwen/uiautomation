/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 30/01/2012
 * Time: 07:10 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of OutUIAControlIsEnabledCommand.
    /// </summary>
    [Cmdlet(VerbsData.Out, "UIAControlIsEnabled")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class OutUIAControlIsEnabledCommand : OutCmdletBase
    {
        public OutUIAControlIsEnabledCommand()
        {
        }
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            WriteObject(this, InputObject.Current.IsEnabled);
            
        }
    }
}
