/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 29.11.2011
 * Time: 14:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of PatternCmdletBase.
    /// </summary>
    //[Cmdlet(VerbsCommon.Set, "PatternCmdletBase")]
    //[Cmdlet]
    public class PatternCmdletBase : HasControlInputCmdletBase
    {
        #region Constructor
        public PatternCmdletBase()
        {
        }
        #endregion Constructor

        protected PatternCmdletBase Child { get; set; }
        
        #region Parameters
        #endregion Parameters
        
        #region Properties
        protected string WhatToDo { get; set; }
        #endregion Properties
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            
            System.Windows.Automation.AutomationElement _control = null;
            try{
                _control = 
                    (System.Windows.Automation.AutomationElement)InputObject;
            } catch (Exception eControlTypeException) {
                WriteDebug("PatternCmdletBase: Control is not an AutomationElement");
                WriteDebug("PatternCmdletBase: " + eControlTypeException.Message);
                WriteObject(this, false);
                return;
            }
            switch (WhatToDo)
            {
// not yet implemented
//                case "Dock":
//                    pattern = 
//                        (System.Windows.Automation.DockPattern)pt;
//                    break;
                case "Expand":
                    ExpandCollapsePattern expandPattern = 
                        _control.GetCurrentPattern(ExpandCollapsePattern.Pattern)
                        as ExpandCollapsePattern;
                    if (expandPattern!=null)
                    {
                        expandPattern.Expand();
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get ExpandCollapsePattern");
                        WriteObject(this, false);
                    }
                    break;
                case "Collapse":
                    ExpandCollapsePattern collapsePattern = 
                        _control.GetCurrentPattern(ExpandCollapsePattern.Pattern)
                        as ExpandCollapsePattern;
                    if (collapsePattern!=null)
                    {
                        collapsePattern.Collapse();
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get ExpandCollapsePattern");
                        WriteObject(this, false);
                    }
                    break;
                case "GridItem":
// not yet implemented
//                    GridItemPattern giPattern =
//                        _control.GetCurrentPattern(GridItemPattern.Pattern)
//                        as GridItemPattern;
//                    
//                    giPattern.Current.
                    
                    break;
// not yet implemented
//                case "Grid":
//                    GridPattern gridPattern = 
//                        _control.GetCurrentPattern(GridPattern.Pattern)
//                        as GridPattern;
//                    if (gridPattern!=null)
//                    {
//                        // invokePattern.Invoke();
//                        // gridPattern.Current.RowCount
//                        // gridPattern.Current.ColumnCount
//                        // gridPattern.GetItem(int row, int column);
//                        WriteObject(true);
//                    }
//                    else{
//                        WriteVerbose(this, "couldn't get GridPattern");
//                        WriteObject(false);
//                    }
//                    break;
                case "Invoke":
                    InvokePattern invokePattern = 
                        _control.GetCurrentPattern(InvokePattern.Pattern)
                        as InvokePattern;
                    if (invokePattern!=null)
                    {
                        invokePattern.Invoke();
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get InvokePattern");
                        WriteObject(this, false);
                    }
                    break;
// not yet implemented
//                case "MultipleView":
//                    pattern = 
//                        (System.Windows.Automation.MultipleViewPattern)pt;
//                    break;
                case "RangeValueGet":
                    RangeValuePattern rvPatternGet = 
                        _control.GetCurrentPattern(RangeValuePattern.Pattern)
                        as RangeValuePattern;
                    if (rvPatternGet!=null)
                    {
                        WriteObject(this, rvPatternGet.Current.Value);
                        // if (this.PassThru){
                        //    WriteObject(this.InputObject);
                        //} else {
                        //    WriteObject(true);
                        //}
                    }
                    break;
                case "RangeValueSet":
                    RangeValuePattern rvPatternSet = 
                        _control.GetCurrentPattern(RangeValuePattern.Pattern)
                        as RangeValuePattern;
                    if (rvPatternSet!=null)
                    {
                        rvPatternSet.SetValue(
                            ((Commands.InvokeUIARangeValuePatternSetCommand)Child).Value);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    break;
// not yet implemented
//                case "ScrollItem":
//                    pattern = 
//                        (System.Windows.Automation.ScrollItemPattern)pt;
//                    break;
//                case "Scroll":
//                    pattern = 
//                        (System.Windows.Automation.ScrollPattern)pt;
//                    break;
                case "SelectionItem":
                    SelectionItemPattern selItemPattern = 
                        _control.GetCurrentPattern(SelectionItemPattern.Pattern)
                        as SelectionItemPattern;
                    if (selItemPattern!=null)
                    {
                        selItemPattern.Select();
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get SelectionItemPattern");
                        WriteObject(this, false);
                    }
                    break;
                case "Selection":
                    SelectionPattern selPattern = 
                        _control.GetCurrentPattern(SelectionPattern.Pattern)
                        as SelectionPattern;
                    if (selPattern!=null)
                    {
                        System.Windows.Automation.AutomationElement[] selection =
                            selPattern.Current.GetSelection();
                        WriteObject(this, selection);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        //} else {
                        //    WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get SelectionPattern");
                        WriteObject(this, false);
                    }
                    break;
// not yet implemented
//                case "TableItem":
//                    pattern = 
//                        (System.Windows.Automation.TableItemPattern)pt;
//                    break;
// not yet implemented
//                case "Table":
//                    pattern = 
//                        (System.Windows.Automation.TablePattern)pt;
//                    break;
// not yet implemented
                case "Text":
//                    pattern = 
//                        (System.Windows.Automation.TextPattern)pt;
//                    break;
                    TextPattern textPattern = 
                        _control.GetCurrentPattern(TextPattern.Pattern)
                        as TextPattern;
                    if (textPattern!=null){
                        // textPattern.DocumentRange.// temporarily 
                    }
                    break;
                case "Toggle":
                    TogglePattern togglePattern = 
                        _control.GetCurrentPattern(TogglePattern.Pattern)
                        as TogglePattern;
                    if (togglePattern!=null)
                    {
                        togglePattern.Toggle();
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get TogglePattern");
                        WriteObject(this, false);
                    }
                    break;
                case "TransformMove":
                    TransformPattern transformMovePattern = 
                        _control.GetCurrentPattern(TransformPattern.Pattern)
                        as TransformPattern;
                    if (transformMovePattern!=null)
                    {
                        transformMovePattern.Move(
                            ((Commands.InvokeUIATransformPatternMoveCommand)Child).TransformMoveX, 
                            ((Commands.InvokeUIATransformPatternMoveCommand)Child).TransformMoveY);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get TransformPattern");
                        WriteObject(this, false);
                    }
                    break;
                case "TransformResize":
                    TransformPattern transformResizePattern = 
                        _control.GetCurrentPattern(TransformPattern.Pattern)
                        as TransformPattern;
                    if (transformResizePattern!=null)
                    {
                        transformResizePattern.Resize(
                            ((Commands.InvokeUIATransformPatternResizeCommand)Child).TransformResizeWidth, 
                            ((Commands.InvokeUIATransformPatternResizeCommand)Child).TransformResizeHeight);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get TransformPattern");
                        WriteObject(this, false);
                    }
                    break;
                case "TransformRotate":
                    TransformPattern transformRotatePattern = 
                        _control.GetCurrentPattern(TransformPattern.Pattern)
                        as TransformPattern;
                    if (transformRotatePattern!=null)
                    {
                        transformRotatePattern.Rotate(
                            ((Commands.InvokeUIATransformPatternRotateCommand)Child).TransformRotateDegrees);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get TransformPattern");
                        WriteObject(this, false);
                    }
                    break;
                case "ValueGet":
                    ValuePattern valuePatternGet = 
                        _control.GetCurrentPattern(ValuePattern.Pattern)
                        as ValuePattern;
                    object result = null;
                    if (valuePatternGet!=null)
                    {
                        result = valuePatternGet.Current.Value;
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get ValuePattern");
                        WriteObject(this, result);
                    }
                    break;
                case "ValueSet":
                    ValuePattern valuePatternSet = 
                        _control.GetCurrentPattern(ValuePattern.Pattern)
                        as ValuePattern;
                    if (valuePatternSet!=null)
                    {
                        valuePatternSet.SetValue(
                            ((Commands.InvokeUIAValuePatternSetCommand)Child).Text);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get ValuePattern. SendKeys is used");
                        _control.SetFocus();
                        System.Windows.Forms.SendKeys.SendWait(
                            ((Commands.InvokeUIAValuePatternSetCommand)Child).Text);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    break;
                case "Window":
                    WindowPattern windowPattern = 
                        _control.GetCurrentPattern(WindowPattern.Pattern)
                        as WindowPattern;
                    if (windowPattern!=null)
                    {
                        windowPattern.SetWindowVisualState(WindowVisualState.Minimized);
                        System.Threading.Thread.Sleep(1000);
                        windowPattern.SetWindowVisualState(WindowVisualState.Normal);
                        windowPattern.WaitForInputIdle(1000);
                        System.Threading.Thread.Sleep(1000);
                        windowPattern.SetWindowVisualState(WindowVisualState.Minimized);
                        System.Threading.Thread.Sleep(1000);
                        windowPattern.SetWindowVisualState(WindowVisualState.Normal);
                        if (this.PassThru){
                            WriteObject(this, this.InputObject);
                        } else {
                            WriteObject(this, true);
                        }
                    }
                    else{
                        WriteVerbose(this, "couldn't get WindowPattern");
                        WriteObject(this, false);
                    }
                    break;
            }
            return;
        }
        
        protected override void EndProcessing()
        {
            this.Child = null;
        }
    }
}
