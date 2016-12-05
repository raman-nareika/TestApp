using System;

namespace Students.Common
{
    public class Application
    {
        public static string ProductName => "Students®";

        public static string GetWindowTitle(string caption)
        {
            if (caption == null) throw new ArgumentNullException(nameof(caption));

            return $@"{ProductName} - {caption}";
        }
    }
}
