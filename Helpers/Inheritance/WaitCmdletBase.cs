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
    /// Description of WaitCmdletBase.
    /// </summary>
    public class WaitCmdletBase : HasTimeoutCmdletBase
    {
        #region Constructor
        public WaitCmdletBase()
        {
            // duplicated !!!
            InputObject = null;
        }
        #endregion Constructor

        #region Parameters
        #endregion Parameters
    }
}
