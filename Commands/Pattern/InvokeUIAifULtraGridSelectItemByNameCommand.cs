/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 30.01.2012
 * Time: 11:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
	/// <summary>
	/// Description of InvokeUIAifUltraGridSelectItemByNameCommand.
	/// </summary>
	[Cmdlet(VerbsLifecycle.Invoke, "UIAifUltraGridSelectItemByName")]
	public class InvokeUIAifUltraGridSelectItemByNameCommand : ULtraGridCmdletBase
	{
		public InvokeUIAifUltraGridSelectItemByNameCommand()
		{
		}
		
		#region Parameters
//		[Parameter(Mandatory=true)]
//		public string[] ItemName {get; set;}
		#endregion Parameters
		
		protected override void ProcessRecord()
		{
			if (!base.CheckControl(this)) return;
			
			ifUltraGridProcessing(ifUltraGridOperations.selectItems);
            
//			// colleciton of the selected rows
//			//AutomationElementCollection selectedItems = null;
//			System.Collections.Generic.List<AutomationElement> selectedItems = 
//				new System.Collections.Generic.List<AutomationElement>();
//			
//			try{
//            AutomationElementCollection tableItems = 
//                this.InputObject.FindAll(TreeScope.Children,
//                             new PropertyCondition(
//                                 AutomationElement.ControlTypeProperty,
//                                 ControlType.Custom));
//            
//            if (tableItems.Count > 0){
//            	bool notTheFirstChild = false;
//                foreach(AutomationElement child in tableItems){
////                    Console.WriteLine("\t" + child.Current.Name);
//                    
////                    if (child.Current.Name.Contains("row") || 
////                        child.Current.Name.Contains("Row")){
//                        AutomationElementCollection row = 
//                            child.FindAll(TreeScope.Children,
//                                          new PropertyCondition(
//                                              AutomationElement.ControlTypeProperty,
//                                              ControlType.Custom));
//                        foreach(AutomationElement grandchild in row){
//                            ValuePattern valPattern = null;
//                            try{
//                            valPattern =
//                                grandchild.GetCurrentPattern(ValuePattern.Pattern)
//                                as ValuePattern;
//                            } catch {
////                            	Console.WriteLine("1");
//                            }
//                            string strValue = "";
//                            try{
//                                strValue = 
//                                    valPattern.Current.Value;
//                            } catch {
////                            	Console.WriteLine("2");
//                            }
//                            
//                            
//                            
////                            if (strValue == clickString ||
////                                strValue == clickString2){
//                            if (IsInTheList(strValue)){
//                                //grandchild.SetFocus(); //fail
//                                //child.SetFocus();//fail
////                                InvokePattern invPattern = null;
////                                try{
////                                    invPattern = 
////                                        child.GetCurrentPattern(InvokePattern.Pattern)
////                                        as InvokePattern;
////                                    invPattern.Invoke();
////                                    //System.Windows.Forms.SendKeys.SendWait(" "); //QMM
////                                    WriteVerbose(this, strValue + " selected");
////                                } catch{
////                                    //Console.WriteLine("3");
////                                    //System.Windows.Forms.SendKeys.SendWait("+ "); //PowerGUI doesn’t react
////                                }
//
//
//								if (ClickControl(this,
//             								 child,
//             								 false,
//             								 false,
//             								 false,
//             								 false,
//             								 notTheFirstChild,
//             								 false,
//             								 0,
//             								 0)){
//                                	selectedItems.Add(child);
//                                	WriteVerbose(this, 
//                                    	         "the " + child.Current.Name + 
//                                    	         " added to the output collection");
//								}
//                                //System.Windows.Point p = child.GetClickablePoint();//fail
//                                //System.Windows.Point p = grandchild.GetClickablePoint();//fail
//                                
//                            }
//                            //Console.WriteLine("\t\t" + grandchild.Current.Name + "\t" + strValue);
//                            WriteVerbose(this, "working with " + 
//                                         grandchild.Current.Name + "\t" + strValue);
//                        }
////                    }
//                    
//                }
//            	//WriteObject(this, true);
//            	WriteObject(this, selectedItems);
//            } else {
//            	WriteVerbose(this, "no elements of type ControlType.Custom were found under the input control");
//            }
//			} catch (Exception ee) {
//				ErrorRecord err = 
//					new ErrorRecord(
//						ee,
//						"ExceptionInSectingItems",
//						ErrorCategory.InvalidOperation,
//						this.InputObject);
//				err.ErrorDetails = new ErrorDetails("Exception were thrown during the cycle of selecting items.");
//				WriteObject(this, false);
//			}
		}
	}
}
