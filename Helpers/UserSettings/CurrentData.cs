/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 16/12/2011
 * Time: 11:43 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Automation;

namespace UIAutomation
{
    /// <summary>
    /// Description of CurrentData.
    /// </summary>
    public static class CurrentData
    {
        static CurrentData()
        {
            Error = new System.Collections.ArrayList(Preferences.MaximumErrorCount);
            TestResults = 
                new System.Collections.Generic.List<TestResult>();
            initTestResults();
        }
        
        public static AutomationElement CurrentWindow { get; set; }
        public static System.Collections.ArrayList Error { get; set; }
        public static string LastCmdlet { get; internal set; }
        public static object LastResult { get; internal set; }
        public static System.Collections.Generic.List<TestResult> TestResults { get; internal set; }
        
        internal static Commands.RecorderForm formRecorder { get; set; }
        
        private static void initTestResults()
        {
            if (TestResults.Count<1){
                TestResults.Add(new TestResult());
            }
        }
        
        internal static void AddTestResult(string previousTestResultLabel,
                                           bool passed)
        {
            initTestResults();
            TestResults[TestResults.Count - 1].Label =
                previousTestResultLabel;
            TestResults[TestResults.Count - 1].Passed = 
                passed;
            TestResults.Add(new TestResult());
        }
        
        internal static void AddTestResultDetail(object detail)
        {
            initTestResults();
            TestResults[CurrentData.TestResults.Count - 1].Details.Add(detail);
        }        
    }
    
    public class TestResult
    {
        public TestResult()
        {
            this.Details = 
                new System.Collections.ArrayList();
        }
        
        public string Label { get; internal set; }
        public System.Collections.ArrayList Details { get; internal set; }
        public bool Passed { get; internal set; }
    }
}
