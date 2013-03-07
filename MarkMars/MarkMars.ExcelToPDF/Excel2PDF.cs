using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.ExcelToPDF
{
    public class Excel2PDF
    {
        public static bool XLSConvertToPDF(string sourceFile, string targetFile)
        {
            bool result = false;

            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;// Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Application application = null;
            Microsoft.Office.Interop.Excel.Workbook workBook = null;

            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();
                object target = targetFile;
                object type = targetType;
                workBook = application.Workbooks.Open(sourceFile
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing
                    , missing);

                if (workBook != null)
                {
                    workBook.ExportAsFixedFormat(targetType
                        , target
                        , Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard
                        , true
                        , true
                        , 1
                        , 3
                        , false
                        , missing);
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }

                if (application != null)
                {
                    application.Quit();
                    application = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return result;
        }
    }
}
