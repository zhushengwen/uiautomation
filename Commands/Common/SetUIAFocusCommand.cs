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
    /// Description of SetUIAFocusCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "UIAFocus")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class SetUIAFocusCommand : HasControlInputCmdletBase
    {
        #region Constructor
        public SetUIAFocusCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        #endregion Parameters
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            InputObject.SetFocus();
            if (this.PassThru){
                WriteObject(this, this.InputObject);
            } else {
                WriteObject(this, true);
            }
        }
    }
}
