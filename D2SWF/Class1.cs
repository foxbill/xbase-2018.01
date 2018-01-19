using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2SWF
{
    public class Class1
    {
        public static void ConvertPdfToSwf(String styFileName, String[] dataFileNames, String outputFileFullName)
        {
            try
            {
                String flashPrinter = String.Concat(AppDomain.CurrentDomain.BaseDirectory, "FlashPrinter.exe");

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(flashPrinter);

                if (String.IsNullOrEmpty(outputFileFullName))
                { return; }
                Int32 intLastDot = outputFileFullName.LastIndexOf(".");
                //*********Temp Programming****************************************
                Int32 intLast = outputFileFullName.LastIndexOf("\\");
                String path = outputFileFullName.Substring(0, intLast);
                String tempFileName = path + "\\PdfToSwf20080923.pdf";
                //*****************************************************************
                String swfFileName = String.Concat(path, "\\PdfToSwf20080923.swf");
                startInfo.Arguments = String.Concat(tempFileName, " -o ", swfFileName);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo = startInfo;
                Boolean isStart = process.Start();
                process.WaitForExit();
                process.Close();     
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
