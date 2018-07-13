using System;
using System.Net;

namespace FtpConnection
{
    public class FtpConnectionManager
    {
        private string username = "";
        private string password = "";
        private string hostname = "";

        private FtpWebRequest getNewRequest(string path)
        {
            var request = (FtpWebRequest) WebRequest.Create("ftp://" + hostname + "/" + path);
            request.Credentials = new NetworkCredential(username, password);

            return request;
        }

        public FtpConnectionManager(string user,string pass,string host)
        {
            username = user;
            password = pass;
            hostname = host;
        }

        /*
        Using the credentials, run a test request to confirm credentials are correct
        */
        public bool Validate() {
            return true;
        }

        /*
        This class takes in the filename and path to the file as well as the remote path
        in case it is not root and returns true on success and false on failure
        */
        public bool Upload(string filename,string localpath,string remotepath)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /*
        This class takes in the filename to download and the remote path to get it from as
        well as the local path to download it to and returns true on success and false on failure
         */
        public bool Download(string filename, string remotepath, string localpath)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /*
        This class takes in the filename to change the file to, the old name and the path and returns
        true on success and false on failure
         */
        public bool Rename(string newFilename, string oldFilename, string remotepath)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /*
        This class takes in the filename of the file to delete and the path and
        returns true on success and false on failure
         */
        public bool Delete(string filename, string remotepath)
        {
            try
            {
                var request = getNewRequest(remotepath + filename);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /*
        I don't know if this should return a bool or if it will need to return something else feel free to change if you take the ticket for it
         */
        public bool listFiles()
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /*
        I don't know what inputs will be needed for this so feel free to add the ones you need if you take the ticket
        */
        public bool ChangePermissions(string filename)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
