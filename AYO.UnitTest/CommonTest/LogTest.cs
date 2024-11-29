using AYO.Link.Log4Net;
using AYO.Link.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace AYO.UnitTest
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void LogInfo()
        {
            Log.Info("LogInfo");
            Log.InfoFormat("LogInfo format", "22");
        }

        [TestMethod]
        public void LogWarn()
        {
            Log.Warn("LogWarn");
            Log.WarnFormat("LogWarn format", "22");
        }

        [TestMethod]
        public void LogDebug()
        {
            Log.Debug("LogDebug");
            Log.DebugFormat("LogDebug format", "22");
        }

        [TestMethod]
        public void LogError()
        {
            Log.Error("LogError");
        }

        [TestMethod]
        public void LogFatal()
        {
            Log.Fatal("LogFatal");
        }

        [TestMethod]
        public void HexToTextTest()
        {
            var hexStr = "03303132C4E3BAC302";
            var utfText = hexStr.HexToText(encoding: Encoding.GetEncoding("GB2312"));
        }
    }
}