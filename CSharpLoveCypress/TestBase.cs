using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace CSharpLoveCypress
{
    public class TestBase
    {
        private readonly ITestOutputHelper outputHelper;

        public TestBase(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        protected void RunCypressTest(string cypressSpecFilePath)
        {
            var testAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(testAssemblyPath))
            {
                throw new Exception("Cannot find test assembly path!");
            }

            var reactAppPath = Path.GetFullPath(Path.Combine(testAssemblyPath, @"..\..\..\..\client"));

            var commandText = $"npx cypress run --spec \"{cypressSpecFilePath}\" & exit";

            var cmd = GetCmd(reactAppPath);

            var cypressProcessOutput = ExecuteCommand(cmd, commandText);

            outputHelper.WriteLine($@"Cypress spec {cypressSpecFilePath} run output:");
            outputHelper.WriteLine(cypressProcessOutput);

            Assert.Equal(0, cmd.ExitCode);
        }

        private Process GetCmd(string workingDirectory)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                RedirectStandardOutput = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            return process;
        }

        private string ExecuteCommand(Process cmd, string commandText)
        {
            outputHelper.WriteLine("Executing command: " + commandText);

            cmd.StartInfo.Arguments = "/C " + commandText;
            cmd.Start();

            var cypressProcessOutput = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();

            return cypressProcessOutput;
        }
    }
}