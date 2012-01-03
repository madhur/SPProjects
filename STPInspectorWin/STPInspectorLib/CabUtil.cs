using System;
using System.IO;
using CAB = Mischel.CabDotNet;

namespace STPInspectorLib
{
    /// <summary>
    /// Contains standard functions related to Cabinet file processing
    /// </summary>
    public class CabUtil
    {
        private string destinationDirectory = string.Empty;
        private bool _manifestFound;
        
        /// <summary>
        /// Checks weather the given file is the Cabinet file or not
        /// </summary>
        /// <param name="path">Path to cabinet file</param>
        /// <returns>TRUE if its cabinet</returns>
        public static bool IsCabinetFile(string path)
        {
            using (CAB.CabDecompressor deComp = new CAB.CabDecompressor())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    CAB.FdiCabinetInfo cabInfo = new CAB.FdiCabinetInfo();

                    if (deComp.IsCabinetFile(fs, cabInfo))
                    {

                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Extracts the Manifest.xml file from the Cabinet package
        /// </summary>
        /// <param name="cabinetFullPath">Full path to the cabinet</param>
        /// <param name="destPath">Destination of Manifest.xml</param>
        /// <returns></returns>
        public string ExtractManifest(string cabinetFullPath,string destPath)
        {
            destinationDirectory=destPath;
            using (CAB.CabDecompressor decomp = new CAB.CabDecompressor())
            {

                // setup event handlers
                // decomp.NotifyCabinetInfo += new CAB.NotifyEventHandler(decomp_NotifyCabinetInfo);
                 decomp.NotifyCloseFile += decomp_NotifyCloseFile;
                decomp.NotifyCopyFile += decomp_NotifyCopyFile;
                //decomp.NotifyEnumerate += new CAB.NotifyEventHandler(decomp_NotifyEnumerate);
                //decomp.NotifyNextCabinet += new CAB.NotifyEventHandler(decomp_NotifyNextCabinet);
                //decomp.NotifyPartialFile += new CAB.NotifyEventHandler(decomp_NotifyPartialFile);

                CAB.FdiCabinetInfo cabInfo = new CAB.FdiCabinetInfo();

                using (FileStream fs = new FileStream(cabinetFullPath, FileMode.Open, FileAccess.Read))
                {
                    if (!decomp.IsCabinetFile(fs, cabInfo))
                    {
                        throw new NotValidCabinetException();
                    }
                }

                // extract path and filename
                //string cabinetPath = Path.GetDirectoryName(cabinetFullPath);
               // if (cabinetPath != string.Empty)
                    //cabinetPath = cabinetPath + Path.DirectorySeparatorChar;

                // let's copy some files!
                if (!decomp.ExtractFiles(cabinetFullPath))
                {
                    throw new IOException();

                }
                if (!_manifestFound)
                {
                    throw new ManifestNotFoundException();
                }
                
                return destinationDirectory + Constants.manifestFileName;
            }



        }

        /// <summary>
        /// Closes the Manifest.xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decomp_NotifyCloseFile(object sender, CAB.NotifyEventArgs e)
        {
            string fname = destinationDirectory + e.args.str1;

            // TODO:  Most of this function probably should be encapsulated in the parent object.
            int err = 0;
            CAB.CabIO.FileClose(e.args.FileHandle, ref err, null);

            try
            {
                // set file date and time
                DateTime fileDateTime = CAB.FCntl.DateTimeFromDosDateTime(e.args.FileDate, e.args.FileTime);
                File.SetCreationTime(fname, fileDateTime);
                File.SetLastWriteTime(fname, fileDateTime);

                // get relevant file attributes and set attributes on the file
                FileAttributes fa = CAB.FCntl.FileAttributesFromFAttrs(e.args.FileAttributes);
                File.SetAttributes(fname, fa);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            e.Result = 1;
        }

        /// <summary>
        /// Copies the manifest.xml to the destination directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decomp_NotifyCopyFile(object sender, CAB.NotifyEventArgs e)
        {
            if(string.Compare(Constants.manifestFileName,e.args.str1,true).Equals(0))
            {
                int err = 0;
                
                string destFilename = destinationDirectory + e.args.str1;
                IntPtr fHandle = CAB.CabIO.FileOpen(destFilename, FileAccess.Write,
                    FileShare.ReadWrite, FileMode.Create, ref err);
                e.Result = (int)fHandle;
                _manifestFound = true;

            }
            else
            {
                e.Result = 0;
            }
        }

        /// <summary>
        /// Deletes the manifest.xml file once the information has been extracted
        /// </summary>
        /// <param name="path">Path to manifest file</param>
        public static void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {

            }
        }


    }
}
