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
                   // string[] seperators = { "," };
                   string[] AllPlanid = args[0].Split(',');
                   foreach (var value in AllPlanid)
                    {
               
                        int planID = Convert.ToInt32(value);
                         string dir="";
                         string dirDes = "";
                         if (value == "10344")
                        {
                         dir = string.Format(AppHelper.FilesDirectory, AppHelper.PlanAbbrev(planID));
                        }
                         if (value == "10606")
                         {
                             dir = string.Format(AppHelper.FilesDirectoryEDI, AppHelper.PlanAbbrev(planID));
                         }


                        if (!Directory.Exists(dir))
                        {
                            throw (new Exception("The directory '" + dir + "' does not exist."));
                        }

                        try
                        {
                            List<string> files = new List<string>();
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.CredData));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.DEA_Expirables));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.INS_Expirables));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.Languages));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.LIC_Expirables));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.Reports));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.SDC_Expirables));
                            files.Add(CreateTxtFile(planID, AppHelper.TextFile.MultiSpecialty));
                            


                            if (value == "10344")
                            {
                                 dirDes = string.Format(AppHelper.FilesDirectoryOMV, AppHelper.PlanAbbrev(planID));
                                //Copy all the files & Replaces any files with the same name
                                 foreach (string newPath in Directory.GetFiles(dir, "*.*",
                                    SearchOption.AllDirectories))
                                     File.Copy(newPath, newPath.Replace(dir, dirDes), true);

                                 AppHelper.SendsuccessfulMail("Done!!");
                            }
                            
                        
                        
                        
                        
                        
                        
                        }
                        catch (Exception E)
                        {
                            AppHelper.LogError(E);
                            //AppHelper.SendErrorMail(E,"Error");
                        }
                    }
                }
                else
                {
                    throw (new Exception("No Plan ID Supplied."));
                }
            }
            catch (Exception E)
            {
                AppHelper.LogError(E);
                AppHelper.SendErrorMail(E, "Error");
            }
        }

        private static void DeleteALLTableData()
        {
            DataTable dt = new DataTable();

            using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
            {
                SqlCommand cm = new SqlCommand("DeleteInCyleALL", _Global);
                cm.CommandTimeout = 120;
                cm.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);

            }


        }

        private static void InsertALLData(DataTable dt)
        {

            try
            {
                using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SqlCommand cm = new SqlCommand("InsertInCycleData", _Global);

                        cm.CommandTimeout = 120;
                        cm.CommandType = CommandType.StoredProcedure;

                        cm.Parameters.AddWithValue("@PlanID", dr["PlanID"]);
                        cm.Parameters.AddWithValue("@ProvID", dr["ProvID"]);
                        cm.Parameters.AddWithValue("@PracID", dr["PracID"]);
                        cm.Parameters.AddWithValue("@PlanProvID", dr["PlanProvID"]);
                        cm.Parameters.AddWithValue("@PlanPracID", dr["PlanPracID"]);
                        cm.Parameters.AddWithValue("@PlanName", dr["PlanName"]);
                        cm.Parameters.AddWithValue("@NxtRptType", dr["NxtRptType"]);
                        cm.Parameters.AddWithValue("@NxtRptDate", dr["NxtRptDate"]);
                        cm.Parameters.AddWithValue("@LastRptType", dr["LastRptType"]);
                        cm.Parameters.AddWithValue("@LastRptDate", dr["LastRptDate"]);
                        cm.Parameters.AddWithValue("@InvDate", dr["InvDate"]);
                        cm.Parameters.AddWithValue("@ProcessDate", dr["ProcessDate"]);
                        cm.Parameters.AddWithValue("@CredType", dr["CredType"]);
                        cm.Parameters.AddWithValue("@NeedCredent", dr["NeedCredent"]);
                        cm.Parameters.AddWithValue("@Rcvrd", dr["Rcvrd"]);
                        cm.Parameters.AddWithValue("@Amount", dr["Amount"]);
                        cm.Parameters.AddWithValue("@Lname", dr["Lname"]);
                        cm.Parameters.AddWithValue("@Fname", dr["Fname"]);
                        cm.Parameters.AddWithValue("@Mname", dr["Mname"]);
                        cm.Parameters.AddWithValue("@Suffix", dr["Suffix"]);
                        cm.Parameters.AddWithValue("@Title", dr["Title"]);
                        cm.Parameters.AddWithValue("@Specialty", dr["Specialty"]);
                        cm.Parameters.AddWithValue("@PracName", dr["PracName"]);
                        cm.Parameters.AddWithValue("@Street", dr["Street"]);
                        cm.Parameters.AddWithValue("@City", dr["City"]);
                        cm.Parameters.AddWithValue("@State", dr["State"]);
                        cm.Parameters.AddWithValue("@Zip", dr["Zip"]);
                        cm.Parameters.AddWithValue("@Phone", dr["Phone"]);
                        cm.Parameters.AddWithValue("@LicState", dr["LicState"]);
                        cm.Parameters.AddWithValue("@LicNum", dr["LicNum"]);
                        cm.Parameters.AddWithValue("@LicExp", dr["LicExp"]);
                        cm.Parameters.AddWithValue("@LicStatus", dr["LicStatus"]);
                        cm.Parameters.AddWithValue("@LicVerifBy", dr["LicVerifBy"]);
                        cm.Parameters.AddWithValue("@LicVerifMthd", dr["LicVerifMthd"]);
                        cm.Parameters.AddWithValue("@LicVerifDate", dr["LicVerifDate"]);
                        cm.Parameters.AddWithValue("@School", dr["School"]);
                        cm.Parameters.AddWithValue("@GradYr", dr["GradYr"]);
                        cm.Parameters.AddWithValue("@SchoolVerifBy", dr["SchoolVerifBy"]);
                        cm.Parameters.AddWithValue("@SchoolVerifMthd", dr["SchoolVerifMthd"]);
                        cm.Parameters.AddWithValue("@SchoolVerifDate", dr["SchoolVerifDate"]);
                        cm.Parameters.AddWithValue("@TrnName", dr["TrnName"]);
                        cm.Parameters.AddWithValue("@TrnYr", dr["TrnYr"]);
                        cm.Parameters.AddWithValue("@TrnVerifBy", dr["TrnVerifBy"]);
                        cm.Parameters.AddWithValue("@TrnVerifMthd", dr["TrnVerifMthd"]);
                        cm.Parameters.AddWithValue("@TrnVerifDate", dr["TrnVerifDate"]);
                        cm.Parameters.AddWithValue("@BoardStatus", dr["BoardStatus"]);
                        cm.Parameters.AddWithValue("@BoardDate", dr["BoardDate"]);
                        cm.Parameters.AddWithValue("@BoardExp", dr["BoardExp"]);
                        cm.Parameters.AddWithValue("@BoardLife", dr["BoardLife"]);
                        cm.Parameters.AddWithValue("@BoardVerifBy", dr["BoardVerifBy"]);
                        cm.Parameters.AddWithValue("@BoardVerifMthd", dr["BoardVerifMthd"]);
                        cm.Parameters.AddWithValue("@BoardVerifDate", dr["BoardVerifDate"]);
                        cm.Parameters.AddWithValue("@DEANum", dr["DEANum"]);
                        cm.Parameters.AddWithValue("@DEAExp", dr["DEAExp"]);
                        cm.Parameters.AddWithValue("@DEAVerifBy", dr["DEAVerifBy"]);
                        cm.Parameters.AddWithValue("@DEAVerifMthd", dr["DEAVerifMthd"]);
                        cm.Parameters.AddWithValue("@DEAVerifDate", dr["DEAVerifDate"]);
                        cm.Parameters.AddWithValue("@TPANum", dr["TPANum"]);
                        cm.Parameters.AddWithValue("@TPAExp", dr["TPAExp"]);
                        cm.Parameters.AddWithValue("@TPAVerifBy", dr["TPAVerifBy"]);
                        cm.Parameters.AddWithValue("@TPAVerifMthd", dr["TPAVerifMthd"]);
                        cm.Parameters.AddWithValue("@TPAVerifDate", dr["TPAVerifDate"]);
                        cm.Parameters.AddWithValue("@DPANum", dr["DPANum"]);
                        cm.Parameters.AddWithValue("@DPAExp", dr["DPAExp"]);
                        cm.Parameters.AddWithValue("@DPAVerifBy", dr["DPAVerifBy"]);
                        cm.Parameters.AddWithValue("@DPAVerifMthd", dr["DPAVerifMthd"]);
                        cm.Parameters.AddWithValue("@DPAVerifDate", dr["DPAVerifDate"]);
                        cm.Parameters.AddWithValue("@SDCNum", dr["SDCNum"]);
                        cm.Parameters.AddWithValue("@SDCExp", dr["SDCExp"]);
                        cm.Parameters.AddWithValue("@SDCVerifBy", dr["SDCVerifBy"]);
                        cm.Parameters.AddWithValue("@SDCVerifMthd", dr["SDCVerifMthd"]);
                        cm.Parameters.AddWithValue("@SDCVerifDate", dr["SDCVerifDate"]);
                        cm.Parameters.AddWithValue("@InsCoName", dr["InsCoName"]);
                        cm.Parameters.AddWithValue("@InsExp", dr["InsExp"]);
                        cm.Parameters.AddWithValue("@InsPolicyNum", dr["InsPolicyNum"]);
                        cm.Parameters.AddWithValue("@InsEachClaim", dr["InsEachClaim"]);
                        cm.Parameters.AddWithValue("@InsAggrClaim", dr["InsAggrClaim"]);
                        cm.Parameters.AddWithValue("@InsVerifBy", dr["InsVerifBy"]);
                        cm.Parameters.AddWithValue("@InsVerifDate", dr["InsVerifDate"]);
                        cm.Parameters.AddWithValue("@HosName", dr["HosName"]);
                        cm.Parameters.AddWithValue("@HosVerifBy", dr["HosVerifBy"]);
                        cm.Parameters.AddWithValue("@HosVerifDate", dr["HosVerifDate"]);
                        cm.Parameters.AddWithValue("@WrkVerifBy", dr["WrkVerifBy"]);
                        cm.Parameters.AddWithValue("@WrkVerifDate", dr["WrkVerifDate"]);
                        cm.Parameters.AddWithValue("@AttestDate", dr["AttestDate"]);
                        cm.Parameters.AddWithValue("@AttestVerifBy", dr["AttestVerifBy"]);
                        cm.Parameters.AddWithValue("@AttestVerifDate", dr["AttestVerifDate"]);
                        cm.Parameters.AddWithValue("@AttestDerog", dr["AttestDerog"]);
                        cm.Parameters.AddWithValue("@NPDBDate", dr["NPDBDate"]);
                        cm.Parameters.AddWithValue("@NPDBVerifBy", dr["NPDBVerifBy"]);
                        cm.Parameters.AddWithValue("@NPDBVerifDate", dr["NPDBVerifDate"]);
                        cm.Parameters.AddWithValue("@NPDBDerog", dr["NPDBDerog"]);
                        cm.Parameters.AddWithValue("@LastNPDBDate", dr["LastNPDBDate"]);
                        cm.Parameters.AddWithValue("@LastNPDBDerog", dr["LastNPDBDerog"]);
                        cm.Parameters.AddWithValue("@SALVerifBy", dr["SALVerifBy"]);
                        cm.Parameters.AddWithValue("@SALVerifMthd", dr["SALVerifMthd"]);
                        cm.Parameters.AddWithValue("@SALVerifDate", dr["SALVerifDate"]);
                        cm.Parameters.AddWithValue("@SALDerog", dr["SALDerog"]);
                        cm.Parameters.AddWithValue("@HCFAVerifBy", dr["HCFAVerifBy"]);
                        cm.Parameters.AddWithValue("@HCFAVerifMthd", dr["HCFAVerifMthd"]);
                        cm.Parameters.AddWithValue("@HCFAVerifDate", dr["HCFAVerifDate"]);
                        cm.Parameters.AddWithValue("@HCFADerog", dr["HCFADerog"]);
                        cm.Parameters.AddWithValue("@DOB", dr["DOB"]);
                        cm.Parameters.AddWithValue("@SSN", dr["SSN"]);
                        cm.Parameters.AddWithValue("@CycleDate", dr["CycleDate"]);
                        cm.Parameters.AddWithValue("@CycleType", dr["CycleType"]);
                        cm.Parameters.AddWithValue("@NPI", dr["NPI"]);
                        cm.Parameters.AddWithValue("@CIN", dr["CIN"]);
                        cm.Parameters.AddWithValue("@Medicare", dr["Medicare"]);
                        cm.Parameters.AddWithValue("@DrugSchedule", dr["Drug Schedule"]);
                        cm.Parameters.AddWithValue("@Gender", dr["Gender"]);
                        cm.Parameters.AddWithValue("@CMSOptOut", dr["CMSOptOut"]);
                        cm.Parameters.AddWithValue("@CMSOptOutVerifBy", dr["CMSOptOutVerifBy"]);
                        cm.Parameters.AddWithValue("@CMSOptOutVerifDate", dr["CMSOptOutVerifDate"]);
                        cm.Parameters.AddWithValue("@CMSOptOutVerifMthd", dr["CMSOptOutVerifMthd"]);
                        cm.Parameters.AddWithValue("@ReviewStatus", dr["ReviewStatus"]);
                        cm.Parameters.AddWithValue("@ReviewDate", dr["ReviewDate"]);
                        // cm.Parameters.AddWithValue("@DateApplicationRecd", dr["DateApplicationRecd"]);

                        SqlDataAdapter da = new SqlDataAdapter(cm);
                        da.Fill(dt);
                    }
                }

            }
            catch (Exception E)
            {
               // SiteHelper.LogError(E);
               // errLbl.Text = "There was an error in generating the file.  Please try again later.";
            }

            //return dt;
        }


        private static DataTable GetALLNewData(object planid)
        {
            DataTable dt = new DataTable();

            using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
            {
                SqlCommand cm = new SqlCommand("GetAllDataForAllPlan", _Global);
                cm.Parameters.Add("@PlanID", planid);
                cm.CommandTimeout = 120;
                cm.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);

            }

            return dt;
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
                        DataTable dtNew = new DataTable();
                        fileName = string.Format(fileName, "CredData");
                        conText = string.Format("Creating the {0} file...", fileName);
                        Console.Write(conText);

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@InvDate", DateTime.Now.ToShortDateString()));
                        parameters.Add(new SqlParameter("@PlanID", PlanID));

                        dtNew = RunSQL("rpt_Reports_txt", parameters, _Global, CommandType.StoredProcedure).Clone();

                        parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@PlanID1", PlanID));
                    //  
                     //   web2_ProvsInCycle
                        foreach (DataRow dr in RunSQL("web2_ProvsInCycleNPI", parameters, _Global, CommandType.StoredProcedure).Rows)
                        {
                            DataRow newRow = dtNew.NewRow();

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

                            dtNew.Rows.Add(newRow);
                        }

                      


                         //ViewState.Add("SortExp", "LName, FName, LicState, Title");
                         //ViewState.Add("SortOrder", "DESC");
                      //  DataTable dt = new DataTable();
                          DeleteALLTableData();
                          InsertALLData(dtNew);
                          dt = GetALLNewData(PlanID);
                      //    dt.DefaultView.Sort = "LName, FName, LicState, Title";
                        
                          //DataView dv = new DataView(dt);
                          //dv.Sort = "LName, FName, LicState, Title";


                    //datatable rearrange column 

                          try
                          {
                              dt.Columns["PlanID"].SetOrdinal(0);
                              dt.Columns["ProvID"].SetOrdinal(1);
                              dt.Columns["PracID"].SetOrdinal(2);
                              dt.Columns["PlanProvID"].SetOrdinal(3);
                              dt.Columns["PlanPracID"].SetOrdinal(4);
                              dt.Columns["PlanName"].SetOrdinal(5);
                              dt.Columns["InvDate"].SetOrdinal(6);
                              dt.Columns["ProcessDate"].SetOrdinal(7);
                              dt.Columns["CredType"].SetOrdinal(8);
                              dt.Columns["Amount"].SetOrdinal(9);
                              dt.Columns["Lname"].SetOrdinal(10);
                              dt.Columns["Fname"].SetOrdinal(11);
                              dt.Columns["Mname"].SetOrdinal(12);
                              dt.Columns["Suffix"].SetOrdinal(13);
                              dt.Columns["Title"].SetOrdinal(14);
                              dt.Columns["DOB"].SetOrdinal(15);
                              dt.Columns["SSN"].SetOrdinal(16);
                              dt.Columns["Gender"].SetOrdinal(17);

                              dt.Columns["Specialty"].SetOrdinal(18);
                              
                              dt.Columns["PracName"].SetOrdinal(19);
                              
                              dt.Columns["Street"].SetOrdinal(20);
                              dt.Columns["City"].SetOrdinal(21);
                              dt.Columns["State"].SetOrdinal(22);
                              dt.Columns["Zip"].SetOrdinal(23);
                              //dt.Columns["Zip4"].SetOrdinal(24);
                              dt.Columns["Phone"].SetOrdinal(24);
                             // dt.Columns["Email"].SetOrdinal(26);
                             // dt.Columns["Tin"].SetOrdinal(27);
                              dt.Columns["LicState"].SetOrdinal(25);
                              dt.Columns["LicNum"].SetOrdinal(26);
                              dt.Columns["LicExp"].SetOrdinal(27);
                              dt.Columns["LicStatus"].SetOrdinal(28);
                              dt.Columns["LicVerifBy"].SetOrdinal(29);
                              dt.Columns["LicVerifMthd"].SetOrdinal(30);
                              dt.Columns["LicVerifDate"].SetOrdinal(31);
                            //  dt.Columns["AIMVerifBy"].SetOrdinal(35);
                            //  dt.Columns["AIMVerifDate"].SetOrdinal(36);
                              dt.Columns["SALDerog"].SetOrdinal(32);
                              dt.Columns["SALVerifBy"].SetOrdinal(33);
                              dt.Columns["SALVerifMthd"].SetOrdinal(34);
                              dt.Columns["SALVerifDate"].SetOrdinal(35);
                              
                              dt.Columns["School"].SetOrdinal(36);
                             
                              dt.Columns["GradYr"].SetOrdinal(37);
                            //  dt.Columns["SchoolStartDate"].SetOrdinal(43);
                            //  dt.Columns["SchoolEndDate"].SetOrdinal(44);
                              dt.Columns["SchoolVerifBy"].SetOrdinal(38);
                              dt.Columns["SchoolVerifMthd"].SetOrdinal(39);
                              dt.Columns["SchoolVerifDate"].SetOrdinal(40);
                              dt.Columns["BoardLife"].SetOrdinal(41);
                              dt.Columns["BoardVerifBy"].SetOrdinal(42);
                              dt.Columns["BoardVerifMthd"].SetOrdinal(43);
                              dt.Columns["BoardVerifDate"].SetOrdinal(44);
                             // dt.Columns["Degree"].SetOrdinal(52);

                              //dt.Columns["InternshipID"].SetOrdinal(53);
                            //  dt.Columns["InternName"].SetOrdinal(54);
                            //  dt.Columns["InternshipYr"].SetOrdinal(45);
                             // dt.Columns["InternshipStartDate"].SetOrdinal(46);
                             // dt.Columns["InternshipEndDate"].SetOrdinal(57);
                            //  dt.Columns["InternshipVerifBy"].SetOrdinal(58);
                            //  dt.Columns["InternshipVerifMthd"].SetOrdinal(59);
                             // dt.Columns["InternshipVerifDate"].SetOrdinal(60);
                              dt.Columns["TrnName"].SetOrdinal(45);
                              dt.Columns["TrnYr"].SetOrdinal(46);
                            //  dt.Columns["TrainingStartDate"].SetOrdinal(63);
                            //  dt.Columns["TrainingEndDate"].SetOrdinal(64);
                              dt.Columns["TrnVerifBy"].SetOrdinal(47);
                              dt.Columns["TrnVerifMthd"].SetOrdinal(48);

                              dt.Columns["TrnVerifDate"].SetOrdinal(49);
                            //  dt.Columns["FellowshipID"].SetOrdinal(68);
                            // dt.Columns["FellowName"].SetOrdinal(69);
                           //   dt.Columns["FellowshipYr"].SetOrdinal(70);
                          //    dt.Columns["FellowshipStartDate"].SetOrdinal(71);
                           //   dt.Columns["FellowshipEndDate"].SetOrdinal(72);
                           //   dt.Columns["FellowshipVerifBy"].SetOrdinal(73);
                           //   dt.Columns["FellowshipVerifMthd"].SetOrdinal(74);
                          //    dt.Columns["FellowshipVerifDate"].SetOrdinal(75);
                            //  dt.Columns["BoardName"].SetOrdinal(76);
                              dt.Columns["BoardStatus"].SetOrdinal(50);
                             // dt.Columns["BoardCertNum"].SetOrdinal(78);
                              dt.Columns["BoardDate"].SetOrdinal(51);
                              dt.Columns["BoardExp"].SetOrdinal(52);
                              dt.Columns["BoardLife"].SetOrdinal(53);
                              dt.Columns["BoardVerifBy"].SetOrdinal(54);
                              dt.Columns["BoardVerifMthd"].SetOrdinal(55);
                              dt.Columns["BoardVerifDate"].SetOrdinal(56);
                              dt.Columns["DEANum"].SetOrdinal(57);
                              dt.Columns["DEAExp"].SetOrdinal(58);
                              dt.Columns["Drug Schedule"].SetOrdinal(59);
                              dt.Columns["DEAVerifBy"].SetOrdinal(60);
                              dt.Columns["DEAVerifMthd"].SetOrdinal(61);
                              dt.Columns["DEAVerifDate"].SetOrdinal(62);
                              dt.Columns["TPANum"].SetOrdinal(63);
                              dt.Columns["TPAExp"].SetOrdinal(64);

                              dt.Columns["TPAVerifBy"].SetOrdinal(65);
                              dt.Columns["TPAVerifMthd"].SetOrdinal(66);
                              dt.Columns["TPAVerifDate"].SetOrdinal(67);
                              dt.Columns["DPANum"].SetOrdinal(68);
                              dt.Columns["DPAExp"].SetOrdinal(69);
                              dt.Columns["DPAVerifMthd"].SetOrdinal(70);
                              dt.Columns["DPAVerifBy"].SetOrdinal(71);
                              dt.Columns["DPAVerifDate"].SetOrdinal(72);

                              dt.Columns["SDCNum"].SetOrdinal(73);
                              dt.Columns["SDCExp"].SetOrdinal(74);
                              dt.Columns["SDCVerifBy"].SetOrdinal(75);
                              dt.Columns["SDCVerifMthd"].SetOrdinal(76);
                              dt.Columns["SDCVerifDate"].SetOrdinal(78);
                              dt.Columns["InsCoName"].SetOrdinal(79);
                              dt.Columns["InsExp"].SetOrdinal(80);
                              dt.Columns["InsPolicyNum"].SetOrdinal(81);
                              dt.Columns["InsEachClaim"].SetOrdinal(82);

                              dt.Columns["InsAggrClaim"].SetOrdinal(83);
                              dt.Columns["InsVerifBy"].SetOrdinal(84);
                              dt.Columns["InsVerifDate"].SetOrdinal(85);
                             // dt.Columns["InsUnderLimit"].SetOrdinal(113);
                              dt.Columns["HosName"].SetOrdinal(86);

                              dt.Columns["HosVerifBy"].SetOrdinal(87);
                              dt.Columns["HosVerifDate"].SetOrdinal(88);
                              dt.Columns["WrkVerifBy"].SetOrdinal(89);
                              dt.Columns["WrkVerifDate"].SetOrdinal(90);
                              dt.Columns["AttestDerog"].SetOrdinal(91);

                              dt.Columns["AttestDate"].SetOrdinal(93);
                              dt.Columns["AttestVerifBy"].SetOrdinal(94);
                              dt.Columns["AttestVerifDate"].SetOrdinal(95);
                              dt.Columns["NPDBDerog"].SetOrdinal(96);
                              dt.Columns["NPDBDate"].SetOrdinal(97);
                              dt.Columns["NPDBVerifBy"].SetOrdinal(98);
                              dt.Columns["NPDBVerifDate"].SetOrdinal(99);
                            //  dt.Columns["HIPDBDerog"].SetOrdinal(127);
                            //  dt.Columns["HIPDBDate"].SetOrdinal(128);
                            //  dt.Columns["HIPDBVerifBy"].SetOrdinal(129);
                           //   dt.Columns["HIPDBVerifDate"].SetOrdinal(130);
                              dt.Columns["LastNPDBDate"].SetOrdinal(100);
                              dt.Columns["LastNPDBDerog"].SetOrdinal(101);
                              dt.Columns["CIN"].SetOrdinal(102);
                            //  dt.Columns["CINState"].SetOrdinal(134);
                              dt.Columns["Medicare"].SetOrdinal(103);
                              // dt.Columns["MedicareState"].SetOrdinal(136);
                             // dt.Columns["RadioFluroNum"].SetOrdinal(137);
                            //  dt.Columns["UPIN"].SetOrdinal(138);
                           //   dt.Columns["CAQH_ID"].SetOrdinal(139);
                              dt.Columns["NPI"].SetOrdinal(104);
                             // dt.Columns["NPIVerifBy"].SetOrdinal(141);
                             // dt.Columns["NPIVerifDate"].SetOrdinal(142);
                            //  dt.Columns["NPIVerifMthd"].SetOrdinal(143);
                              dt.Columns["HCFADerog"].SetOrdinal(105);
                              dt.Columns["HCFAVerifBy"].SetOrdinal(106);
                              dt.Columns["HCFAVerifMthd"].SetOrdinal(107);
                              dt.Columns["HCFAVerifDate"].SetOrdinal(108);
                            
                              
                             // dt.Columns["HCFACode"].SetOrdinal(148);
                              dt.Columns["CMSOptOut"].SetOrdinal(109);
                              dt.Columns["CMSOptOutVerifBy"].SetOrdinal(110);
                              dt.Columns["CMSOptOutVerifDate"].SetOrdinal(111);
                              dt.Columns["CMSOptOutVerifMthd"].SetOrdinal(112);
                             // dt.Columns["StateLEIEDerog"].SetOrdinal(153);
                             // dt.Columns["StateLEIEVerifBy"].SetOrdinal(154);
                             // dt.Columns["StateLEIEVerifDate"].SetOrdinal(155);
                             // dt.Columns["StateLEIEVerifMthd"].SetOrdinal(156);
                            //  dt.Columns["EPLSDerog"].SetOrdinal(157);
                            //  dt.Columns["EPLSVerifBy"].SetOrdinal(158);
                             // dt.Columns["EPLSVerifDate"].SetOrdinal(159);
                           //   dt.Columns["EPLSVerifMthd"].SetOrdinal(160);
                            //  dt.Columns["OFACDerog"].SetOrdinal(161);
                            //  dt.Columns["OFACVerifBy"].SetOrdinal(162);
                             // dt.Columns["OFACVerifDate"].SetOrdinal(163);
                            //  dt.Columns["OFACVerifMthd"].SetOrdinal(164);
                            //  dt.Columns["ECFMGNum"].SetOrdinal(165);
                            //  dt.Columns["ECFMGVerifDate"].SetOrdinal(166);
                           //   dt.Columns["ECFMGVerifBy"].SetOrdinal(167);
                          //    dt.Columns["ECFMGVerifMthd"].SetOrdinal(168);
                           //   dt.Columns["ICC_Number"].SetOrdinal(169);
                           //   dt.Columns["ICC_ExpDate"].SetOrdinal(170);
                           //   dt.Columns["ICC_VerifBy"].SetOrdinal(171);
                            //  dt.Columns["ICC_VerifMthd"].SetOrdinal(172);
                             // dt.Columns["ICC_VerifDate"].SetOrdinal(173);
                             // dt.Columns["CLIA_Number"].SetOrdinal(157);
                             // dt.Columns["CLIA_ExpDate"].SetOrdinal(175);
                             // dt.Columns["CLIA_VerifBy"].SetOrdinal(176);
                             // dt.Columns["CLIA_VerifMthd"].SetOrdinal(177);
                           //   dt.Columns["CLIA_VerifDate"].SetOrdinal(178);
                            //  dt.Columns["StartDate"].SetOrdinal(179);
                              dt.Columns["NeedCredent"].SetOrdinal(113);
                          //    dt.Columns["NeedCredentBy"].SetOrdinal(181);
                            //  dt.Columns["NeedCredentDate"].SetOrdinal(182);
                              dt.Columns["Rcvrd"].SetOrdinal(114);
                              dt.Columns["NxtRptType"].SetOrdinal(115);
                              dt.Columns["NxtRptDate"].SetOrdinal(116);
                             // dt.Columns["ReCred"].SetOrdinal(186);
                              dt.Columns["LastRptType"].SetOrdinal(117);
                          //////    dt.Columns["LastRptDate"].SetOrdinal(118);
                           ////   dt.Columns["CycleType"].SetOrdinal(119);
                        ////      dt.Columns["CycleDate"].SetOrdinal(120);
                            //  dt.Columns["PanelType"].SetOrdinal(191);
                           //   dt.Columns["FirstExpDate"].SetOrdinal(192);
                           //   dt.Columns["SubPlanID"].SetOrdinal(193);
                            //  dt.Columns["SubPlanName"].SetOrdinal(197);
                           ////   dt.Columns["DateApplicationRecd"].SetOrdinal(121);
                             // dt.Columns["SSNDerog"].SetOrdinal(196);
                              //dt.Columns["SSNVerifby"].SetOrdinal(197);
                            //  dt.Columns["SSNVerifDate"].SetOrdinal(198);
                             // dt.Columns["SSNVerifMthd"].SetOrdinal(199);
                           ////   dt.Columns["ReviewStatus"].SetOrdinal(122);
                          ////    dt.Columns["ReviewDate"].SetOrdinal(123);
                             // dt.Columns["MiscDerog"].SetOrdinal(202);
                          }
                          catch (Exception)
                          {
                              
                              throw;
                          }

                                               
                        ////latest code


                    //        //  dt.Columns["NxtRptType"].SetOrdinal(6);
                    //        //  dt.Columns["NxtRptDate"].SetOrdinal(7);
                    //       //   dt.Columns["LastRptType"].SetOrdinal(8);
                    //       //   dt.Columns["LastRptDate"].SetOrdinal(9);
                    //          dt.Columns["InvDate"].SetOrdinal(6);



                    //              dt.Columns["ProcessDate"].SetOrdinal(11);
                    //              dt.Columns["CredType"].SetOrdinal(12);
                    //             // dt.Columns["NeedCredent"].SetOrdinal(13);
                    //////////////    dt.Columns["Rcvrd"].SetOrdinal(14);
                    //              dt.Columns["Amount"].SetOrdinal(15);
                    //                  dt.Columns["Lname"].SetOrdinal(16);
                    //                  dt.Columns["Fname"].SetOrdinal(17);
                    //                  dt.Columns["Mname"].SetOrdinal(18);
                    //                  dt.Columns["Suffix"].SetOrdinal(19);
                    //                  dt.Columns["Title"].SetOrdinal(20);
                    //                 dt.Columns["DOB"].SetOrdinal(102);
                    //                 dt.Columns["SSN"].SetOrdinal(103);
                    //                 dt.Columns["Gender"].SetOrdinal(110);
                    //                     dt.Columns["Specialty"].SetOrdinal(21);
                    //                     dt.Columns["PracName"].SetOrdinal(22);
                    //                     dt.Columns["Street"].SetOrdinal(23);
                    //                     dt.Columns["City"].SetOrdinal(24);
                    //                     dt.Columns["State"].SetOrdinal(25);
                    //                     dt.Columns["Zip"].SetOrdinal(26);

                    //     dt.Columns.Remove("Zip4");
                    //     dt.Columns["Phone"].SetOrdinal(27);
                    //         dt.Columns.Remove("Email");
                    //         dt.Columns.Remove("Tin");

                    //////////////   ;
                    //             dt.Columns["LicState"].SetOrdinal(28);
                    //             dt.Columns["LicNum"].SetOrdinal(29);
                    //             dt.Columns["LicExp"].SetOrdinal(30);
                    //             dt.Columns["LicStatus"].SetOrdinal(31);
                    //             dt.Columns["LicVerifBy"].SetOrdinal(32);
                    //          //   dt.Columns["LicStatus"].SetOrdinal(33);
                    //                 dt.Columns["LicVerifMthd"].SetOrdinal(33);
                    //                 dt.Columns["LicVerifDate"].SetOrdinal(34);
                    //      dt.Columns.Remove("AIMVerifBy");
                    //    //  dt.Columns.Remove("HIPDBDerog");
                    //      dt.Columns.Remove("AIMVerifDate");
                    //      dt.Columns["SALDerog"].SetOrdinal(97);

                    //     dt.Columns["SALVerifBy"].SetOrdinal(94);
                    //     dt.Columns["SALVerifMthd"].SetOrdinal(95);
                    //     dt.Columns["SALVerifDate"].SetOrdinal(96);
                        
                    //////////////   


                    //         dt.Columns["School"].SetOrdinal(35);
                    //         dt.Columns["GradYr"].SetOrdinal(36);
                    //      dt.Columns.Remove("SchoolStartDate");
                    //   dt.Columns.Remove("SchoolEndDate");
                    //       dt.Columns["SchoolVerifBy"].SetOrdinal(37);
                    //       dt.Columns["SchoolVerifMthd"].SetOrdinal(38);
                    //       dt.Columns["SchoolVerifDate"].SetOrdinal(39);


                    ////////////    dt.Columns["TrnName"].SetOrdinal(40);

                    ////////////    dt.Columns["TrnYr"].SetOrdinal(41);
                        
                    ////////////    dt.Columns["TrnVerifBy"].SetOrdinal(42);
                    ////////////    dt.Columns["TrnVerifMthd"].SetOrdinal(43);
                    ////////////    dt.Columns["TrnVerifDate"].SetOrdinal(44);
                    ////////////    dt.Columns["BoardStatus"].SetOrdinal(45);
                    ////////////    dt.Columns["BoardDate"].SetOrdinal(46);
                    ////////////    dt.Columns["BoardExp"].SetOrdinal(47);
                    ////////////    dt.Columns["BoardLife"].SetOrdinal(48);
                    ////////////    dt.Columns["BoardVerifBy"].SetOrdinal(49);
                    ////////////    dt.Columns["BoardVerifMthd"].SetOrdinal(50);
                    ////////////    dt.Columns["BoardVerifDate"].SetOrdinal(51);
                    ////////////    dt.Columns["DEANum"].SetOrdinal(52);
                    ////////////    dt.Columns["DEAExp"].SetOrdinal(53);
                    ////////////    dt.Columns["DEAVerifBy"].SetOrdinal(54);
                    ////////////    dt.Columns["DEAVerifMthd"].SetOrdinal(55);
                    ////////////    dt.Columns["DEAVerifDate"].SetOrdinal(56);


                    ////////////    dt.Columns["TPANum"].SetOrdinal(57);
                    ////////////    dt.Columns["TPAExp"].SetOrdinal(58);
                    ////////////    dt.Columns["TPAVerifBy"].SetOrdinal(59);
                    ////////////    dt.Columns["TPAVerifMthd"].SetOrdinal(60);
                    ////////////    dt.Columns["TPAVerifDate"].SetOrdinal(61);
                    ////////////    //--DPANum,DPAVerifDate
                    ////////////    dt.Columns["DPANum"].SetOrdinal(62);
                    ////////////    dt.Columns["DPAExp"].SetOrdinal(63);
                    ////////////    dt.Columns["DPAVerifBy"].SetOrdinal(64);
                    ////////////    dt.Columns["DPAVerifMthd"].SetOrdinal(65);
                    ////////////    dt.Columns["DPAVerifDate"].SetOrdinal(66);
                        
                    ////////////    dt.Columns["SDCNum"].SetOrdinal(67);
                    ////////////    dt.Columns["SDCExp"].SetOrdinal(68);
                    ////////////    dt.Columns["SDCVerifBy"].SetOrdinal(69);
                    ////////////    dt.Columns["SDCVerifMthd"].SetOrdinal(70);
                    ////////////    dt.Columns["SDCVerifDate"].SetOrdinal(71);
                    ////////////    dt.Columns["InsCoName"].SetOrdinal(72);
                    ////////////    dt.Columns["InsExp"].SetOrdinal(73);
                    ////////////    dt.Columns["InsPolicyNum"].SetOrdinal(74);
                    ////////////    dt.Columns["InsEachClaim"].SetOrdinal(75);
                    ////////////    dt.Columns["InsAggrClaim"].SetOrdinal(76);
                    ////////////    dt.Columns["InsVerifBy"].SetOrdinal(77);
                    ////////////    dt.Columns["InsVerifDate"].SetOrdinal(78);
                    ////////////    dt.Columns["HosName"].SetOrdinal(79);
                    ////////////    dt.Columns["HosVerifBy"].SetOrdinal(80);
                    ////////////    dt.Columns["HosVerifDate"].SetOrdinal(81);
                    ////////////    dt.Columns["WrkVerifBy"].SetOrdinal(82);
                    ////////////    dt.Columns["WrkVerifDate"].SetOrdinal(83);
                    ////////////    dt.Columns["AttestDate"].SetOrdinal(84);
                    ////////////    dt.Columns["AttestVerifBy"].SetOrdinal(85);
                    ////////////    dt.Columns["AttestVerifDate"].SetOrdinal(86);
                    ////////////    dt.Columns["AttestDerog"].SetOrdinal(87);
                    ////////////    dt.Columns["NPDBDate"].SetOrdinal(88);
                    ////////////    dt.Columns["NPDBVerifBy"].SetOrdinal(89);

                    ////////////    dt.Columns["NPDBVerifDate"].SetOrdinal(90);
                        																																																																																
                    ////////////    dt.Columns["NPDBDerog"].SetOrdinal(91);
                    ////////////    dt.Columns["LastNPDBDate"].SetOrdinal(92);
                    ////////////    dt.Columns["LastNPDBDerog"].SetOrdinal(93);
                    ////////////   
                    ////////////    dt.Columns["HCFAVerifBy"].SetOrdinal(98);
                    ////////////    dt.Columns["HCFAVerifMthd"].SetOrdinal(99);
                    ////////////    dt.Columns["HCFAVerifDate"].SetOrdinal(100);
                    ////////////    dt.Columns["HCFADerog"].SetOrdinal(101);
                    ////////////   
                    ////////////    dt.Columns["CycleDate"].SetOrdinal(104);
                    ////////////    dt.Columns["CycleType"].SetOrdinal(105);
                    ////////////    dt.Columns["NPI"].SetOrdinal(106);
                    ////////////    dt.Columns["CIN"].SetOrdinal(107);
                    ////////////    //												
                    ////////////    dt.Columns["Medicare"].SetOrdinal(108);
                    ////////////    dt.Columns["Drug Schedule"].SetOrdinal(109);
                    ////////////   
                    ////////////    dt.Columns["CMSOptOut"].SetOrdinal(111);

                    ////////////    dt.Columns["CMSOptOutVerifBy"].SetOrdinal(112);
                    ////////////    dt.Columns["CMSOptOutVerifDate"].SetOrdinal(113);
                    ////////////    dt.Columns["CMSOptOutVerifMthd"].SetOrdinal(114);
                    ////////////    dt.Columns["ReviewStatus"].SetOrdinal(115);
                    ////////////    dt.Columns["ReviewDate"].SetOrdinal(116);
                    ////////////    dt.Columns["DateApplicationRecd"].SetOrdinal(117);
                       

                    ////////////   
                    ////////////   // dt.Columns.Remove("NPDBVerifBy");
                    ////////////  
                    ////////////      dt.Columns.Remove("HIPDBVerifBy");
                    ////////////    

                    ////////////  ///     dt.Columns.Remove("LastNPDBDerog");

                    ////////////       dt.Columns.Remove("Degree");
                    ////////////        dt.Columns.Remove("InternshipID");
                    ////////////       dt.Columns.Remove("InternName");
                    ////////////       dt.Columns.Remove("MedicareState");
                    ////////////dt.Columns.Remove("InternshipYr");
                    ////////////dt.Columns.Remove("InternshipStartDate");
                    //////////// // dt.Columns.Remove("LastNPDBDerog");
                    ////////////      dt.Columns.Remove("InternshipEndDate");

                    ////////////   dt.Columns.Remove("InternshipVerifBy");
                    ////////////   dt.Columns.Remove("InternshipVerifMthd");
                    ////////////        dt.Columns.Remove("NPIVerifDate");
                    ////////////      dt.Columns.Remove("InternshipVerifDate");
                          
                    ////////////       //dt.Columns.Remove("HCFADerog");
                    ////////////       dt.Columns.Remove("TrainingStartDate");
                    ////////////       dt.Columns.Remove("TrainingEndDate");
                    ////////////       dt.Columns.Remove("FellowshipID");

                    ////////////  // dt.Columns.Remove("HCFAVerifDate");

                    ////////////   dt.Columns.Remove("FellowName");
                    ////////////   dt.Columns.Remove("FellowshipYr");
                    ////////////   dt.Columns.Remove("FellowshipStartDate");
                    ////////////   dt.Columns.Remove("FellowshipEndDate");
                    ////////////      //dt.Columns.Remove("CMSOptOutVerifMthd");
                    ////////////      dt.Columns.Remove("FellowshipVerifBy");

                    ////////////      dt.Columns.Remove("FellowshipVerifMthd");
                    ////////////      dt.Columns.Remove("FellowshipVerifDate");
                    ////////////      dt.Columns.Remove("BoardName");
                    ////////////      dt.Columns.Remove("BoardCertNum");

                    ////////////      dt.Columns.Remove("InsUnderLimit");
                    ////////////      //dt.Columns.Remove("HIPDBDerog");
                    ////////////      dt.Columns.Remove("HIPDBDate");

                    ////////////    //  dt.Columns.Remove("HIPDBVerifBy");
                    ////////////      dt.Columns.Remove("HIPDBVerifDate");
                    ////////////      dt.Columns.Remove("CINState");

                    ////////////     // dt.Columns.Remove("MedicareState");
                    ////////////      dt.Columns.Remove("RadioFluroNum");

                         
                    ////////////      dt.Columns.Remove("UPIN");
                    ////////////      dt.Columns.Remove("CAQH_ID");
                    ////////////      dt.Columns.Remove("NPIVerifBy");
                    ////////////      //dt.Columns.Remove("NPIVerifDate");

                    ////////////      dt.Columns.Remove("NPIVerifMthd");
                    ////////////      dt.Columns.Remove("HCFACode");
                    ////////////      dt.Columns.Remove("StateLEIEDerog");

                    ////////////      dt.Columns.Remove("StateLEIEVerifBy");
                    ////////////      dt.Columns.Remove("StateLEIEVerifDate");

                    ////////////      dt.Columns.Remove("StateLEIEVerifMthd");

                    ////////////        dt.Columns.Remove("EPLSDerog");
                    ////////////        dt.Columns.Remove("EPLSVerifBy");

                    ////////////        dt.Columns.Remove("EPLSVerifDate");
                    ////////////        dt.Columns.Remove("EPLSVerifMthd");
                    ////////////        dt.Columns.Remove("OFACDerog");

                    ////////////    //    //										
                          
                    ////////////        dt.Columns.Remove("OFACVerifBy");
                    ////////////        dt.Columns.Remove("OFACVerifDate");
                    ////////////        dt.Columns.Remove("OFACVerifMthd");

                    ////////////        dt.Columns.Remove("ECFMGNum");
                    ////////////        dt.Columns.Remove("ECFMGVerifDate");
                    ////////////        dt.Columns.Remove("ECFMGVerifBy");
                    ////////////        dt.Columns.Remove("ECFMGVerifMthd");
                    ////////////        dt.Columns.Remove("ICC_Number");

                    ////////////        dt.Columns.Remove("ICC_ExpDate");

                    ////////////        dt.Columns.Remove("ICC_VerifBy");
                    ////////////        dt.Columns.Remove("ICC_VerifMthd");
                    ////////////        dt.Columns.Remove("ICC_VerifDate");


                    ////////////        dt.Columns.Remove("CLIA_Number");
                    ////////////        dt.Columns.Remove("CLIA_ExpDate");
                    ////////////        dt.Columns.Remove("CLIA_VerifBy");
                    ////////////        dt.Columns.Remove("CLIA_VerifMthd");
                    ////////////        dt.Columns.Remove("CLIA_VerifDate");
                    ////////////        dt.Columns.Remove("NeedCredentBy");
                    ////////////        dt.Columns.Remove("NeedCredentDate");
                    ////////////        dt.Columns.Remove("ReCred");
                    ////////////        dt.Columns.Remove("PanelType");
                    ////////////        // 									

                    ////////////        dt.Columns.Remove("FirstExpDate");

                    ////////////        dt.Columns.Remove("SubPlanID");
                    ////////////        dt.Columns.Remove("SubPlanName");
                    ////////////        dt.Columns.Remove("SSNDerog");
                    ////////////        dt.Columns.Remove("SSNVerifby");
                    ////////////        dt.Columns.Remove("SSNVerifDate");
                    ////////////        dt.Columns.Remove("SSNVerifMthd");

                    ////////////        dt.Columns.Remove("MiscDerog");
                    ////////////        dt.Columns.Remove("StartDate"); 
                        


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


        private static string CreateTxtFileTest(int PlanID, AppHelper.TextFile TxtFile)
        {
            string rtnStr = "";
            Program p = new Program ();
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

                       dt = p.GetCredReport(PlanID,TxtFile);

                        //////////DataTable dtNew = new DataTable();
                        //////////fileName = string.Format(fileName, "CredData");
                        //////////conText = string.Format("Creating the {0} file...", fileName);
                        //////////Console.Write(conText);

                        //////////parameters = new List<SqlParameter>();
                        //////////parameters.Add(new SqlParameter("@InvDate", DateTime.Now.ToShortDateString()));
                        //////////parameters.Add(new SqlParameter("@PlanID", PlanID));

                        //////////dtNew = RunSQL("rpt_Reports_txt", parameters, _Global, CommandType.StoredProcedure).Clone();

                        //////////parameters = new List<SqlParameter>();
                        //////////parameters.Add(new SqlParameter("@PlanID1", PlanID));
                        ////////////  
                        ////////////   web2_ProvsInCycle
                        //////////foreach (DataRow dr in RunSQL("web2_ProvsInCycleNPI", parameters, _Global, CommandType.StoredProcedure).Rows)
                        //////////{
                        //////////    DataRow newRow = dtNew.NewRow();

                        //////////    PlanProvData(PlanID, dr["ProvID"], ref newRow);
                        //////////    PlanPracData(PlanID, dr["ProvID"], ref newRow);
                        //////////    ProvData(dr["ProvID"], ref newRow);
                        //////////    EducationData(dr["ProvID"], ref newRow);
                        //////////    HospitalData(PlanID, dr["ProvID"], ref newRow);
                        //////////    IDNumbersData(dr["ProvID"], 3, ref newRow);
                        //////////    IDNumbersData(dr["ProvID"], 5, ref newRow);
                        //////////    IDNumbersData(dr["ProvID"], 9, ref newRow);
                        //////////    IDNumbersData(dr["ProvID"], 10, ref newRow);
                        //////////    IDNumbersData(dr["ProvID"], 11, ref newRow);
                        //////////    IDNumbersData(dr["ProvID"], 12, ref newRow);
                        //////////    InsuranceData(dr["ProvID"], ref newRow);

                        //////////    dtNew.Rows.Add(newRow);
                        //////////}


                        //////////DeleteALLTableData();
                        //////////InsertALLData(dtNew);
                        //////////dt = GetALLNewData(PlanID);
                      

                        //////////try
                        //////////{
                        //////////    dt.Columns["PlanID"].SetOrdinal(0);
                        //////////    dt.Columns["ProvID"].SetOrdinal(1);
                        //////////    dt.Columns["PracID"].SetOrdinal(2);
                        //////////    dt.Columns["PlanProvID"].SetOrdinal(3);
                        //////////    dt.Columns["PlanPracID"].SetOrdinal(4);
                        //////////    dt.Columns["PlanName"].SetOrdinal(5);
                        //////////    dt.Columns["InvDate"].SetOrdinal(6);
                        //////////    dt.Columns["ProcessDate"].SetOrdinal(7);
                        //////////    dt.Columns["CredType"].SetOrdinal(8);
                        //////////    dt.Columns["Amount"].SetOrdinal(9);
                        //////////    dt.Columns["Lname"].SetOrdinal(10);
                        //////////    dt.Columns["Fname"].SetOrdinal(11);
                        //////////    dt.Columns["Mname"].SetOrdinal(12);
                        //////////    dt.Columns["Suffix"].SetOrdinal(13);
                        //////////    dt.Columns["Title"].SetOrdinal(14);
                        //////////    dt.Columns["DOB"].SetOrdinal(15);
                        //////////    dt.Columns["SSN"].SetOrdinal(16);
                        //////////    dt.Columns["Gender"].SetOrdinal(17);

                        //////////    dt.Columns["Specialty"].SetOrdinal(18);

                        //////////    dt.Columns["PracName"].SetOrdinal(19);

                        //////////    dt.Columns["Street"].SetOrdinal(20);
                        //////////    dt.Columns["City"].SetOrdinal(21);
                        //////////    dt.Columns["State"].SetOrdinal(22);
                        //////////    dt.Columns["Zip"].SetOrdinal(23);
                        //////////    //dt.Columns["Zip4"].SetOrdinal(24);
                        //////////    dt.Columns["Phone"].SetOrdinal(25);
                        //////////    // dt.Columns["Email"].SetOrdinal(26);
                        //////////    // dt.Columns["Tin"].SetOrdinal(27);
                        //////////    dt.Columns["LicState"].SetOrdinal(28);
                        //////////    dt.Columns["LicNum"].SetOrdinal(29);
                        //////////    dt.Columns["LicExp"].SetOrdinal(30);
                        //////////    dt.Columns["LicStatus"].SetOrdinal(31);
                        //////////    dt.Columns["LicVerifBy"].SetOrdinal(32);
                        //////////    dt.Columns["LicVerifMthd"].SetOrdinal(33);
                        //////////    dt.Columns["LicVerifDate"].SetOrdinal(34);
                        //////////    //  dt.Columns["AIMVerifBy"].SetOrdinal(35);
                        //////////    //  dt.Columns["AIMVerifDate"].SetOrdinal(36);
                        //////////    dt.Columns["SALDerog"].SetOrdinal(37);
                        //////////    dt.Columns["SALVerifBy"].SetOrdinal(38);
                        //////////    dt.Columns["SALVerifMthd"].SetOrdinal(39);
                        //////////    dt.Columns["SALVerifDate"].SetOrdinal(40);

                        //////////    dt.Columns["School"].SetOrdinal(41);

                        //////////    dt.Columns["GradYr"].SetOrdinal(42);
                        //////////    dt.Columns["SchoolStartDate"].SetOrdinal(43);
                        //////////    dt.Columns["SchoolEndDate"].SetOrdinal(44);
                        //////////    dt.Columns["SchoolVerifBy"].SetOrdinal(45);
                        //////////    dt.Columns["SchoolVerifMthd"].SetOrdinal(46);
                        //////////    dt.Columns["SchoolVerifDate"].SetOrdinal(47);
                        //////////    dt.Columns["BoardLife"].SetOrdinal(48);
                        //////////    dt.Columns["BoardVerifBy"].SetOrdinal(49);
                        //////////    dt.Columns["BoardVerifMthd"].SetOrdinal(50);
                        //////////    dt.Columns["BoardVerifDate"].SetOrdinal(51);
                        //////////    dt.Columns["Degree"].SetOrdinal(52);
                        //////////    dt.Columns["InternshipID"].SetOrdinal(53);
                        //////////    dt.Columns["InternName"].SetOrdinal(54);
                        //////////    dt.Columns["InternshipYr"].SetOrdinal(55);
                        //////////    dt.Columns["InternshipStartDate"].SetOrdinal(56);
                        //////////    dt.Columns["InternshipEndDate"].SetOrdinal(57);
                        //////////    dt.Columns["InternshipVerifBy"].SetOrdinal(58);
                        //////////    dt.Columns["InternshipVerifMthd"].SetOrdinal(59);
                        //////////    dt.Columns["InternshipVerifDate"].SetOrdinal(60);
                        //////////    dt.Columns["TrnName"].SetOrdinal(61);
                        //////////    dt.Columns["TrnYr"].SetOrdinal(62);
                        //////////    dt.Columns["TrainingStartDate"].SetOrdinal(63);
                        //////////    dt.Columns["TrainingEndDate"].SetOrdinal(64);
                        //////////    dt.Columns["TrnVerifBy"].SetOrdinal(65);
                        //////////    dt.Columns["TrnVerifMthd"].SetOrdinal(66);
                        //////////    dt.Columns["TrnVerifDate"].SetOrdinal(67);
                        //////////    dt.Columns["FellowshipID"].SetOrdinal(68);
                        //////////    dt.Columns["FellowName"].SetOrdinal(69);
                        //////////    dt.Columns["FellowshipYr"].SetOrdinal(70);
                        //////////    dt.Columns["FellowshipStartDate"].SetOrdinal(71);
                        //////////    dt.Columns["FellowshipEndDate"].SetOrdinal(72);
                        //////////    dt.Columns["FellowshipVerifBy"].SetOrdinal(73);
                        //////////    dt.Columns["FellowshipVerifMthd"].SetOrdinal(74);
                        //////////    dt.Columns["FellowshipVerifDate"].SetOrdinal(75);
                        //////////    dt.Columns["BoardName"].SetOrdinal(76);
                        //////////    dt.Columns["BoardStatus"].SetOrdinal(77);
                        //////////    dt.Columns["BoardCertNum"].SetOrdinal(78);
                        //////////    dt.Columns["BoardDate"].SetOrdinal(79);
                        //////////    dt.Columns["BoardExp"].SetOrdinal(80);
                        //////////    dt.Columns["BoardLife"].SetOrdinal(81);
                        //////////    dt.Columns["BoardVerifBy"].SetOrdinal(82);
                        //////////    dt.Columns["BoardVerifMthd"].SetOrdinal(83);
                        //////////    dt.Columns["BoardVerifDate"].SetOrdinal(84);
                        //////////    dt.Columns["DEANum"].SetOrdinal(85);
                        //////////    dt.Columns["DEAExp"].SetOrdinal(86);
                        //////////    dt.Columns["Drug Schedule"].SetOrdinal(87);
                        //////////    dt.Columns["DEAVerifBy"].SetOrdinal(88);
                        //////////    dt.Columns["DEAVerifMthd"].SetOrdinal(89);
                        //////////    dt.Columns["DEAVerifDate"].SetOrdinal(90);
                        //////////    dt.Columns["TPANum"].SetOrdinal(91);
                        //////////    dt.Columns["TPAExp"].SetOrdinal(92);
                        //////////    dt.Columns["TPAVerifBy"].SetOrdinal(93);
                        //////////    dt.Columns["TPAVerifMthd"].SetOrdinal(94);
                        //////////    dt.Columns["TPAVerifDate"].SetOrdinal(95);
                        //////////    dt.Columns["DPANum"].SetOrdinal(96);
                        //////////    dt.Columns["DPAExp"].SetOrdinal(97);
                        //////////    dt.Columns["DPAVerifMthd"].SetOrdinal(98);
                        //////////    dt.Columns["DPAVerifBy"].SetOrdinal(99);
                        //////////    dt.Columns["DPAVerifDate"].SetOrdinal(100);
                        //////////    dt.Columns["SDCNum"].SetOrdinal(101);
                        //////////    dt.Columns["SDCExp"].SetOrdinal(102);
                        //////////    dt.Columns["SDCVerifBy"].SetOrdinal(103);
                        //////////    dt.Columns["SDCVerifMthd"].SetOrdinal(104);
                        //////////    dt.Columns["SDCVerifDate"].SetOrdinal(105);
                        //////////    dt.Columns["InsCoName"].SetOrdinal(106);
                        //////////    dt.Columns["InsExp"].SetOrdinal(107);
                        //////////    dt.Columns["InsPolicyNum"].SetOrdinal(108);
                        //////////    dt.Columns["InsEachClaim"].SetOrdinal(109);
                        //////////    dt.Columns["InsAggrClaim"].SetOrdinal(110);
                        //////////    dt.Columns["InsVerifBy"].SetOrdinal(111);
                        //////////    dt.Columns["InsVerifDate"].SetOrdinal(112);
                        //////////    dt.Columns["InsUnderLimit"].SetOrdinal(113);
                        //////////    dt.Columns["HosName"].SetOrdinal(114);
                        //////////    dt.Columns["HosVerifBy"].SetOrdinal(115);
                        //////////    dt.Columns["HosVerifDate"].SetOrdinal(116);
                        //////////    dt.Columns["WrkVerifBy"].SetOrdinal(117);
                        //////////    dt.Columns["WrkVerifDate"].SetOrdinal(118);
                        //////////    dt.Columns["AttestDerog"].SetOrdinal(119);
                        //////////    dt.Columns["AttestDate"].SetOrdinal(120);
                        //////////    dt.Columns["AttestVerifBy"].SetOrdinal(121);
                        //////////    dt.Columns["AttestVerifDate"].SetOrdinal(122);
                        //////////    dt.Columns["NPDBDerog"].SetOrdinal(123);
                        //////////    dt.Columns["NPDBDate"].SetOrdinal(124);
                        //////////    dt.Columns["NPDBVerifBy"].SetOrdinal(125);
                        //////////    dt.Columns["NPDBVerifDate"].SetOrdinal(126);
                        //////////    dt.Columns["HIPDBDerog"].SetOrdinal(127);
                        //////////    dt.Columns["HIPDBDate"].SetOrdinal(128);
                        //////////    dt.Columns["HIPDBVerifBy"].SetOrdinal(129);
                        //////////    dt.Columns["HIPDBVerifDate"].SetOrdinal(130);
                        //////////    dt.Columns["LastNPDBDate"].SetOrdinal(131);
                        //////////    dt.Columns["LastNPDBDerog"].SetOrdinal(132);
                        //////////    dt.Columns["CIN"].SetOrdinal(133);
                        //////////    dt.Columns["CINState"].SetOrdinal(134);
                        //////////    dt.Columns["Medicare"].SetOrdinal(135);
                        //////////    dt.Columns["MedicareState"].SetOrdinal(136);
                        //////////    dt.Columns["RadioFluroNum"].SetOrdinal(137);
                        //////////    dt.Columns["UPIN"].SetOrdinal(138);
                        //////////    dt.Columns["CAQH_ID"].SetOrdinal(139);
                        //////////    dt.Columns["NPI"].SetOrdinal(140);
                        //////////    dt.Columns["NPIVerifBy"].SetOrdinal(141);
                        //////////    dt.Columns["NPIVerifDate"].SetOrdinal(142);
                        //////////    dt.Columns["NPIVerifMthd"].SetOrdinal(143);
                        //////////    dt.Columns["HCFADerog"].SetOrdinal(144);
                        //////////    dt.Columns["HCFAVerifBy"].SetOrdinal(145);
                        //////////    dt.Columns["HCFAVerifMthd"].SetOrdinal(146);
                        //////////    dt.Columns["HCFAVerifDate"].SetOrdinal(147);
                        //////////    dt.Columns["HCFACode"].SetOrdinal(148);
                        //////////    dt.Columns["CMSOptOut"].SetOrdinal(149);
                        //////////    dt.Columns["CMSOptOutVerifBy"].SetOrdinal(150);
                        //////////    dt.Columns["CMSOptOutVerifDate"].SetOrdinal(151);
                        //////////    dt.Columns["CMSOptOutVerifMthd"].SetOrdinal(152);
                        //////////    dt.Columns["StateLEIEDerog"].SetOrdinal(153);
                        //////////    dt.Columns["StateLEIEVerifBy"].SetOrdinal(154);
                        //////////    dt.Columns["StateLEIEVerifDate"].SetOrdinal(155);
                        //////////    dt.Columns["StateLEIEVerifMthd"].SetOrdinal(156);
                        //////////    dt.Columns["EPLSDerog"].SetOrdinal(157);
                        //////////    dt.Columns["EPLSVerifBy"].SetOrdinal(158);
                        //////////    dt.Columns["EPLSVerifDate"].SetOrdinal(159);
                        //////////    dt.Columns["EPLSVerifMthd"].SetOrdinal(160);
                        //////////    dt.Columns["OFACDerog"].SetOrdinal(161);
                        //////////    dt.Columns["OFACVerifBy"].SetOrdinal(162);
                        //////////    dt.Columns["OFACVerifDate"].SetOrdinal(163);
                        //////////    dt.Columns["OFACVerifMthd"].SetOrdinal(164);
                        //////////    dt.Columns["ECFMGNum"].SetOrdinal(165);
                        //////////    dt.Columns["ECFMGVerifDate"].SetOrdinal(166);
                        //////////    dt.Columns["ECFMGVerifBy"].SetOrdinal(167);
                        //////////    dt.Columns["ECFMGVerifMthd"].SetOrdinal(168);
                        //////////    dt.Columns["ICC_Number"].SetOrdinal(169);
                        //////////    dt.Columns["ICC_ExpDate"].SetOrdinal(170);
                        //////////    dt.Columns["ICC_VerifBy"].SetOrdinal(171);
                        //////////    dt.Columns["ICC_VerifMthd"].SetOrdinal(172);
                        //////////    dt.Columns["ICC_VerifDate"].SetOrdinal(173);
                        //////////    dt.Columns["CLIA_Number"].SetOrdinal(157);
                        //////////    dt.Columns["CLIA_ExpDate"].SetOrdinal(175);
                        //////////    dt.Columns["CLIA_VerifBy"].SetOrdinal(176);
                        //////////    dt.Columns["CLIA_VerifMthd"].SetOrdinal(177);
                        //////////    dt.Columns["CLIA_VerifDate"].SetOrdinal(178);
                        //////////    dt.Columns["StartDate"].SetOrdinal(179);
                        //////////    dt.Columns["NeedCredent"].SetOrdinal(180);
                        //////////    dt.Columns["NeedCredentBy"].SetOrdinal(181);
                        //////////    dt.Columns["NeedCredentDate"].SetOrdinal(182);
                        //////////    dt.Columns["Rcvrd"].SetOrdinal(183);
                        //////////    dt.Columns["NxtRptType"].SetOrdinal(184);
                        //////////    dt.Columns["NxtRptDate"].SetOrdinal(185);
                        //////////    dt.Columns["ReCred"].SetOrdinal(186);
                        //////////    dt.Columns["LastRptType"].SetOrdinal(187);
                        //////////    dt.Columns["LastRptDate"].SetOrdinal(188);
                        //////////    dt.Columns["CycleType"].SetOrdinal(189);
                        //////////    dt.Columns["CycleDate"].SetOrdinal(190);
                        //////////    dt.Columns["PanelType"].SetOrdinal(191);
                        //////////    dt.Columns["FirstExpDate"].SetOrdinal(192);
                        //////////    dt.Columns["SubPlanID"].SetOrdinal(193);
                        //////////    dt.Columns["SubPlanName"].SetOrdinal(197);
                        //////////    dt.Columns["DateApplicationRecd"].SetOrdinal(195);
                        //////////    dt.Columns["SSNDerog"].SetOrdinal(196);
                        //////////    dt.Columns["SSNVerifby"].SetOrdinal(197);
                        //////////    dt.Columns["SSNVerifDate"].SetOrdinal(198);
                        //////////    dt.Columns["SSNVerifMthd"].SetOrdinal(199);
                        //////////    dt.Columns["ReviewStatus"].SetOrdinal(200);
                        //////////    dt.Columns["ReviewDate"].SetOrdinal(201);
                        //////////    dt.Columns["MiscDerog"].SetOrdinal(202);
                        //////////}
                        //////////catch (Exception)
                        //////////{

                        //////////    throw;
                        //////////}


                        ////latest code




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
                                //NewRow["Medicare"] = dr["Number"];
                                NewRow["NPI"] = dr["Number"];
                                break;
                            case 5:
                                
                                NewRow["Medicare"] = dr["Number"];
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

        private DataTable GetData(int planid)
        {
            DataTable dt = new DataTable();

            using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
            {
                SqlCommand cm = new SqlCommand("web2_ProvsInCycleNPI", _Global);
                cm.CommandTimeout = 120;
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@PlanID1", planid);

                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);

            }

            return dt;
        }
        //new method
        public DataTable GetCredReport(int PlanID, AppHelper.TextFile TxtFile)
        {
            int planID = PlanID;
            bool error = false;
           // fileName = string.Format(TxtFile, "CredData");
           // conText = string.Format("Creating the {0} file...", TxtFile);
          //  Console.Write(conText);

            using (DataTable dt = CreateTable(planID))
            {
                try
                {
                    //foreach (DataRow dr in GetData().Table.Rows)
                    foreach (DataRow dr in GetData(planID).DefaultView.Table.Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        PlanProvData(planID, dr["ProvID"], ref newRow);
                        PlanPracData(planID, dr["ProvID"], ref newRow);
                        ProvData(dr["ProvID"], ref newRow);
                        EducationData(dr["ProvID"], ref newRow);
                        HospitalData(planID, dr["ProvID"], ref newRow);
                        IDNumbersData(dr["ProvID"], 3, ref newRow);
                        IDNumbersData(dr["ProvID"], 5, ref newRow);
                        IDNumbersData(dr["ProvID"], 9, ref newRow);
                        IDNumbersData(dr["ProvID"], 10, ref newRow);
                        IDNumbersData(dr["ProvID"], 11, ref newRow);
                        IDNumbersData(dr["ProvID"], 12, ref newRow);
                        InsuranceData(dr["ProvID"], ref newRow);
                        dt.Rows.Add(newRow);
                    }
                }
                catch (Exception E)
                {
                    error = true;
                   // SiteHelper.LogError(E);
                 //   errLbl.Text = E.Message;//"There was an error in generating the file.  Please try again later.";
                }
                DataTable dtNew = new DataTable();
                //if (planID == "10344" || planID == "10459" || planID == "10475" || planID == "10477" || planID == "10468")
                //{
                    DeleteALLTableData();
                    InsertALLData(dt);
                    dtNew = GetALLNewData(planID);
                    
                    return dtNew;
                //}
                //else
                //{
                    
                //    return dt;
                //}

            }
        
        
        }

        private DataTable CreateTable(int PlanID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection _Global = new SqlConnection(AppHelper.GlobalConStr))
                {
                    SqlCommand cm = new SqlCommand("rpt_Reports_txt", _Global);
                    cm.CommandTimeout = 120;
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@InvDate", DateTime.Now.ToShortDateString());
                    cm.Parameters.AddWithValue("@PlanID", PlanID);


                    SqlDataAdapter da = new SqlDataAdapter(cm);
                    da.Fill(dt);
                }
            }
            catch (Exception E)
            {
                //SiteHelper.LogError(E);
               // errLbl.Text = "There was an error in generating the file.  Please try again later.";
            }

            return dt.Clone();
        }
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
                            string rptFile = "";
                            if (PlanID==10344)
                            {
                             rptFile = string.Format(@"\\192.168.0.241\{0}\{1}\{2}\{3}\{4}", root, month, abbrev, day, SFRFileName);
                            }

                            if (PlanID == 10606)
                            {
                                rptFile = string.Format(@"\\192.168.0.241\{0}\{1}\{2}\{3}\{4}", root, month, abbrev, day, SFRFileName);
                            }

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
            string file = "";
            if (PlanID == 10344)
            {
                 file = string.Format(AppHelper.FilesDirectory, AppHelper.PlanAbbrev(PlanID)) + FileName;
            }
            if (PlanID == 10606)
            {
                file = string.Format(AppHelper.FilesDirectoryEDI, AppHelper.PlanAbbrev(PlanID)) + FileName;
            }

       //     string file = string.Format(AppHelper.FilesDirectory, AppHelper.PlanAbbrev(PlanID)) + FileName;

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
