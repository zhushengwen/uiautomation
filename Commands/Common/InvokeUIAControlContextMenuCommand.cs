﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 03/02/2012
 * Time: 09:07 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Security.Principal;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of GetUIAControlContextMenuCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAControlContextMenu")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class GetUIAControlContextMenuCommand : HasControlInputCmdletBase
    {
        #region Constructor
        public GetUIAControlContextMenuCommand()
        {
        }
        #endregion Constructor
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            AutomationElement resultElement = null;
            
            // preform a right click on the control
            if (!ClickControl(this,
                               this.InputObject,
                               true,
                               false,
                               false,
                               false,
                               false,
                               false,
                               false,
                               0,
                               0)){
                ErrorRecord err = 
                    new ErrorRecord(
                        new Exception("Couldn't click on this control"),
                        "couldNotClick",
                        ErrorCategory.InvalidResult,
                        this.InputObject);
                err.ErrorDetails = 
                    new ErrorDetails("Could not click on the control");
// 20120209 
//                WriteError(this, err);
//                return;
                WriteError(this, err, true);
            } else {
                WriteVerbose(this, "clicked on " + this.InputObject.Current.Name);
            }
            
            // System.Threading.Thread.Sleep(1000);
            
            // get the cursor coordinates
            int x = 
                System.Windows.Forms.Cursor.Position.X;
            int y = 
                System.Windows.Forms.Cursor.Position.Y;
            
            WriteVerbose(this, "cursor coordinate X = " + x.ToString());
            WriteVerbose(this, "cursor coordinate Y = " + y.ToString());
            
            // get the context menu window
            int processId = 
                this.InputObject.Current.ProcessId;
            WriteVerbose(this, "process Id = " + processId.ToString());
            AutomationElementCollection windowsByPID = null;
            startDate = System.DateTime.Now;
            bool breakSearch = false;
            do {
                if (processId!=0){
                    windowsByPID = 
                        AutomationElement.RootElement.FindAll(TreeScope.Children,
                                                              new AndCondition(
                                                                  new PropertyCondition(
                                                                      AutomationElement.ProcessIdProperty,
                                                                      processId),
                                                                  new OrCondition(
                                                                      new PropertyCondition(
                                                                          AutomationElement.ControlTypeProperty,
                                                                          ControlType.Window),
                                                                      new PropertyCondition(
                                                                          AutomationElement.ControlTypeProperty,
                                                                          ControlType.Pane),
                                                                      new PropertyCondition(
                                                                          AutomationElement.ControlTypeProperty,
                                                                          ControlType.Menu)
                                                                  )));
                }
                if (windowsByPID.Count>0){
                    WriteVerbose(this, 
                                 "there are " +
                                 windowsByPID.Count.ToString() + 
                                 " windows running within the process");
                    System.DateTime nowDate = 
                        System.DateTime.Now;
                    if ((nowDate - startDate).TotalSeconds>3){
                        breakSearch = true;
                        break;
                    }
                    if (windowsByPID.Count==1){
                        System.Threading.Thread.Sleep(200);
                        continue;
                    }
                    foreach(AutomationElement element in windowsByPID){
                        WriteVerbose(this, element.Current.Name);
                        WriteVerbose(this, element.Current.BoundingRectangle.ToString());
                        try{
                            if (element.Current.BoundingRectangle.X==x &&
                                element.Current.BoundingRectangle.Y==y){
                                WriteVerbose(this, 
                                             "the element " +
                                             element.Current.Name + 
                                             " is what has been searching for");
                                resultElement = element;
                                breakSearch = true;
                                break;
                            }
                        }catch {
                            
                        }
                    }
                }
            } while (!breakSearch);
            
            // return the context menu window
            if (resultElement!=null){
                WriteObject(this, resultElement);
            } else {
                WriteObject(this, null);
            }
        }
    }
}
