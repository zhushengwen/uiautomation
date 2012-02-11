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

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of InvokeUIAifUltraGridSelectItemByNameCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "UIAifUltraGridSelectItemByName")]
    public class InvokeUIAifUltraGridSelectItemByNameCommand : ULtraGridCmdletBase
    {
        #region Constructor
        public InvokeUIAifUltraGridSelectItemByNameCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        #endregion Parameters
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            ifUltraGridProcessing(ifUltraGridOperations.selectItems);
        }
    }
}
