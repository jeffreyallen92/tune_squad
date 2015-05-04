﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using LIFES.Schedule;

namespace LIFES.FileIO
{
    /*
     * Class Name: FileOut.cs
     * Created By: Scott Smoke
     * Date: 3/24/2015
     * Modified by: Jordan Beck
     * 
     * Description: This class will output the created final exam in
     * the requested format.
     */
    class FileOut
    {
        private string filename;
        /*
         * Method: FileOut
         * Parameters: String filename
         * Output: N/A
         * Created By: Scott Smoke
         * Date: 3/24/2015
         * Modified By: Scott Smoke
         * 
         * Description: This constructs an object. 
         */
        public FileOut(string file)
        {
            filename = file;
        }

        /*
         * Method: WriteToPDF
         * Parameters: String filename
         * Output: Saved file of the pdf format
         * Created By: Scott Smoke
         * Date: 3/24/2015
         * Modified By: Riley Smith
         * 
         * Description: This method will output the final exam 
         * schedule to a pdf. This method uses the open source 
         * PDFSharp library by MigraDoc Foundation more information 
         * here: http://www.pdfsharp.net/. This will read the data 
         * structure returned from the schedule function and insert 
         * the data into a pdf.
         * 
         * Sources: http://csharp.net-informations.com/file/create-pdf.htm
         *          http://www.pdfsharp.net/wiki/TextLayout-sample.ashx?AspxAutoDetectCookieSupport=1
         */
        public void WriteToPDF()
        {
            //to do use pdfsharp
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Times New Roman", 12);
            XTextFormatter tf = new XTextFormatter(graph);
            //adding data to pdf

            int yCord = 0;

            tf.DrawString(Globals.semester + " " + Globals.year, font,
                XBrushes.Black, new XRect(40, yCord, pdfPage.Width.Point,
                    pdfPage.Height.Point), XStringFormats.TopLeft);

            yCord += 12;

            tf.DrawString(Globals.totalEnrollemntsFileName, font,
                XBrushes.Black, new XRect(40, yCord, pdfPage.Width.Point,
                    pdfPage.Height.Point), XStringFormats.TopLeft);

            yCord += 12;

            tf.DrawString(Globals.timeConstraints.ToString(), font,
                XBrushes.Black, new XRect(40, yCord, pdfPage.Width.Point,
                    pdfPage.Height.Point), XStringFormats.TopLeft);

            yCord += 12 * 5;

            // Blank line.
            tf.DrawString(" ", font,
                XBrushes.Black, new XRect(40, yCord, pdfPage.Width.Point,
                    pdfPage.Height.Point), XStringFormats.TopLeft);

            yCord += 12;

            //add schedule

            foreach (FinalExamDay ele in Globals.examWeek)
            {
                tf.DrawString("Day Class Times Exam Time", font,
                XBrushes.Black, new XRect(40, yCord, pdfPage.Width.Point,
                    pdfPage.Height.Point), XStringFormats.TopLeft);

                yCord += 12;

                string header = "";
                header += ele.GetDay();

                foreach (FinalExam exam in ele.GetExams())
                {
                    string classTime = "";
                    CompressedClassTime compressedTime = exam.GetCompressedClass();

                    classTime += compressedTime.GetClassTimes().
                        First().getDayOfTheWeek()
                        + " ";
                    classTime += MilitaryToDateTime(compressedTime.
                        GetClassTimes().First().getClassStartTime()).
                        ToString("hh:mm tt")
                        + "-";
                    classTime += MilitaryToDateTime(compressedTime.
                        GetClassTimes().First().getClassEndTime()).
                        ToString("hh:mm tt");

                    header += " " + classTime + " ";

                    string examTimes = "";
                    examTimes += MilitaryToDateTime(exam.GetStartTime()).
                        ToString("hh:mm tt")
                        + "-" + MilitaryToDateTime(exam.GetEndTime()).
                        ToString("hh:mm tt");

                    header += " " + examTimes;

                    tf.DrawString(header, font,
                        XBrushes.Black, new XRect(40, yCord, pdfPage.Width.Point,
                        pdfPage.Height.Point), XStringFormats.TopLeft);

                    header = "";
                    yCord += 12;

                    foreach (ClassTime time in compressedTime.
                        GetClassTimes())
                    {
                        if (time != compressedTime.GetClassTimes().First())
                        {
                            string classTimes = "";
                            classTimes += time.getDayOfTheWeek() + " ";
                            classTimes += MilitaryToDateTime(time.
                                getClassStartTime()).
                                ToString("hh:mm tt") + "-";
                            classTimes += MilitaryToDateTime(time.
                                getClassEndTime()).
                                ToString("hh:mm tt");

                      //      file.Write("\t\t" + classTimes + "\n");
                        }
                    }

                    yCord += 12;
    
                }

                graph.Dispose();
                pdfPage = pdf.AddPage();
                graph = XGraphics.FromPdfPage(pdfPage);
                font = new XFont("Times New Roman", 12);
                tf = new XTextFormatter(graph);

                yCord = 0;
            }
 
            pdf.Save(filename);
}

