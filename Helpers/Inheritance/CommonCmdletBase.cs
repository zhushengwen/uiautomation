/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 12/2/2011
 * Time: 5:51 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Management.Automation.Runspaces;
using System.Windows.Automation;

namespace UIAutomation
{
    /// <summary>
    /// The CommonCmdletBase class in the top of cmdlet hierarchy.
    /// </summary>
    [Cmdlet(VerbsCommon.Show,"UIAModuleSettings")]
    public class CommonCmdletBase : PSCmdlet
    // internal class CommonCmdletBase : PSCmdlet
    {
        #region Constructor
        public CommonCmdletBase()
        {
            #region creating the log file
            try{
                Global.CreateLogFile();
            } catch { }
            #endregion creating the log file
            CurrentData.LastCmdlet = CmdletName(this);
        }
        #endregion Constructor
        
        #region Properties
        protected AutomationElement EventSource { get; set; }
        protected AutomationEventArgs EventArgs { get; set; }
        #endregion Properties
        
        protected override void BeginProcessing()
        {
            WriteObject("[UIAutomation.Preferences]::Timeout = " + 
                        Preferences.Timeout.ToString());
            WriteObject("[UIAutomation.Preferences]::SleepInterval = " + 
                        Preferences.OnSleepDelay.ToString());
            WriteObject("[UIAutomation.Preferences]::TranscriptInterval = " + 
                        Preferences.TranscriptInterval.ToString());
            WriteObject("[UIAutomation.Preferences]::OnErrorScreenShot = " + 
                        Preferences.OnErrorScreenShot.ToString());
            WriteObject("[UIAutomation.Preferences]::ScreenShotFolder = " + 
                        Preferences.ScreenShotFolder);
        }
        
        protected override void EndProcessing()
        {
            #region close the log
            try{
                Global.CloseLogFile();
            } catch { }
            #endregion close the log
        }
        
        #region Write methods
        private void WriteLog(string record)
        {
            try{
                Global.WriteToLogFile(record);
            } catch (Exception e){
                WriteVerbose(this, "Unable to write to the log file: " +
                             Preferences.LogPath);
                WriteVerbose(this, e.Message);
            }
        }
        
            #region Cmdlet Signature
        internal string CmdletName(CommonCmdletBase cmdlet)
        {
            string result = String.Empty;
            if (cmdlet==null) return result;
            result = 
                cmdlet.GetType().Name;
            result = 
                result.Replace("UIA", "-UIA").Replace("Command", "");
            return result;
        }
            
        private string CmdletSignature(CommonCmdletBase cmdlet)
        {
            string result = CmdletName(cmdlet);
            result += ": ";
            return result;
        }
            #endregion Cmdlet Signature
        
        internal protected void WriteVerbose(CommonCmdletBase cmdlet, string text)
        {
            string reportString = 
                CmdletSignature(cmdlet) +  
                text;
            if (cmdlet!=null) try{ WriteVerbose(reportString); } catch{ }
            WriteLog(reportString);
        }
        
        internal protected void WriteVerbose(CommonCmdletBase cmdlet, object obj)
        {
            string reportString = 
                CmdletSignature(cmdlet) +  
                obj.ToString();
            if (cmdlet!=null) try{ WriteVerbose(reportString); } catch{ }
            WriteLog(reportString);
        }
        
        internal protected void WriteDebug(CommonCmdletBase cmdlet, string text)
        {
            string reportString = 
                CmdletSignature(cmdlet) +  
                text;
            WriteLog(reportString);
        }
        
        internal protected void WriteDebug(CommonCmdletBase cmdlet, object obj)
        {
            string reportString = 
                CmdletSignature(cmdlet) +  
                obj.ToString();
            WriteLog(reportString);
        }
        
        internal protected void WriteObject(CommonCmdletBase cmdlet, object obj)
        {
            // Global.
            // UIAHelper.Highlight(obj);
            
            //
            //
            //
            //
            if (cmdlet!=null){
                try{
                    AutomationElement elt = obj as AutomationElement;
                    WriteVerbose(this, "current cmdlet: " +  this.GetType().Name);
                    // WriteVerbose(this, "last: " +  CurrentData.LastCmdlet);
                    WriteVerbose(this, "highlighting the following element:");
                    WriteVerbose(this, "Name = " + elt.Current.Name);
                    WriteVerbose(this, "AutomationId = " + elt.Current.AutomationId);
                    WriteVerbose(this, "ControlType = " + elt.Current.ControlType.ProgrammaticName);
                    WriteVerbose(this, "X = " + elt.Current.BoundingRectangle.X.ToString());
                    WriteVerbose(this, "Y = " + elt.Current.BoundingRectangle.Y.ToString());
                    WriteVerbose(this, "Width = " + elt.Current.BoundingRectangle.Width.ToString());
                    WriteVerbose(this, "Height = " + elt.Current.BoundingRectangle.Height.ToString());
                } catch (Exception eee) {
                    WriteVerbose(this, eee.Message);
                }
            }
            //
            //
            //
            //
            
            if (Preferences.Highlight || ((HasScriptBlockCmdletBase)cmdlet).Highlight){
                WriteVerbose(this, "run Highlighter");
                try{
                    AutomationElement element = 
                        obj as AutomationElement;
                    UIAHelper.Highlight(element);
                } catch (Exception ee) {
                    WriteVerbose(this, "unable to highlight: " + ee.Message);
                    WriteVerbose(this, obj.ToString());
                }
//                try{ UIAHelper.Highlight(obj); } catch (Exception ee) {
//                    WriteVerbose(this, "unable to highlight: " + ee.Message);
//                }
            }
            if (cmdlet!=null){
                // run scriptblocks
                if (cmdlet is HasScriptBlockCmdletBase){
                    // if (obj==null || (obj is bool && ((bool)obj)==false)){
                    if (obj==null){
                        RunOnErrorScriptBlocks(((HasScriptBlockCmdletBase)cmdlet));
                    } else if (obj is bool && ((bool)obj)==false){
                        RunOnErrorScriptBlocks(((HasScriptBlockCmdletBase)cmdlet));
                    } else if (obj!=null){
                        RunOnSuccessScriptBlocks(((HasScriptBlockCmdletBase)cmdlet));
                    }
                }
                CurrentData.LastResult = obj;
                if (((HasScriptBlockCmdletBase)cmdlet).TestResultLabel!=null &&
                    ((HasScriptBlockCmdletBase)cmdlet).TestResultLabel.Length>0){
                    CurrentData.AddTestResult(((HasScriptBlockCmdletBase)cmdlet).TestResultLabel,
                                              ((HasScriptBlockCmdletBase)cmdlet).TestPassed);
                }
            }
            System.Threading.Thread.Sleep(Preferences.OnSuccessDelay);
            if (cmdlet!=null) try{ WriteObject(obj); } catch{ }
            string reportString =
                CmdletSignature(cmdlet) +  
                obj.ToString();
            // 20120206 try{ WriteVerbose(reportString); } catch{ }
            if (cmdlet!=null) try{ WriteVerbose(reportString); } catch{ }
            WriteLog(reportString);
            
            // run scriptblocks
//            if (cmdlet is HasScriptBlockCmdletBase &&
//                ((HasScriptBlockCmdletBase)cmdlet).Action!=null &&
//                ((HasScriptBlockCmdletBase)cmdlet).Action.Length>0){
//            if (cmdlet is HasScriptBlockCmdletBase){
//                if (obj!=null){
//                    RunScriptBlocks(((HasScriptBlockCmdletBase)cmdlet));
//                }
//            }
//            if (cmdlet is HasScriptBlockCmdletBase &&
//                ((HasScriptBlockCmdletBase)cmdlet).OnErrorAction!=null &&
//                ((HasScriptBlockCmdletBase)cmdlet).OnErrorAction.Length>0){
        }
        
        private void writeErrorToTheList(ErrorRecord err)
        {
            CurrentData.Error.Add(err);
            if (CurrentData.Error.Count>Preferences.MaximumErrorCount){
                do{
                    CurrentData.Error.RemoveAt(0);
                } while (CurrentData.Error.Count>Preferences.MaximumErrorCount);
                CurrentData.Error.Capacity = Preferences.MaximumErrorCount;
            }
        }
        
        protected void WriteError(CommonCmdletBase cmdlet, ErrorRecord err, bool terminating)
        {
            if (cmdlet!=null){
                // run scriptblocks
                if (cmdlet is HasScriptBlockCmdletBase){
                    RunOnErrorScriptBlocks(((HasScriptBlockCmdletBase)cmdlet));
                }
                // write error to the test results collection
                // CurrentData.TestResults[CurrentData.TestResults.Count - 1].Details.Add(err);
                CurrentData.AddTestResultDetail(err);
            
                // write test result label
                if (((HasScriptBlockCmdletBase)cmdlet).TestResultLabel!=null &&
                    ((HasScriptBlockCmdletBase)cmdlet).TestResultLabel.Length>0){
                    CurrentData.AddTestResult(((HasScriptBlockCmdletBase)cmdlet).TestResultLabel,
                                              ((HasScriptBlockCmdletBase)cmdlet).TestPassed);
                }
                // write an error to the Error list
                writeErrorToTheList(err);
                System.Threading.Thread.Sleep(Preferences.OnErrorDelay);
                if (terminating){
                    ThrowTerminatingError(err);
                } else {
                    WriteError(err);
                }
            }
        }
        #endregion Write methods
        
        #region sleep and run scripts
        protected void SleepAndRunScriptBlocks(HasTimeoutCmdletBase cmdlet)
        {
            RunOnSleepScriptBlocks(cmdlet);
            System.Threading.Thread.Sleep(Preferences.OnSleepDelay);
            // RunScriptBlocks(cmdlet);
        }
        #endregion sleep and run scripts
        
        #region Invoke-UIAScript
        // 20120209 protected void RunEventScriptBlocks(EventCmdletBase cmdlet)
        internal protected void RunEventScriptBlocks(EventCmdletBase cmdlet)
        {
            System.Collections.Generic.List<ScriptBlock> blocks =
                new System.Collections.Generic.List<ScriptBlock>();
            // WriteVerbose(this, "RunEventScriptBlocks 1 fired");
            if (cmdlet.EventAction!=null &&
                cmdlet.EventAction.Length>0){
                // WriteVerbose(this, "RunEventScriptBlocks 2 fired");
                foreach(ScriptBlock sb in cmdlet.EventAction){
                    // WriteVerbose(this, "RunEventScriptBlocks 3 fired");
                    blocks.Add(sb);
                }
            }
            // WriteVerbose(this, "RunEventScriptBlocks 4 fired");
            runScriptBlocks(blocks, cmdlet, true);
            // runEventScriptBlocks(blocks, cmdlet); //, true);
        }
        
        // 20120209 protected void RunOnSuccessScriptBlocks(HasScriptBlockCmdletBase cmdlet)
        internal protected void RunOnSuccessScriptBlocks(HasScriptBlockCmdletBase cmdlet)
        {
            runTwoScriptBlockCollections(
                Preferences.OnSuccessAction,
                cmdlet.OnSuccessAction,
                cmdlet);
        }
        
        // 20120209 protected void RunOnErrorScriptBlocks(HasScriptBlockCmdletBase cmdlet)
        internal protected void RunOnErrorScriptBlocks(HasScriptBlockCmdletBase cmdlet)
        {
            runTwoScriptBlockCollections(
                Preferences.OnErrorAction,
                cmdlet.OnErrorAction,
                cmdlet);
        }
        
        // 20120209 protected void RunOnSleepScriptBlocks(HasTimeoutCmdletBase cmdlet)
        internal protected void RunOnSleepScriptBlocks(HasTimeoutCmdletBase cmdlet)
        {
            runTwoScriptBlockCollections(
                Preferences.OnSleepAction,
                // 20120209 cmdlet.OnRefreshAction,
                cmdlet.OnSleepAction,
                cmdlet);
        }
        
        private void runTwoScriptBlockCollections(
            ScriptBlock[] blocks1,
            ScriptBlock[] blocks2,
            HasScriptBlockCmdletBase cmdlet)
        {
            System.Collections.Generic.List<ScriptBlock> blocks =
                new System.Collections.Generic.List<ScriptBlock>();
            if (blocks1!=null &&
                blocks1.Length>0){
                foreach(ScriptBlock sb in blocks1){
                    blocks.Add(sb);
                }
            }
            if (blocks2!=null &&
                blocks2.Length>0){
                foreach (ScriptBlock sb in blocks2) {
                    blocks.Add(sb);
                }
            }
            runScriptBlocks(blocks, cmdlet, false);
        }
        
        private void runScriptBlocks(System.Collections.Generic.List<ScriptBlock> blocks,
                                     HasScriptBlockCmdletBase cmdlet,
                                     bool eventHandlers)
        {
            try{
                if (blocks!=null &&
                    blocks.Count>0){
                    // WriteVerbose(this, "runScriptBlocks 1 fired");
                    foreach(ScriptBlock sb in blocks){
                        // WriteVerbose(this, "runScriptBlocks 2 fired");
                        if (sb!=null){
                            // WriteVerbose(this, "runScriptBlocks 3 fired");
                            try{
                                if (eventHandlers){
                                    // WriteVerbose(this, "runScriptBlocks 4 fired");
                                    // runScriptBlock runner = new runScriptBlock(runSBEvent);
                                    // runScriptBlock runner = new runScriptBlock(runSBEvent);
                                    // test
                                    runScriptBlock runner = new runScriptBlock(runSBEvent);
                                    // WriteVerbose(this, "runScriptBlocks 5 fired");
                                    runner(sb, cmdlet.EventSource, cmdlet.EventArgs);
                                    // WriteVerbose(this, "runScriptBlocks 6 fired");
                                } else {
                                    // runScriptBlock runner = new runScriptBlock(runSB);
                                    runScriptBlock runner = new runScriptBlock(runSBAction);
                                    runner(sb, cmdlet.EventSource, cmdlet.EventArgs);
                                }
                            } catch (Exception eInner){
                                ErrorRecord err = 
                                    new ErrorRecord(
                                        eInner,
                                        "InvokeException",
                                        ErrorCategory.OperationStopped,
                                        sb);
                                err.ErrorDetails = 
                                    new ErrorDetails("Error in " +
                                                     sb.ToString());
                                WriteError(this, err, false);
                            }
                        }
                    }
                }
            } catch (Exception eOuter) {
                WriteError(this, 
                           new ErrorRecord(eOuter, "runScriptBlocks", ErrorCategory.InvalidArgument, null),
                           true);
            }
        }
        #endregion Invoke-UIAScript
        
        internal protected System.DateTime startDate;
        protected System.Windows.Automation.AutomationElement _window = null;
        protected System.Windows.Automation.AutomationElement aeCtrl = null;
        internal protected System.Windows.Automation.AutomationElement rootElement;
        
        #region Get-UIAControl
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "get")]
        public AndCondition getControlConditions(GetControlCmdletBase cmdlet)
        {
            System.Windows.Automation.ControlType ctrlType = null;
            System.Windows.Automation.AndCondition conditions = null;
            System.Windows.Automation.PropertyCondition condition = null;
            if (cmdlet.ControlType!=null && cmdlet.ControlType.Length>0){
                ctrlType = 
                    UIAHelper.GetControlTypeByTypeName(cmdlet.ControlType);
            }
            System.Windows.Automation.PropertyCondition ctrlTypeCondition = null,
                classCondition = null, titleCondition = null, autoIdCondition = null;
            WriteVerbose(cmdlet, "ctrlType = " + ctrlType.ProgrammaticName);
            int conditionsCounter = 0;
            if (ctrlType!=null){
                ctrlTypeCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.ControlTypeProperty,
                                ctrlType);
                WriteVerbose(cmdlet, "ControlTypeProperty '" +
                             ctrlType.ProgrammaticName + "' is used");
                conditionsCounter++;
            }
            if (cmdlet.Class!=null && cmdlet.Class!="")
            {
                classCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.ClassNameProperty,
                                cmdlet.Class);
                WriteVerbose(cmdlet, "ClassNameProperty '" + 
                             cmdlet.Class + "' is used");
                conditionsCounter++;
            }
            if (cmdlet.AutomationId!=null && cmdlet.AutomationId!="")
            {
                autoIdCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.AutomationIdProperty,
                                cmdlet.AutomationId);
                WriteVerbose(cmdlet, "AutomationIdProperty '" + 
                             cmdlet.AutomationId + "' is used");
                conditionsCounter++;
            }
            if (cmdlet.Name!=null && cmdlet.Name!="") // allow empty name
            {
                titleCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.NameProperty,
                                cmdlet.Name);
                WriteVerbose(cmdlet, "NameProperty '" + 
                             cmdlet.Name + "' is used");
                conditionsCounter++;
            }

            if (conditionsCounter>1)
            {
                try{
                    System.Collections.ArrayList l = new System.Collections.ArrayList();
                    if (classCondition!=null)l.Add(classCondition);
                    if (ctrlTypeCondition!=null)l.Add(ctrlTypeCondition);
                    if (titleCondition!=null)l.Add(titleCondition);
                    if (autoIdCondition!=null)l.Add(autoIdCondition);
                    System.Type t = typeof(System.Windows.Automation.Condition);
                    System.Windows.Automation.Condition[] conds = 
                        ((System.Windows.Automation.Condition[])l.ToArray(t));
                conditions =
                    new System.Windows.Automation.AndCondition(conds);
                WriteVerbose(cmdlet, "used conditions " + 
                             "ClassName = '" + classCondition.Value + "', " + 
                             "ControlType = '" + ctrlTypeCondition.Value + "', " + 
                             "Name = '" + titleCondition.Value + "', " + 
                             "AutomationId = '" + autoIdCondition.Value + "'");
                } catch (Exception eConditions){
                    WriteDebug(cmdlet, "conditions related exception " +
                                eConditions.Message);
                }
            } else if (conditionsCounter==1)
            {
                if (classCondition!=null){ condition = classCondition; }
                else if (ctrlTypeCondition!=null){ condition = ctrlTypeCondition; }
                else if (titleCondition!=null){ condition = titleCondition; }
                else if (autoIdCondition!=null){ condition = autoIdCondition; }
                WriteVerbose(cmdlet, "condition " + 
                             condition.GetType().Name + "'" + 
                             condition.Value + "' is used");
            }
            else if (conditionsCounter==0)
            {
                WriteVerbose(cmdlet, "neither ControlType nor Class nor Title are present");
                WriteObject(null); //# produce the output
                return null;
            }
            try{
                if (conditions!=null){
                    WriteVerbose(cmdlet, "conditions: " +
                                 conditions.GetConditions());
                } else if (condition!=null){
                    WriteVerbose(cmdlet, "conditions: " +
                                 condition);
                    conditions = 
                        new AndCondition(condition,
                                         Condition.TrueCondition);
                }
                return conditions;
            } catch {
                WriteVerbose(cmdlet, "conditions or condition are null");
                return null;
            }
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "get")]
        protected void getControl(GetControlCmdletBase cmdlet)
        {
            aeCtrl = null;
            System.Windows.Automation.AndCondition conditions = null;
            conditions = getControlConditions(cmdlet);
            
            int processId = 0;
            do {
                if (conditions!=null){ // && condition==null){
                    aeCtrl = _window.FindFirst(System.Windows.Automation.TreeScope.Descendants,
                                                   conditions);
//                } else if (conditions==null && condition!=null){
//                    aeCtrl = _window.FindFirst(System.Windows.Automation.TreeScope.Descendants,
//                                                   condition);
                }
                processId = _window.Current.ProcessId;
                
                // if (aeCtrl==null && cmdlet.Title.Length>0){
                if (aeCtrl==null && cmdlet.Name.Length>0){
                // using API
                    aeCtrl =
                        UIAHelper.GetControlByTitle(cmdlet.InputObject, 
                                                    cmdlet.Name);
                                                    // cmdlet.Title);
                }
                
                if (aeCtrl!=null)
                {
                    WriteVerbose(cmdlet, "aeCtrl = " + aeCtrl.Current.Name);
                    break;
                }
                SleepAndRunScriptBlocks(cmdlet);
                // System.Threading.Thread.Sleep(Preferences.SleepInterval);
                ////impossible due to inheritance and the absense of scriptblock here
                // SleepAndRunScriptBlocks(cmdlet);
                System.DateTime nowDate = System.DateTime.Now;
                
                try{
                    WriteVerbose(cmdlet, "class: '" +
                                 cmdlet.Class + 
                                 "', control type: '" + 
                                 cmdlet.ControlType + 
                                 "' , title: '" +
                                 cmdlet.Name +                                 
                                 // cmdlet.Title +
                                 "' , seconds: " + 
                                 ((nowDate - startDate).TotalSeconds).ToString());
                } catch { }
                
                if (aeCtrl!=null || (nowDate - startDate).TotalSeconds>cmdlet.Timeout/1000)
                {
                    if (aeCtrl==null){
                        ErrorRecord err = 
                            new ErrorRecord(
                                new Exception(),
                                "ControlIsNull",
                                ErrorCategory.OperationTimeout,
                                aeCtrl);
                        err.ErrorDetails =
                            new ErrorDetails(
                                CmdletSignature(cmdlet) + "timeout expired for class: ' + " +
                                cmdlet.Class + 
                                ", control type: " + 
                                cmdlet.ControlType + 
                                ", title: " +
                                cmdlet.Name);
                                // cmdlet.Title);
                        UIAHelper.GetDesktopScreenshot("Get-UIAControl_ControlEqNull");
                        WriteError(this, err, true);
                    } else{
                        WriteVerbose(cmdlet, "got the control: " +
                                     aeCtrl);
                    }
                    break;
                }
                else{
                    rootElement =
                        System.Windows.Automation.AutomationElement.RootElement;
                    if (processId>0){
                    try{
                        System.Windows.Automation.PropertyCondition pIDcondition =
                            new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.ProcessIdProperty,
                                            processId);
                            _window =
                                rootElement.FindFirst(System.Windows.Automation.TreeScope.Children,
                                                     pIDcondition);
                        } catch {//"process is gone"
                            // get new window
                        }
                    } else {
                        WriteVerbose(cmdlet, "failed to get the process Id");
                        WriteObject(this, null);
                        return;
                    } //#describe the output
                }
            } while (cmdlet.Wait);
            if (aeCtrl!=null)
            {
                WriteVerbose(cmdlet, aeCtrl);
            //} // 20120127
            // if (SetFocus){ aeCtrl.SetFocus(); }
            // 20120208 if (cmdlet.Highlight){ Global.PaintRectangle(aeCtrl); }
            } // 20120127
            WriteObject(this, aeCtrl);
            return;
        }
        #endregion Get-UIAControl
        
        #region Action delegate
        private void runSBEvent(ScriptBlock sb, 
                                AutomationElement src,
                                AutomationEventArgs e)
        {
            // 20120206 Collection<PSObject> psObjects = null;
            try{
                System.Management.Automation.Runspaces.Runspace.DefaultRunspace =
                    RunspaceFactory.CreateRunspace();
                try{
                    System.Management.Automation.Runspaces.Runspace.DefaultRunspace.Open();
                } catch (Exception e1){
                    ErrorRecord err = 
                        new ErrorRecord(e1,
                                        "ErrorOnOpeningRunspace",
                                        ErrorCategory.InvalidOperation,
                                        sb);
                    err.ErrorDetails = 
                        new ErrorDetails("Unable to run a scriptblock");
                    WriteError(this, err, false);
                }
                try{
//                    System.Management.Automation.Runspaces.Runspace.
//                        DefaultRunspace.SessionStateProxy.SetVariable("src", src);
//                    System.Management.Automation.Runspaces.Runspace.
//                        DefaultRunspace.SessionStateProxy.SetVariable("e", e);
//                    System.Management.Automation.Runspaces.Runspace.
//                        DefaultRunspace.SessionStateProxy.SetVariable("global:src1", src);
//                    System.Management.Automation.Runspaces.Runspace.
//                        DefaultRunspace.SessionStateProxy.SetVariable("global:e1", e);
                    // Pipeline p1 = 
                    //    System.Management.Automation.Runspaces.Runspace.
                    //    DefaultRunspace.CreateNestedPipeline(sb.ToString(), false);
                    // p1.Input.Write(src);
                    // p1.Input.Write(e);
                    // psObjects =
                    //    p1.Invoke();
                    System.Collections.Generic.List<object> inputParams = 
                        new System.Collections.Generic.List<object>();
                    inputParams.Add(src);
                    inputParams.Add(e);
                    object[] inputParamsArray = inputParams.ToArray();
                    // psObjects = 
                        sb.InvokeReturnAsIs(inputParamsArray);
                        // sb.Invoke(inputParamsArray);
                    
                } catch (Exception e2){
                    ErrorRecord err = 
                        new ErrorRecord(e2,
                                        "ErrorInOpenedRunspace",
                                        ErrorCategory.InvalidOperation,
                                        sb);
                    err.ErrorDetails = 
                        new ErrorDetails("Unable to run a scriptblock");
                    WriteError(this, err, true);
                }
//            psObjects =
//                sb.Invoke();
            } catch (Exception eOuter) {
                ErrorRecord err = 
                    new ErrorRecord(eOuter,
                                    "ErrorInInvokingScriptBlock", //"ErrorinCreatingRunspace",
                                    ErrorCategory.InvalidOperation,
                                    System.Management.Automation.Runspaces.Runspace.DefaultRunspace);
                err.ErrorDetails = 
                    new ErrorDetails("Unable to issue the following command:\r\n" + 
                                     "System.Management.Automation.Runspaces.Runspace.DefaultRunspace = RunspaceFactory.CreateRunspace();");
            }
        }
        #endregion Action delegate
        
        #region Action delegate
        private void runSBAction(ScriptBlock sb, 
                                 AutomationElement src,
                                 AutomationEventArgs e)
        {
            Collection<PSObject> psObjects = null;
            try{
                psObjects =
                    sb.Invoke();
            } catch (Exception eOuter) {
                ErrorRecord err = 
                    new ErrorRecord(eOuter,
                                    "ErrorInInvokingScriptBlock",
                                    ErrorCategory.InvalidOperation,
                                    System.Management.Automation.Runspaces.Runspace.DefaultRunspace);
                err.ErrorDetails = 
                    new ErrorDetails("Unable to issue the following command:\r\n" + 
                                     "System.Management.Automation.Runspaces.Runspace.DefaultRunspace = RunspaceFactory.CreateRunspace();");
            }
        }
        #endregion Action delegate
    
    }
    #region Action delegate
    delegate void runScriptBlock(ScriptBlock sb, 
                                 AutomationElement src, 
                                 AutomationEventArgs e);
    #endregion Action delegate
}
