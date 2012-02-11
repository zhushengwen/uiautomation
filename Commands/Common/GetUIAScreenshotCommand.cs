/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 17.01.2012
 * Time: 11:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of GetUIAScreenshotCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAScreenshot")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAScreenshotCommand : HasControlInputCmdletBase
    {
        #region Constructor
        public GetUIAScreenshotCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=false)]
        public string Description { get; set; }
        #endregion Parameters
        
        protected override void ProcessRecord()
        {
            if (this.InputObject==null ||
                !(this.InputObject is AutomationElement)){
                UIAHelper.GetDesktopScreenshot(this.Description);
            } else {
                UIAHelper.GetControlScreenshot(this.InputObject, this.Description);
            }
        }
    }
}
