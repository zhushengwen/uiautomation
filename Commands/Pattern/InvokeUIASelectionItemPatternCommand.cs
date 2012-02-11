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
    /// Description of InvokeUIASelectionItemPatternCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIASelectionItemPattern")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIASelectionItemPatternCommand : PatternCmdletBase
    { public InvokeUIASelectionItemPatternCommand(){ WhatToDo = "SelectionItem"; }
        
        #region Parameters
        [Parameter(Mandatory=true)]
        public string[] ItemName { get; set; }
        #endregion Parameters
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            // if there's a selection, add items to the selection
            System.Windows.Automation.AutomationElement[] selectedItems = null;
            bool thereIsSelection = false;
            try{
                System.Windows.Automation.SelectionPattern selPattern = null;
                selPattern = 
                    this.InputObject.GetCurrentPattern(
                        System.Windows.Automation.SelectionPattern.Pattern)
                        as System.Windows.Automation.SelectionPattern;
                selectedItems = 
                    selPattern.Current.GetSelection();
                if (selectedItems.Length>0){
                    thereIsSelection = true;
                }
            } catch (Exception eSelection) {
                WriteDebug(eSelection.Message);
                WriteVerbose(this, "there wasn't a selection");
            }
            
            for(int itemNumber = 0;
                itemNumber<this.ItemName.Length;
                itemNumber++)
            {
                if (selectedItems!=null)
                {
                    // foreach(System.Windows.Automation.AutomationElement elt in selectedItems){
                        // if (this.ItemName[itemNumber]==elt.Current.Name){
                        //    continue;
                        //}
                    //}
                }
                // the item with the current ItemName name
                // is not in the selection
                // get it and add to the selection
                WriteVerbose(this, "searching for items with Name = " + 
                             this.ItemName[itemNumber]);
                System.Windows.Automation.AutomationElementCollection newItemsToSelection = 
                    this.InputObject.FindAll(
                        System.Windows.Automation.TreeScope.Descendants,
                        new System.Windows.Automation.PropertyCondition(
                            System.Windows.Automation.AutomationElement.NameProperty,
                            this.ItemName[itemNumber]));
                if (newItemsToSelection!=null && newItemsToSelection.Count>0){
                    foreach(System.Windows.Automation.AutomationElement elt in
                            newItemsToSelection){
                        try{
                            WriteVerbose(this, "trying to select the " + 
                                         elt.Current.Name + 
                                         " item");
                            System.Windows.Automation.SelectionItemPattern selItemPattern = null;
                            selItemPattern = 
                                elt.GetCurrentPattern(
                                    System.Windows.Automation.SelectionItemPattern.Pattern)
                                as System.Windows.Automation.SelectionItemPattern;
                            // selItemPattern.Select();
                            if (thereIsSelection){
                                WriteVerbose(this, "adding to the existing selection");
                                selItemPattern.AddToSelection();
                            } else {
                                WriteVerbose(this, "creating the selection");
                                selItemPattern.Select();
                                thereIsSelection = true;
                            }
                        } catch (Exception eSelItem){
                            WriteVerbose(eSelItem.Message);
                            WriteVerbose(this, "could not get a SelectionItemPattern");
                        }
                    }
                }
            }
            

            
        }
    }
    
    /// <summary>
    /// Description of InvokeUIACustomSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIACustomSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIACustomSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIACustomSelectItemCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIADataItemSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIADataItemSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIADataItemSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIADataItemSelectItemCommand(){ } }
        
    /// <summary>
    /// Description of InvokeUIAImageSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAImageSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIAImageSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIAImageSelectItemCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIAListItemSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAListItemSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIAListItemSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIAListItemSelectItemCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIAMenuItemSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAMenuItemSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIAMenuItemSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIAMenuItemSelectItemCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIARadioButtonSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIARadioButtonSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIARadioButtonSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIARadioButtonSelectItemCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIATabItemSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIATabItemSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIATabItemSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIATabItemSelectItemCommand(){ } }
    
    /// <summary>
    /// Description of InvokeUIATreeItemSelectItemCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIATreeItemSelectItem")]
    [OutputType(typeof(bool))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class InvokeUIATreeItemSelectItemCommand : InvokeUIASelectionItemPatternCommand
    { public InvokeUIATreeItemSelectItemCommand(){ } }

}
