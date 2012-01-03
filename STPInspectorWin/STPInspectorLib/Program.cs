// testfdi.cs - test CAB File Decompression Interface (FDI) 
// Jim Mischel <jim@mischel.com>, 11/21/2004
// Updated for .NET 2.0, 07/06/2006

// Define this if you want to output to the console.
#define DO_OUTPUT

using System;
using System.IO;
using Mischel.CabDotNet;

namespace testfdi
{
    class Class1
    {
        private static string destinationDirectory = string.Empty;

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(
                    "TESTFDI - Demonstrates how to use the CabDecompressor interface.\n\n" +
                    "Usage: TESTFDI cabinet dest_dir\n\n" +
                    "Where <cabinet> is the name of a cabinet file, and <dest_dir>\n" +
                    "is the destination for the files extracted\n\n" +
                    "  e.g. testfdi c:\\test1.cab c:\\\n");
                return;
            }

            destinationDirectory = args[1];
            if (destinationDirectory != string.Empty &&
                !destinationDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                destinationDirectory = destinationDirectory + Path.DirectorySeparatorChar;
            }
            /*
                        // code to stress test FDI
                        // this code attempts to decompress every cab file in the Windows distribution
                        DirectoryInfo di = new DirectoryInfo(@"c:\i386");
                        FileInfo[] fi = di.GetFiles();
                        foreach (FileInfo file in fi)
                        {
                            Console.WriteLine("Decompressing {0}.", file.Name);
                            doFdiTest(file.FullName);
                        }
            */

