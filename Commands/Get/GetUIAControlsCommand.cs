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

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of GetUIAControls.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAControls")]
    [OutputType(new[]{ typeof(object) })]
    // disabled in the current release
    internal class GetUIAControlsCommand : GetCmdletBase
    {
        #region Constructor
        public GetUIAControlsCommand()
        {
            Class = String.Empty;
            Name = String.Empty;
            ControlType = String.Empty;
        }
        #endregion Constructor

        #region Parameters
        [Parameter(Mandatory=false)]
        public string Class { get; set; }
        [Parameter(Mandatory=false)]
        [Alias("Title")]
        public string Name { get; set; }
        [Parameter(Mandatory=false)]
        public string ControlType { get; set; }
        [ValidateNotNullOrEmpty()]
        [Parameter(Mandatory=true, 
            ValueFromPipeline=true, 
            HelpMessage="This is usually the output from Get-UIAWindow" )] 
        public System.Windows.Automation.AutomationElement[] Window { get; set; }
        
        
        
            #region Blocking parameters
//        [Parameter(Mandatory=false)]
//        private SwitchParameter PassThru { get; set; }
            #endregion Blocking parameters
        
        #endregion Parameters

        private new System.Windows.Automation.AutomationElement[] _window = null;
        
        protected override void BeginProcessing(){
            startDate = System.DateTime.Now;
        }
        
        protected override void ProcessRecord(){
            System.Collections.Generic.List<System.Windows.Automation.AutomationElement> aeOutCtrls = 
                new System.Collections.Generic.List<System.Windows.Automation.AutomationElement>();
            System.Windows.Automation.AutomationElementCollection aeCtrls = null;
            // System.Windows.Automation.AutomationElement[] aeCtrls = null;
            System.Windows.Automation.ControlType ctrlType = null;
            System.Windows.Automation.AndCondition conditions = null;
            System.Windows.Automation.PropertyCondition condition = null;
            if (ControlType!=null && ControlType.Length>0){
                ctrlType = 
                    UIAHelper.GetControlTypeByTypeName(this.ControlType);
            }
            // ctrlTypeCondition = classCondition = titleCondition = false;
            System.Windows.Automation.PropertyCondition ctrlTypeCondition = null,
                classCondition = null, titleCondition = null;
            WriteVerbose(this, "ctrlType = " + ctrlType);
            if (ctrlType!=null){
                ctrlTypeCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.ControlTypeProperty,
                                ctrlType);
            }
            if (Class!=null && Class!="")
            {
                classCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.ClassNameProperty,
                                Class);
            }
            // if (Title!=null &&  Title!="")
            if (Name!=null &&  Name!="")
            {
                titleCondition =
                    new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.NameProperty,
                                Name);
                                // Title);
            }
            // if (ctrlTypeCondition && classCondition && titleCondition)
            if (ctrlTypeCondition!=null && classCondition!=null && titleCondition!=null)
            {
                conditions =
                    new System.Windows.Automation.AndCondition(classCondition,
                                                                      ctrlTypeCondition,
                                                                      titleCondition);
            }
            // elseif (ctrlTypeCondition && classCondition);
            else if (ctrlTypeCondition!=null && classCondition!=null)
            {
                conditions =
                    new System.Windows.Automation.AndCondition(classCondition,
                                                                      ctrlTypeCondition);
            }
            // elseif (classCondition && titleCondition);
            else if (classCondition!=null && titleCondition!=null)
            {
                conditions =
                    new System.Windows.Automation.AndCondition(classCondition,
                                                                      titleCondition);
            }
            // elseif (ctrlTypeCondition && titleCondition);
            else if (ctrlTypeCondition!=null && titleCondition!=null)
            {
                conditions =
                    new System.Windows.Automation.AndCondition(ctrlTypeCondition,
                                                                      titleCondition);
            }
            // elseif (ctrlTypeCondition);
            else if (ctrlTypeCondition!=null)
            {
                condition =
                    ctrlTypeCondition;
            }
            // elseif (classCondition);
            else if (classCondition!=null)
            {
                condition =
                    classCondition;
            }
            // elseif (titleCondition);
            else if (titleCondition!=null)
            {
                condition =
                    titleCondition;
            }
