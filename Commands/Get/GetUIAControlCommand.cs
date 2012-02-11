/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 29.11.2011
 * Time: 4:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;
using System.Xml.Serialization.Configuration;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of GetUIAControl.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAControl")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAControlCommand : GetControlCmdletBase
    {
        #region Constructor
        public GetUIAControlCommand()
        {
        }
        #endregion Constructor

        #region Parameters
        #endregion Parameters

        private string _processName;
        private string _windowName;
            
        protected override void BeginProcessing(){
            WriteVerbose(this, "ControlType = " + ControlType);
            WriteVerbose(this, "Class = " + Class);
            // WriteVerbose(this, "Title = " + Title);
            WriteVerbose(this, "Name = " + Name);
            WriteVerbose(this, "AutomationId = " + AutomationId);
            WriteVerbose(this, "Timeout " + Timeout.ToString());
            
            startDate = System.DateTime.Now;
            // 20120208 if (Highlight){ Global.MinimizeRectangle(); }
        }
        
        protected override void ProcessRecord(){
            
            #region Preparation
            try{
                WriteDebug(this, "Window is null? " + 
                             (InputObject==null));
                _window = InputObject;
            } catch (Exception eCastWindowToAutomationElement){
                WriteDebug(this, "Window is not of AutomationElement type");
                WriteDebug(this, 
                            eCastWindowToAutomationElement.Message);
                WriteObject(this, null);
                UIAHelper.GetDesktopScreenshot(CmdletName(this) + "_BeginProcessing");
                return;
            }
            _windowName = _window.Current.Name;
            _processName = 
                (System.Diagnostics.Process.GetProcessById(_window.Current.ProcessId)).StartInfo.FileName;
            #endregion Preparation
            
            getControl(this);
        }
        
        protected override void EndProcessing()
        {
            aeCtrl = null;
            _window = null;
            rootElement = null;
        }
    }
    
    /// <summary>
    /// Description of GetUIAButton.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAButton")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAButtonCommand : GetUIAControlCommand
    { public GetUIAButtonCommand(){ ControlType = "Button"; }
        //[Parameter(Mandatory=false)]
        // protected string ControlType { get; set; }
    }

    /// <summary>
    /// Description of GetUIACalendar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIACalendar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIACalendarCommand : GetUIAControlCommand
    { public GetUIACalendarCommand(){ ControlType = "Calendar"; } }
    
    /// <summary>
    /// Description of GetUIACheckBox.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIACheckBox")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIACheckBoxCommand : GetUIAControlCommand
    { public GetUIACheckBoxCommand(){ ControlType = "CheckBox"; } }
    
    /// <summary>
    /// Description of GetUIAComboBox.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAComboBox")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAComboBoxCommand : GetUIAControlCommand
    { public GetUIAComboBoxCommand(){ ControlType = "ComboBox"; } }
    
    /// <summary>
    /// Description of GetUIACustom.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIACustom")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIACustomCommand : GetUIAControlCommand
    { public GetUIACustomCommand(){ ControlType = "Custom"; } }
    
    /// <summary>
    /// Description of GetUIADataGrid.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIADataGrid")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIADataGridCommand : GetUIAControlCommand
    { public GetUIADataGridCommand(){ ControlType = "DataGrid"; } }
    
    /// <summary>
    /// Description of GetUIADataItem.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIADataItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIADataItemCommand : GetUIAControlCommand
    { public GetUIADataItemCommand(){ ControlType = "DataItem"; } }
    
    /// <summary>
    /// Description of GetUIADocument.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIADocument")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIADocumentCommand : GetUIAControlCommand
    { public GetUIADocumentCommand(){ ControlType = "Document"; } }

    /// <summary>
    /// Description of GetUIAEdit.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAEdit")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAEditCommand : GetUIAControlCommand
    { public GetUIAEditCommand(){ ControlType = "Edit"; } }
    
    /// <summary>
    /// Description of GetUIATextBox.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATextBox")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATextBoxCommand : GetUIAEditCommand
    { public GetUIATextBoxCommand(){ ControlType = "Edit"; } }
    
    /// <summary>
    /// Description of GetUIAGroup.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAGroup")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAGroupCommand : GetUIAControlCommand
    { public GetUIAGroupCommand(){ ControlType = "Group"; } }
    
    /// <summary>
    /// Description of GetUIAGroupBox.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAGroupBox")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAGroupBoxCommand : GetUIAGroupCommand
    { public GetUIAGroupBoxCommand(){ ControlType = "Group"; } }
    
    /// <summary>
    /// Description of GetUIAHeader.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAHeader")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAHeaderCommand : GetUIAControlCommand
    { public GetUIAHeaderCommand(){ ControlType = "Header"; } }
    
    /// <summary>
    /// Description of GetUIAHeaderItem.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAHeaderItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAHeaderItemCommand : GetUIAControlCommand
    { public GetUIAHeaderItemCommand(){ ControlType = "HeaderItem"; } }
    
    /// <summary>
    /// Description of GetUIAHyperlink.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAHyperlink")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAHyperlinkCommand : GetUIAControlCommand
    { public GetUIAHyperlinkCommand(){ ControlType = "Hyperlink"; } }
    
    /// <summary>
    /// Description of GetUIALinkLabel.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIALinkLabel")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIALinkLabelCommand : GetUIAHyperlinkCommand
    { public GetUIALinkLabelCommand(){ ControlType = "Hyperlink"; } }

    /// <summary>
    /// Description of GetUIAImage.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAImage")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAImageCommand : GetUIAControlCommand
    { public GetUIAImageCommand(){ ControlType = "Image"; } }
    
    /// <summary>
    /// Description of GetUIAList.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAList")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAListCommand : GetUIAControlCommand
    { public GetUIAListCommand(){ ControlType = "List"; } }
    
    /// <summary>
    /// Description of GetUIAListItem.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAListItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAListItemCommand : GetUIAControlCommand
    { public GetUIAListItemCommand(){ ControlType = "ListItem"; } }
    
    /// <summary>
    /// Description of GetUIAMenu.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAMenu")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAMenuCommand : GetUIAControlCommand
    { public GetUIAMenuCommand(){ ControlType = "Menu"; } }
    
    /// <summary>
    /// Description of GetUIAMenuBar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAMenuBar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAMenuBarCommand : GetUIAControlCommand
    { public GetUIAMenuBarCommand(){ ControlType = "MenuBar"; } }

    /// <summary>
    /// Description of GetUIAMenuItem.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAMenuItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAMenuItemCommand : GetUIAControlCommand
    { public GetUIAMenuItemCommand(){ ControlType = "MenuItem"; } }
    
    /// <summary>
    /// Description of GetUIAPane.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAPane")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAPaneCommand : GetUIAControlCommand
    { public GetUIAPaneCommand(){ ControlType = "Pane"; } }
    
    /// <summary>
    /// Description of GetUIAProgressBar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAProgressBar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAProgressBarCommand : GetUIAControlCommand
    { public GetUIAProgressBarCommand(){ ControlType = "ProgressBar"; } }
    
    /// <summary>
    /// Description of GetUIARadioButton.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIARadioButton")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIARadioButtonCommand : GetUIAControlCommand
    { public GetUIARadioButtonCommand(){ ControlType = "RadioButton"; } }
    
    /// <summary>
    /// Description of GetUIAScrollBar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAScrollBar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAScrollBarCommand : GetUIAControlCommand
    { public GetUIAScrollBarCommand(){ ControlType = "ScrollBar"; } }

    /// <summary>
    /// Description of GetUIASeparator.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIASeparator")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIASeparatorCommand : GetUIAControlCommand
    { public GetUIASeparatorCommand(){ ControlType = "Separator"; } }
    
    /// <summary>
    /// Description of GetUIASlider.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIASlider")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIASliderCommand : GetUIAControlCommand
    { public GetUIASliderCommand(){ ControlType = "Slider"; } }
    
    /// <summary>
    /// Description of GetUIASpinner.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIASpinner")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIASpinnerCommand : GetUIAControlCommand
    { public GetUIASpinnerCommand(){ ControlType = "Spinner"; } }
    
    /// <summary>
    /// Description of GetUIASplitButton.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIASplitButton")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIASplitButtonCommand : GetUIAControlCommand
    { public GetUIASplitButtonCommand(){ ControlType = "SplitButton"; } }
    
    /// <summary>
    /// Description of GetUIAStatusBar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAStatusBar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAStatusBarCommand : GetUIAControlCommand
    { public GetUIAStatusBarCommand(){ ControlType = "StatusBar"; } }

    /// <summary>
    /// Description of GetUIATab.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATab")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATabCommand : GetUIAControlCommand
    { public GetUIATabCommand(){ ControlType = "Tab"; } }
    
    /// <summary>
    /// Description of GetUIATabItem.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATabItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATabItemCommand : GetUIAControlCommand
    { public GetUIATabItemCommand(){ ControlType = "TabItem"; } }
    
    /// <summary>
    /// Description of GetUIATable.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATable")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATableCommand : GetUIAControlCommand
    { public GetUIATableCommand(){ ControlType = "Table"; } }
    
    /// <summary>
    /// Description of GetUIAText.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAText")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATextCommand : GetUIAControlCommand
    { public GetUIATextCommand(){ ControlType = "Text"; } }
    
    /// <summary>
    /// Description of GetUIALabel.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIALabel")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIALabelCommand : GetUIATextCommand
    { public GetUIALabelCommand(){ ControlType = "Text"; } }
    
    /// <summary>
    /// Description of GetUIAThumb.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAThumb")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAThumbCommand : GetUIAControlCommand
    { public GetUIAThumbCommand(){ ControlType = "Thumb"; } }

    /// <summary>
    /// Description of GetUIATitleBar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATitleBar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATitleBarCommand : GetUIAControlCommand
    { public GetUIATitleBarCommand(){ ControlType = "TitleBar"; } }
    
    /// <summary>
    /// Description of GetUIAToolBar.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAToolBar")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAToolBarCommand : GetUIAControlCommand
    { public GetUIAToolBarCommand(){ ControlType = "ToolBar"; } }
    
    /// <summary>
    /// Description of GetUIAToolTip.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAToolTip")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAToolTipCommand : GetUIAControlCommand
    { public GetUIAToolTipCommand(){ ControlType = "ToolTip"; } }
    
    /// <summary>
    /// Description of GetUIATree.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATree")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATreeCommand : GetUIAControlCommand
    { public GetUIATreeCommand(){ ControlType = "Tree"; } }
    
    /// <summary>
    /// Description of GetUIATreeItem.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIATreeItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIATreeItemCommand : GetUIAControlCommand
    { public GetUIATreeItemCommand(){ ControlType = "TreeItem"; } }
    
    /// <summary>
    /// Description of GetUIAChildWindow.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAChildWindow")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAChildWindowCommand : GetUIAControlCommand
    { public GetUIAChildWindowCommand(){ ControlType = "Window"; } }
    
}
