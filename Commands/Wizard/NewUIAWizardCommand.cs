/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 08/02/2012
 * Time: 02:29 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of NewUIAWizardCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "UIAWizard")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    //public class NewUIAWizardCommand : WizardCmdletBase
    internal class NewUIAWizardCommand : WizardCmdletBase
    {
        public NewUIAWizardCommand()
        {
        }
        
        [Parameter]
        public string Name { get; set; }
        
    }
}
