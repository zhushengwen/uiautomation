/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 01/12/2011
 * Time: 12:36 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of ConvertFromUIADataGridCommand.
    /// </summary>
    [Cmdlet(VerbsData.ConvertFrom, "UIADataGrid")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class ConvertFromUIADataGridCommand : ConvertFromCmdletBase
    {
        #region Constructor
        public ConvertFromUIADataGridCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        #endregion Parameters
        
        // private bool SelectedOnly { get; set; }
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;

            string strData = String.Empty;
            System.Windows.Automation.AutomationElement _control = 
                this.InputObject;
            GridPattern gridPattern = null;

            try{
                gridPattern = 
                    //_control.GetCurrentPattern(GridPattern.Pattern)
                    this.InputObject.GetCurrentPattern(GridPattern.Pattern)
                    as GridPattern;
                
            
                    bool res1 = 
                        UIAHelper.GetHeaderItems(ref _control, out strData, this.Delimiter);
                    if (res1){
                        // WriteObject(this, strData);
                        WriteObject(strData);
                    } else {
                        WriteVerbose(this, strData);
                    }
                    
                    // temporary!!!
                    // Selection
                    System.Windows.Automation.AutomationElement[] selectedItems = null;
                    if (this.SelectedOnly){
                        // if there's a selection, get items in the selection
                        try{
                            System.Windows.Automation.SelectionPattern selPattern;
                            selPattern = 
                                this.InputObject.GetCurrentPattern(
                                    System.Windows.Automation.SelectionPattern.Pattern)
                                    as System.Windows.Automation.SelectionPattern;
                            selectedItems = 
                                selPattern.Current.GetSelection();
                        } catch (Exception eSelection) {
                            WriteDebug(this, eSelection.Message);
                            WriteVerbose(this, "there wasn't a selection");
                        }
                    }
                    
                    
                    // temporary!!!
                    // get rows
                    if (gridPattern.Current.RowCount>0){
                        RunOnSuccessScriptBlocks(this);
                            for (int rowsCounter = 0;
                                 rowsCounter<gridPattern.Current.RowCount;
                                 rowsCounter++){
                                if (this.SelectedOnly && selectedItems.Length>0){
                                } else {
                                    // without a selection
                                    string outString = 
                                        UIAHelper.GetOutputStringUsingTableGridPattern<System.Windows.Automation.GridPattern>(
                                            gridPattern,
                                            gridPattern.Current.ColumnCount,
                                            rowsCounter,
                                            this.Delimiter);
    //                                    getOutputString(ref tblPattern,
    //                                                    rowsCounter);
                                    // output a row
                                    WriteObject(outString);
                                }
                            }
                        //}
                    }
            
            
            } catch {

            
                if (gridPattern==null)
                {
                    WriteVerbose(this, "couldn't get TablePattern");
                    WriteVerbose(this, "trying to enumerate columns and rows");
                    
                    
                    bool res2 = 
                        UIAHelper.GetHeaders(ref _control, out strData, this.Delimiter);
                    if (res2){
                        WriteObject(strData);
                    } else {
                        WriteVerbose(this, strData);
                    }
                    
                    
                    System.Collections.Generic.List<string> rows = 
                        UIAHelper.GetOutputStringUsingItemsValuePattern(this.InputObject,
                                                                        this.Delimiter);
                    if (rows.Count>0){
                        // RunScriptBlocks(this);
                        RunOnSuccessScriptBlocks(this);
                        foreach(string row in rows){
                            WriteObject(row);
                        }
                    }
                }
            }
        }
    }
}