            if (doFdiTest(args[0]))
                Console.WriteLine("TestFdi was successful.");
            else
                Console.WriteLine("TestFdi failed.");
        }

        static bool doFdiTest(string cabinetFullPath)
        {
            using (CabDecompressor decomp = new CabDecompressor())
            {
                
                // setup event handlers
                decomp.NotifyCabinetInfo += new NotifyEventHandler(decomp_NotifyCabinetInfo);
                decomp.NotifyCloseFile += new NotifyEventHandler(decomp_NotifyCloseFile);
                decomp.NotifyCopyFile += new NotifyEventHandler(decomp_NotifyCopyFile);
                decomp.NotifyEnumerate += new NotifyEventHandler(decomp_NotifyEnumerate);
                decomp.NotifyNextCabinet += new NotifyEventHandler(decomp_NotifyNextCabinet);
                decomp.NotifyPartialFile += new NotifyEventHandler(decomp_NotifyPartialFile);

                FdiCabinetInfo cabInfo = new FdiCabinetInfo();

                using (FileStream fs = new FileStream(cabinetFullPath, FileMode.Open, FileAccess.Read))
                {
                    if (!decomp.IsCabinetFile(fs, cabInfo))
                    {
                        Console.WriteLine("File is not a cabinet.");
                        return false;
                    }
#if DO_OUTPUT
                    // The file is a cabinet.  Display some info.
                    Console.WriteLine(
                        "Information on cabinet file '{0}'\n" +
                        "   Total length of cabinet file : {1}\n" +
                        "   Number of folders in cabinet : {2}\n" +
                        "   Number of files in cabinet   : {3}\n" +
                        "   Cabinet set ID               : {4}\n" +
                        "   Cabinet number in set        : {5}\n" +
                        "   RESERVE area in cabinet?     : {6}\n" +
                        "   Chained to prev cabinet?     : {7}\n" +
                        "   Chained to next cabinet?     : {8}\n",
                        cabinetFullPath,
                        cabInfo.Length,
                        cabInfo.FolderCount,
                        cabInfo.FileCount,
                        cabInfo.SetId,
                        cabInfo.CabinetNumber,
                        cabInfo.HasReserve,
                        cabInfo.HasPrev,
                        cabInfo.HasNext);
#endif
                }
                // extract path and filename
                string cabinetName = Path.GetFileName(cabinetFullPath);
                string cabinetPath = Path.GetDirectoryName(cabinetFullPath);
                if (cabinetPath != string.Empty)
                    cabinetPath = cabinetPath + Path.DirectorySeparatorChar;
                
                // let's copy some files!
                if (!decomp.ExtractFiles(cabinetFullPath))
                {
                    Console.WriteLine("FdiCopy failed.  Error code {0}", decomp.ErrorInfo.FdiErrorCode);
                    return false;
                }
              
                return true;

            }
        }

        public static void decomp_NotifyCabinetInfo(object sender, NotifyEventArgs e)
        {
#if DO_OUTPUT
            Console.WriteLine("Cabinet Info" +
                "  Next cabinet     = {0}\n" +
                "  Next disk        = {1}\n" +
                "  Cabinet path     = {2}\n" +
                "  Cabinet set id   = {3}\n" +
                "  Cabinet # in set = {4}",
                e.args.str1, e.args.str2, e.args.CabinetPathName,
                e.args.SetId, e.args.CabinetNumber);
#endif
            e.Result = 0;
        }

        public static void decomp_NotifyPartialFile(object sender, NotifyEventArgs e)
        {
            Console.WriteLine("Partial File\n" +
                "  Name of continued file            = {0}\n" +
                "  Name of cabinet where file starts = {1}\n" +
                "  Name of disk where file starts    = {2}",
                e.args.str1, e.args.str2, e.args.CabinetPathName);
            e.Result = 0;
        }

        public static void decomp_NotifyCopyFile(object sender, NotifyEventArgs e)
        {
            string response = "Y";
#if DO_OUTPUT
            Console.Write("Copy File\n" +
                "  File name in cabinet   = {0}\n" +
                "  Uncompressed file size = {1}\n" +
                " Copy this file? (y/n): ",
                e.args.str1, e.args.Size);
            do
            {
                response = Console.ReadLine().ToUpper();
            } while (response != "Y" && response != "N");
#endif

            if (response == "Y")
            {
                int err = 0;
                    string destFilename = destinationDirectory + e.args.str1;
#if DO_OUTPUT
                Console.WriteLine(destFilename);
#endif
                IntPtr fHandle = CabIO.FileOpen(destFilename, FileAccess.Write,
                    FileShare.ReadWrite, FileMode.Create, ref err);
                e.Result = (int)fHandle;
            }
            else
            {
                e.Result = 0;
            }
        }

        public static void decomp_NotifyCloseFile(object sender, NotifyEventArgs e)
        {
#if DO_OUTPUT
            Console.WriteLine("Close File Info\n" +
                "  File name in cabinet = {0}", e.args.str1);
#endif
            string fname = destinationDirectory + e.args.str1;

            // TODO:  Most of this function probably should be encapsulated in the parent object.
            int err = 0;
            CabIO.FileClose(e.args.FileHandle, ref err, null);

            // set file date and time
            DateTime fileDateTime = FCntl.DateTimeFromDosDateTime(e.args.FileDate, e.args.FileTime);
            File.SetCreationTime(fname, fileDateTime);
            File.SetLastWriteTime(fname, fileDateTime);

            // get relevant file attributes and set attributes on the file
            FileAttributes fa = FCntl.FileAttributesFromFAttrs(e.args.FileAttributes);
            File.SetAttributes(fname, fa);

            e.Result = 1;
        }

        public static void decomp_NotifyNextCabinet(object sender, NotifyEventArgs e)
        {
#if DO_OUTPUT
            Console.WriteLine("Next Cabinet\n" +
                "  Name of next cabinet where file continued = {0}\n" +
                "  Name of next disk where file continued    = {1}\n" +
                "  Cabinet path name                         = {2}\n",
                e.args.str1, e.args.str2, e.args.str3);
#endif
            e.Result = 0;
        }

        public static void decomp_NotifyEnumerate(object sender, NotifyEventArgs e)
        {
        }
    }
}
