using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Org.SftpTest.Shared {
    public static class Extensions {
        [DebuggerStepThrough]
        public static bool ContainsIn(this string s, string set, bool caseSensitive = false) {
            if (s.IsBlank() || set.IsBlank())
                return false;

            string[] tokens = set.Split(Constants.CommaDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens) {
                if (caseSensitive) {
                    if (s.Contains(token.Trim()))
                        return true;
                }
                else {
                    if (s.ToLower().Contains(token.ToLower()))
                        return true;
                }
            }

            return false;
        }

        [DebuggerStepThrough]
        public static bool IsBlank(this string s) => s is null || s.Trim().Length == 0;

        [DebuggerStepThrough]
        public static bool IsNotBlank(this string s) => !s.IsBlank();

        [DebuggerStepThrough]
        public static bool In(this string value, string set, bool caseSensitive = true) {
            if (value == null || set == null)
                return false;

            value = value.Trim();
            set = set.Trim();

            if (set.Trim().Length == 0)
                return false;

            string[] tokens = set.Split(Constants.CommaDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens) {
                if (caseSensitive) {
                    if (value == token.Trim())
                        return true;
                }
                else {
                    if (value.ToLower() == token.ToLower().Trim())
                        return true;
                }
            }

            return false;
        }

        [DebuggerStepThrough]
        public static string ToReport(this Exception value) {
            StringBuilder sb = new StringBuilder();

            Exception ex = value;
            bool moreExceptions = true;
            int level = 0;

            var messageList = new List<string>();
            Exception ex2 = ex;
            string msg = ex.Message;
            if (ex2.GetType().Name == "ReflectionTypeLoadException")
                msg += " [SEE TYPE LOAD EXCEPTION DETAIL BELOW]";

            messageList.Add(msg);
            while (ex2.InnerException != null) {
                ex2 = ex2.InnerException;
                msg = ex2.Message;
                if (ex2.GetType().Name == "ReflectionTypeLoadException")
                    msg += " [SEE TYPE LOAD EXCEPTION DETAIL BELOW]";

                messageList.Add(msg);
            }

            sb.Append("Exception Message Summary:" + Environment.NewLine);
            for (int i = messageList.Count - 1; i > -1; i--) {
                sb.Append("[" + i.ToString("00") + "] " + messageList.ElementAt(i) + Environment.NewLine);
            }

            sb.Append(Environment.NewLine);

            while (moreExceptions) {
                if (ex.Message.StartsWith("!"))
                    return ex.Message.Substring(1);

                string message = ex.Message;

                if (ex.GetType().Name == "ReflectionTypeLoadException") {
                    string typeLoadMessage = "*** REFLECTION TYPE LOAD EXCEPTION ***";
                    var typeLoadException = (ReflectionTypeLoadException)ex;
                    if (typeLoadException.LoaderExceptions.Length > 0) {
                        int typeLoadIndex = 0;
                        foreach (var lx in typeLoadException.LoaderExceptions) {
                            typeLoadMessage += Environment.NewLine;
                            typeLoadMessage += "[" + typeLoadIndex.ToString() + "] " + lx.ToString();
                            typeLoadIndex++;
                        }
                    }

                    if (typeLoadMessage.IsNotBlank())
                        message = typeLoadMessage;
                }

                sb.Append("Level:" + level.ToString() + " Type=" + ex.GetType().ToString() + Environment.NewLine +
                          "Message: " + message + Environment.NewLine +
                          "StackTrace:" + ex.StackTrace + Environment.NewLine);

                if (ex.InnerException == null)
                    moreExceptions = false;
                else {
                    sb.Append(Environment.NewLine);
                    ex = ex.InnerException;
                    level++;
                }
            }

            string report = sb.ToString();
            return report;
        }
    }
}