using System.Collections.Generic;
using System.IO;

namespace Code.Helpers
{
    public class ArgumentParser
    {
        public string FilePath { get; private set; }
        public IList<string> ErrorMessages { get; }
        
        public ArgumentParser(IReadOnlyList<string> args)
        {
            ErrorMessages = new List<string>();

            if (args.Count == 0)
            {
                ErrorMessages.Add("No arguments have been passed in");
                return;
            }
            
            for (var index = 0; index < args.Count; index++)
            {
                if (args[index] != "--f")
                {
                    continue;
                }

                //index will be ready to jump into next pair of arguments
                SetFilePathAndIndex(args, ref index);
            }

            ValidateFilePath();
        }

        private void SetFilePathAndIndex(IReadOnlyList<string> args, ref int index)
        {
            var indexWithinRange = ++index < args.Count;
            if (indexWithinRange)
            {
                FilePath = args[index];
            }
        }

        private void ValidateFilePath()
        {
            if (FilePath == null)
            {
                ErrorMessages.Add("You need to specify a file path");
                return;
            }

            if (!File.Exists(FilePath))
            {
                FilePath = null;
                ErrorMessages.Add("You need to provide an existing file path");
                return;
            }

            var fileSize = new FileInfo(FilePath).Length;
            if (fileSize <= 102400)
            {
                return;
            }
            
            FilePath = null;
            ErrorMessages.Add("The file exceeds the allowed limit of 100kB");
        }
    }
}