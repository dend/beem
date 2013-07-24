using System.IO;
using System.IO.IsolatedStorage;

namespace Beem.Utility
{
    public static class RecordManager
    {
        public static void GetRecords()
        {

                IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

            if (file.DirectoryExists("/Music"))
            {
                string[] fileNames = file.GetFileNames("/Music/*");
                Binder.Instance.Recorded.Clear();

                if (fileNames.Length != 0)
                {
                    foreach (string name in fileNames)
                    {
                        Binder.Instance.Recorded.Add(name);
                    }
                }
            }
        }

        public static byte[] GetRecordByteArray(string name)
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

            MemoryStream stream = new MemoryStream();
            using (IsolatedStorageFileStream fStream = new IsolatedStorageFileStream("Music/" + name,
                FileMode.Open, file))
            {
                byte[] readBuffer = new byte[4096];
                int bytesRead = 0;

                while ((bytesRead = fStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    stream.Write(readBuffer, 0, bytesRead);
                }
            }

            return stream.ToArray();
        }
    }
}
