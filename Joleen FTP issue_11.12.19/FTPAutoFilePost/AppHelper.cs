using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace FTPAutoFilePost
{
    class AppHelper
    {
        public enum TextFile { CredData, Reports, DEA_Expirables, INS_Expirables, Languages, LIC_Expirables, SDC_Expirables, MultiSpecialty };

        public static string FilesDirectory = ConfigurationManager.AppSettings["FilesDirectory"];

        public static string FilesDirectoryEDI = ConfigurationManager.AppSettings["FilesDirectoryEDI"];

        public static string FilesDirectoryOMV = ConfigurationManager.AppSettings["FilesDirectoryOMV"];
        

        public static string GlobalConStr = ConfigurationManager.ConnectionStrings["SQL_Global"].ConnectionString;
        public static string GlobalOdbConStr = ConfigurationManager.ConnectionStrings["OleDB_Global"].ConnectionString;

        public static void LogError(Exception E)
        {
            // Code that runs when an unhandled error occurs
            try
            {
                using (TextWriter errorFile = File.AppendText(Directory.GetCurrentDirectory() + "\\Error_" + DateTime.Now.ToString("MMddyyyy") + ".log"))
                {
                    errorFile.WriteLine("----------------------------------------------------------");
                    errorFile.WriteLine("Date: " + DateTime.Now.ToString());
                    errorFile.WriteLine("Source: " + E.Source);
                    errorFile.WriteLine("Message: " + E.Message);
                    errorFile.WriteLine("Stack Trace: " + E.StackTrace);
                    if (E.InnerException != null)
                    {
                        errorFile.WriteLine("*********** Inner Exception ***********");
                        errorFile.WriteLine("Source: " + E.InnerException.Source);
                        errorFile.WriteLine("Message: " + E.InnerException.Message);
                        errorFile.WriteLine("Stack Trace: " + E.InnerException.StackTrace);
                    }
                    errorFile.WriteLine();
                    errorFile.Close();
                }
                //TODO: Uncomment SendErrorMail()
                SendErrorMail(E, "");
            }
            catch (Exception Ex)
            {
                using (TextWriter otherFile = File.AppendText(Directory.GetCurrentDirectory() + "\\Other_" + DateTime.Now.ToString("MMddyyyy") + ".log"))
                {
                    otherFile.WriteLine("----------------------------------------------------------");
                    otherFile.WriteLine("Date: " + DateTime.Now.ToString());
                    otherFile.WriteLine("Source: " + Ex.Source);
                    otherFile.WriteLine("Message: " + Ex.Message);
                    otherFile.WriteLine("Stack Trace: " + Ex.StackTrace);
                    if (Ex.InnerException != null)
                    {
                        otherFile.WriteLine("*********** Inner Exception ***********");
                        otherFile.WriteLine("Source: " + Ex.InnerException.Source);
                        otherFile.WriteLine("Message: " + Ex.InnerException.Message);
                        otherFile.WriteLine("Stack Trace: " + Ex.InnerException.StackTrace);
                    }
                    otherFile.WriteLine("----------------------------------------------------------");
                    otherFile.WriteLine();
                    otherFile.WriteLine("----------------------------------------------------------");
                    otherFile.WriteLine("Date: " + DateTime.Now.ToString());
                    otherFile.WriteLine("Source: " + E.Source);
                    otherFile.WriteLine("Message: " + E.Message);
                    otherFile.WriteLine("Stack Trace: " + E.StackTrace);
                    if (E.InnerException != null)
                    {
                        otherFile.WriteLine("*********** Inner Exception ***********");
                        otherFile.WriteLine("Source: " + E.InnerException.Source);
                        otherFile.WriteLine("Message: " + E.InnerException.Message);
                        otherFile.WriteLine("Stack Trace: " + E.InnerException.StackTrace);
                    }
                    otherFile.WriteLine();
                    otherFile.Close();
                }
            }
        }
        public static void SendErrorMail(Exception E, string Notes)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("Admin@VerifPoint.com");
            //msg.To.Add(new MailAddress("Admin@VerifPoint.com"));
            msg.To.Add(new MailAddress("zahid.hhussain@gmail.com"));
         //   msg.CC.Add(new MailAddress("AParacha@verifpoint.com"));
            msg.Subject = "Error in FTPAutoFilePost.exe";

            /* * * * * * * * Start Format Messagge Body * * * * * * * */
            string body = "An error occured in FTPAutoFilePost.exe\n"
                + "---------------------------------------------------------------------------------\n"
                + DateTime.Now.ToString() + "\n"
                + "Source: " + E.Source + "\n"
                + "Notes: " + Notes + "\n"
                + "Message: " + E.Message + "\n"
                + "-----------------------------------StackTrace------------------------------------\n"
                + E.StackTrace + "\n";

            if (E.InnerException != null)
            {
                body += "*********** Inner Exception ***********\n"
                    + "Source: " + E.InnerException.Source + "\n"
                    + "Message: " + E.InnerException.Message + "\n"
                    + "Stack Trace: " + E.InnerException.StackTrace + "\n";
            }
            body += "\n---------------------------------------------------------------------------------\n"
                + "Thank you,\n The FTP Automatic File Posting Program";
            /* * * * * * * * End Format Messagge Body * * * * * * * */

            msg.Body = body;
            SmtpClient mailClient = new SmtpClient();
            mailClient.Send(msg);
        }

        public static void SendsuccessfulMail(string Notes)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("Admin@VerifPoint.com");
            //msg.To.Add(new MailAddress("Admin@VerifPoint.com"));
            msg.To.Add(new MailAddress("zahid.hhussain@gmail.com"));
            msg.CC.Add(new MailAddress("AParacha@verifpoint.com"));
            msg.Subject = "Successfull Post files in FTP";

            /* * * * * * * * Start Format Messagge Body * * * * * * * */
            string body = "\n"
                + "---------------------------------------------------------------------------------\n"
                + DateTime.Now.ToString() + "\n"




                + "Message:     Dear Client, All file were generated successfully. Thanks " + "\n"
                + "-----------------------------------StackTrace------------------------------------\n";
                

            body += "\n---------------------------------------------------------------------------------\n"
                + "Thank you,\n The FTP Automatic File Posting Program";
            /* * * * * * * * End Format Messagge Body * * * * * * * */

            msg.Body = body;
            SmtpClient mailClient = new SmtpClient();
            mailClient.Send(msg);
        }
        public static object Chk_DBNull(string Value)
        {
            object returnObj;
            if (string.IsNullOrEmpty(Value)) { returnObj = DBNull.Value; }
            else { returnObj = Value; }
            return returnObj;
        }
        public static string FormatDate(object DateStr)
        {
            string returnStr = DateStr.ToString();

            try
            {
                returnStr = Convert.ToDateTime(DateStr).ToString("MM/dd/yyyy");
                return returnStr;
            }
            catch
            {
                return returnStr;
            }
        }
        public static bool IsDate(object Value)
        {
            try
            {
                Value = Convert.ToDateTime(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsContCred(int PlanID)
        {
            bool contCred = false;

            try
            {
                using (SqlConnection _Global = new SqlConnection(GlobalConStr))
                {
                    SqlCommand cm = new SqlCommand("SELECT CASE WHEN [NoU]=0 THEN 1 ELSE 0 END as [ContCred] FROM [Plan] WHERE [PlanID]=@PlanID", _Global);
                    cm.CommandTimeout = 120;
                    cm.Parameters.AddWithValue("@PlanID", PlanID);

                    cm.Connection.Open();
                    using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            contCred = Convert.ToBoolean(dr["ContCred"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                LogError(E);
            }

            return contCred;
        }
        public static bool IsCS(int PlanID)
        {
            bool rtnBool = false;

            try
            {
                using (SqlConnection _Global = new SqlConnection(GlobalConStr))
                {
                    SqlCommand cm = new SqlCommand("SELECT [CS-StartDate] FROM [Plan] WHERE [PlanID]=@PlanID", _Global);
                    cm.Parameters.AddWithValue("@PlanID", PlanID);

                    cm.Connection.Open();
                    using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            if (dr["CS-StartDate"] != DBNull.Value) { rtnBool = true; }
                        }
                    }
                }
            }
            catch (Exception E)
            {
                LogError(E);
            }

            return rtnBool;
        }
        public static string PlanAbbrev(int PlanID)
        {
            string rtnStr = "";

            try
            {
                using (SqlConnection _Global = new SqlConnection(GlobalConStr))
                {
                    SqlCommand cm = new SqlCommand("SELECT [Abbrev] FROM [Plan] WHERE [PlanID]=@PlanID", _Global);
                    cm.Parameters.AddWithValue("@PlanID", PlanID);

                    cm.Connection.Open();
                    using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            rtnStr = dr["Abbrev"].ToString();
                        }
                    }
                }
            }
            catch (Exception E)
            {
                LogError(E);
            }

            return rtnStr;
        }
    }
}
