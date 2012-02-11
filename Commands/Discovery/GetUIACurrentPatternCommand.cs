/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 30/11/2011
 * Time: 10:12 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of GetUIACurrentPatternCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIACurrentPattern")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIACurrentPatternCommand: DiscoveryCmdletBase
    {
        #region Constructor
        public GetUIACurrentPatternCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [ValidateNotNullOrEmpty()]
        [Parameter(Mandatory=true, 
            ValueFromPipeline=true, 
            HelpMessage="This is usually the output from Get-UIAControl" )] 
        public System.Windows.Automation.AutomationElement Control { get; set; }
        [Parameter(Mandatory=true)]
        public string Name { get; set; }
        #endregion Parameters
        
        System.Windows.Automation.AutomationElement _control = null;
        
        protected override void ProcessRecord()
        {
            object result = null; // ?

            if (!base.CheckControl(this)) return;
            
            WriteVerbose(this, _control.Current);
            WriteVerbose(this, 
                         (_control.GetSupportedPatterns()).Length.ToString());
            foreach(System.Windows.Automation.AutomationPattern p in _control.GetSupportedPatterns())
            {
                WriteVerbose(this, p.ProgrammaticName);
            }
            System.Windows.Automation.AutomationPattern pattern = 
                UIAHelper.GetPatternByName(Name);
            result = 
                UIAHelper.GetCurrentPattern(ref _control,
                                            pattern);
            WriteVerbose(this, result);
            WriteObject(this, result);
        }
    }
}