//            else
            else if (ctrlTypeCondition==null && classCondition==null && titleCondition==null)
            {
                WriteVerbose(this, "neither ControlType nor Class not Title are present");
                WriteObject(null); //# produce the output
                return;
            }
            try{
                WriteVerbose(this, "conditions: " + 
                             conditions.GetConditions());
            } catch {
                WriteVerbose(this, "conditions: " + conditions);
            }
            int processId = 0;
            do {
        //#Write-Host "do";
            for (int i = 0; i<_window.Length; i++)
            {
//                if (_window is System.Array ||
//                    _window is System.Windows.Automation.AutomationElementCollection)
//                {
                if (conditions!=null && condition==null){
                    aeCtrls = _window[i].FindAll(System.Windows.Automation.TreeScope.Descendants,
                                                   conditions);
                } else if (conditions==null && condition!=null){
                    aeCtrls = _window[i].FindAll(System.Windows.Automation.TreeScope.Descendants,
                                                   condition);
                }
                processId = _window[0].Current.ProcessId;
//                }
                if (aeCtrls.Count>0)
                {
                    foreach(System.Windows.Automation.AutomationElement elt in aeCtrls)
                    {
                        aeOutCtrls.Add(elt);
                    }
                }
            }
//                else
//                {
//                    if (conditions!=null && condition==null){
//            aeCtrls = _window.FindFirst(System.Windows.Automation.TreeScope.//Subtree,
//                                                     conditions);
//                    } else if (conditions==null && condition!=null){
//            aeCtrls = _window.FindFirst(System.Windows.Automation.TreeScope.//Subtree,
//                                                     condition);
//                    }
//                    if (_window is System.Array ||
//                        _window is System.Windows.Automation.AutomationElementCollection){
//                        processId = _window[0].Current.ProcessId;
//                    } else{
//        processId = ((System.Windows.Automation.AutomationElement)Window).Current.ProcessId;
//                    }
//                }
                // System.Threading.Thread.Sleep(Preferences.SleepInterval);
                // RunScriptBlocks(this);
                // SleepAndRunScriptBlocks(this);
                System.DateTime nowDate = System.DateTime.Now;
                WriteVerbose(this, "class: (Class), control type: (ControlType), title: (Title), seconds: ((private:nowDate - private:startDate).TotalSeconds)");
                // if (aeCtrls.Count>0 || (nowDate - startDate).TotalSeconds>this.Timeout/1000)
                if (aeOutCtrls.Count>0 || (nowDate - startDate).TotalSeconds>this.Timeout/1000)
                {
                    if (aeOutCtrls.Count==0){
                        WriteVerbose(this, "timeout expired for class: ' + " +
                                     this.Class + 
                                     ", control type: " + 
                                     this.ControlType + 
                                     ", title: " +
                                     this.Name);
                                     // this.Title);
                    } else{
                        WriteVerbose(this, "got the controls: " +
                                     aeOutCtrls);
                    }
                    break;
                }
                else{
                    System.Windows.Automation.AutomationElement rootElement =
                        System.Windows.Automation.AutomationElement.RootElement;
                    
                    if (processId>0){
                    try{
                        System.Windows.Automation.PropertyCondition pIDcondition =
                            new System.Windows.Automation.PropertyCondition(
                                System.Windows.Automation.AutomationElement.ProcessIdProperty,
                                            processId);
//                        if (Window is System.Array ||
//                            Window is System.Windows.Automation.AutomationElementCollection){
//                            Window[0] =
//                                rootElement.FindFirst(System.Windows.Automation.TreeScope.Children,
//                                                     pIDcondition);
//                        } else{
                            _window[0] =
                                rootElement.FindFirst(System.Windows.Automation.TreeScope.Children,
                                                     pIDcondition);
//                        }
                        } catch {//"process is gone"
                            // get new window
                        }
                    } else {
                        WriteVerbose(this, "failed to get the process Id");
                        WriteObject(this, null);
                        return;
                    } //#describe the ouptut
                }
            } while (Wait);
            if (aeOutCtrls.Count>0)
            {
                WriteVerbose(this, "" + aeOutCtrls);
//                Paint-Rect aeCtrls.Current.NativeWindowHandle
//                    aeCtrls.Current.BoundingRectangle;
            }
            // WriteObject(this, aeOutCtrls);
            try{
                foreach(System.Windows.Automation.AutomationElement elt in aeOutCtrls)
                {
                    if (elt==null)
                    {
                        WriteVerbose(this, "removing element: " + elt.Current.Name);
                        aeOutCtrls.Remove(elt);
                    }
                }
                System.Windows.Automation.AutomationElement[] outArray = 
                    (System.Windows.Automation.AutomationElement[])aeOutCtrls.ToArray();
                WriteObject(this, (object[])outArray);
                // WriteObject(this, aeWndArray);
            } catch (Exception outException){
                WriteDebug(this, "Could not cast the output array to System.Object[]");
                WriteDebug(this, "" + outException.Message);
            }
        }
            
            
    }
}
