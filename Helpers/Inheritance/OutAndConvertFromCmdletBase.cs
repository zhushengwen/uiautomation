/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 23/01/2012
 * Time: 10:57 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of OutAndConvertFromCmdletBase.
    /// </summary>
    public class OutAndConvertFromCmdletBase : HasControlInputCmdletBase
    {
        #region Constructor
        public OutAndConvertFromCmdletBase()
        {
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=false)]
        internal new SwitchParameter PassThru { get; set; }
        #endregion Parameters
    }
}
