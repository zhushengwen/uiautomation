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
    /// Description of SetUIAWizardCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "UIAWizard")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    //public class SetUIAWizardCommand : WizardCmdletBase
    internal class SetUIAWizardCommand : WizardCmdletBase
    {
        public SetUIAWizardCommand()
        {
        }
    }
}
