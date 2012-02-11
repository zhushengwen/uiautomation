/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 19/01/2012
 * Time: 11:32 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of GetControlCmdletBase.
    /// </summary>
    public class GetControlCmdletBase : GetCmdletBase
    {
        #region Constructor
        public GetControlCmdletBase()
        {
            Class = String.Empty;
            // Title = String.Empty;
            Name = String.Empty;
            ControlType = String.Empty;
            AutomationId = String.Empty;
            InputObject = CurrentData.CurrentWindow;
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
        [Parameter(Mandatory=false)]
        public string AutomationId { get; set; }
        #endregion Parameters
    }
}
