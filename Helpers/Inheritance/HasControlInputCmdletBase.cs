/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 08.12.2011
 * Time: 8:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Management.Automation;
using System.Windows.Automation;
using System.Runtime.InteropServices;

namespace UIAutomation
{
    
        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MOUSEINPUT")]
        internal struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYBDINPUT")]
        internal struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HARDWAREINPUT")]
        internal struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        
        [StructLayout(LayoutKind.Explicit)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MOUSEKEYBDHARDWAREINPUT")]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;

            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;

            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INPUT")]
        internal struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
    
    /// <summary>
    /// Description of HasControlInputCmdletBase.
    /// </summary>
    public class HasControlInputCmdletBase : HasScriptBlockCmdletBase // CommonCmdletBase
    {
        #region Constructor
        public HasControlInputCmdletBase()
        {
            InputObject = null;
            PassThru = true;
        }
        #endregion Constructor
        
        #region Parameters
        [ValidateNotNullOrEmpty()]
        // 20120123
        //[Parameter(Mandatory=true, 
        [Parameter(Mandatory=false, 
            ValueFromPipeline=true, 
            Position=0,
            HelpMessage="This is usually the output from Get-UIAControl" )] 
        public virtual System.Windows.Automation.AutomationElement InputObject { get; set; }
        [Parameter(Mandatory=false)]
        public virtual SwitchParameter PassThru { get; set; }
        #endregion Parameters
        
        #region declarations
        // http://msdn.microsoft.com/en-us/library/windows/desktop/ms646244(v=vs.85).aspx
        protected uint WM_MOUSEACTIVATE             = 0x0021;
        protected uint WM_LBUTTONDOWN               = 0x0201;
        protected uint WM_LBUTTONUP                 = 0x0202;
        protected uint WM_LBUTTONDBLCLK             = 0x0203;
        protected uint WM_RBUTTONDOWN               = 0x0204;
        protected uint WM_RBUTTONUP                 = 0x0205;
        protected uint WM_RBUTTONDBLCLK             = 0x0206;
        protected uint WM_MBUTTONDOWN               = 0x0207;
        protected uint WM_MBUTTONUP                 = 0x0208;
        protected uint WM_MBUTTONDBLCLK             = 0x0209;
        
        protected uint MK_CONTROL                    = 0x0008;    // The CTRL key is down.
        protected uint MK_LBUTTON                    = 0x0001;    // The left mouse button is down.
        protected uint MK_MBUTTON                    = 0x0010;    // The middle mouse button is down.
        protected uint MK_RBUTTON                    = 0x0002;    // The right mouse button is down.
        protected uint MK_SHIFT                        = 0x0004;    // The SHIFT key is down.
        // 20120206 protected uint MK_XBUTTON1                    = 0x0020;    // The first X button is down.
        // 20120206 protected uint MK_XBUTTON2                    = 0x0040;    // The second X button is down.
        
        protected uint WM_KEYDOWN                   = 0x0100;
        protected uint WM_KEYUP                     = 0x0101;
        protected uint WM_SYSKEYDOWN                = 0x0104;
        protected uint WM_SYSKEYUP                  = 0x0105;
        
        // http://msdn.microsoft.com/en-us/library/ms927178.aspx
        protected uint VK_SHIFT                        = 0x0010;    // SHIFT key
        protected uint VK_CONTROL                    = 0x0011;    // CTRL key
        
        protected uint VK_LSHIFT                    = 0xA0;    // Left SHIFT
        protected uint VK_RSHIFT                    = 0xA1;    // Right SHIFT
        protected uint VK_LCONTROL                    = 0xA2;    // Left CTRL
        protected uint VK_RCONTROL                    = 0xA3;    // Right CTRL
            
        [DllImport("user32.dll", EntryPoint="PostMessage", CharSet=CharSet.Auto)]
        protected static extern bool PostMessage1(IntPtr hWnd, uint Msg,
                                                IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", EntryPoint="SendMessage", CharSet=CharSet.Auto)]
        protected static extern bool SendMessage1(IntPtr hWnd, uint Msg,
                                                IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll")]
        // static extern bool SetCursorPos(int X, int Y);
        protected static extern bool SetCursorPos(int X, int Y);
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INPUT")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MOUSE")]
        protected const int INPUT_MOUSE = 0;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INPUT")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYBOARD")]
        protected const int INPUT_KEYBOARD = 1;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INPUT")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HARDWARE")]
        protected const int INPUT_HARDWARE = 2;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYEVENTF")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "EXTENDEDKEY")]
        protected const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYEVENTF")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYUP")]
        protected const uint KEYEVENTF_KEYUP = 0x0002;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYEVENTF")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UNICODE")]
        protected const uint KEYEVENTF_UNICODE = 0x0004;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KEYEVENTF")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SCANCODE")]
        protected const uint KEYEVENTF_SCANCODE = 0x0008;
        

        
        [DllImport("user32.dll", SetLastError = true)]
        // protected static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        internal static extern uint SendInput(uint nInputs, INPUT pInputs, int cbSize);
        
