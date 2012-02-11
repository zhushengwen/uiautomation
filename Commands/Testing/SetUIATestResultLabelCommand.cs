/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 01/02/2012
 * Time: 05:15 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of SetUIATestResultLabelCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "UIATestResultLabel")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class SetUIATestResultLabelCommand : HasScriptBlockCmdletBase
    {
        #region Constructor
        public SetUIATestResultLabelCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=true)]
        [ValidateNotNullOrEmpty()]
        public new string TestResultLabel { get; set; }
        [Parameter(Mandatory=false)]
        public new SwitchParameter TestPassed { get; set; }
        #endregion Parameters
        
        protected override void BeginProcessing()
        {
            CurrentData.AddTestResult(this.TestResultLabel, this.TestPassed);
        }
    }
}
