using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD_Project.Global_Classes
{
    public class clsGlobal
    {
        static public clsUser CurrentUser; 
        static public bool RememberUsernameAndPassword(string username, string password)
        {
            string FolderPath = System.IO.Directory.GetCurrentDirectory(); // Get Project Folder Path
            string FilePath = FolderPath + "\\data.text";

            if(username == "" & File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return true;
            }

            string DataSave = username + "#//#" + password;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(DataSave);

                return true;
            }
        }
        static public bool GetStoredCredential(ref string UserName, ref string Password)
        {
            try
            {
                string FolderPath = System.IO.Directory.GetCurrentDirectory(); // Get Project Folder Path
                string FilePath = FolderPath + "\\data.text";

                if (File.Exists(FilePath))
                {

                    using (StreamReader reader = new StreamReader(FilePath))
                    {
                        string Line = "";

                        while ((Line = reader.ReadLine()) != null)
                        {
                            string[] Data = new string[2];
                            Data = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            UserName = Data[0];
                            Password = Data[1];
                        }
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }


        }
    }
}
