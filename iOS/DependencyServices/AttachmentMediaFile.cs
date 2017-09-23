using System;
using System.Linq;
using UIKit;

namespace AudioKetab.iOS
{
    public class AttachmentMediaFile
    {
        private readonly Func<byte[]> _bytesGetter;

        public string LocalPath { get; private set; }

        public string Name { get; private set; }

        public AttachmentMediaFileType Type { get; private set; }

        public AttachmentMediaFile(string localPath, AttachmentMediaFileType type, byte[] bytesGetter, string name = null)
        {
            LocalPath = localPath;
            Type = type;
            _bytesGetter = () =>
            {
                return bytesGetter;
            };

            if (string.IsNullOrEmpty(name))
            {
                Name = FileNameHelper.PrepareName(localPath);
            }
            else
            {
                Name = name;
            }

          
			
		}

        public byte[] GetBytes()
        {
            return _bytesGetter();
        }
    }

    public enum AttachmentMediaFileType
    {
        Photo = 0,
        Audio = 1,
        Doc = 2,
        Video = 3,
    }

    public static class FileNameHelper
    {
        private const string Prefix = "IMG";

        public static string PrepareName(string localPath)
        {
            var name = string.Empty;

            if (!string.IsNullOrEmpty(localPath))
            {
                name = localPath.Split('/').Last();
            }

            return name;
        }

        public static string GenerateUniqueFileName(Extension extension)
        {
            var format = ".jpg";

            var fileName = string.Concat(Prefix, '_', DateTime.UtcNow.Ticks, format);

            return fileName;
        }

        public enum Extension
        {
            JPG
        }
    }
}