using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoadNeuseCSV
{
    class LogFile
    {
        private string fileName;
        public LogFile()
        {
            fileName = "C:\\NeuseLogger\\LogResults.txt";
        }
        public LogFile(string fileName)
        {
            this.fileName = fileName;
        }
        ////Use LogFile to document the test run results
        /// <summary>
        /// The MyLogFile method is used to document details of each test run.
        /// </summary>
        public void MyLogFile(string strCategory, string strMessage)
        {
            // Store the script names and test results in a output text file.
            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
            {
                writer.WriteLine("{0}{1}", strCategory, strMessage);
            }
        }
    }
}
