using NLog;
using Xunit;
using System.IO;

namespace NLog_Legacy_Check
{
    public class LoggingTests
    {
        private const string Error1 = "This error should appear in file one.";
        private const string Warning2 = "While this warning is, definitely.";
        private const string LogFile1 = "file1.log";
        private const string LogFile2 = "file2.log";

        [Fact]
        public void StartTest()
        {
            File.Delete(LogFile1);
            File.Delete(LogFile2);

            var logger1 = LogManager.GetLogger("one");
            var logger2 = LogManager.GetLogger("two");

            logger1.Error(Error1);
            logger1.Debug("This debug info must not surface in that file.");

            logger2.Error("This warning, in turn, is not to be contained in file to as well.");
            logger2.Warn(Warning2);
            logger2.Trace("Tracing is below minimum level for file 2.");

            string content1;
            string content2;
            using (var fileReader = new StreamReader(LogFile1))
            {
                content1 = fileReader.ReadToEnd().Replace("\r", string.Empty).Replace("\n", string.Empty);
            }
            using (var fileReader = new StreamReader(LogFile2))
            {
                content2 = fileReader.ReadToEnd().Replace("\r", string.Empty).Replace("\n", string.Empty);
            }

            Assert.Equal<string>(Error1, content1);
            Assert.Equal<string>(Warning2, content2);
        }
    }
}