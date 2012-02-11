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
    /// Description of WaitUIAControlIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAControlIsEnabled")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAControlIsEnabledCommand : WaitCmdletBase
    {
        #region Constructor
        public WaitUIAControlIsEnabledCommand()
        {
            this.ControlType = null;
        }
        #endregion Constructor

        #region Parameters
        #endregion Parameters

        // 20120206 private System.DateTime startDate;
        protected ControlType ControlType { get; set; }

        protected override void BeginProcessing(){
            WriteVerbose(this, "Timeout " + Timeout.ToString());
            try{
                this.ControlType =
                    this.InputObject.Current.ControlType;
            } catch { }
            
            if (this.InputObject!=null && 
                this.InputObject.Current.ControlType!=null){
                WriteVerbose(this, "ControlType " + 
                             this.ControlType.ProgrammaticName);
            }
            startDate = System.DateTime.Now;
        }
        
        protected override void ProcessRecord(){
            if (!base.CheckControl(this)) return;
            
            System.Windows.Automation.AutomationElement _control = null;
            
//            try{
//                _control = 
//                    (System.Windows.Automation.AutomationElement)this.Control;
            if (this.ControlType!=this.InputObject.Current.ControlType){
//                WriteDebug(this, "Control is not of " + 
//                           this.ControlType.ProgrammaticName + 
//                           " type");
                WriteObject(this, null);
                ErrorRecord err = 
                    new ErrorRecord(
                        new Exception(),
                        "WrongControlType",
                        ErrorCategory.InvalidArgument,
                        this.InputObject);
                err.ErrorDetails = 
                    new ErrorDetails("Control is not of " +
                                     this.ControlType.ProgrammaticName +
                                     " type");
                // 20120209 WriteError(this, err);
                    
//                WriteError(this, new ErrorRecord(new Exception(), 
//                                           "Control is not of " +
//                                           this.ControlType.ProgrammaticName + 
//                                           " type",
//                                           ErrorCategory.InvalidArgument,
//                                           this.ControlType));
                UIAHelper.GetDesktopScreenshot(CmdletName(this) + "_BadControl");
                // 20120209 return;
                WriteError(this, err, true);
            }
//            } catch {
//                WriteDebug(this, "Control is not an AutomationElement");
//                WriteObject(this, null);
//                UIAHelper.GetDesktopScreenshot(CmdletName(this) + "_BadControl");
//                return;
//            }
            
            _control = this.InputObject;
            
            this.Wait = !(_control.Current).IsEnabled;
            do
            {
                // RunOnRefreshScriptBlocks(this);
                // System.Threading.Thread.Sleep(Preferences.SleepInterval);
                SleepAndRunScriptBlocks(this);
                // if (
                System.DateTime nowDate = System.DateTime.Now;
                try{
                    WriteVerbose(this,
                                 "AutomationID: " + 
                                 _control.Current.AutomationId +
                                 ", title: " + 
                                 _control.Current.Name + 
                                 ", Enabled = " + 
                                 _control.Current.IsEnabled.ToString() +
                                 ", seconds: " + 
                                 ((nowDate - startDate).TotalSeconds).ToString());
                } catch { }
                if (!base.CheckControl(this))
                {
                    WriteObject(this, false);
                    UIAHelper.GetDesktopScreenshot(CmdletName(this) + "_NoControl");
                    return;
                }
                this.Wait = !(_control.Current).IsEnabled;
                if ((nowDate - startDate).TotalSeconds>this.Timeout/1000)
                {
                    WriteVerbose(this, "timeout expired for AutomationId: " + 
                                 _control.Current.AutomationId +
                                ", title: " +
                                 _control.Current.Name);
                    UIAHelper.GetDesktopScreenshot(CmdletName(this) + "_Timeout");
                    WriteError(this, new ErrorRecord(new Exception(),
                                               CmdletName(this) + ": timeout expired for AutomationId: " + 
                                               _control.Current.AutomationId +
                                               ", title: " +
                                               _control.Current.Name,
                                               ErrorCategory.OperationTimeout,
                                               _control), true);
                }
                if (_control==null){
                    WriteVerbose(this, "the control is unavailable");
                    UIAHelper.GetDesktopScreenshot(CmdletName(this) + "_ControlEqNull");
                    WriteError(this, new ErrorRecord(new Exception(),
                                               CmdletName(this) + ": control is unavailable. AutomationId: " + 
                                               _control.Current.AutomationId +
                                               ", title: " +
                                               _control.Current.Name,
                                               ErrorCategory.OperationTimeout,
                                               _control), true);
                }
            } while (Wait);
            
            WriteObject(this, _control);
        }
    }
    
    /// <summary>
    /// Description of WaitUIAButtonIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAButtonIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAButtonIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAButtonIsEnabledCommand(){ this.ControlType = ControlType.Button; } }

    /// <summary>
    /// Description of WaitUIACalendarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIACalendarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIACalendarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIACalendarIsEnabledCommand(){ this.ControlType = ControlType.Calendar; } }
    
    /// <summary>
    /// Description of WaitUIACheckBoxIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIACheckBoxIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIACheckBoxIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIACheckBoxIsEnabledCommand(){ this.ControlType = ControlType.CheckBox; } }
    
    /// <summary>
    /// Description of WaitUIAComboBoxIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAComboBoxIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAComboBoxIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAComboBoxIsEnabledCommand(){ this.ControlType = ControlType.ComboBox; } }
    
    /// <summary>
    /// Description of WaitUIACustomIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIACustomIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIACustomIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIACustomIsEnabledCommand(){ this.ControlType = ControlType.Custom; } }
    
    /// <summary>
    /// Description of WaitUIADataGridIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIADataGridIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIADataGridIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIADataGridIsEnabledCommand(){ this.ControlType = ControlType.DataGrid; } }
    
    /// <summary>
    /// Description of WaitUIADataItemIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIADataItemIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIADataItemIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIADataItemIsEnabledCommand(){ this.ControlType = ControlType.DataItem; } }
    
    /// <summary>
    /// Description of WaitUIADocumentIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIADocumentIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIADocumentIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIADocumentIsEnabledCommand(){ this.ControlType = ControlType.Document; } }

    /// <summary>
    /// Description of WaitUIAEditIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAEditIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAEditIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAEditIsEnabledCommand(){ this.ControlType = ControlType.Edit; } }
    
    /// <summary>
    /// Description of WaitUIATextBoxIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATextBoxIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATextBoxIsEnabledCommand : WaitUIAEditIsEnabledCommand
    { public WaitUIATextBoxIsEnabledCommand(){ this.ControlType = ControlType.Edit; } }
    
    /// <summary>
    /// Description of WaitUIAGroupIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAGroupIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAGroupIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAGroupIsEnabledCommand(){ this.ControlType = ControlType.Group; } }
    
    /// <summary>
    /// Description of WaitUIAGroupBoxIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAGroupBoxIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAGroupBoxIsEnabledCommand : WaitUIAGroupIsEnabledCommand
    { public WaitUIAGroupBoxIsEnabledCommand(){ this.ControlType = ControlType.Group; } }
    
    /// <summary>
    /// Description of WaitUIAHeaderIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAHeaderIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAHeaderIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAHeaderIsEnabledCommand(){ this.ControlType = ControlType.Header; } }
    
    /// <summary>
    /// Description of WaitUIAHeaderItemIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAHeaderItemIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAHeaderItemIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAHeaderItemIsEnabledCommand(){ this.ControlType = ControlType.HeaderItem; } }
    
    /// <summary>
    /// Description of WaitUIAHyperlinkIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAHyperlinkIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAHyperlinkIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAHyperlinkIsEnabledCommand(){ this.ControlType = ControlType.Hyperlink; } }
    
    /// <summary>
    /// Description of WaitUIALinkLabelIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIALinkLabelIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIALinkLabelIsEnabledCommand : WaitUIAHyperlinkIsEnabledCommand
    { public WaitUIALinkLabelIsEnabledCommand(){ this.ControlType = ControlType.Hyperlink; } }

    /// <summary>
    /// Description of WaitUIAImageIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAImageIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAImageIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAImageIsEnabledCommand(){ this.ControlType = ControlType.Image; } }
    
    /// <summary>
    /// Description of WaitUIAListIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAListIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAListIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAListIsEnabledCommand(){ this.ControlType = ControlType.List; } }
    
    /// <summary>
    /// Description of WaitUIAListItemIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAListItemIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAListItemIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAListItemIsEnabledCommand(){ this.ControlType = ControlType.ListItem; } }
    
    /// <summary>
    /// Description of WaitUIAMenuIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAMenuIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAMenuIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAMenuIsEnabledCommand(){ this.ControlType = ControlType.Menu; } }
    
    /// <summary>
    /// Description of WaitUIAMenuBarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAMenuBarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAMenuBarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAMenuBarIsEnabledCommand(){ this.ControlType = ControlType.MenuBar; } }

    /// <summary>
    /// Description of WaitUIAMenuItemIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAMenuItemIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAMenuItemIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAMenuItemIsEnabledCommand(){ this.ControlType = ControlType.MenuItem; } }
    
    /// <summary>
    /// Description of WaitUIAPaneIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAPaneIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAPaneIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAPaneIsEnabledCommand(){ this.ControlType = ControlType.Pane; } }
    
    /// <summary>
    /// Description of WaitUIAProgressBarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAProgressBarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAProgressBarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAProgressBarIsEnabledCommand(){ this.ControlType = ControlType.ProgressBar; } }
    
    /// <summary>
    /// Description of WaitUIARadioButtonIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIARadioButtonIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIARadioButtonIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIARadioButtonIsEnabledCommand(){ this.ControlType = ControlType.RadioButton; } }
    
    /// <summary>
    /// Description of WaitUIAScrollBarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAScrollBarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAScrollBarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAScrollBarIsEnabledCommand(){ this.ControlType = ControlType.ScrollBar; } }

    /// <summary>
    /// Description of WaitUIASeparatorIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIASeparatorIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIASeparatorIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIASeparatorIsEnabledCommand(){ this.ControlType = ControlType.Separator; } }
    
    /// <summary>
    /// Description of WaitUIASliderIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIASliderIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIASliderIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIASliderIsEnabledCommand(){ this.ControlType = ControlType.Slider; } }
    
    /// <summary>
    /// Description of WaitUIASpinner.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIASpinner")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIASpinnerCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIASpinnerCommand(){ this.ControlType = ControlType.Spinner; } }
    
    /// <summary>
    /// Description of WaitUIASplitButtonIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIASplitButtonIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIASplitButtonIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIASplitButtonIsEnabledCommand(){ this.ControlType = ControlType.SplitButton; } }
    
    /// <summary>
    /// Description of WaitUIAStatusBarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAStatusBarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAStatusBarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAStatusBarIsEnabledCommand(){ this.ControlType = ControlType.StatusBar; } }

    /// <summary>
    /// Description of WaitUIATabIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATabIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATabIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATabIsEnabledCommand(){ this.ControlType = ControlType.Tab; } }
    
    /// <summary>
    /// Description of WaitUIATabItemIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATabIsEnabledItem")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATabItemIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATabItemIsEnabledCommand(){ this.ControlType = ControlType.TabItem; } }
    
    /// <summary>
    /// Description of WaitUIATableIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATableIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATableIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATableIsEnabledCommand(){ this.ControlType = ControlType.Table; } }
    
    /// <summary>
    /// Description of WaitUIATextIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATextIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATextIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATextIsEnabledCommand(){ this.ControlType = ControlType.Text; } }
    
    /// <summary>
    /// Description of WaitUIALabelIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIALabelIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIALabelIsEnabledCommand : WaitUIATextIsEnabledCommand
    { public WaitUIALabelIsEnabledCommand(){ this.ControlType = ControlType.Text; } }
    
    /// <summary>
    /// Description of WaitUIAThumbIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAThumbIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAThumbIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAThumbIsEnabledCommand(){ this.ControlType = ControlType.Thumb; } }

    /// <summary>
    /// Description of WaitUIATitleBarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATitleBarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATitleBarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATitleBarIsEnabledCommand(){ this.ControlType = ControlType.TitleBar; } }
    
    /// <summary>
    /// Description of WaitUIAToolBarIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAToolBarIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAToolBarIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAToolBarIsEnabledCommand(){ this.ControlType = ControlType.ToolBar; } }
    
    /// <summary>
    /// Description of WaitUIAToolTipIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAToolTipIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAToolTipIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAToolTipIsEnabledCommand(){ this.ControlType = ControlType.ToolTip; } }
    
    /// <summary>
    /// Description of WaitUIATreeIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATreeIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATreeIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATreeIsEnabledCommand(){ this.ControlType = ControlType.Tree; } }
    
    /// <summary>
    /// Description of WaitUIATreeItemIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIATreeItemIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIATreeItemIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIATreeItemIsEnabledCommand(){ this.ControlType = ControlType.TreeItem; } }
    
    /// <summary>
    /// Description of WaitUIAChildWindowIsEnabled.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "UIAChildWindowIsEnabled")]
    [OutputType(typeof(object))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class WaitUIAChildWindowIsEnabledCommand : WaitUIAControlIsEnabledCommand
    { public WaitUIAChildWindowIsEnabledCommand(){ this.ControlType = ControlType.Window; } }
    
}
