/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 08/02/2012
 * Time: 02:30 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of StepUIAWizardCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Step, "UIAWizard")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    //public class StepUIAWizardCommand : WizardCmdletBase
    internal class StepUIAWizardCommand : WizardCmdletBase
    {
        public StepUIAWizardCommand()
        {
        }
        
        [Parameter]
        public string StepName { get; set; }
        [Parameter]
        public SwitchParameter Forward { get; set; }
    }
}
