/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 30/11/2011
 * Time: 08:45 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation.Commands
{
	/// <summary>
	/// Description of InvokeUIAExpandTreeItemCommand.
	/// </summary>
	[Cmdlet(VerbsLifecycle.Invoke, "UIAExpandTreeItem")]
	[OutputType(typeof(bool))]
	public class InvokeUIAExpandTreeItemCommand : PatternCmdletBase
	{
		#region Constructor
		public InvokeUIAExpandTreeItemCommand()
		{
		}
		#endregion Constructor
		
		#region Parameters
//		[ValidateNotNullOrEmpty()]
//		[Parameter(Mandatory=true, 
//			ValueFromPipeline=true, 
//			Position=0,
//			HelpMessage="This is usually the output from Select-UIAControl" )] 
//		public System.Windows.Automation.AutomationElement Control {get; set;}
		#endregion Parameters
		
//		protected override void BeginProcessing()
//		{
//			WriteVerbose("Invoke-UIAExpandTreeItem: Text = " + Text);
//		}
		
		protected override void ProcessRecord()
		{
			if (Control == null)
			{
				WriteVerbose("Invoke-UIAExpandTreeItem: Control is null");
				WriteObject(false);
				return;
			}
			System.Windows.Automation.AutomationElement _control = null;
			try{
				_control = 
					(System.Windows.Automation.AutomationElement)Control;
			} catch (Exception eControlTypeException) {
				WriteVerbose("Invoke-UIAExpandTreeItem: Control is not an AutomationElement");
				WriteVerbose("Invoke-UIAExpandTreeItem: " + eControlTypeException.Message);
				WriteObject(false);
				return;
			}
			System.Windows.Automation.ExpandCollapsePattern expandPattern =
				(System.Windows.Automation.ExpandCollapsePattern)
				UIAHelper.GetCurrentPattern(ref _control,
				                            System.Windows.Automation.ExpandCollapsePattern.Pattern);
			if (expandPattern != null)
			{
				expandPattern.Expand();
				WriteObject(true);
			}
			else{
				WriteVerbose("Invoke-UIAExpandTreeItem: couldn't get ExpandCollapsePattern");
				WriteObject(false);
			}
			return;
		}
		
		
	}
}
