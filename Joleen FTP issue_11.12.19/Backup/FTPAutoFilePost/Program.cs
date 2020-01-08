using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace FTPAutoFilePost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    int planID = Convert.ToInt32(args[0]);

                    string dir = string.Format(AppHelper.FilesDirectory, AppHelper.PlanAbbrev(planID));
                    if (!Directory.Exists(dir))
                    {
                        throw (new Exception("The directory '" + dir + "' does not exist."));
                    }

                    List<string> files = new List<string>();
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.CredData));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.DEA_Expirables));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.INS_Expirables));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.Languages));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.LIC_Expirables));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.Reports));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.SDC_Expirables));
                    files.Add(CreateTxtFile(planID, AppHelper.TextFile.MultiSpecialty));
                }
                else
                {
                    throw (new Exception("No Plan ID Supplied."));
                }
            }
            catch (Exception E)
            {
                AppHelper.LogError(E);
            }
        }

        private static string CreateTxtFile(int PlanID, AppHelper.TextFile TxtFile)
        {
            string rtnStr = "";

            string conText = "";
            using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
            {
                List<SqlParameter> parameters;
                DataTable dt = new DataTable();
                string fileName = DateTime.Now.ToString("MMddyyyy") + "_{0}.txt";

                #region Text Files
                switch (TxtFile)
                {
                    case AppHelper.TextFile.CredData:
                        fileName = string.Format(fileName, "CredData");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@InvDate", DateTime.Now.ToShortDateString()));
                        parameters.Add(new SqlParameter("@PlanID", PlanID));

                        dt = RunSQL("rpt_Reports_txt", parameters, _Global, CommandType.StoredProcedure).Clone();

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@PlanID1", PlanID));
                        foreach (DataRow dr in RunSQL("web2_ProvsInCycle", parameters, _Global, CommandType.StoredProcedure).Rows)
                        {
                            DataRow newRow = dt.NewRow();

                            PlanProvData(PlanID, dr["ProvID"], ref newRow);
                            PlanPracData(PlanID, dr["ProvID"], ref newRow);
                            ProvData(dr["ProvID"], ref newRow);
                            EducationData(dr["ProvID"], ref newRow);
                            HospitalData(PlanID, dr["ProvID"], ref newRow);
                            IDNumbersData(dr["ProvID"], 3, ref newRow);
                            IDNumbersData(dr["ProvID"], 5, ref newRow);
                            IDNumbersData(dr["ProvID"], 9, ref newRow);
                            IDNumbersData(dr["ProvID"], 10, ref newRow);
                            IDNumbersData(dr["ProvID"], 11, ref newRow);
                            IDNumbersData(dr["ProvID"], 12, ref newRow);
                            InsuranceData(dr["ProvID"], ref newRow);
                            
                            dt.Rows.Add(newRow);
                        }

                        break;
                    case AppHelper.TextFile.DEA_Expirables:
                        fileName = string.Format(fileName, "DEAExpirables");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@Type1", "all"));
                        parameters.Add(new SqlParameter("@PlanID1", PlanID));
                        parameters.Add(new SqlParameter("@ProvID1", DBNull.Value));
                        parameters.Add(new SqlParameter("@Advance1", false));

                        dt = RunSQL("web2_DEAExp", parameters, _Global, CommandType.StoredProcedure);
                        break;
                    case AppHelper.TextFile.INS_Expirables:
                        fileName = string.Format(fileName, "INSExpirables");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@Type1", "all"));
                        parameters.Add(new SqlParameter("@PlanID1", PlanID));
                        parameters.Add(new SqlParameter("@ProvID1", DBNull.Value));
                        parameters.Add(new SqlParameter("@Advance1", false));

                        dt = RunSQL("web2_INSExp", parameters, _Global, CommandType.StoredProcedure);
                        break;
                    case AppHelper.TextFile.Languages:
                        fileName = string.Format(fileName, "Languages");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@PlanID", PlanID));

                        string sqlTxt = "SELECT [KeyC].[PlanID], [ProvLanguages].[ProvID], [KeyC].[PlanProvID], [LKUP_Languages].[Language] "
                                        + "FROM [KeyC] INNER JOIN [ProvLanguages] "
                                        + "     ON [KeyC].[ProvID]=[ProvLanguages].[ProvID] INNER JOIN [LKUP_Languages] "
                                        + "		    ON [ProvLanguages].[LanguageID]=[LKUP_Languages].[LanguageID] "
                                        + "WHERE [KeyC].[PlanID]=@PlanID "
                                        + "ORDER BY [KeyC].[ProvID], [LKUP_Languages].[Language]";
                        dt = RunSQL(sqlTxt, parameters, _Global, CommandType.Text);
                        break;
                    case AppHelper.TextFile.LIC_Expirables:
                        fileName = string.Format(fileName, "LICExpirables");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@Type1", "all"));
                        parameters.Add(new SqlParameter("@PlanID1", PlanID));
                        parameters.Add(new SqlParameter("@ProvID1", DBNull.Value));
                        parameters.Add(new SqlParameter("@Advance1", false));

                        dt = RunSQL("web2_LicExp", parameters, _Global, CommandType.StoredProcedure);
                        break;
                    case AppHelper.TextFile.SDC_Expirables:
                        fileName = string.Format(fileName, "SDCExpirables");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@Type1", "all"));
                        parameters.Add(new SqlParameter("@PlanID1", PlanID));
                        parameters.Add(new SqlParameter("@ProvID1", DBNull.Value));
                        parameters.Add(new SqlParameter("@Advance1", false));

                        dt = RunSQL("web2_SDCExp", parameters, _Global, CommandType.StoredProcedure);
                        break;
                    case AppHelper.TextFile.Reports:
                        fileName = string.Format(fileName, "Reports");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        rtnStr = GetSFRTextFile(fileName, "Reports.txt", PlanID, conText);
                        break;
                    case AppHelper.TextFile.MultiSpecialty:
                        fileName = string.Format(fileName, "MultiSpc");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        rtnStr = GetSFRTextFile(fileName, "MultiSpc.txt", PlanID, conText);
                        break;
                }
                #endregion

                if ((TxtFile != AppHelper.TextFile.Reports) && (TxtFile != AppHelper.TextFile.MultiSpecialty))
                {
                    rtnStr = GetTextFile(dt, PlanID, fileName, conText);
                }
            }
            Console.SetCursorPosition(conText.Length, Console.CursorTop);
            Console.WriteLine("Done!         ");

            return rtnStr;
        }
        private static DataTable RunSQL(string SqlText, List<SqlParameter> Parameters, SqlConnection Con, CommandType CmdType)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cm = new SqlCommand(SqlText, Con);
                cm.CommandType = CmdType;
                cm.CommandTimeout = 240;
                foreach (SqlParameter param in Parameters)
                {
                    cm.Parameters.Add(param);
                }
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
            }
            catch (Exception E)
            {
                AppHelper.LogError(E);
            }

            return dt;
        }
        private static void PlanProvData(object PlanID, object ProvID, ref DataRow NewRow)
        {
            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT [Plan].[Name], [KeyC].* FROM [Plan] INNER JOIN [KeyC] ON [Plan].[PlanID]=[KeyC].[PlanID] WHERE [KeyC].[PlanID]=? AND [KeyC].[ProvID]=?", _Global);
                cm.Parameters.AddWithValue("@PlanID", PlanID);
                cm.Parameters.AddWithValue("@ProvID", ProvID);

                cm.Connection.Open();
                using (OleDbDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        NewRow["PlanName"] = dr["Name"];
                        NewRow["PlanID"] = dr["PlanID"];
                        NewRow["ProvID"] = dr["ProvID"];
                        NewRow["PlanProvID"] = dr["PlanProvID"];
                        NewRow["NxtRptType"] = dr["NxtRptType"];
                        NewRow["NxtRptDate"] = dr["NxtRptDate"];
                        NewRow["LastRptType"] = dr["LastRptType"];
                        NewRow["LastRptDate"] = dr["LastRptDate"];
                        NewRow["NeedCredent"] = dr["NeedCredent"];
                        NewRow["Rcvrd"] = dr["Rcvrd"];
                        NewRow["NPDBDate"] = dr["NPDBDate"];
                        NewRow["NPDBVerifBy"] = dr["NPDBVerifBy"];
                        NewRow["NPDBVerifDate"] = dr["NPDBVerifDate"];
                        NewRow["NPDBDerog"] = dr["NPDB"];
                    }
                }
            }
        }
        private static void PlanPracData(object PlanID, object ProvID, ref DataRow NewRow)
        {
            foreach (DataRow dr in GetPlanPracData(PlanID, ProvID).Rows)
            {
                NewRow["PracID"] = dr["PracID"];
                NewRow["PlanPracID"] = dr["PlanPracID"];
                NewRow["PracName"] = (dr["PlanPracName"] == DBNull.Value) ? dr["Name"] : dr["PlanPracName"];
                NewRow["Street"] = dr["Street"];
                NewRow["City"] = dr["City"];
                NewRow["State"] = dr["State"];
                NewRow["Zip"] = dr["Zip"];
                NewRow["Phone"] = dr["Phone"];
                NewRow["Fax"] = dr["Fax"];
            }
        }
        private static DataTable GetPlanPracData(object PlanID, object ProvID)
        {
            DataTable dt = new DataTable();

            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT TOP 1 [Prac].*, [KeyP].[PlanPracID], [KeyP].[PlanPracName] "
                                                    + "FROM [KeyP] (nolock) INNER JOIN [Prac] (nolock) "
                                                    + "  ON [KeyP].[PracID]=[Prac].[PracID] "
                                                    + "WHERE ([KeyP].[Primary]=1 OR [KeyP].[CredOffice]=1) AND "
                                                    + "     [KeyP].[PlanID]=? AND [KeyP].[ProvID]=? "
                                                    + "ORDER BY [KeyP].[Primary], [KeyP].[CredOffice], [KeyP].[StartDate] DESC", _Global);
                cm.Parameters.AddWithValue("@PlanID", PlanID);
                cm.Parameters.AddWithValue("@ProvID", ProvID);

                OleDbDataAdapter da = new OleDbDataAdapter(cm);
                da.Fill(dt);
            }

            if (dt.Rows.Count <= 0)
            {
                using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
                {
                    OleDbCommand cm = new OleDbCommand("SELECT TOP 1 [Prac].*, [KeyP].[PlanPracID], [KeyP].[PlanPracName] "
                                                        + "FROM [KeyP] (nolock) INNER JOIN [Prac] (nolock) "
                                                        + "  ON [KeyP].[PracID]=[Prac].[PracID] "
                                                        + "WHERE [KeyP].[PlanID]=? AND [KeyP].[ProvID]=? AND "
                                                        + "     [KeyP].[Billing]=0 AND (([KeyP].[Home]=0) OR ([KeyP].[Home]=1 AND [KeyP].[Practice]=1)) "
                                                        + "ORDER BY [Mailing] DESC, [Billing] DESC, [Home] DESC, [StartDate] DESC", _Global);
                    cm.Parameters.AddWithValue("@PlanID", PlanID);
                    cm.Parameters.AddWithValue("@ProvID", ProvID);

                    OleDbDataAdapter da = new OleDbDataAdapter(cm);
                    da.Fill(dt);
                }
            }

            return dt;
        }
        private static void ProvData(object ProvID, ref DataRow NewRow)
        {
            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT [Prov].*, [Training].[Name] as 'TrnName' FROM [Prov] LEFT JOIN [HosTrn] [Training] ON [Prov].[TrnID]=[Training].[HosTrnID] WHERE [ProvID]=?", _Global);
                cm.Parameters.AddWithValue("@ProvID", ProvID);

                cm.Connection.Open();
                using (OleDbDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        NewRow["Lname"] = dr["Lname"];
                        NewRow["Fname"] = dr["Fname"];
                        NewRow["Mname"] = dr["Mname"];
                        NewRow["Suffix"] = dr["Suffix"];
                        NewRow["Title"] = dr["Title"];
                        NewRow["Specialty"] = dr["Specialty"];
                        NewRow["LicState"] = dr["LicState"];
                        NewRow["LicNum"] = dr["LicNum"];
                        NewRow["LicExp"] = dr["LicExp"];
                        NewRow["LicStatus"] = dr["LicStatus"];
                        NewRow["LicVerifBy"] = dr["LicVerifBy"];
                        NewRow["LicVerifMthd"] = dr["LicVerifMthd"];
                        NewRow["LicVerifDate"] = dr["LicVerifDate"];
                        NewRow["TrnName"] = dr["TrnName"];
                        NewRow["TrnYr"] = dr["TrnYr"];
                        NewRow["TrnVerifBy"] = dr["TrnVerifBy"];
                        NewRow["TrnVerifMthd"] = dr["TrnVerifMthd"];
                        NewRow["TrnVerifDate"] = dr["TrnVerifDate"];
                        NewRow["BoardStatus"] = dr["BoardStatus"];
                        NewRow["BoardDate"] = dr["BoardDate"];
                        NewRow["BoardExp"] = dr["BoardExp"];
                        NewRow["BoardLife"] = dr["BoardLife"];
                        NewRow["BoardVerifBy"] = dr["BoardVerifBy"];
                        NewRow["BoardVerifMthd"] = dr["BoardVerifMthd"];
                        NewRow["BoardVerifDate"] = dr["BoardVerifDate"];
                        NewRow["WrkVerifBy"] = dr["WrkVerifBy"];
                        NewRow["WrkVerifDate"] = dr["WrkVerifDate"];
                        NewRow["AttestDate"] = dr["AttestDate"];
                        NewRow["AttestVerifBy"] = dr["AttestVerifBy"];
                        NewRow["AttestVerifDate"] = dr["AttestVerifDate"];
                        NewRow["AttestDerog"] = dr["AttestDerog"];
                        NewRow["SALVerifBy"] = dr["SALVerifBy"];
                        NewRow["SALVerifMthd"] = dr["SALVerifMthd"];
                        NewRow["SALVerifDate"] = dr["SALVerifDate"];
                        NewRow["SALDerog"] = dr["SALDerog"];
                        NewRow["HCFADerog"] = dr["HCFADerog"];
                        NewRow["DOB"] = dr["DOB"];
                        NewRow["SSN"] = dr["SSN"];
                        NewRow["Gender"] = dr["Gender"];
                        NewRow["CMSOptOut"] = dr["CMSOptOut"];
                        NewRow["CMSOptOutVerifBy"] = dr["CMSOptOutVerifBy"];
                        NewRow["CMSOptOutVerifDate"] = dr["CMSOptOutVerifDate"];
                        NewRow["CMSOptOutVerifMthd"] = dr["CMSOptOutVerifMthd"];
                    }
                }
            }
        }
        private static void EducationData(object ProvID, ref DataRow NewRow)
        {
            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT TOP 1 [Education].*, [School].[Name] as 'School' FROM [Education] INNER JOIN [School] ON [Education].[SchoolID]=[School].[SchoolID] WHERE [ProvID]=? ORDER BY [StartDate] DESC", _Global);
                cm.Parameters.AddWithValue("@ProvID", ProvID);

                cm.Connection.Open();
                using (OleDbDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        NewRow["School"] = dr["School"];
                        NewRow["GradYr"] = dr["GradYr"];
                        NewRow["SchoolVerifBy"] = dr["VerifBy"];
                        NewRow["SchoolVerifMthd"] = dr["VerifMthd"];
                        NewRow["SchoolVerifDate"] = dr["VerifDate"];
                    }
                }
            }
        }
        private static void HospitalData(object PlanID, object ProvID, ref DataRow NewRow)
        {
            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT TOP 1 [HosPrivileges].*, [HosTrn].[Name] as 'HosName' "
                                                    + "FROM [KeyHos] INNER JOIN [HosPrivileges] "
                                                    + "     ON [KeyHos].[ProvID]=[HosPrivileges].[ProvID] AND "
                                                    + "     [KeyHos].[HosID]=[HosPrivileges].[HosID] INNER JOIN [HosTrn] "
                                                    + "     ON [KeyHos].[HosID]=[HosTrn].[HosTrnID] "
                                                    + "WHERE [KeyHos].[PlanID]=? AND [KeyHos].[ProvID]=? "
                                                    + "ORDER BY [StartDate] DESC", _Global);
                cm.Parameters.AddWithValue("@PlanD", PlanID);
                cm.Parameters.AddWithValue("@ProvID", ProvID);

                cm.Connection.Open();
                using (OleDbDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        NewRow["HosName"] = dr["HosName"];
                        NewRow["HosVerifBy"] = dr["VerifBy"];
                        //NewRow["HosVerifMthd"] = dr["VerifMthd"];
                    }
                }
            }
        }
        private static void IDNumbersData(object ProvID, int TypeID, ref DataRow NewRow)
        {
            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT TOP 1 [IDNumbers].* FROM [IDNumbers] WHERE [ProvID]=? AND [TypeID]=? ORDER BY [Primary] DESC", _Global);
                cm.Parameters.AddWithValue("@ProvID", ProvID);
                cm.Parameters.AddWithValue("@TypeID", TypeID);

                cm.Connection.Open();
                using (OleDbDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        switch (TypeID)
                        {
                            case 3:
                                NewRow["Medicare"] = dr["Number"];
                                break;
                            case 5:
                                NewRow["NPI"] = dr["Number"];
                                break;
                            case 9:
                                NewRow["DEANum"] = dr["Number"];
                                NewRow["DEAExp"] = dr["ExpDate"];
                                NewRow["DEAVerifBy"] = dr["VerifBy"];
                                NewRow["DEAVerifMthd"] = dr["VerifMthd"];
                                NewRow["DEAVerifDate"] = dr["VerifDate"];
                                NewRow["Drug Schedule"] = dr["Drug Schedule"];
                                break;
                            case 10:
                                NewRow["SDCNum"] = dr["Number"];
                                NewRow["SDCExp"] = dr["ExpDate"];
                                NewRow["SDCVerifBy"] = dr["VerifBy"];
                                NewRow["SDCVerifMthd"] = dr["VerifMthd"];
                                NewRow["SDCVerifDate"] = dr["VerifDate"];
                                break;
                            case 11:
                                NewRow["TPANum"] = dr["Number"];
                                NewRow["TPAExp"] = dr["ExpDate"];
                                NewRow["TPAVerifBy"] = dr["VerifBy"];
                                NewRow["TPAVerifMthd"] = dr["VerifMthd"];
                                NewRow["TPAVerifDate"] = dr["VerifDate"];
                                break;
                            case 12:
                                NewRow["DPANum"] = dr["Number"];
                                NewRow["DPAExp"] = dr["ExpDate"];
                                NewRow["DPAVerifBy"] = dr["VerifBy"];
                                NewRow["DPAVerifMthd"] = dr["VerifMthd"];
                                NewRow["DPAVerifDate"] = dr["VerifDate"];
                                break;
                        }
                    }
                }
            }
        }
        private static void InsuranceData(object ProvID, ref DataRow NewRow)
        {
            using (OleDbConnection _Global = new OleDbConnection(AppHelper.GlobalOdbConStr))
            {
                OleDbCommand cm = new OleDbCommand("SELECT TOP 1 [Insurance].*, [InsCo].[InsCoName] FROM [Insurance] INNER JOIN [InsCo] ON [Insurance].[InsCoID]=[InsCo].[InsCoID] WHERE [Insurance].[ProvID]=? ORDER BY [EffectiveDate] DESC", _Global);
                cm.Parameters.AddWithValue("@ProvID", ProvID);

                cm.Connection.Open();
                using (OleDbDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        NewRow["InsCoName"] = dr["InsCoName"];
                        NewRow["InsExp"] = dr["ExpDate"];
                        NewRow["InsPolicyNum"] = dr["PolicyNum"];
                        NewRow["InsEachClaim"] = dr["EachClaim"];
                        NewRow["InsAggrClaim"] = dr["AggrClaim"];
                        NewRow["InsVerifBy"] = dr["VerifBy"];
                        NewRow["InsVerifDate"] = dr["VerifDate"];
                    }
                }
            }
        }
        /*private static string GetReportsTxt(string FileName, int PlanID, string ConText)
        {
            string rtnStr = "";

            try
            {
                DataTable dt = GetInvDates(PlanID);
                string abbrev = AppHelper.PlanAbbrev(PlanID);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (StreamWriter txtFile = new StreamWriter(ms))
                    {
                        List<string> linesLst = new List<string>();
                        int i = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            DateTime invDate = Convert.ToDateTime(dr["InvDate"]);
                            string month = invDate.ToString("MM");
                            string day = invDate.ToString("dd");
                            string year = invDate.ToString("yyyy");
                            string root = (invDate.Year == DateTime.Now.Year ? "Current" : "Archive " + year);

                            string rptFile = string.Format(@"\\Reports\{0}\{1}\{2}\{3}\Reports.txt", root, month, abbrev, day);
                            if (File.Exists(rptFile))
                            {
                                using (StreamReader sr = new StreamReader(rptFile))
                                {
                                    using (StringReader reader = new StringReader(sr.ReadToEnd()))
                                    {
                                        string line = null;
                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            if ((line != "No Data Files") && (!linesLst.Contains(line)))
                                            {
                                                txtFile.WriteLine(line);
                                                linesLst.Add(line);
                                            }

                                            Console.SetCursorPosition(ConText.Length, Console.CursorTop);
                                            Console.Write(i++);
                                        }
                                        reader.Close();
                                    }
                                    sr.Close();
                                }
                            }
                        }
                        if (linesLst.Count <= 0) { txtFile.WriteLine("No Data Files"); }
                        txtFile.Close();
                    }
                    rtnStr = MemoryStreamToFile(ms, FileName, PlanID);
                }
            }
            catch (Exception E)
            {
                AppHelper.LogError(E);
            }

            return rtnStr;
        }*/
        private static string GetSFRTextFile(string FileName, string SFRFileName, int PlanID, string ConText)
        {
            string rtnStr = "";

            try
            {
                DataTable dt = GetInvDates(PlanID);
                string abbrev = AppHelper.PlanAbbrev(PlanID);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (StreamWriter txtFile = new StreamWriter(ms))
                    {
                        List<string> linesLst = new List<string>();
                        int i = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            DateTime invDate = Convert.ToDateTime(dr["InvDate"]);
                            string month = invDate.ToString("MM");
                            string day = invDate.ToString("dd");
                            string year = invDate.ToString("yyyy");
                            //string root = (invDate.Year == DateTime.Now.Year ? "Current" : "Archive " + year);
                            string root = year;
                            string rptFile = string.Format(@"\\OldReports\{0}\{1}\{2}\{3}\{4}", root, month, abbrev, day, SFRFileName);

                            //string rptFile = string.Format(@"\\Reports\{0}\{1}\{2}\{3}\{4}", root, month, abbrev, day, SFRFileName);
                            if (File.Exists(rptFile))
                            {
                                using (StreamReader sr = new StreamReader(rptFile))
                                {
                                    using (StringReader reader = new StringReader(sr.ReadToEnd()))
                                    {
                                        string line = null;
                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            if ((line != "No Data Files") && (!linesLst.Contains(line)))
                                            {
                                                txtFile.WriteLine(line);
                                                linesLst.Add(line);
                                            }

                                            Console.SetCursorPosition(ConText.Length, Console.CursorTop);
                                            Console.Write(i++);
                                        }
                                        reader.Close();
                                    }
                                    sr.Close();
                                }
                            }
                        }
                        if (linesLst.Count <= 0) { txtFile.WriteLine("No Data Files"); }
                        txtFile.Close();
                    }
                    rtnStr = MemoryStreamToFile(ms, FileName, PlanID);
                }
            }
            catch (Exception E)
            {
                AppHelper.LogError(E);
            }

            return rtnStr;
        }
        private static DataTable GetInvDates(int PlanID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("InvDate", Type.GetType("System.DateTime"));

            foreach (DataRow dr in GetReports(PlanID).Rows)
            {
                if (dt.Select(string.Format("[InvDate]='{0}'", dr["InvDate"])).Length == 0)
                {
                    DataRow row = dt.NewRow();
                    row["InvDate"] = dr["InvDate"];
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
        private static DataTable GetReports(int PlanID)
        {
            DataTable dt = new DataTable();
            DateTime to = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime from = to;

            switch (to.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                    from = to.AddDays(-4);
                    break;
                default:
                    from = to.AddDays(-2);
                    break;
            }

            using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
            {
                SqlCommand cm = new SqlCommand("web2_ReportsSearch", _Global);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@Type", "A");
                cm.Parameters.AddWithValue("@From", from);
                cm.Parameters.AddWithValue("@To", to);
                cm.Parameters.AddWithValue("@PlanID", PlanID);

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
            }

            return dt;
        }
        private static string GetTextFile(DataTable dt, int PlanID, string FileName, string ConText)
        {
            string rtnStr = "";

            if ((PlanID != 999) && (PlanID != 10206))
            {
                if (dt.Columns.IndexOf("SiteVisit") > -1) { dt.Columns.Remove("SiteVisit"); }
                if (dt.Columns.IndexOf("ChartAudit") > -1) { dt.Columns.Remove("ChartAudit"); }
            }
            if (dt.Columns.IndexOf("Practice") > -1) { dt.Columns.Remove("Practice"); }
            if (dt.Columns.IndexOf("Images") > -1) { dt.Columns.Remove("Images"); }
            if (dt.Columns.IndexOf("Status") > -1) { dt.Columns.Remove("Status"); }
            if (dt.Columns.IndexOf("OI") > -1)
            {
                dt.Columns.Remove("OI");
                if (!AppHelper.IsContCred(PlanID))
                {
                    if (dt.Columns.IndexOf("LicExp") > -1) { dt.Columns.Remove("LicExp"); }
                    if (dt.Columns.IndexOf("InsExp") > -1) { dt.Columns.Remove("InsExp"); }
                    if (dt.Columns.IndexOf("DEAExp") > -1) { dt.Columns.Remove("DEAExp"); }
                    if (dt.Columns.IndexOf("SDCExp") > -1) { dt.Columns.Remove("SDCExp"); }
                }
            }
            if (dt.Columns.IndexOf("Delink") > -1) { dt.Columns.Remove("Delink"); }
            if (dt.Columns.IndexOf("Fax") > -1) { dt.Columns.Remove("Fax"); }
            if (dt.Columns.IndexOf("Com") > -1) { dt.Columns.Remove("Com"); }
            if (dt.Columns.IndexOf("CPR") > -1) { dt.Columns.Remove("CPR"); }

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter txtWriter = new StreamWriter(ms))
                {

                    if (dt.Rows.Count != 0)
                    {
                        int numColumns = dt.Columns.Count;
                        string line = "";
                        foreach (DataColumn column in dt.Columns)
                        {
                            line = line + "\"" + column.ColumnName + "\",";
                        }
                        txtWriter.WriteLine(line);

                        int i = 0;
                        int total = dt.Rows.Count;
                        foreach (DataRow row in dt.Rows)
                        {
                            line = "";
                            foreach (DataColumn column in dt.Columns)
                            {
                                string val = row[column].ToString().Trim();
                                if (val == "")
                                {
                                    line = line + ",";
                                }
                                else
                                {
                                    line = line + "\"" + AppHelper.FormatDate(val) + "\",";
                                }
                            }
                            txtWriter.WriteLine(line);

                            Console.SetCursorPosition(ConText.Length, Console.CursorTop);
                            Console.Write(i++ + " of " + total);
                        }
                    }
                    else
                    {
                        txtWriter.WriteLine("No Data Files");
                    }
                    txtWriter.Close();
                }

                rtnStr = MemoryStreamToFile(ms, FileName, PlanID);
            }

            return rtnStr;
        }
        private static string MemoryStreamToFile(MemoryStream MS, string FileName, int PlanID)
        {
            string file = string.Format(AppHelper.FilesDirectory, AppHelper.PlanAbbrev(PlanID)) + FileName;

            using (FileStream fs = new FileStream(@file, FileMode.Create))
            {
                byte[] data = MS.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Close();
            }

            return file;
        }
    }
}
