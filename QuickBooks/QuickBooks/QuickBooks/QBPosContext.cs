using System;
using System.Threading;
using System.Windows;
using log4netWrapper;
using QBPOSXMLRPLib;
using QuickBooks.Properties;

namespace QuickBooks
{
    public static class QBPosContext
    {
       static RequestProcessor rp = new RequestProcessorClass();
        private static bool isConnected = false;
        static string ticket = null;
        static string response = null;

        public static string ProcessXML(string input, string QBCompanyFile)
        {
            string res = null;
            ///bool started = false;
            try
            {

                while (isConnected == true)
                {
                    Thread.Sleep(1000);
                }

                if (ticket == null) ticket = BeginSession(QBCompanyFile);
                isConnected = true;
                res = rp.ProcessRequest(ticket, input).ToString();
                isConnected = false;
                return res;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                
                //return string.Empty;
                Logger.Log(LoggingLevel.Error, ex.Message + ":-Input-:" + input + ":-Response-:" + res);
                throw new Exception(ex.Message);
                
            }
            finally
            {
                if (ticket != null)
                {
                    rp.EndSession(ticket);
                }
                if (rp != null)
                {
                    rp.CloseConnection();
                    ticket = null;
                }
            }
        }

        private static string BeginSession(string QBCompanyFile)
        {
            rp.OpenConnection("QB2POS", "QB2POS");
            string connString = QBCompanyFile;//"Computer Name=server;Company Data=hills and valley gd;Version=11";

            return rp.BeginSession(connString);
            
        }

        //public static void Dispose()
        //{
        //    CloseSession();
        //}

        private static void CloseSession()
        {
            rp = null;
        }
    }
}