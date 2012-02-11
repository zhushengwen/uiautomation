/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 15/12/2011
 * Time: 10:27 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

/*
using System;
using System.Management.Automation;
using System.Management.Automation.Provider;
using UIAutomation.Commands;

namespace UIAutomation
{
    /// <summary>
    /// Description of UIAProvider.
    /// </summary>
    [CmdletProvider("UIAutomation", ProviderCapabilities.None)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class UIAProvider : DriveCmdletProvider // CmdletProvider
    {
//        public UIAProvider()
//        {
//        }
        
//        protected override System.Management.Automation.ProviderInfo Start(System.Management.Automation.ProviderInfo providerInfo)
//        {
//            // providerInfo.Name = "UIAutomation";
//            providerInfo.Description = "This is a simple UI Automation provider";
//            // providerInfo.Drives = 
//            return base.Start(providerInfo);
//        }
//        
//        protected override void Stop()
//        {
//            
//        }
        
//        private void test()
//        {
//            // WriteItemObject((object)item, path, true);
//            // WriteError(this, (ErrorRecord)rec);
//            // ThrowTerminatingError((ErrorRecord)rec);
//            // WriteVerbose("");
//            // WriteDebug("");
//            // ShouldProcess(
//            // ShouldContinue(
//            // ProviderInfo
//            // PSDriveInfo
//            // SessionState
//        }
        
        // public ErrorRecord 
        
        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
              // Check if the drive object is null.
              if (drive==null)
              {
                WriteError(new ErrorRecord(
                      new ArgumentNullException("drive"),
                      "NullDrive",
                      ErrorCategory.InvalidArgument,
                      null));
            return null;
            }
//                                                          ""));
            // Check if the drive root is not null or empty
            // and if it is an existing file.
            System.Windows.Automation.AutomationElement _element = null;
            if (String.IsNullOrEmpty(drive.Root) && 
                ( (_element = (new GetUIAWindowCommand()).GetWindow(drive.Root, 
                                                                       drive.Root))
                                                         ==null))
//                ((new GetUIAWindowCommand()).GetWindow(drive.Root,
//                                                          "")
//                                                         ==null) &&
//                ((new GetUIAWindowCommand()).GetWindow("",
//                                                          drive.Root)
//                                                         ==null))
            {
                WriteError(new ErrorRecord(
                    new ArgumentException("drive.Root"),
                    "NoRoot",
                    ErrorCategory.InvalidArgument,
                    drive));
                return null;
            }
            // Create a new drive and create an ODBC connection to the new drive.
            UIADriveInfo uIAPSDriveInfo = new UIADriveInfo(drive);
            uIAPSDriveInfo.Window = _element;
            // OdbcConnectionStringBuilder builder = new OdbcConnectionStringBuilder();

            // builder.Driver = "Microsoft Access Driver (*.mdb)";
            // builder.Add("DBQ", drive.Root);
          
            // OdbcConnection conn = new OdbcConnection(builder.ConnectionString);
            // conn.Open();
            // accessDBPSDriveInfo.Connection = conn;
            // UIADriveInfo

            return uIAPSDriveInfo;
        } // End NewDrive.
        
    }
    

    
    internal class UIADriveInfo : PSDriveInfo
    {
//        private string _path;
        private System.Windows.Automation.AutomationElement _window = null;
        
//        public UIADriveInfo(string path, PSDriveInfo drive) : base(drive)
//        {
//            _path = path;
//        }
        
        public System.Windows.Automation.AutomationElement Window
        {
            get { return _window; }
            internal set {_window = value; }
        }
        
        public UIADriveInfo(//string ProcessName, 
                            // string Title, 
                            PSDriveInfo drive) : base(drive)
        {
            //_path = path;
            _window = 
                (new GetUIAWindowCommand()).GetWindow(drive.Root,
                                                         "");
            if (_window!=null){
                // return _window;
                return;
            }
            _window = 
                (new GetUIAWindowCommand()).GetWindow("",
                                                         drive.Root);
            // if (_window!=null){
            //    return _window;
            //}
            // return null;
        }
//        
//        public UIADriveInfo(string title, PSDriveInfo drive) : base(drive)
//        {
//            _path = path;
//        }
    }
}
*/