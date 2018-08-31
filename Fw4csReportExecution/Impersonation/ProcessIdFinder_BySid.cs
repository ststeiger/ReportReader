
namespace Fw4csReportExecution
{


    class ProcessIdFinder_BySid
    {


        public const int TOKEN_QUERY = 0X00000008;

        const int ERROR_NO_MORE_ITEMS = 259;

        enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId
        }

        [System.Runtime.InteropServices.DllImport("advapi32")]
        static extern bool OpenProcessToken(
            System.IntPtr ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref System.IntPtr TokenHandle // handle to open access token
        );

        [System.Runtime.InteropServices.DllImport("kernel32")]
        static extern System.IntPtr GetCurrentProcess();

        [System.Runtime.InteropServices.DllImport("advapi32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool GetTokenInformation(
            System.IntPtr hToken,
            TOKEN_INFORMATION_CLASS tokenInfoClass,
            System.IntPtr TokenInformation,
            int tokeInfoLength,
            ref int reqLength
        );

        [System.Runtime.InteropServices.DllImport("kernel32")]
        static extern bool CloseHandle(System.IntPtr handle);

        [System.Runtime.InteropServices.DllImport("advapi32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool ConvertSidToStringSid(
            System.IntPtr pSID,
            [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out
            , System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
            ref string pStringSid
        );

        [System.Runtime.InteropServices.DllImport("advapi32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool ConvertStringSidToSid(
            [System.Runtime.InteropServices.In, System.Runtime.InteropServices
                .MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                string pStringSid,
            ref System.IntPtr pSID
        );

        /// <span class="code-SummaryComment"><summary></span>
        /// Collect User Info
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="pToken">Process Handle</param></span>
        public static bool DumpUserInfo(System.IntPtr pToken, out System.IntPtr SID)
        {
            int Access = TOKEN_QUERY;
            System.IntPtr procToken = System.IntPtr.Zero;
            bool ret = false;
            SID = System.IntPtr.Zero;
            try
            {
                if (OpenProcessToken(pToken, Access, ref procToken))
                {
                    ret = ProcessTokenToSid(procToken, out SID);
                    CloseHandle(procToken);
                }
                return ret;
            }
            catch (System.Exception err)
            {
                return false;
            }
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct _SID_AND_ATTRIBUTES
        {
            public System.IntPtr Sid;
            public int Attributes;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct TOKEN_USER
        {
            public _SID_AND_ATTRIBUTES User;
        }


        private static bool ProcessTokenToSid(System.IntPtr token, out System.IntPtr SID)
        {
            TOKEN_USER tokUser;
            const int bufLength = 256;
            System.IntPtr tu = System.Runtime.InteropServices.Marshal.AllocHGlobal(bufLength);
            bool ret = false;
            SID = System.IntPtr.Zero;
            try
            {
                int cb = bufLength;
                ret = GetTokenInformation(token,
                        TOKEN_INFORMATION_CLASS.TokenUser, tu, cb, ref cb);
                if (ret)
                {
                    tokUser = (TOKEN_USER)System.Runtime.InteropServices.
                        Marshal.PtrToStructure(tu, typeof(TOKEN_USER));

                    SID = tokUser.User.Sid;
                }
                return ret;
            }
            catch (System.Exception err)
            {
                return false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(tu);
            }
        }

        public static string ExGetProcessInfoByPID
            (int PID, out string SID)//, out string OwnerSID)
        {
            System.IntPtr _SID = System.IntPtr.Zero;
            SID = string.Empty;
            try
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(PID);
                if (DumpUserInfo(process.Handle, out _SID))
                {
                    ConvertSidToStringSid(_SID, ref SID);
                }
                return process.ProcessName;
            }
            catch
            {
                return "Unknown";
            }
        }

    }


}
