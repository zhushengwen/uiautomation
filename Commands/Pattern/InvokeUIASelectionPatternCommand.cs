/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 22/12/2011
 * Time: 06:46 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of InvokeUIASelectionPatternCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIASelectionPattern")]
    //[OutputType(typeof(System.Windows.Automation.AutomationElement[]))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIASelectionPatternCommand : PatternCmdletBase
    { public InvokeUIASelectionPatternCommand(){ WhatToDo = "Selection"; }
    }
    
    /// <summary>
    /// Description of InvokeUIACalendarSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIACalendarSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIACalendarSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIACalendarSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIAComboBoxSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAComboBoxSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIAComboBoxSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIAComboBoxSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIACustomSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIACustomSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIACustomSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIACustomSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIADataGridSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIADataGridSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIADataGridSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIADataGridSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIAListSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAListSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIAListSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIAListSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIASliderSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIASliderSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIASliderSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIASliderSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIASpinnerSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIASpinnerSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIASpinnerSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIASpinnerSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIATabSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIATabSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIATabSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIATabSelectCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIATreeSelectCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIATreeSelect")]
    //[OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIATreeSelectCommand : InvokeUIASelectionPatternCommand 
    { public InvokeUIATreeSelectCommand(){ } }
}