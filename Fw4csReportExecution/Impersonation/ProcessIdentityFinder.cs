
namespace Fw4csReportExecution.Impersonation
{


    public class ProcessIdentityFinder
    {

        private static System.Security.Principal.WindowsIdentity GetWindowsIdentity(string userName)
        {
            
            using (System.DirectoryServices.AccountManagement.UserPrincipal user =
              System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(
                System.DirectoryServices.AccountManagement.UserPrincipal.Current.Context,
                System.DirectoryServices.AccountManagement.IdentityType.SamAccountName,
                userName
                ) ??
              System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(
                System.DirectoryServices.AccountManagement.UserPrincipal.Current.Context,
                System.DirectoryServices.AccountManagement.IdentityType.UserPrincipalName,
                userName
                ))
            {
                return user == null
                  ? null
                  : new System.Security.Principal.WindowsIdentity(user.UserPrincipalName);
            } // End Using user 

        } // End Function GetWindowsIdentity 



        public static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            System.Management.ManagementObjectSearcher searcher = 
                new System.Management.ManagementObjectSearcher(query)
            ;

            System.Management.ManagementObjectCollection processList = searcher.Get();

            foreach (System.Management.ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = System.Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));

                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                } // End if (returnVal == 0) 

            } // Next obj 

            return "NO OWNER";
        } // End Function GetProcessOwner 








        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(System.IntPtr ProcessHandle, uint DesiredAccess, out System.IntPtr TokenHandle);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]

        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool CloseHandle(System.IntPtr hObject);

        private static string GetProcessUser(System.Diagnostics.Process process)
        {
            System.IntPtr processHandle = System.IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);

                System.Security.Principal.WindowsIdentity wi = 
                    new System.Security.Principal.WindowsIdentity(processHandle);

                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            } // End Try 
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            } // End Catch 
            finally
            {
                if (processHandle != System.IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                } // End if (processHandle != System.IntPtr.Zero) 
            } // End Finally 

        } // End Function GetProcessUser 


        public static void AddImpersonatedToGroup()
        {
            try
            {
                using (System.DirectoryServices.AccountManagement.PrincipalContext pcLocal =
                    new System.DirectoryServices.AccountManagement.PrincipalContext(
                        System.DirectoryServices.AccountManagement.ContextType.Machine
                        )
                )
                {
                    System.DirectoryServices.AccountManagement.GroupPrincipal group =
                        System.DirectoryServices.AccountManagement.GroupPrincipal
                        .FindByIdentity(pcLocal, "Administratoren")
                    ;

                    System.Console.WriteLine(group.DistinguishedName);

                    using (System.DirectoryServices.AccountManagement.PrincipalContext pcDomain 
                        = new System.DirectoryServices.AccountManagement.PrincipalContext(
                        System.DirectoryServices.AccountManagement.ContextType.Domain, "COMPANY") // "AAA"
                    ) 
                    {
                        group.Members.Add(pcDomain, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, "firstname.lastname");
                        group.Save();
                    };

                };
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

        } // End Function AddImpersonatedToGroup 



        /// <summary>
        /// Adds the supplied user into the (local) group
        /// </summary>
        /// <param name="userName">the full username (including domain)</param>
        /// <param name="groupName">the name of the group</param>
        /// <returns>true on success; 
        /// false if the group does not exist, or if the user is already in the group, or if the user cannont be added to the group</returns>
        public static bool AddUserToLocalGroup(string userName, string groupName)
        {
            System.DirectoryServices.DirectoryEntry userGroup = null;

            try
            {
                string groupPath = string.Format(System.Globalization.CultureInfo.CurrentUICulture
                    , "WinNT://{0}/{1},group", System.Environment.MachineName, groupName
                );

                userGroup = new System.DirectoryServices.DirectoryEntry(groupPath);

                if ((null == userGroup) 
                    || (true == string.IsNullOrEmpty(userGroup.SchemaClassName)) 
                    || (0 != string.Compare(userGroup.SchemaClassName, "group", true
                                , System.Globalization.CultureInfo.CurrentUICulture)))
                    return false;

                string userPath = string.Format(System.Globalization.CultureInfo.CurrentUICulture
                    , "WinNT://{0},user", userName
                );

                userGroup.Invoke("Add", new object[] { userPath });
                userGroup.CommitChanges();

                return true;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (null != userGroup) userGroup.Dispose();
            }

        } // End Function AddUserToLocalGroup 


        public static string GetProcessInfoByPID(int PID, out string User, out string Domain)
        {
            User = string.Empty;
            Domain = string.Empty;
            string OwnerSID = string.Empty;
            string processname = string.Empty;
            try
            {
                System.Management.ObjectQuery sq = new System.Management.ObjectQuery(@"
SELECT * FROM Win32_Process WHERE ProcessID = '" + PID + @"' 
");

                System.Management.ManagementObjectSearcher searcher = 
                    new System.Management.ManagementObjectSearcher(sq);

                if (searcher.Get().Count == 0)
                    return OwnerSID;

                foreach (System.Management.ManagementObject oReturn in searcher.Get())
                {
                    // Invoke the method and populate the o array with the user name and domain
                    string[] o = new string[2];
                    oReturn.InvokeMethod("GetOwner", (object[])o);

                    // int pid = (int)oReturn["ProcessID"];
                    processname = (string)oReturn["Name"];

                    // dr[2] = oReturn["Description"];
                    User = o[0];

                    if (User == null)
                        User = string.Empty;

                    Domain = o[1];

                    if (Domain == null)
                        Domain = string.Empty;

                    string[] sid = new string[1];
                    oReturn.InvokeMethod("GetOwnerSid", (object[])sid);
                    OwnerSID = sid[0];
                    return OwnerSID;
                } // Next oReturn 

            } // End Try 
            catch
            {
                return OwnerSID;
            } // End Catch 

            return OwnerSID;
        } // End Function GetProcessInfoByPID 


        public static void Test()
        {

            using (System.Security.Principal.WindowsIdentity wi =
                // new System.Security.Principal.WindowsIdentity(InvalidHandle))
                //System.Security.Principal.WindowsIdentity.GetAnonymous())
                GetWindowsIdentity(@"domain\username")
            )
            {
                using (System.Security.Principal.WindowsImpersonationContext context = wi.Impersonate())
                {
                    // string n; string d;
                    // string la = GetProcessInfoByPID(System.Diagnostics.Process.GetCurrentProcess().Id, out n, out d);
                    
                    System.Console.WriteLine(IdCache.MyId);

                    string foo = GetProcessUser(System.Diagnostics.Process.GetCurrentProcess());
                    System.Console.WriteLine(foo);

                    // string bar = GetProcessOwner(System.Diagnostics.Process.GetCurrentProcess().Id);
                    // System.Console.WriteLine(bar);

                    System.Console.WriteLine(wi.Name);
                    System.Console.WriteLine(System.Environment.UserName);

                    System.Security.Principal.WindowsIdentity wai = System.Security.Principal
                        .WindowsIdentity.GetCurrent(true);

                    System.Console.WriteLine(wai.User);
                } // End Using context 

            } // End Using WindowsIdentity 

        } // End Sub Test 


    } // End Class ProcessIdentityFinder 


} // End Namspace Fw4csReportExecution.Impersonation 
