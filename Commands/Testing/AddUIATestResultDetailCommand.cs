/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 01/02/2012
 * Time: 05:28 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of AddUIATestResultDetailCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "UIATestResultDetail")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class AddUIATestResultDetailCommand : HasScriptBlockCmdletBase
    {
        #region Constructor
        public AddUIATestResultDetailCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=true)]
        [ValidateNotNullOrEmpty()]
        public string TestResultDetail { get; set; }
        #endregion Parameters
        
        protected override void BeginProcessing()
        {
            CurrentData.AddTestResultDetail(this.TestResultDetail);
        }
    }
}
