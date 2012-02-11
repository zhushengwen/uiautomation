/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 28/11/2011
 * Time: 08:00 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Windows.Automation;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace UIAutomation
{
    /// <summary>
    /// Description of UIAHelper.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UIA")]
    public static class UIAHelper
    {
        #region Screenshot taking
        [DllImport("user32.dll")] 
         internal extern static IntPtr GetDesktopWindow(); 
  
        [DllImport("user32.dll")] 
         private static extern IntPtr GetWindowDC(IntPtr hwnd); 
         
         [DllImport("user32.dll")] 
         private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hDc); 
  
        [DllImport("gdi32.dll")] 
         internal static extern UInt64 BitBlt 
             (IntPtr hDestDC, 
             int x, int y, int nWidth, int nHeight, 
             IntPtr hSrcDC, 
             int xSrc, int ySrc, 
             System.Int32 dwRop); 
         
        [Flags]
        private enum DrawingOptions
        {
            PRF_CHECKVISIBLE = 0x00000001,
            PRF_NONCLIENT = 0x00000002,
            PRF_CLIENT = 0x00000004,
            PRF_ERASEBKGND = 0x00000008,
            PRF_CHILDREN = 0x00000010,
            PRF_OWNED = 0x00000020
        }

        private const int WM_PRINT = 0x0317;
        private const int WM_PRINTCLIENT = 0x0318;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr dc,
            DrawingOptions opts);

        #endregion Screenshot taking
        #region getting a control
        [DllImport("user32.dll", EntryPoint="FindWindowEx", CharSet=CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, 
                                          IntPtr hwndChildAfter,
                                          string lpszClass,
                                          string lpszWindow);
        #endregion getting a control
        
        private static Highlighter highlighter = null;
        
        private static System.Windows.Automation.AutomationElement element = null;
        
        internal static AutomationElement GetControlByTitle(AutomationElement container, 
                                                            string title)
        {
            AutomationElement result = null;
            if (container==null) return null;
            if (title.Length==0) return null;
            try{
                System.IntPtr handle =
                        new System.IntPtr(container.Current.NativeWindowHandle);
                IntPtr control = 
                    FindWindowEx(handle, IntPtr.Zero, null, title);
                if (control==IntPtr.Zero) return null;
                result = 
                    AutomationElement.FromHandle(control);
                return result;
            } catch {
                return null;
            }
        }
        
        internal static void Highlight(AutomationElement element)  //(object obj)
        {
            if (highlighter!=null) highlighter.Dispose();
            
            highlighter = new Highlighter(element);

        }
        
        internal static string GetTimedFileName()
        {
            string result = 
                System.DateTime.Now.Year.ToString() +
                System.DateTime.Now.Month.ToString() +
                System.DateTime.Now.Day.ToString() +
                System.DateTime.Now.Hour.ToString() +
                System.DateTime.Now.Minute.ToString() +
                System.DateTime.Now.Second.ToString() +
                System.DateTime.Now.Millisecond.ToString();
            return result;
        }
        
        private static string getTimedFileNameForScreenShot()
        {
            string result = 
                Preferences.ScreenShotFolder +
                @"\" + 
                GetTimedFileName();
            return result;
        }
 
        [STAThread]
        internal static void GetDesktopScreenshot(string description) //, int monitor)
        {
            Image myImage = 
                new Bitmap(Screen.PrimaryScreen.Bounds.Width, 
                           Screen.PrimaryScreen.Bounds.Height);
             Graphics gr1 = Graphics.FromImage(myImage); 

            IntPtr dc1 = gr1.GetHdc(); 
            
            // for now, the primary display only
            IntPtr desktopHandle = GetDesktopWindow();
            IntPtr dc2 = GetWindowDC(desktopHandle); 
             // IntPtr dc2 = GetWindowDC(GetDesktopWindow()); 
 
             BitBlt(dc1, 0, 0, Screen.PrimaryScreen.Bounds.Width,
                 Screen.PrimaryScreen.Bounds.Height, dc2, 0, 0, 13369376); 

             gr1.ReleaseHdc(dc1);
            
             if (description.Length>0){
                description = 
                    "_" + 
                    description;
            }
            string fileName = 
                getTimedFileNameForScreenShot() + 
                description + 
                ".jpg";
             myImage.Save(fileName, ImageFormat.Jpeg); 
             // CurrentData.TestResults[CurrentData.TestResults.Count - 1].Details.Add(fileName);
             CurrentData.AddTestResultDetail(fileName);
             
             //
             //
             ReleaseDC(desktopHandle, dc2);
             //
             //
        }
        
        [STAThread]
        internal static void GetControlScreenshot(AutomationElement control, 
                                                  string description)
        {
            

    
            using (Bitmap bm = new Bitmap((int)control.Current.BoundingRectangle.Width + 2,
                                          (int)control.Current.BoundingRectangle.Height + 2))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    IntPtr dc = g.GetHdc();
                    try
                    {
                        IntPtr handle = 
                            new IntPtr(control.Current.NativeWindowHandle);
                        SendMessage(handle, WM_PRINT, dc,
                            DrawingOptions.PRF_CHILDREN |
                            DrawingOptions.PRF_CLIENT |
                            DrawingOptions.PRF_NONCLIENT);
                    }
                    finally
                    {
                        g.ReleaseHdc(dc);
                    }
                    
                    string fileName = 
                        getTimedFileNameForScreenShot() + 
                        description + 
                        ".jpg";
                    
                    bm.Save(fileName, ImageFormat.Jpeg);
                    CurrentData.AddTestResultDetail(fileName);
                }
            }
            
            
            
            // CurrentData.TestResults[CurrentData.TestResults.Count - 1].Details.Add(err);
            // CurrentData.AddTestResultDetail(fileName);
        }
        
        #region Start-UIATranscript
        
        private static string errorMessageInTheGatheringCycle = String.Empty;
        private static bool errorInTheGatheringCycle = false;
        private static string errorMessageInTheInnerCycle = String.Empty;
        private static bool errorInTheInnerCycle = false;
        
        internal static void ProcessingTranscript(TranscriptCmdletBase cmdlet)
        {
            Global.GTranscript = true;
            int counter = 0;
            
            // 20120206 System.Windows.Automation.AutomationElement rootElement = 
            // 20120206     System.Windows.Automation.AutomationElement.RootElement;
            cmdlet.rootElement = 
                System.Windows.Automation.AutomationElement.RootElement;
            // 20120206 System.DateTime startDate = 
            cmdlet.startDate =
                System.DateTime.Now;
            do
            {
                bool res = ProcessingTranscriptOnce(cmdlet, counter);
                if (!res) break;
                
//                cmdlet.RunOnSleepScriptBlocks(cmdlet);
//                System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
//                while (cmdlet.Paused){
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
//                        processingElement(cmdlet, element);
//                    }
//                    if (errorInTheGatheringCycle){
//                        cmdlet.WriteDebug(cmdlet, "An error is in the control hierarchy gathering cycle");
//                        cmdlet.WriteDebug(cmdlet, errorMessageInTheGatheringCycle);
//                        errorInTheGatheringCycle = false;
//                    }
//                } catch (Exception eUnknown){
//                    cmdlet.WriteDebug(cmdlet, eUnknown.Message);
//                }
//                System.DateTime nowDate = System.DateTime.Now;
//                if ((nowDate - cmdlet.startDate).TotalSeconds>cmdlet.Timeout/1000) break;
            } while (Global.GTranscript);
        }
        
        internal static bool ProcessingTranscriptOnce(TranscriptCmdletBase cmdlet,
                                                      int counter)
        {
            cmdlet.RunOnSleepScriptBlocks(cmdlet);
            System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
            while (cmdlet.Paused){
                System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
            }
            counter++;
            
            try{
                // use Windows forms mouse code instead of WPF
                System.Drawing.Point mouse = System.Windows.Forms.Cursor.Position;
                System.Windows.Automation.AutomationElement element = 
                    System.Windows.Automation.AutomationElement.FromPoint(
                        new System.Windows.Point(mouse.X, mouse.Y));
                if (element!=null)
                {
                    processingElement(cmdlet, element);
                }
                if (errorInTheGatheringCycle){
                    cmdlet.WriteDebug(cmdlet, "An error is in the control hierarchy gathering cycle");
                    cmdlet.WriteDebug(cmdlet, errorMessageInTheGatheringCycle);
                    errorInTheGatheringCycle = false;
                }
            } catch (Exception eUnknown){
                cmdlet.WriteDebug(cmdlet, eUnknown.Message);
            }
            System.DateTime nowDate = System.DateTime.Now;
            if ((nowDate - cmdlet.startDate).TotalSeconds>cmdlet.Timeout/1000) return false; // break;
            return true;
        }
        
            #region working with an element
        private static void processingElement(TranscriptCmdletBase cmdlet, AutomationElement element)
        {
            // UIAHelper.Highlight(element);
            try{
                CacheRequest cacheRequest = new CacheRequest();
                cacheRequest.AutomationElementMode = AutomationElementMode.None;
                cacheRequest.TreeFilter = Automation.RawViewCondition;
                cacheRequest.Add(AutomationElement.NameProperty);
                cacheRequest.Add(AutomationElement.AutomationIdProperty);
                cacheRequest.Add(AutomationElement.ClassNameProperty);
                cacheRequest.Add(AutomationElement.ControlTypeProperty);
                // cache patterns?
                
                // cacheRequest.Activate();
                cacheRequest.Push();
                element.FindFirst(TreeScope.Element, Condition.TrueCondition);
                // element.GetUpdatedCache(cacheRequest);
                
                // if (lastRecordedItem.Count==0 || element!=lastRecordedItem[0]){
                    try{
                        // lastRecordedItem = new System.Collections.ArrayList();
                        cmdlet.WriteVerbose(cmdlet, "getting ControlType");
                        string elementControlType = String.Empty;
                        try{
                            elementControlType = 
                                element.Current.ControlType.ProgrammaticName;
                                // getControlTypeNameOfAutomationElement(element, element);
                                // element.Cached.ControlType.ProgrammaticName.Substring(
                                //    element.Cached.ControlType.ProgrammaticName.IndexOf('.') + 1);
                        } catch {
                            elementControlType = 
                                element.Cached.ControlType.ProgrammaticName;
                        }
                        elementControlType = 
                            elementControlType.Substring(
                            elementControlType.IndexOf('.') + 1);
                        string elementVerbosity = String.Empty;
                        // if (elementControlType.Length>0){
                        // if (element 
                        
                        // added 20120126
                        if (elementControlType.Length==0){
                            return;
                        }
    
                            elementVerbosity = 
                                "Get-UIA" + elementControlType;
                            try{
                                cmdlet.WriteVerbose(cmdlet, "getting AutomationId");
                                if (element.Current.AutomationId.Length>0){
                                    elementVerbosity += (" -AutomationId '" + element.Current.AutomationId + "'");
                                }
                            }catch{
                                if (element.Cached.AutomationId.Length>0){
                                    elementVerbosity += (" -AutomationId '" + element.Cached.AutomationId + "'");
                                }
                            }
                            if (!cmdlet.NoClassInformation){
                                try{
                                    cmdlet.WriteVerbose(cmdlet, "getting ClassName");
                                    if (element.Current.ClassName.Length>0){
                                        elementVerbosity += (" -Class '" + element.Current.ClassName + "'");
                                    }
                                }catch{
                                    if (element.Cached.ClassName.Length>0){
                                        elementVerbosity += (" -Class '" + element.Cached.ClassName + "'");
                                    }
                                }
                            }
                            try{
                                cmdlet.WriteVerbose(cmdlet, "getting Name");
                                if (element.Current.Name.Length>0){
                                    elementVerbosity += (" -Title '" + element.Current.Name + "'");
                                }
                            }catch{
                                if (element.Cached.Name.Length>0){
                                    elementVerbosity += (" -Title '" + element.Cached.Name + "'");
                                }
                            }
                        //}
                            
                        if (cmdlet.lastRecordedItem.Count==0 || elementVerbosity!=cmdlet.lastRecordedItem[0].ToString()){
                            
                            if (!cmdlet.NoEvents){
                                cmdlet.WriteVerbose(cmdlet, "unsubscribe events");
                                unsubscribeEvents(cmdlet); // thePreviouslyUsedElement);
                                cmdlet.WriteVerbose(cmdlet, "events were unsubscribed");
                            }
                            
                            UIAHelper.Highlight(element);
                            
                            cmdlet.lastRecordedItem = new System.Collections.ArrayList();
                            string strElementPatterns = String.Empty;
                            
                            // if (WriteCurrentPattern){
                                // strElementPatterns = String.Empty;
                                try{
                                    System.Windows.Automation.AutomationPattern[] elementPatterns =
                                        element.GetSupportedPatterns();
                                    
                                    if (!cmdlet.NoEvents){
                                        subscribeEvents(cmdlet, element, elementPatterns);
                                    }
                                    
                                    if (cmdlet.WriteCurrentPattern){
                                        foreach (System.Windows.Automation.AutomationPattern ptrn in elementPatterns)
                                        {
                                            strElementPatterns += "\r\n\t\t#";
                                            strElementPatterns +=
                                                ptrn.ProgrammaticName.Replace("Identifiers.Pattern", "");
                                        }
                                        if (strElementPatterns.Length==0){
                                            strElementPatterns += "\r\n\t\t# no supported pattterns";
                                        }
                                    }
                                    strElementPatterns += "\r\n";
                                } catch {
                                    // ErrorRecord
                                    // 20120126
                                    if (element==null){
                                        // continue;
                                        return;
                                    }
                                }
                            //}
                            cmdlet.lastRecordedItem.Add(elementVerbosity);
                            cmdlet.WriteVerbose(elementVerbosity);
                            
                            collectAncestors(cmdlet, element);

                            if (!errorInTheInnerCycle){
                                if (cmdlet.lastRecordedItem.Count>0){
                                    cmdlet.recording.Add(cmdlet.lastRecordedItem);
                                    if (cmdlet.WriteCurrentPattern){
                                        cmdlet.recordingPatterns.Add(strElementPatterns);
                                    }
                                }
                            }
                        // inTheOutercycle:
                            
                            cmdlet.thePreviouslyUsedElement = element;
                        
                        }
                    } catch (Exception eGatheringCycle) {
                        errorInTheGatheringCycle = true;
                        errorMessageInTheGatheringCycle = 
                            eGatheringCycle.Message;
                    }
                cacheRequest.Pop();
                
            } catch (Exception eUnknown) {
                cmdlet.WriteDebug(cmdlet, eUnknown.Message);
            }
        }
            #endregion working with an element
        
            #region collect ancestors
        private static void collectAncestors(TranscriptCmdletBase cmdlet, AutomationElement element)
        {
            System.Windows.Automation.TreeWalker walker = 
                new System.Windows.Automation.TreeWalker(
                    System.Windows.Automation.Condition.TrueCondition);
            System.Windows.Automation.AutomationElement testparent;
            
            try{
                testparent =
                    walker.GetParent(element);
                while (testparent!=null){
                    testparent = 
                        walker.GetParent(testparent);
                    if (testparent!=null){
                        if (testparent==cmdlet.rootElement)
                        { testparent = null; }
                        else{
                            string parentControlType = 
                                // getControlTypeNameOfAutomationElement(testparent, element);
                                // testparent.Current.ControlType.ProgrammaticName.Substring(
                                //    element.Current.ControlType.ProgrammaticName.IndexOf('.') + 1);
                                //
                                // experimental
                                testparent.Current.ControlType.ProgrammaticName.Substring(
                                    testparent.Current.ControlType.ProgrammaticName.IndexOf('.') + 1);
                                //
                                // if (parentControlType.Length==0){
                                //    break;
                                //}
                                
                            // in case this element is an upper-level Pane 
                            // residing directrly under the RootElement
                            // change type to window
                            // i.e. Get-UIAPane -> Get-UIAWindow
                            // since Get-UIAPane is unable to get something more than
                            // a window's child pane control 
                            if (parentControlType=="Pane" || parentControlType=="Menu"){
                                if (walker.GetParent(testparent)==cmdlet.rootElement){
                                    parentControlType = "Window";
                                }
                            }
                                
                            string parentVerbosity = 
                                @"Get-UIA" + parentControlType;
                            try{
                                if (testparent.Current.AutomationId.Length>0){
                                    parentVerbosity += (" -AutomationId '" + testparent.Current.AutomationId + "'");
                                }
                            }catch{ }
                            if (!cmdlet.NoClassInformation){
                                try{
                                    if (testparent.Current.ClassName.Length>0){
                                        parentVerbosity += (" -Class '" + testparent.Current.ClassName + "'");
                                    }
                                }catch{ }
                            }
                            try{
                                if (testparent.Current.Name.Length>0){
                                    parentVerbosity += (" -Title '" + testparent.Current.Name + "'");
                                }
                            }catch{ }

                            if (cmdlet.lastRecordedItem[cmdlet.lastRecordedItem.Count - 1].ToString()!=parentVerbosity){
                                cmdlet.lastRecordedItem.Add(parentVerbosity);
                                cmdlet.WriteVerbose(parentVerbosity);
                            }
                        }
                    }
                }
            } catch (Exception eErrorInTheInnerCycle){
                cmdlet.WriteDebug(cmdlet, eErrorInTheInnerCycle.Message);
                errorMessageInTheInnerCycle =
                    eErrorInTheInnerCycle.Message;
                errorInTheInnerCycle = true;
            }
        }
            #endregion collect ancestors
            
            
        #region Subscribe events
        // private static void subscribeEvents(TranscriptCmdletBase cmdlet, 
        internal static void subscribeEvents(//TranscriptCmdletBase cmdlet, 
                                             // CommonCmdletBase cmdlet,
                                             HasScriptBlockCmdletBase cmdlet,
                                            AutomationElement element,
                                            AutomationPattern[] supportedPatterns)
        {
            try{ // experimental
        
            if (element==null){ return; }
            if (supportedPatterns==null ||
                supportedPatterns.Length<1){ return; }
            try{
                
                // cache requiest object for collecting properties
                CacheRequest cacheRequest = null;
                
                try{
                    cacheRequest = new CacheRequest();
                    cacheRequest.AutomationElementMode = AutomationElementMode.None;
                    cacheRequest.TreeFilter = Automation.RawViewCondition;
                    cacheRequest.Add(AutomationElement.NameProperty);
                    // cacheRequest.Add(AutomationElement.AutomationIdProperty);
                    cacheRequest.Add(AutomationElement.ControlTypeProperty);
                    // cacheRequest.Add(AutomationElement.ClassNameProperty);
                    // cacheRequest.Push();
                    cacheRequest.Activate();
                    // element.FindFirst(TreeScope.Element, Condition.TrueCondition);
                } catch (Exception eCacheRequest){
                    cmdlet.WriteVerbose("Cahce request failed for" + element.Current.Name);
                    cmdlet.WriteVerbose(eCacheRequest.Message);
                }
                
                foreach (AutomationPattern pattern in supportedPatterns){
                    try{
                        if (element==null) { break; }
                        switch (pattern.ProgrammaticName){
                            case "SelectionItemPatternIdentifiers.Pattern":
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  SelectionItemPattern.ElementAddedToSelectionEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet,
                                                  element,
                                                  SelectionItemPattern.ElementRemovedFromSelectionEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet,
                                                  element,
                                                  SelectionItemPattern.ElementSelectedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                break;
                            case "SelectionPatternIdentifiers.Pattern":
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  SelectionPattern.InvalidatedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                break;
                            case "InvokePatternIdentifiers.Pattern":
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  InvokePattern.InvokedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                break;
                            case "AutomationElementIdentifiers.Pattern":
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.AsyncContentLoadedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.LayoutInvalidatedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.MenuClosedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.MenuOpenedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.ToolTipClosedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.ToolTipOpenedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.AutomationFocusChangedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.AutomationPropertyChangedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  AutomationElement.StructureChangedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                break;
                            case "TextPatternIdentifiers.Pattern":
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  TextPattern.TextChangedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  TextPattern.TextSelectionChangedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                break;
                            case "WindowPatternIdentifiers.Pattern":
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  WindowPattern.WindowOpenedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                cmdlet.SubscribeToEvents(cmdlet, 
                                                  element,
                                                  WindowPattern.WindowClosedEvent,
                                                  null,
                                                  cmdlet.OnUIRecordingAutomationEvent);
                                break;
                        }
                    } 
                    catch { }
//                    // finish our cache request
//                    cacheRequest.Pop();
                }
                
                // finish our cache request
                cacheRequest.Pop();
                
            } 
            catch { return; }
            cmdlet.ElementToSubscribe = element; // ???????????????
            
            
            } 
            catch (Exception eUnknown) {
                // WriteVerbose("!!!!!subscribeEvents " + eUnknown.Message);
                cmdlet.WriteDebug(cmdlet, eUnknown.Message);
            } // experimental
        }
        #endregion Subscribe events
        
        #region Unsubscribe events
        private static void unsubscribeEvents(TranscriptCmdletBase cmdlet) // AutomationElement element)
        {
            try{
                if (cmdlet.subscribedEvents!=null &&
                    cmdlet.subscribedEvents.Count>0 && 
                    cmdlet.thePreviouslyUsedElement!=null){
                    for (int i = 0; i<cmdlet.subscribedEvents.Count; i++){
                        Automation.RemoveAutomationEventHandler(
                            (AutomationEvent)cmdlet.subscribedEventsIds[i],
                            cmdlet.thePreviouslyUsedElement,
                            (AutomationEventHandler)cmdlet.subscribedEvents[i]);
                    }
                }
            } catch (Exception eUnknown) {
                cmdlet.WriteDebug(cmdlet, eUnknown.Message);
                return;
            }
        }
        #endregion Unsubscribe events
            
        #endregion Start-UIATranscript
        
        // temporary!!!
        // internal static System.Windows.Automation.AutomationElement GetAutomationElementFromPoint()
        public static System.Windows.Automation.AutomationElement GetAutomationElementFromPoint()
        {
            System.Windows.Automation.AutomationElement result = 
                null;
            try{
                element = 
                    AutomationElement.FromPoint(new System.Windows.Point(
                        Cursor.Position.X, 
                        Cursor.Position.Y));
                result = element;
            } 
            catch { }
            return result;
        }
        
        internal static System.Windows.Automation.AutomationPattern[] GetElementPatternsFromPoint()
        {
            System.Windows.Automation.AutomationPattern[] result = null;
            GetAutomationElementFromPoint();
            result = element.GetSupportedPatterns();
            return result;
        }
        
        #region get an ancestor with a handle
        internal static AutomationElement GetAncestorWithHandle(AutomationElement element)
        {
            System.Windows.Automation.TreeWalker walker = 
                new System.Windows.Automation.TreeWalker(
                    System.Windows.Automation.Condition.TrueCondition);
            System.Windows.Automation.AutomationElement testparent;
            
            try{
                testparent =
                    walker.GetParent(element);
                while (testparent!=null && 
                       testparent.Current.NativeWindowHandle==0){
                    testparent = 
                        walker.GetParent(testparent);
                    if (testparent!=null && 
                        testparent.Current.NativeWindowHandle!=0){
                            return testparent;
                    }
                }
                if (testparent.Current.NativeWindowHandle!=0){
                    return testparent;
                } else {
                    return null;
                }
            } catch {
                return null;
            }
        }
        #endregion get an ancestor with a handle
        
        internal static object GetCurrentPatternFromPoint()
        {
            object result = null;
            GetAutomationElementFromPoint();
            result = 
                GetCurrentPattern(ref element,
                                  null);
            if (result!=null){
                // writer.WriteLine("GetCurrentPatternFromPoint: result = " + result.ToString());
                ////writer.WriteLine(GetCurrentPatternFromPoint: element.Current.ClassName);
            } else {
                // writer.WriteLine("GetCurrentPatternFromPoint: result = null");
            }
            // writer.Flush();
            return result;
        }
        
        internal static System.Windows.Automation.AutomationPattern GetPatternByName(string patternName)
        {
            System.Windows.Automation.AutomationPattern result = null;
            // normalize name
            patternName = 
                patternName.Substring(0, patternName.Length - 7) + 
                "Pattern";
            patternName.Replace("dock", "Dock");
            switch (patternName) {
                case "DockPattern":
                    result = System.Windows.Automation.DockPattern.Pattern;
                    break;
                case "ExpandCollapsePattern":
                    result = System.Windows.Automation.ExpandCollapsePattern.Pattern;
                    break;
                case "GridItemPattern":
                    result = System.Windows.Automation.GridItemPattern.Pattern;
                    break;
                case "GridPattern":
                    result = System.Windows.Automation.GridPattern.Pattern;
                    break;
                case "InvokePattern":
                    result = System.Windows.Automation.InvokePattern.Pattern;
                    break;
                case "MultipleViewPattern":
                    result = System.Windows.Automation.MultipleViewPattern.Pattern;
                    break;
                case "RangeValuePattern":
                    result = System.Windows.Automation.RangeValuePattern.Pattern;
                    break;
                case "ScrollItemPattern":
                    result = System.Windows.Automation.ScrollItemPattern.Pattern;
                    break;
                case "ScrollPattern":
                    result = System.Windows.Automation.ScrollPattern.Pattern;
                    break;
                case "SelectionItemPattern":
                    result = System.Windows.Automation.SelectionItemPattern.Pattern;
                    break;
                case "SelectionPattern":
                    result = System.Windows.Automation.SelectionPattern.Pattern;
                    break;
                case "TableItemPattern":
                    result = System.Windows.Automation.TableItemPattern.Pattern;
                    break;
                case "TablePattern":
                    result = System.Windows.Automation.TablePattern.Pattern;
                    break;
                case "TextPattern":
                    result = System.Windows.Automation.TextPattern.Pattern;
                    break;
                case "TogglePattern":
                    result = System.Windows.Automation.TogglePattern.Pattern;
                    break;
                case "TransformPattern":
                    result = System.Windows.Automation.TransformPattern.Pattern;
                    break;
                case "ValuePattern":
                    result = System.Windows.Automation.ValuePattern.Pattern;
                    break;
                case "WindowPattern":
                    result = System.Windows.Automation.WindowPattern.Pattern;
                    break;
                default:
                    result = null;
                    break; 
            }
            return result;
        }
        
        internal static object GetCurrentPattern(
            ref System.Windows.Automation.AutomationElement element,
            System.Windows.Automation.AutomationPattern patternType)
        {
            object result = 
                null;
            try{
                System.Windows.Automation.AutomationPattern[] supportedPatterns = 
                    element.GetSupportedPatterns();
                if (supportedPatterns==null ||
                    supportedPatterns.Length<1)
                {
                    return result;
                }
                object pattern = null;
                foreach(System.Windows.Automation.AutomationPattern ptrn in supportedPatterns)
                {
                    if (patternType.ProgrammaticName==ptrn.ProgrammaticName ||
                           patternType==null){
                        if (element.TryGetCurrentPattern(
                                ptrn, out pattern))
                        {
                            object resPattern = 
                                element.GetCurrentPattern(ptrn);
                                    // as System.Windows.Automation.AutomationPattern;
                            result = resPattern;
                            return result;
                        }
                    }
                }
            } catch //(Exception ePatternProblem){
                //
            {

            }
            return result;
        }
        
        public static System.Windows.Automation.ControlType 
            GetControlTypeByTypeName(string controlType)
        {
            System.Windows.Automation.ControlType ctrlType = null;
            string _controlType = controlType.ToUpper();
            switch (_controlType)
            {
                case "BUTTON": { ctrlType = System.Windows.Automation.ControlType.Button; break; }
                case "CALENDAR": { ctrlType = System.Windows.Automation.ControlType.Calendar; break; }
                case "CHECKBOX": { ctrlType = System.Windows.Automation.ControlType.CheckBox; break; }
                case "COMBOBOX": { ctrlType = System.Windows.Automation.ControlType.ComboBox; break; }
                case "CUSTOM": { ctrlType = System.Windows.Automation.ControlType.Custom; break; }
                case "DATAGRID": { ctrlType = System.Windows.Automation.ControlType.DataGrid; break; }
                case "DATAITEM": { ctrlType = System.Windows.Automation.ControlType.DataItem; break; }
                case "DOCUMENT": { ctrlType = System.Windows.Automation.ControlType.Document; break; }
                case "EDIT": { ctrlType = System.Windows.Automation.ControlType.Edit; break; }
                case "GROUP": { ctrlType = System.Windows.Automation.ControlType.Group; break; }
                case "HEADER": { ctrlType = System.Windows.Automation.ControlType.Header; break; }
                case "HEADERITEM": { ctrlType = System.Windows.Automation.ControlType.HeaderItem; break; }
                case "HYPERLINK": { ctrlType = System.Windows.Automation.ControlType.Hyperlink; break; }
                case "IMAGE": { ctrlType = System.Windows.Automation.ControlType.Image; break; }
                case "LIST": { ctrlType = System.Windows.Automation.ControlType.List; break; }
                case "LISTITEM": { ctrlType = System.Windows.Automation.ControlType.ListItem; break; }
                case "MENU": { ctrlType = System.Windows.Automation.ControlType.Menu; break; }
                case "MENUBAR": { ctrlType = System.Windows.Automation.ControlType.MenuBar; break; }
                case "MENUITEM": { ctrlType = System.Windows.Automation.ControlType.MenuItem; break; }
                case "PANE": { ctrlType = System.Windows.Automation.ControlType.Pane; break; }
                case "PROGRESSBAR": { ctrlType = System.Windows.Automation.ControlType.ProgressBar; break; }
                case "RADIOBUTTON": { ctrlType = System.Windows.Automation.ControlType.RadioButton; break; }
                case "SCROLLBAR": { ctrlType = System.Windows.Automation.ControlType.ScrollBar; break; }
                case "SEPARATOR": { ctrlType = System.Windows.Automation.ControlType.Separator; break; }
                case "SLIDER": { ctrlType = System.Windows.Automation.ControlType.Slider; break; }
                case "SPINNER": { ctrlType = System.Windows.Automation.ControlType.Spinner; break; }
                case "SPLITBUTTON": { ctrlType = System.Windows.Automation.ControlType.SplitButton; break; }
                case "STATUSBAR": { ctrlType = System.Windows.Automation.ControlType.StatusBar; break; }
                case "TAB": { ctrlType = System.Windows.Automation.ControlType.Tab; break; }
                case "TABITEM": { ctrlType = System.Windows.Automation.ControlType.TabItem; break; }
                case "TABLE": { ctrlType = System.Windows.Automation.ControlType.Table; break; }
                case "TEXT": { ctrlType = System.Windows.Automation.ControlType.Text; break; }
                case "THUMB": { ctrlType = System.Windows.Automation.ControlType.Thumb; break; }
                case "TITLEBAR": { ctrlType = System.Windows.Automation.ControlType.TitleBar; break; }
                case "TOOLBAR": { ctrlType = System.Windows.Automation.ControlType.ToolBar; break; }
                case "TOOLTIP": { ctrlType = System.Windows.Automation.ControlType.ToolTip; break; }
                case "TREE": { ctrlType = System.Windows.Automation.ControlType.Tree; break; }
                case "TREEITEM": { ctrlType = System.Windows.Automation.ControlType.TreeItem; break; }
                case "WINDOW": { ctrlType = System.Windows.Automation.ControlType.Window; break; }
                default:
                    // WriteVerbose(this, "there's no ControlType value.");
                    ctrlType = null;
                    break;
            }
            return ctrlType;
        }
        
        internal static bool GetHeaderItems(
            ref System.Windows.Automation.AutomationElement element,
            out string strResultString,
            char delimiter)
        {
            System.Windows.Automation.AutomationElementCollection headerItems = 
                element.FindAll(
                    System.Windows.Automation.TreeScope.Descendants,
                    new System.Windows.Automation.PropertyCondition(
                        System.Windows.Automation.AutomationElement.ControlTypeProperty,
                        System.Windows.Automation.ControlType.HeaderItem));
            if (headerItems==null || headerItems.Count==0){
                // WriteVerbose(this, "no headers found");
                strResultString = 
                    "no headers found";
                return false;
            }
            else {
                string headersRow = String.Empty;
                // foreach (System.Windows.Automation.AutomationElement elt in
                //         headerItems)
                for (int headersCounter = 0;
                     headersCounter<headerItems.Count;
                     headersCounter++)
                {
                    headersRow += '"';
                    // headersRow += elt.Current.Name;
                    headersRow +=
                        headerItems[headersCounter].Current.Name;
                    headersRow += '"';
                    if (headersCounter<(headerItems.Count - 1)){
                        headersRow += delimiter;
                    }
                }
                // output headers
                // WriteObject(headersRow);
                strResultString = headersRow;
                return true;
            }
        }
        
        internal static bool GetHeaders(
            ref System.Windows.Automation.AutomationElement element,
            out string strResultString,
            char delimiter)
        {
            System.Windows.Automation.AutomationElementCollection headerItems = 
                element.FindAll(
                    System.Windows.Automation.TreeScope.Descendants,
                    new System.Windows.Automation.PropertyCondition(
                        System.Windows.Automation.AutomationElement.ControlTypeProperty,
                        System.Windows.Automation.ControlType.Header));
            if (headerItems==null || headerItems.Count==0){
                // WriteVerbose(this, "no headers found");
                strResultString = 
                    "no headers found";
                return false;
            }
            else {
                string headersRow = String.Empty;
                // foreach (System.Windows.Automation.AutomationElement elt in
                //         headerItems)
                for (int headersCounter = 0;
                     headersCounter<headerItems.Count;
                     headersCounter++)
                {
                    headersRow += '"';
                    // headersRow += elt.Current.Name;
                    headersRow +=
                        headerItems[headersCounter].Current.Name;
                    headersRow += '"';
                    if (headersCounter<(headerItems.Count - 1)){
                        headersRow += delimiter;
                    }
                }
                // output headers
                // WriteObject(headersRow);
                strResultString = headersRow;
                return true;
            }
        }
        
        internal static string GetOutputStringUsingTableGridPattern<T>(
            T pattern,
            int columnsNumber,
            int rowsCounter,
            char delimiter)
        {
            // string result = String.Empty;
            string onerow = String.Empty;
            for (int columnsCounter = 0;
                 columnsCounter<columnsNumber;
                 columnsCounter++){
                onerow += '"';
                object[] coordinates = 
                    { rowsCounter, columnsCounter };
                onerow +=
                    ((System.Windows.Automation.AutomationElement)
                     pattern.GetType().GetMethod(
                        "GetItem").Invoke(
                            pattern,
                            coordinates)).Current.Name;
                            //{ coordinates)).Current.Name;
//                    pattern.GetType().InvokeMember(
//                        "GetItem",
//                        System.Reflection.BindingFlags.Public,
//                    pattern.GetItem(
//                        rowsCounter,
//                        columnsCounter).Current.Name;
                onerow += '"';
                if (columnsCounter<(columnsNumber - 1)){
                    onerow += delimiter;
                }
            }
            return onerow;
        }
        
        internal static System.Collections.Generic.List<string> 
            GetOutputStringUsingItemsValuePattern(
                AutomationElement control,
                char delimiter)
        {
            System.Collections.Generic.List<string> rowsCollection = 
                new System.Collections.Generic.List<string>();
            
            AutomationElementCollection rows = 
                control.FindAll(TreeScope.Children,
                           new PropertyCondition(
                               AutomationElement.ControlTypeProperty,
                               ControlType.Custom));
            if (rows.Count>0){
                foreach( AutomationElement row in rows){
                    AutomationElementCollection rowItems = 
                        row.FindAll(TreeScope.Children,
                                    new PropertyCondition(
                                        AutomationElement.ControlTypeProperty,
                                        ControlType.Custom));
                    if (rowItems.Count>0){
                        string onerow = String.Empty;
                        for(int i = 0; i<rowItems.Count; i++){
                            ValuePattern valPattern = null;
                            string strValue = String.Empty;
                            strValue += '"';
                            try{
                                valPattern = 
                                    rowItems[i].GetCurrentPattern(ValuePattern.Pattern)
                                    as ValuePattern;
                                strValue += valPattern.Current.Value;
                            } catch {
                                // nothing to do
                            }
                            strValue += '"';
                            onerow += strValue;
                            if (i<(rowItems.Count - 1)){
                                onerow += delimiter;
                            }
                        }
                        if (onerow.Length>2){
                            rowsCollection.Add(onerow);
                        }
                        // WriteObject(onerow);
                    }
                }
            } else {
                return rowsCollection;
            }
            return rowsCollection;
        }
        
    }
}