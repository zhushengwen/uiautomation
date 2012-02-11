/*
 * Created by SharpDevelop.
 * User: apetrov1
 * Date: 07/02/2012
 * Time: 07:56 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation.Commands.Pattern
{
	/// <summary>
	/// Description of GetUIAifUltraGridSelectionCommand.
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "UIAifUltraGridSelection")]
	public class GetUIAifUltraGridSelectionCommand : ULtraGridCmdletBase
	{
		public GetUIAifUltraGridSelectionCommand()
		{
		}
		
		#region Parameters
		[Parameter(Mandatory=true)]
		internal new string[] ItemName {get; set;}
		#endregion Parameters
	}
}
