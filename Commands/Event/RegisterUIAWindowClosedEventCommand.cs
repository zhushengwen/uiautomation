/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 20/01/2012
 * Time: 09:51 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of RegisterUIAWindowClosedEventCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "UIAWindowClosedEvent")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class RegisterUIAWindowClosedEventCommand : EventCmdletBase
    {
        #region Constructor
        public RegisterUIAWindowClosedEventCommand()
        {
            base.AutomationEventType = 
                WindowPattern.WindowClosedEvent;
            base.AutomationEventHandler = OnUIAutomationEvent;
        }
        #endregion Constructor
    }
}
