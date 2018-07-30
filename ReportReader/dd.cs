
// https://stackoverflow.com/questions/7092331/windows-c-sharp-implementation-of-linux-dd-command
// https://www.c-sharpcorner.com/UploadFile/53fd7d/create-hard-disk-partition-using-C-Sharp884/
// https://ndswanson.wordpress.com/2014/08/12/using-diskpart-with-c/
namespace ReportReader
{

    // umount /dev/sdb<X>
    // dd bs=4M if=/path/to/archlinux.iso of=/dev/sdx status=progress oflag=sync
    // sudo dd if=NameOfImageToWrite.img of=/dev/rdiskNUMBER bs=1m
    // dd if=image.iso of=/dev/sdb bs=4M
    // https://stackoverflow.com/questions/6161823/dd-how-to-calculate-optimal-blocksize
    class dd
    {

        private const int FILE_ATTRIBUTE_SYSTEM = 0x4;
        private const int FILE_FLAG_SEQUENTIAL_SCAN = 0x8;

        [System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(string fileName
            , [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] System.IO.FileAccess fileAccess
            , [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] System.IO.FileShare fileShare
            , System.IntPtr securityAttributes
            , [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] System.IO.FileMode creationDisposition
            , int flags
            , System.IntPtr template
        );

        // string inputData = "TempFile.bin"; 
        // string target = @"\\.\E:"
        public static void CopyBinaryData(string inputData, string target)
        {
            using (Microsoft.Win32.SafeHandles.SafeFileHandle device =
                  CreateFile(target
                , System.IO.FileAccess.Read
                , System.IO.FileShare.Write | System.IO.FileShare.Read | System.IO.FileShare.Delete
                , System.IntPtr.Zero, System.IO.FileMode.Open
                , FILE_ATTRIBUTE_SYSTEM | FILE_FLAG_SEQUENTIAL_SCAN
                , System.IntPtr.Zero)
            )
            {
                if (device.IsInvalid)
                {
                    throw new System.IO.IOException("Unable to access drive. Win32 Error Code " + System.Runtime.InteropServices.Marshal.GetLastWin32Error());
                } // End if (device.IsInvalid) 

                /*
                using (System.IO.FileStream dest = System.IO.File.Open(inputData, System.IO.FileMode.Create))
                {
                    using (System.IO.FileStream src = new System.IO.FileStream(device, System.IO.FileAccess.Read))
                    {
                        src.CopyTo(dest);
                    } // End Using src 

                } // End Using dest 
                */

                using (System.IO.FileStream dest = System.IO.File.Open(inputData, System.IO.FileMode.Create))
                {
                    using (System.IO.FileStream src = new System.IO.FileStream(device, System.IO.FileAccess.Read))
                    {

                        byte[] buffer = new byte[32768]; // BlockSize/buffer-size...
                        int read;
                        while ((read = src.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            dest.Write(buffer, 0, read);
                        } // Whend

                    } // End Using src 

                } // End Using dest 

            } // End Using device 

        } // End Sub CopyBinaryData 


        // https://www.codeproject.com/Questions/1017377/how-to-get-gpt-partition-information-in-csharp
        public static void DriveInfos()
        {
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                System.Console.WriteLine("Name:{0}", drive.Name);
                if (drive.IsReady)
                {
                    System.Console.WriteLine("Size:{0}", drive.TotalSize);
                    System.Console.WriteLine("AvailableFreeSpace:{0}", drive.AvailableFreeSpace);
                    System.Console.WriteLine("DriveFormat:{0}", drive.DriveFormat);
                    System.Console.WriteLine("DriveType:{0}", drive.DriveType);
                    System.Console.WriteLine("TotalFreeSpace:{0}", drive.TotalFreeSpace);
                    System.Console.WriteLine("VolumeLabel:{0}", drive.VolumeLabel);
                } // End if (drive.IsReady) 

                System.Console.ReadLine();
            } // next drive 

        } // End Sub DriveInfos 


        public static void Test()
        {
            string phPath = GetPhysicalDevicePath('J');
            System.Console.WriteLine(phPath);
            CopyBinaryData("Image.raw", phPath);
            System.Console.WriteLine("finished");
        } // End Sub Test 


        public static string GetPhysicalDevicePath(char DriveLetter)
        {
            using ( System.Management.ManagementClass devs = new System.Management.ManagementClass(@"Win32_Diskdrive"))
            {
                using (System.Management.ManagementObjectCollection diskDriveInstances = devs.GetInstances())
                { 

                    foreach (System.Management.ManagementObject thisPhysicalDiskDrive in diskDriveInstances)
                    {
                        foreach (System.Management.ManagementObject thisPartition in thisPhysicalDiskDrive.GetRelated("Win32_DiskPartition"))
                        {
                            foreach (System.Management.ManagementBaseObject thisLogicalDisk in thisPartition.GetRelated("Win32_LogicalDisk"))
                            {
                                string DevName = string.Format("{0}", thisLogicalDisk["Name"]);
                                if (DevName[0] == DriveLetter)
                                    return string.Format("{0}", thisPhysicalDiskDrive["DeviceId"]);
                            } // End Using thisLogicalDisk 

                        } // End Using thisPartition 

                    } // Next thisPhysicalDiskDrive 

                } // End Using diskDriveInstances 

            } // End Using devs 

            return "";
        } // End Function GetPhysicalDevicePath 


    } // End Class dd 


} // End Namespace ReportReader 
