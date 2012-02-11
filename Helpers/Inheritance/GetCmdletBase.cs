/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 29.11.2011
 * Time: 14:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of GetCmdletBase.
    /// </summary>
    public class GetCmdletBase : HasTimeoutCmdletBase
    {
        #region Constructor
        public GetCmdletBase()
        {
        }
        #endregion Constructor

        #region Parameters
        [Parameter(Mandatory=false)]
        internal new SwitchParameter PassThru { get; set; }
        #endregion Parameters
    }
}