//        [DllImport("user32.dll")]
//        protected static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
//           UIntPtr dwExtraInfo);
        
        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "keybd")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "event")]
        protected static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
           int dwExtraInfo);
        
        
        [DllImport("user32.dll")]
        protected static extern short GetKeyState(uint vkCode);
        
        #endregion declarations
        
        protected override void BeginProcessing()
        {
            
        }
        
        protected bool GetColorProbe(HasControlInputCmdletBase cmdlet,
                                     AutomationElement element)
        {
            bool result = false;
            
            
            
            
            
            return result;
        }
        
        protected bool ClickControl(HasControlInputCmdletBase cmdlet,
                                    AutomationElement element,
                                    bool RightClick,
                                    bool MidClick,
                                    bool Alt,
                                    bool Shift,
                                    bool Ctrl,
                                    bool inSequence,
                                    bool DoubleClick,
                                    int RelativeX,
                                    int RelativeY)
        {
            bool result = false;
            
            AutomationElement whereToClick = 
                element;
            WriteVerbose(cmdlet, 
                         "where the click will be performed: " +
                         element.Current.Name);
            AutomationElement whereTheHandle = 
                whereToClick;
            
            if (whereToClick.Current.NativeWindowHandle==0){
            
                WriteVerbose(cmdlet, "The handle of this control equals to zero");
                WriteVerbose(cmdlet, "trying to use one of its ancestors");
                
                whereTheHandle = 
                    UIAHelper.GetAncestorWithHandle(whereToClick);
                if (whereTheHandle.Current.NativeWindowHandle==0){
                    ErrorRecord err = 
                        new ErrorRecord(new Exception("The handle of this control equals to zero"),
                                        "ZeroHandle",
                                        ErrorCategory.InvalidArgument,
                                        whereTheHandle);
                    err.ErrorDetails = 
                        new ErrorDetails("This control does not have a handle");
                    WriteError(cmdlet, err, true);
                    // return;
                    // 20120209 result = false;
                    // 20120209 return result;
                } else {
                    WriteVerbose(cmdlet, 
                                 "the control with a handle is " + 
                                 whereTheHandle.Current.Name);
                    WriteVerbose(cmdlet, 
                                 "its handle is " + 
                                 whereTheHandle.Current.NativeWindowHandle.ToString());
                    WriteVerbose(cmdlet, 
                                 "its rectangle is " + 
                                 whereTheHandle.Current.BoundingRectangle.ToString());
                }
            }
            
            WriteVerbose(cmdlet, 
                         "the element the click will be performed on has rectangle: " + 
                         whereToClick.Current.BoundingRectangle.ToString());
            
            // these x and y are for the SetCursorPos call
            // they are screen coordinates
            int x = (int)whereToClick.Current.BoundingRectangle.X + 
                ((int)whereToClick.Current.BoundingRectangle.Width / 2); // + 3;
            int y = (int)whereToClick.Current.BoundingRectangle.Y + 
                ((int)whereToClick.Current.BoundingRectangle.Height / 2); // + 3;
            
            // if the -X and -Y paramters are supplied (only for SetCursorPos)
            if (RelativeX!=0 && RelativeY!=0){
                x = RelativeX;
                y = RelativeY;
                WriteVerbose(cmdlet, "coordinates are taken from the input parameters");
            }
            WriteVerbose(cmdlet, "X = " + x.ToString());            
            WriteVerbose(cmdlet, "Y = " + y.ToString());
            
            // PostMessage's (click) second parameter
            uint uDown = 0;
            uint uUp = 0;
            
            // these x and y are window-related coordinates
            int relativeX = x - (int)whereTheHandle.Current.BoundingRectangle.X;
            int relativeY = y - (int)whereTheHandle.Current.BoundingRectangle.Y;
            WriteVerbose(cmdlet, "relative X = " + relativeX.ToString());            
            WriteVerbose(cmdlet, "relative Y = " + relativeY.ToString());
            
            // PostMessage's (click) third and fourth paramters (the third'll be reasigned later)
            System.IntPtr wParamDown = IntPtr.Zero;
            System.IntPtr wParamUp = IntPtr.Zero;
            System.IntPtr lParam = 
                new IntPtr(((new IntPtr(relativeX)).ToInt32() & 0xFFFF) +
                           (((new IntPtr(relativeY)).ToInt32() & 0xFFFF) << 16));
            
            // PostMessage's (keydown/keyup) fourth parameter
            uint uCtrlDown = 0x401D;
            uint uCtrlUp = 0xC01D;
            uint uShiftDown = 0x402A;
            uint uShiftUp = 0xC02A;
            System.IntPtr lParamKeyDown = IntPtr.Zero;
            System.IntPtr lParamKeyUp = IntPtr.Zero;
            
//            if (Ctrl){
//                unsafe{
//                    KEYBDINPUT kb;//={ 0 };
//                    INPUT Input = new INPUT(); //={ 0 };
//    //                if (bExtended)
//    //                    kb.dwFlags = KEYEVENT_EXTENDEDKEY;
//                    kb.wVk = (ushort)VK_CONTROL;
//                    Input.Type = INPUT_KEYBOARD;
//                    
//                    // Input.Data = kb;
//                    SendInput(1, Input, sizeof(INPUT));
//                }
//            }
            
            if (Ctrl){
                lParamKeyDown = 
                    new IntPtr(((new IntPtr(0x0001)).ToInt32() & 0xFFFF) +
                               (((new IntPtr(uCtrlDown)).ToInt32() & 0xFFFF) << 16));
                lParamKeyUp = 
                    new IntPtr(((new IntPtr(0x0001)).ToInt32() & 0xFFFF) +
                               (((new IntPtr(uCtrlUp)).ToInt32() & 0xFFFF) << 16));
                WriteVerbose(this, "control parameters for KeyDown/KeyUp have been prepared");
            }
            if (Shift){
                lParamKeyDown = 
                    new IntPtr(((new IntPtr(0x0001)).ToInt32() & 0xFFFF) +
                               (((new IntPtr(uShiftDown)).ToInt32() & 0xFFFF) << 16));
                lParamKeyUp = 
                    new IntPtr(((new IntPtr(0x0001)).ToInt32() & 0xFFFF) +
                               (((new IntPtr(uShiftUp)).ToInt32() & 0xFFFF) << 16));
                WriteVerbose(this, "shift parameters for KeyDown/KeyUp have been prepared");
            }
            // PostMessage's (activate) third parameter
            uint ulAct = 0;
            uint uhAct = 0;
            
            uint mask = 0;
            if (Ctrl){
                mask |= MK_CONTROL;
                WriteVerbose(this, "control parameters for ButtonDown/ButtonUp have been prepared");
            }
            if (Shift){
                mask |= MK_SHIFT;
                WriteVerbose(this, "shift parameters for ButtonDown/ButtonUp have been prepared");
            }
            
            if (RightClick && !DoubleClick){
                WriteVerbose(cmdlet, "right click");
                uhAct = uDown = WM_RBUTTONDOWN;
                uUp = WM_RBUTTONUP;
                wParamDown = new IntPtr(MK_RBUTTON | mask);
                wParamUp = new IntPtr(mask);
                ulAct = MK_RBUTTON;
            } else if (RightClick && DoubleClick){
                WriteVerbose(cmdlet, "right double click");
                uhAct = uDown = WM_RBUTTONDBLCLK;
                uUp = WM_RBUTTONUP;
                wParamDown = new IntPtr(MK_RBUTTON | mask);
                wParamUp = new IntPtr(mask);
                ulAct = MK_RBUTTON;
            } else if (MidClick && !DoubleClick) {
                WriteVerbose(cmdlet, "middle button click");
                uhAct = uDown = WM_MBUTTONDOWN;
                uUp = WM_MBUTTONUP;
                wParamDown = new IntPtr(MK_MBUTTON | mask);
                wParamUp = new IntPtr(mask);
                ulAct = MK_MBUTTON;
            } else if (MidClick && DoubleClick) {
                WriteVerbose(cmdlet, "middle button double click");
                uhAct = uDown = WM_MBUTTONDBLCLK;
                uUp = WM_MBUTTONUP;
                wParamDown = new IntPtr(MK_MBUTTON | mask);
                wParamUp = new IntPtr(mask);
                ulAct = MK_MBUTTON;
            } else if (DoubleClick){
                WriteVerbose(cmdlet, "left double click");
                uhAct = uDown = WM_LBUTTONDBLCLK;
                uUp = WM_LBUTTONUP;
                wParamDown = new IntPtr(MK_LBUTTON | mask);
                wParamUp = new IntPtr(mask);
                ulAct = MK_LBUTTON;
            } else {
                WriteVerbose(cmdlet, "left click");
                uhAct = uDown = WM_LBUTTONDOWN;
                uUp = WM_LBUTTONUP;
                wParamDown = new IntPtr(MK_LBUTTON | mask);
                wParamUp = new IntPtr(mask);
                ulAct = MK_LBUTTON;
            }
            
            System.IntPtr handle =
                    new System.IntPtr(whereTheHandle.Current.NativeWindowHandle);
            WriteVerbose(cmdlet, 
                         "the handle of the element the click will be performed on is " + 
                         handle.ToString());
            
//            SetCursorPos((int)whereToClick.Current.BoundingRectangle.X,
//                         (int)whereToClick.Current.BoundingRectangle.Y);
            WriteVerbose(cmdlet, "X = " + x.ToString());
            WriteVerbose(cmdlet, "Y = " + y.ToString());
            
//            // IntPtr hDesk = OpenInputDesktop(0, true, 0);
//            // IntPtr hDesk = OpenInputDesktop(DF_ALLOWOTHERACCOUNTHOOK, true, GENERIC_ALL);
//            // IntPtr hDesk = OpenInputDesktop(0, true, 0x0000037f);
//            
//            IntPtr hOrigThread = GetThreadDesktop(GetCurrentThreadId());
//            IntPtr hOrigDesktop = OpenInputDesktop(0, false, DESKTOP_SWITCHDESKTOP);
//            WriteVerbose(this, 
//                         "the handle to the input desktop is " + 
//                         hOrigDesktop.ToString());
//            // MessageBox.Show(x.ToString());
//            bool Success = SetThreadDesktop(hOrigDesktop);
//            WriteVerbose(this, 
//                         "the input desktop has been set (SetThreadDesktop): " + 
//                         Success.ToString());
//            Success = SwitchDesktop(hOrigDesktop);
//            // MessageBox.Show(Success.ToString());
//            WriteVerbose(this, 
//                         "the input desktop has been set (SwitchDesktop): " + 
//                         Success.ToString());
            
            
//  // Save original desktop
//  hOriginalThread = GetThreadDesktop(GetCurrentThreadId());
//  hOriginalInput = OpenInputDesktop(0, FALSE, DESKTOP_SWITCHDESKTOP);
//
//  // Create a new Desktop and switch to it
//  hNewDesktop = CreateDesktop("NewDesktopName", NULL, NULL, 0, GENERIC_ALL, NULL);
//  SetThreadDesktop(hNewDesktop);
//  SwitchDesktop(hNewDesktop);
//
//  // Execute thread/process in the new desktop
//  StartThread();
//  StartProcess();
//
//  // Restore original desktop
//  SwitchDesktop(hOriginalInput);
//  SetThreadDesktop(hOriginalThread);
//
//  // Close the Desktop
//  CloseDesktop(hNewDesktop);
            
            try{
                whereTheHandle.SetFocus();
            } catch { }
            
            bool setCursorPosResult = 
                SetCursorPos(x, y);
            WriteVerbose(cmdlet, "SetCursorPos result = " + setCursorPosResult.ToString());
            
            System.Threading.Thread.Sleep(Preferences.OnClickDelay);
            
            
            // trying to heal context menu clicks
            System.Diagnostics.Process windowProcess = 
                System.Diagnostics.Process.GetProcessById(
                    whereTheHandle.Current.ProcessId);
            if (windowProcess!=null){
                IntPtr mainWindow = 
                    windowProcess.MainWindowHandle;
                if (mainWindow!=IntPtr.Zero){
//                    System.IntPtr lParam2 = 
//                        new IntPtr(((new IntPtr(0x0001)).ToInt32() & 0xFFFF) +
//                                   (((new IntPtr(0x0201)).ToInt32() & 0xFFFF) << 16));
                    System.IntPtr lParam2 = 
                        new IntPtr(((new IntPtr(ulAct)).ToInt32() & 0xFFFF) +
                                   (((new IntPtr(uhAct)).ToInt32() & 0xFFFF) << 16));
                    bool res0 = 
                        SendMessage1(handle, WM_MOUSEACTIVATE, 
                                     mainWindow, lParam2);
                    WriteVerbose(this, "WM_MOUSEACTIVATE is sent");
                }
            }
            
//            if (Ctrl){
//                PostMessage1(handle, WM_KEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
//            }
//            if (Shift){
//                PostMessage1(handle, WM_KEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
//            }
            
            if (Ctrl){
//                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
//                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
//                WriteVerbose(this, "WM_SYSKEYDOWN VK_CONTROL");
                // press the control key
                // keybd_event((byte)VK_LCONTROL, 0x45, 0, 0);
                keybd_event((byte)VK_LCONTROL, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);
                WriteVerbose(this, " the control button has been pressed");
            }
            if (Shift){
//                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
//                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
//                WriteVerbose(this, "WM_SYSKEYDOWN VK_SHIFT");
                // press the shift key
                // keybd_event((byte)VK_LSHIFT, 0x45, 0, 0);
                keybd_event((byte)VK_LSHIFT, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);
                WriteVerbose(this, " the shift button has been pressed");
            }
            
            bool res1 = PostMessage1(handle, uDown, wParamDown, lParam);
            
//            if (Ctrl){
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
////                WriteVerbose(this, "WM_SYSKEYDOWN VK_CONTROL");
//                // press the control key
//                keybd_event((byte)VK_CONTROL, 0x45, 0, 0);
//            }
//            if (Shift){
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
////                WriteVerbose(this, "WM_SYSKEYDOWN VK_SHIFT");
//                // press the shift key
//                keybd_event((byte)VK_SHIFT, 0x45, 0, 0);
//            }
            
            // bool res2 = PostMessage1(handle, uUp, IntPtr.Zero, lParam);
            // bool res2 = PostMessage1(handle, uUp, wParam, lParam);
            // System.Threading.Thread.Sleep(50);
            bool res2 = PostMessage1(handle, uUp, wParamUp, lParam);
            
//            if (Ctrl){
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_CONTROL), lParamKeyDown);
////                WriteVerbose(this, "WM_SYSKEYDOWN VK_CONTROL");
//            }
//            if (Shift){
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
////                PostMessage1(handle, WM_SYSKEYDOWN, new IntPtr(VK_SHIFT), lParamKeyDown);
////                WriteVerbose(this, "WM_SYSKEYDOWN VK_SHIFT");
//            }
            
            if (!inSequence){
                if (Ctrl){
//                    PostMessage1(handle, WM_SYSKEYUP, new IntPtr(VK_CONTROL), lParamKeyUp);
//                    WriteVerbose(this, "WM_SYSKEYUP VK_CONTROL");
                    //    ::ZeroMemory
                    // release the control key
                    // keybd_event((byte)VK_LCONTROL, 0x45, KEYEVENTF_KEYUP, 0);
                    keybd_event((byte)VK_LCONTROL, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                    WriteVerbose(this, " the control button has been released");
                }
                if (Shift){
//                    PostMessage1(handle, WM_SYSKEYUP, new IntPtr(VK_SHIFT), lParamKeyUp);
//                    WriteVerbose(this, "WM_SYSKEYUP VK_SHIFT");
                    // release the shift key
                    // keybd_event((byte)VK_LSHIFT, 0x45, KEYEVENTF_KEYUP, 0);
                    keybd_event((byte)VK_LSHIFT, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                    WriteVerbose(this, " the shift button has been released");
                }
            }
//            bool res1 = SendMessage1(handle, uDown, wParam, lParam);
//            bool res2 = SendMessage1(handle, uUp, IntPtr.Zero, lParam);

            WriteVerbose(cmdlet,
                         "PostMessage " + uDown.ToString() + 
                         //"SendMessage " + uDown.ToString() + 
                         " result = " + res1.ToString());
            WriteVerbose(cmdlet, 
                         "PostMessage " + uUp.ToString() + 
                         //"SendMessage " + uUp.ToString() + 
                         " result = " + res2.ToString());
            // if (!res1 && !res2){
            if (res1 && res2){
                result = true;
            } else {
                result = false;
            }
            return result;
        }
        
        internal bool CheckControl(HasControlInputCmdletBase cmdlet)
        {
            bool result = false;
            // string cmdletName = cmdlet.GetType().Name.Replace("UIA", "-UIA");
            string cmdletName = CmdletName(cmdlet);
            
            if (cmdlet.InputObject==null)
            {
                WriteVerbose(cmdletName + ": Control is null");
                if (this.PassThru){
                    WriteObject(this, null);
                } else {
                    result = false;
                    WriteObject(this, result);
                }
                return false;
            }
            System.Windows.Automation.AutomationElement _control = null;
            try{
                _control = 
                    (System.Windows.Automation.AutomationElement)(cmdlet.InputObject);
                WriteVerbose(cmdlet,
                             "the given control is of the " + 
                             cmdlet.InputObject.Current.ControlType.ProgrammaticName + 
                             " type");
                result = true;
                // WriteObject(result); 
                // there's no need to output the True value
                // since the output will be what we want 
                // (some part of AutomationElement, as an example)
            } catch (Exception eControlTypeException) {
                WriteDebug(cmdletName + ": Control is not an AutomationElement");
                WriteDebug(cmdletName + ": " + eControlTypeException.Message);
                if (this.PassThru){
                    // result = null;
                    WriteObject(this, null);
                } else {
                    result = false;
                    WriteObject(this, result);
                }
                // result = false;
                // WriteObject(result);
                // return result;
                return false;
            }
            return result;
        }
    

        
    }
    

    
}