/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 20/01/2012
 * Time: 09:56 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of UnregisterUIAEventCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Unregister, "UIAEvent")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class UnregisterUIAEventCommand : EventCmdletBase
    {
        #region Constructor
        public UnregisterUIAEventCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=false)]
        public SwitchParameter All { get; set; }
        [Parameter(Mandatory=false)]
        public AutomationEventHandler EventHandler { get; set; }
        
        [Parameter(Mandatory=false)]
        internal new SwitchParameter PassThru { get; set; }
        [Parameter(Mandatory=false)]
        internal new System.Windows.Automation.AutomationElement InputObject { get; set; }
        #endregion Parameters
        
        protected override void BeginProcessing()
        {
            if (this.All){
                Automation.RemoveAllEventHandlers();
            } else {
                try{
                    if (this.InputObject!=null && 
                        this.EventHandler!=null){
                        Automation.RemoveAutomationEventHandler(
                            null,
                            this.InputObject,
                            this.EventHandler);
                    }
                } 
                catch {
                    
                }
            }
        }
    }
}
