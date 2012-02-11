/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 01.02.2012
 * Time: 12:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of GetUIAControlDescendantsCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAControlDescendants")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAControlDescendantsCommand : GetUIAControlCommand
    {
        #region Constructor
        public GetUIAControlDescendantsCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=false)]
        internal new SwitchParameter Wait { get; set; }
        [Alias("Milliseconds")]
        [Parameter(Mandatory=false)]
        internal new int Timeout { get; set; }
        [Parameter(Mandatory=false)]
        internal new int Seconds {
            get{ return Timeout / 1000; } 
            set{ Timeout = value * 1000; }
        }
        #endregion Parameters
        
        protected override void BeginProcessing(){
            WriteVerbose(this, "ControlType = " + ControlType);
            WriteVerbose(this, "Class = " + Class);
            // WriteVerbose(this, "Title = " + Title);
            WriteVerbose(this, "Name = " + Name);
            WriteVerbose(this, "AutomationId = " + AutomationId);
        }
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            AndCondition conditions = getControlConditions(this);
            AutomationElementCollection result = null;
            if (conditions!=null){
                result = 
                    this.InputObject.FindAll(TreeScope.Descendants,
                                             conditions);
            } else {
                result =
                    this.InputObject.FindAll(TreeScope.Descendants,
                                             Condition.TrueCondition);
            }
            WriteObject(this, result);
        }
    }
}
