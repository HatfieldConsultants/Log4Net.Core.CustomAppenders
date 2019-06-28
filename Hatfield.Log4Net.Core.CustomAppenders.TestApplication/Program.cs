/*
   Copyright 2019 Hatfield Consultants

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
   
   http://www.apache.org/licenses/LICENSE-2.0
   
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Hatfield.Log4Net.Core.CustomAppenders.TestApplication
{
    class Program
    {
        // Make sure that log4net.config in the SMTPAppender project has the option
        // "Copy to output" set to "Copy Always"
        private const string CONFIG_FILE_NAME = "log4net.config";

        private static ILog _log;

        static void Main(string[] args)
        {
            var repositoryAssembly = Assembly.GetEntryAssembly();
            var configFile = new FileInfo(CONFIG_FILE_NAME);
            if (!configFile.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Could not find log4net.config file. Ensure that Copy to output set to Copy Always on the log4net.config file.");
            }
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, configFile);

            _log = LogManager.GetLogger(repositoryAssembly, repositoryAssembly.ManifestModule.Name.Replace(".dll", "").Replace(".", " "));

            _log.Warn("This should not trigger an email with default settings");
            _log.Error("This should trigger an email with default settings");
        }
    }
}
