using System.Runtime.InteropServices;
using TVManagmentSystem.DbContexti;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace TVManagmentSystem.Services
{
    public class ExcelServices:IexcellService
    {

        private readonly GlobalDbContext _db;

        public ExcelServices(GlobalDbContext db)
        {
            _db = db;
        }

        public void UpdateExcell()
        {
            string excelFilePath = @"E:\excel\All In 1 2023.Aug.09 Updates.xlsm"; // Path to your Excel file
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new System.IO.FileInfo(excelFilePath)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1]; // Assuming the first worksheet

                    int fi = 1; // Initial value for fi
                    int ac = 1; // Initial value for ac

                    // Loop through each row
                    while (fi < 5 && ac <= worksheet.Dimension.End.Row) // Ensure ac doesn't exceed the last row
                    {
                        int res;
                        if (!int.TryParse(worksheet.Cells[ac, 1].Text, out res)) // Check if cell in column 1 is numeric
                        {
                            // Check if a comment already exists for the cell
                            ExcelComment comment = worksheet.Cells[ac, 4].Comment;
                            if (comment != null)
                            {
                                // Update the existing comment
                                comment.Text = fi.ToString();
                            }
                            else
                            {
                                // Add a new comment
                                worksheet.Cells[ac, 4].AddComment(fi.ToString(), "Guga");
                            }
                        }

                        ac++; // Increment ac
                        fi++; // Increment fi
                    }

                    // Save the changes to the Excel file
                    excelPackage.Save();
                    Console.WriteLine("Excel file modified successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }
        }

        public void DisplayComments()
        {
            string excelFilePath = @"E:\excel\All In 1 2023.Aug.09 Updates.xlsm"; // Path to your Excel file
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new System.IO.FileInfo(excelFilePath)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1]; // Assuming the first worksheet

                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            ExcelComment comment = worksheet.Cells[row, col].Comment;
                            if (comment != null)
                            {
                                Console.WriteLine("Comment at Row {0}, Column {1}: {2}", row, col, comment.Text);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }
        }

    }
}
