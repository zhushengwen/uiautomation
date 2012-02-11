/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 30.11.2011
 * Time: 11:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;

namespace UIAutomation.Commands
{
    /// <summary>
    /// Description of StartUIATranscriptCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "UIATranscript")]
    //[OutputType(new[]{ typeof(object) })]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class StartUIATranscriptCommand : TranscriptCmdletBase
    {
        #region Constructor
        public StartUIATranscriptCommand()
        {
            LongRecordingFileName = String.Empty;
            ShortRecordingFileName = String.Empty;
        }
        #endregion Constructor
        
        #region Parameters
        [Parameter(Mandatory=false)]
        public string LongRecordingFileName { get; set; }
        [Parameter(Mandatory=false)]
        public string ShortRecordingFileName { get; set; }
        #endregion Parameters
        
        #region BeginProcessing
        
//        string errorMessageInTheGatheringCycle = String.Empty;
//        bool errorInTheGatheringCycle = false;
//        string errorMessageInTheInnerCycle = String.Empty;
//        bool errorInTheInnerCycle = false;
        
        protected override void BeginProcessing()
        {
            
            if (!this.NoUI){
                this.Timeout = 604800000;
                // frmRecorder formRecorder = 
                CurrentData.formRecorder = 
                    new RecorderForm(this);
                // formRecorder.ShowDialog();
                CurrentData.formRecorder.Show();
                CurrentData.formRecorder.Hide();
                try{
                this.Events.SubscribeEvent((object)CurrentData.formRecorder.btnStop,
                                           "BtnStopClick",
                                           "formRecorder",
                                           new PSObject(),
                                           System.Management.Automation.ScriptBlock.Create(""), // CurrentData.formRecorder.BtnStopClick,
                                           true,
                                           false);
                } catch { }
                CurrentData.formRecorder.ShowDialog();
                return;
            } else {
                UIAHelper.ProcessingTranscript(this);
            }
#region old
//            Global.GTranscript = true;
//            int counter = 0;
//            
//            if (!this.NoUI){
//                this.Timeout = 604800000;
//            }
//            
//            // 20120206 System.Windows.Automation.AutomationElement rootElement = 
//            // 20120206     System.Windows.Automation.AutomationElement.RootElement;
//            rootElement = 
//                System.Windows.Automation.AutomationElement.RootElement;
//            // 20120206 System.DateTime startDate = 
//            startDate =
//                System.DateTime.Now;
//            do
//            {
//                RunOnSleepScriptBlocks(this);
//                System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
//                while (Paused){
//                    System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
//                }
//                counter++;
//                
//                try{
//                    // use Windows forms mouse code instead of WPF
//                    System.Drawing.Point mouse = System.Windows.Forms.Cursor.Position;
//                    System.Windows.Automation.AutomationElement element = 
//                        System.Windows.Automation.AutomationElement.FromPoint(
//                            new System.Windows.Point(mouse.X, mouse.Y));
//                    if (element!=null)
//                    {
//                        processingElement(element);
//                    }
//                    if (errorInTheGatheringCycle){
//                        WriteDebug(this, "An error is in the control hierarchy gathering cycle");
//                        WriteDebug(this, errorMessageInTheGatheringCycle);
//                        errorInTheGatheringCycle = false;
//                    }
//                } catch (Exception eUnknown){
//                    WriteDebug(this, eUnknown.Message);
//                }
//                System.DateTime nowDate = System.DateTime.Now;
//                if ((nowDate - startDate).TotalSeconds>this.Timeout/1000) break;
//            } while (Global.GTranscript);
#endregion old
        }
        #endregion BeginProcessing
        
        #region Script header
        private void writeHeader(ref System.IO.StreamWriter fileWriter, 
                                         string fileName){
            try{
        
            fileWriter.WriteLine(@"################################################################################");
            fileWriter.WriteLine("#\tScript name:\t" + fileName);
            fileWriter.WriteLine("#\tScript usage:\tpowershell.exe -command " + fileName);
            fileWriter.WriteLine("#\tNote:\tThe script is generated by automatic means. It may or may not require");
            fileWriter.WriteLine("#\t\t\tmanual amendment. Use the script in production with care.");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\tHelp:\tIn case you've got stuck with the script or the UIAutomation module itself,");
            fileWriter.WriteLine("#\t\t\tplease follow the troubleshooting procedure:");
            fileWriter.WriteLine("#\t\t\tTurn on $VerbosePreference and $DebugPreference variables");
            fileWriter.WriteLine("#\t\t\tIt will give you a clue in case a cmdlet is waiting for something extremely long time.");
            fileWriter.WriteLine("#\t\t\tBy default, Select- cmdlets are equipped with Timeout equalling to 10 seconds (10000)");
            fileWriter.WriteLine("#\t\t\tWhen -Verbose and -Debug keay are activated, Select- cmdlets produce richer output.");
            fileWriter.WriteLine("#\t\t\tIf you see repetitive lines looking like 'title:' ... 'classname:' ...");
            fileWriter.WriteLine("#\t\t\tit's a hint that a control or a window is unavailable at the moment.");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\t\t\tOne more advice with the module is on how and what to use.");
            fileWriter.WriteLine("#\t\t\tIf you are especially interested in a control of specific type, issue the command like this:");
            fileWriter.WriteLine("#\t\t\tGet-Command -Module UIA* *Edit* #(or, what's equal to the previous, Get-Command -Module UIA* *TextBox*)");
            fileWriter.WriteLine("#\t\t\tThe output returns current offering for the control you want");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\t\t\tSimilarly, you can filter cmdlets by an action you are planning to perform with the control");
            fileWriter.WriteLine("#\t\t\tGet-Command -Module UIA* *Click* #(or Get-Command -Module UIA* *Toggle*)");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\tAuthor:\tAlexander Petrovskiy");
            fileWriter.WriteLine("#\tContact:\thttp://SoftwareTestingUsingPowerShell.WordPress.com");
            fileWriter.WriteLine("################################################################################");
            fileWriter.WriteLine(@"cls");
            fileWriter.WriteLine(@"Set-StrictMode -Version Latest;");
            fileWriter.WriteLine(@"# the following lines are not necessary for script to be working, they're for your information only");
            fileWriter.WriteLine(@"#region Preferences");
            fileWriter.WriteLine(@"$DebugPreference = [System.Management.Automation.ActionPreference]::Continue;");
            fileWriter.WriteLine(@"$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;");
            fileWriter.WriteLine(@"[UIAutomation.Preferences]::Timeout = 10000;");
            fileWriter.WriteLine(@"#endregion Preferences");
            fileWriter.Flush();
            
            } 
            catch (Exception eUnknown) {
                WriteDebug(this, eUnknown.Message);
            }
        }
        #endregion Script header
        
        #region EndProcessing
        protected override void EndProcessing()
        {
            try{
                if (recording.Count>0){
                #region preparing script files
                // use user's %TEMP%
                string recordingFileName =
                    System.Environment.GetEnvironmentVariable(
                        "TEMP",
                        System.EnvironmentVariableTarget.User) + 
                    @"\";
                string shRecordingFileName = 
                    recordingFileName;
                // file names from parameters -Long... and -Short...
                if (this.LongRecordingFileName.Length>0){
                    recordingFileName +=
                        this.LongRecordingFileName;
                }
                if (this.ShortRecordingFileName.Length>0){
                    shRecordingFileName +=
                        this.ShortRecordingFileName;
                }
                // genearated file names
                if (this.LongRecordingFileName.Length==0){
                    recordingFileName += @"UIAutomation_recording_";
                }
                if (this.ShortRecordingFileName.Length==0){
                    shRecordingFileName += 
                        @"UIAutomation_recording_short_";
                }
                string datetime = 
                    (((((System.DateTime.Now.ToShortDateString().ToString() +
                        "_" + 
                        System.DateTime.Now.ToShortTimeString()).Replace(":", "_")).Replace("/", "_")).Replace(";", "_")).Replace(@"\", "_")).Replace(" ", "_");
                if (this.LongRecordingFileName.Length==0){
                    recordingFileName += datetime;
                    recordingFileName += ".ps1";
                }
                if (this.ShortRecordingFileName.Length==0){
                    shRecordingFileName += datetime;
                    shRecordingFileName += ".ps1";
                }
                
                WriteDebug(this, "long recording file name " + 
                           recordingFileName);
                WriteDebug(this, "short recording file name " +
                           shRecordingFileName);
                
                System.IO.StreamWriter writerToLongFile = 
                    new System.IO.StreamWriter(recordingFileName, true);
                System.IO.StreamWriter writerToShortFile = 
                    new System.IO.StreamWriter(shRecordingFileName, true);

                if (!NoScriptHeader){
                    writeHeader(ref writerToLongFile, recordingFileName);
                    writeHeader(ref writerToShortFile, shRecordingFileName);
                }
                #endregion preparing script files
                
                #region writing the script
                for (int j = 0; j<recording.Count; j++)
                {
                    try{
                    System.Collections.ArrayList recordList = (System.Collections.ArrayList)recording[j];
                    string longRecordingString = String.Empty;
                    string shortRecordingString = String.Empty;
                    string tempString = String.Empty;
                    for (int i = (recordList.Count - 1); i >= 0; i--){
                        tempString = String.Empty;
                        tempString += recordList[i];
                        if (i<(recordList.Count - 1)){
                            if (tempString.Contains("Get-UIAWindow")){
                                tempString = 
                                    tempString.Replace("Get-UIAWindow",
                                                          "Get-UIAChildWindow");
                            }
                            // if the second or further in the pipeline
                            tempString = 
                                "\t" + tempString;
                        }
                        longRecordingString += tempString;
                        if (i>0){
                            // all but the last in the pipeline
                            longRecordingString += " | `\r\n";
                        } else {
                            // the last in the pipeline
                            longRecordingString += ";";
                        }
                        if (i==(recordList.Count - 1) ||
                            i==0 || // the last element or invoked event
                            tempString.Contains("Get-UIAChildWindow") ||
                            i==1 || //) // the last element in case of having invoked event
                            tempString.Contains("Tree") ||
                            tempString.Contains("Menu") ||
                            tempString.Contains("Tool") ||
                            // tempString.Contains("Tab") || // also Table
                            tempString.Contains("Tab") ||
                            tempString.Contains("Table") ||
                            tempString.Contains("List") ||
                            tempString.Contains("Grid") ||
                            tempString.Contains("Button") ||
                            tempString.Contains("Combo"))
                        {
                            shortRecordingString += tempString;
                            if (i>0){
                                // all but the last in the pipeline
                                shortRecordingString += " | `\r\n";
                            } else {
                                // the last in the pipeline
                                shortRecordingString += ";";
                            }
                        }
                    }
                    if (WriteCurrentPattern){
                        longRecordingString +=
                            recordingPatterns[j];
                        shortRecordingString +=
                            recordingPatterns[j];
                    }
                    writerToLongFile.WriteLine(longRecordingString); writerToLongFile.Flush();
                    writerToShortFile.WriteLine(shortRecordingString); writerToShortFile.Flush();
                    } catch (Exception eBuildingRecordingString){
                        WriteDebug(eBuildingRecordingString.Message);
                    }
                }
                #endregion writing the script
                writerToLongFile.Flush(); writerToLongFile.Close();
                writerToShortFile.Flush(); writerToShortFile.Close();
                try{
                    System.Diagnostics.Process.Start("notepad.exe", recordingFileName);
                    System.Diagnostics.Process.Start("notepad.exe", shRecordingFileName);
                } catch {
                    WriteObject(this, "The full script recorded is here: " + 
                                recordingFileName);
                    WriteObject(this, "The short script recorded is here: " + 
                                shRecordingFileName);
                }
                } // the end of if (recording.Count>0)
            } catch (Exception eRecording) {
                WriteObject(this, false);
                WriteDebug(this, "Could not save the recording");
                WriteDebug(this, eRecording.Message);
            }
        }
        #endregion EndProcessing
        
        #region getControlTypeNameOfAutomationElement
        private string getControlTypeNameOfAutomationElement(
            AutomationElement element,
            AutomationElement element2)
        {
            string result = String.Empty;
            if (element!=null && element2!=null){
                element.Current.ControlType.ProgrammaticName.Substring(
                    element2.Current.ControlType.ProgrammaticName.IndexOf('.') + 1);
            }
            return result;
        }
        #endregion getControlTypeNameOfAutomationElement
    }
    
    /// <summary>
    /// Description of StartUIARecorderCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "UIARecorder")]
    //[OutputType(new[]{ typeof(object) })]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public class StartUIARecorderCommand : StartUIATranscriptCommand
    { public StartUIARecorderCommand(){ } }
}
