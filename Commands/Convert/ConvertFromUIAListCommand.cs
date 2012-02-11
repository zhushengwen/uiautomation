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

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of ConvertFromUIAListCommand.
    /// </summary>
    [Cmdlet(VerbsData.ConvertFrom, "UIAList")]
    // disabled in the current release
    internal class ConvertFromUIAListCommand : ConvertFromCmdletBase
    {
        #region Constructor
        public ConvertFromUIAListCommand()
        {
        }
        #endregion Constructor
        
        #region Parameters
        #endregion Parameters
        
        protected override void ProcessRecord()
        {
            if (!base.CheckControl(this)) return;
            
            //_control.SetFocus();
            
            // get headers
            
            // output headers
            
            // get rows
            
            // output rows
            
        }
        
//        protected override void EndProcessing()
//        {
//            //_control = null;
//        }
    }
}