        /*
         * Method: WriteToCSV
         * Parameters: N/A
         * Output: Saved file in the CSV format
         * Created By: Scott Smoke
         * Date: 3/24/2015
         * Modified By: Joshua Ford
         * 
         * Description: This will write the data that is returned from the 
         * scheduler to a file in the CSV format.
         */
        public void WriteToCSV()
        {
            if (filename !="")
            {
                System.IO.StreamWriter file = 
                    new System.IO.StreamWriter(filename);
                //code to write goes here
                file.WriteLine(Globals.semester + " " + Globals.year);
                file.WriteLine(Globals.totalEnrollemntsFileName);
                file.WriteLine(Globals.timeConstraints.ToString());
                if (Globals.adminApproved)
                {
                    file.WriteLine("0");
                }
                else
                {
                    file.WriteLine("1");
                }
                
				//place exam schedule
                foreach (FinalExamDay ele in Globals.examWeek)
                {
                    file.WriteLine("Day,Class Times,Exam Time");
                    file.Write(ele.GetDay() + ",");
              
                    foreach (FinalExam exam in ele.GetExams())
                    {
                        string classTime = "";
                        CompressedClassTime compressedTime = exam.GetCompressedClass();

                        classTime +=  compressedTime.GetClassTimes().

							First().GetDayOfTheWeek() + ",";
                        classTime += MilitaryToDateTime(compressedTime.
                            GetClassTimes().First().GetClassStartTime()).
                            ToString("hh:mm tt")
                            + "-";
                            classTime += MilitaryToDateTime(compressedTime.
                                GetClassTimes().First().GetClassEndTime()).
                                ToString("hh:mm tt");

                        file.Write(classTime + ",");

                        string examTimes = "";
                        examTimes += MilitaryToDateTime(exam.GetStartTime()).
                            ToString("hh:mm tt")
                            + "-" + MilitaryToDateTime(exam.GetEndTime()).
                            ToString("hh:mm tt");

                        file.Write(examTimes + "\n");

                        foreach (ClassTime time in compressedTime.
                            GetClassTimes())
                        {
                            if (time != compressedTime.GetClassTimes().First())
                            {
                                string classTimes = "";
                                classTimes += time.GetDayOfTheWeek() + ",";
                                classTimes += MilitaryToDateTime(time.
                                    GetClassStartTime()).
                                    ToString("hh:mm tt") + "-";
                                classTimes += MilitaryToDateTime(time.
                                    GetClassEndTime()).
                                    ToString("hh:mm tt");

                                file.Write(classTimes + "\n");
                            }
                        }
                        file.Write("\n");
                    }
                }
                file.Close();
            } 
        }

        /*
         * Method: WriteToText
         * Parameters: N/A
         * Output: A saved file in plain text
         * Created By: Scott Smoke
         * Date: 3/24/2015
         * Modified By: Riley Smith
         * 
         * Description: This will write the data that is returned from 
         * the scheduler to a plain text file.
         */
        public void WriteToText() 
        {
            if (filename != "")
            {
                System.IO.StreamWriter file = 
                    new System.IO.StreamWriter(filename);
                //code to write goes here
                file.WriteLine(Globals.semester + " " +Globals.year);
                file.WriteLine(Globals.totalEnrollemntsFileName);
                file.WriteLine(Globals.timeConstraints.ToString());
                if (Globals.adminApproved)
                {
                    file.WriteLine("0");
                }
                else
                {
                    file.WriteLine("1");
                }
                file.WriteLine("\n");
                //place exam schedule
                foreach (FinalExamDay ele in Globals.examWeek)
                {
                    file.WriteLine("Day\t Class Times\t\t\t\t\t\t Exam Time");
                    file.Write(ele.GetDay());
              
                    foreach (FinalExam exam in ele.GetExams())
                    {
                        string classTime = "";
                        CompressedClassTime compressedTime = exam.GetCompressedClass();

                        classTime +=  compressedTime.GetClassTimes().
                            First().GetDayOfTheWeek()
                            + " ";
                        classTime += MilitaryToDateTime(compressedTime.
                            GetClassTimes().First().GetClassStartTime()).
                            ToString("hh:mm tt")
                            + "-";
                            classTime += MilitaryToDateTime(compressedTime.
                                GetClassTimes().First().GetClassEndTime()).
                                ToString("hh:mm tt");

                        file.Write("\t\t" + classTime + "\t\t\t");

                        string examTimes = "";
                        examTimes += MilitaryToDateTime(exam.GetStartTime()).
                            ToString("hh:mm tt")
                            + "-" + MilitaryToDateTime(exam.GetEndTime()).
                            ToString("hh:mm tt");

                        file.Write("\t" + examTimes + "\n");

                        foreach (ClassTime time in compressedTime.
                            GetClassTimes())
                        {
                            if (time != compressedTime.GetClassTimes().First())
                            {
                                string classTimes = "";
                                classTimes += time.GetDayOfTheWeek() + " ";
                                classTimes += MilitaryToDateTime(time.
                                    GetClassStartTime()).
                                    ToString("hh:mm tt") + "-";
                                classTimes += MilitaryToDateTime(time.
                                    GetClassEndTime()).
                                    ToString("hh:mm tt");

                                file.Write("\t\t" + classTimes + "\n");
                            }
                        }

                        file.Write("\n");
                    }
                }

                file.Close();
            }  
        }

        /*
        * Method: MilitaryToDateTime
        * Parameters: int (Military Time)
        * Output: DateTime
        * Created By: Riley Smith
        * Date: 5/3/2015
        * Modified By: Riley Smith
        * 
        * Description: Converts a MilitaryTime int to a standard DateTime.
        * 
        * Source:
        * http://forums.asp.net/t/1503263.aspx?How+to+convert+integer+representing+military+time+into+DateTime+object
        */
        public static DateTime MilitaryToDateTime(int time)
        {
            int Hours = time / 100;
            int Minutes = time - Hours * 100;
            DateTime Result = DateTime.MinValue;


            Result = Result.AddHours(Hours);
            Result = Result.AddMinutes(Minutes);


            return Result;
        }

    }
}
