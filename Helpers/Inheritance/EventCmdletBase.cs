/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 20/01/2012
 * Time: 09:29 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of HasScriptBlockCmdletBase.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "UIADummyEvent")]
    public class EventCmdletBase: HasControlInputCmdletBase
    {
        #region Constructor
        public EventCmdletBase()
        {
            this.InputObject = CurrentData.CurrentWindow;
            this.AutomationEventType = null;
            this.AutomationProperty = null;
            this.AutomationEventHandler = null;
        }
        #endregion Constructor

        #region Parameters
        [Parameter(Mandatory=false)]
        internal new SwitchParameter OnErrorScreenShot { get; set; }
        [Parameter(Mandatory=false)]
        public new ScriptBlock[] EventAction { get; set; }
        #endregion Parameters
        
        #region Properties
        protected AutomationEvent AutomationEventType { get; set; }
        protected AutomationProperty AutomationProperty { get; set; }
        protected AutomationEventHandler AutomationEventHandler { get; set; }
        #endregion Properties
        
        protected override void ProcessRecord()
        {
            if (this.InputObject==null) return;
            
            SubscribeToEvents(this,
                              this.InputObject, 
                              this.AutomationEventType,
                              this.AutomationProperty,
                              this.AutomationEventHandler);
        }
        
        #region OnUIAutomationEvent
        protected void OnUIAutomationEvent(object src, AutomationEventArgs e)
        {
            if (!checkNotNull(src, e)) return;
            RunEventScriptBlocks(this);
            try{
                WriteVerbose(this, e.EventId + " on " + (src as AutomationElement) + " fired");
            } catch { }
        }
        #endregion OnUIAutomationEvent
        
        #region OnUIAutomationPropertyChangedEvent
        protected void OnUIAutomationPropertyChangedEvent(object src, AutomationPropertyChangedEventArgs e)
        {
            if (!checkNotNull(src, e)) return;
            RunEventScriptBlocks(this);
            try{
                WriteVerbose(this, e.EventId + "on " + (src as AutomationElement) + " fired");
            } catch { }
        }
        #endregion OnUIAutomationPropertyChangedEvent
        
        #region OnUIStructureChangedEvent
        protected void OnUIStructureChangedEvent(object src, StructureChangedEventArgs e)
        {
            if (!checkNotNull(src, e)) return;
            RunEventScriptBlocks(this);
            try{
                WriteVerbose(this, e.EventId + "on " + (src as AutomationElement) + " fired");
            } catch { }
        }
        #endregion OnUIStructureChangedEvent
        
        #region OnUIWindowClosedEvent
        protected void OnUIWindowClosedEvent(object src, WindowClosedEventArgs e)
        {
            if (!checkNotNull(src, e)) return;
            RunEventScriptBlocks(this);
            try{
                WriteVerbose(this, e.EventId + "on " + (src as AutomationElement) + " fired");
            } catch { }
        }
        #endregion OnUIWindowClosedEvent
    }
}