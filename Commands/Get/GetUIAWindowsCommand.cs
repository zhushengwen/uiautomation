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
    /// Description of GetUIAWindows.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "UIAWindows")]
    [OutputType(new[]{ typeof(object) })]
    // disabled in the current release
    internal class GetUIAWindowsCommand : GetWindowCmdletBase
    {
        #region Constructor
        public GetUIAWindowsCommand()
        {
            ProcessName = String.Empty;
            Name = String.Empty;
        }
        #endregion Constructor
        
        #region Parameters
        const string ParamSetProcessName = "ProcessNameSet";
        const string ParamSetName = "TitleSet";
        
        #endregion Parameters

        protected override void BeginProcessing()
        {
            startDate = System.DateTime.Now;
            try{
                // if (this.ProcessName=="" && this.Title=="") 
                if (this.ProcessName=="" && this.Name=="") 
                {
                    WriteVerbose(this, "ProcessName==null && Title==null");
                    WriteObject(this, null);
                    return;
                } // describe
                WriteVerbose(this, "timeout countdown started for process: " + 
                             this.ProcessName + ", title: " + this.Name);
                             // this.ProcessName + ", title: " + this.Title);
            } catch (Exception eCheckParameters) 
            {
                WriteDebug(this, eCheckParameters.Message);
                WriteObject(this, null);
                return;
            } // describe
            System.Windows.Automation.AutomationElement[]aeForm = null;
            WriteVerbose(this, "getting the root element");
            System.Windows.Automation.AutomationElement rootElement = 
                System.Windows.Automation.AutomationElement.RootElement;
            if (rootElement==null)
            {
                WriteVerbose(this, "rootElement==null");
                WriteObject(this, null);
                return;
            }
            else
            {
                WriteVerbose(this, rootElement.Current);
            }
        //#}
        //#Process{
            System.Collections.Generic.List<System.Windows.Automation.AutomationElement> aeWndArray = null;
            WriteVerbose(this, "getting process Id");
            int processId = 0;
            do {
                System.Windows.Automation.AndCondition conditions = null;
                System.Windows.Automation.AutomationElementCollection aeForms = null;
                if (this.ProcessName.Length>0){
                    WriteVerbose(this, "processName.Length>0");
                    try{
                        System.Diagnostics.Process[] processes = 
                            System.Diagnostics.Process.GetProcessesByName(this.ProcessName);
                        // only the first
                        processId = processes[0].Id;
                        conditions =
                                new System.Windows.Automation.AndCondition(
                                    new System.Windows.Automation.PropertyCondition(
                                        System.Windows.Automation.AutomationElement.ProcessIdProperty,
                                        processId),
                                    new System.Windows.Automation.PropertyCondition(
                                        System.Windows.Automation.AutomationElement.ControlTypeProperty,
                                        System.Windows.Automation.ControlType.Window));
                    } catch{
                        conditions = 
                                new System.Windows.Automation.AndCondition(
                                    new System.Windows.Automation.PropertyCondition(
                                        System.Windows.Automation.AutomationElement.ProcessIdProperty,
                                        processId),
                                    new System.Windows.Automation.PropertyCondition( 
                                        System.Windows.Automation.AutomationElement.ControlTypeProperty,
                                        System.Windows.Automation.ControlType.Window));
                    }
                    aeForms = 
                        rootElement.FindAll(System.Windows.Automation.TreeScope.Children,
                                            conditions);
                }
                else
                {
                    WriteVerbose(this, "processName.Length==0");
                    System.Windows.Automation.PropertyCondition condition = 
                            new System.Windows.Automation.PropertyCondition( 
                                System.Windows.Automation.AutomationElement.ControlTypeProperty,
                                System.Windows.Automation.ControlType.Window);
                    aeForms = 
                        rootElement.FindAll(System.Windows.Automation.TreeScope.Children,
                                            condition);
                }
//                System.Windows.Automation.AutomationElementCollection aeForms = 
//                    rootElement.FindAll(System.Windows.Automation.TreeScope.Children,
//                                                 conditions);
                // System.Collections.ArrayList aeWndArray = 
                //    new System.Collections.ArrayList();
                aeWndArray = 
                    new System.Collections.Generic.List<System.Windows.Automation.AutomationElement>();
                if (aeForms!=null && aeForms.Count>0)
                {
                    WriteVerbose(this, "aeForms!=null && aeForms.Count>0");
                        foreach (System.Windows.Automation.AutomationElement form in aeForms)
                        {
                            // if (this.Title.Length>0){
                            if (this.Name.Length>0){
                                if (System.Text.RegularExpressions.Regex.IsMatch(
                                    form.Current.Name.ToUpper(), this.Name.ToUpper()))
                                    // form.Current.Name.ToUpper(), this.Title.ToUpper()))
                                {
                                            WriteVerbose(this, "app name: " + 
                                                         form.Current.Name);
                                    aeWndArray.Add(form);
                                }
                            }
                            else{
                                WriteVerbose(this, "app name: " + form.Current.Name);
                                aeWndArray.Add(form);
                            }
                        }
                }
                // System.Threading.Thread.Sleep(Preferences.SleepInterval);
                // RunScriptBlocks(this);
                // SleepAndRunScriptBlocks(this);
                System.DateTime nowDate = System.DateTime.Now;
                WriteVerbose(this, "process: " + 
                             this.ProcessName + 
                             ", title: " + 
                             this.Name + 
                             // this.Title + 
                             ", seconds: " + (nowDate - startDate).TotalSeconds);
                if (aeWndArray.Count>0 || (nowDate - startDate).TotalSeconds>this.Timeout/1000)
                {
                    if (aeWndArray.Count==0){
                        WriteVerbose(this, "timeout expired for process: " + 
                                     this.ProcessName + ", title: " + this.Name);
                                     // this.ProcessName + ", title: " + this.Title);
                    }else{
                        WriteVerbose(this, "got the window: " + 
                                     ((System.Windows.Automation.AutomationElement)aeWndArray[0]).Current.Name);
                    }
                    WriteVerbose(this, "no wait");
                    this.Wait = false;
                    WriteVerbose(this, "break");
                    break;
                }
                WriteVerbose(this, "while");
            } while (this.Wait);
            if (aeWndArray!=null && aeWndArray.Count>0)
            {
                WriteVerbose(this, "aeWndArray.count>0");
                WriteVerbose(this, aeWndArray.Count.ToString());
                WriteVerbose(this, aeWndArray[0].ToString());
                WriteVerbose(this, aeWndArray[0].Current.ToString());
                WriteVerbose(this, aeWndArray[0].Current.Name);
                //#Paint-Rect $aeWndArray[0].Current.NativeWindowHandle `
                //#    $aeWndArray[0].Current.BoundingRectangle;
                try{
                    WriteVerbose(this, "before foreach");
                    foreach(System.Windows.Automation.AutomationElement elt in aeWndArray)
                    {
                        WriteVerbose(this, "foreach");
                        if (elt==null)
                        {
                            WriteVerbose(this, "clean-up");
                            WriteVerbose(this, "removing element: " + elt.Current.Name);
                            aeWndArray.Remove(elt);
                        }
                    }
                    WriteVerbose(this, "after foreach");
                    System.Windows.Automation.AutomationElement[] outArray = 
                        (System.Windows.Automation.AutomationElement[])aeWndArray.ToArray();
                    WriteVerbose(this, outArray.Length.ToString());
                    WriteVerbose(this, outArray[0].Current.Name);
                    WriteVerbose(this, "before WriteObject");
                    // WriteObject((object[])outArray);
                    // WriteObject(outArray);
                    // WriteObject(aeWndArray);
                    WriteObject(this, (object[])aeWndArray.ToArray());
                    return;
                    // WriteObject(this, aeWndArray);
                } catch (Exception outException){
                    WriteDebug(this, "Could not cast the output array to System.Object[]");
                    WriteDebug(this, outException.Message);
                }
            }
            if (aeForm!=null)
            {
                WriteVerbose(this, aeForm);
                WriteVerbose(this, "aeForm.Current.GetType() = aeForm.Current.GetType()");
            }
            WriteVerbose(this, "aeForm==null");
            //#Paint-Rect $aeForm.Current.NativeWindowHandle `
            //#    $aeForm.Current.BoundingRectangle;
            WriteObject(this, aeForm);
        }
    }
}
