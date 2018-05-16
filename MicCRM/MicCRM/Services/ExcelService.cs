using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MicCRM.Services
{
    public class ExcelService
    {
        public async Task<List<List<String>>> FromExcel(IFormFile files, string userID)
        {
            List<List<String>> lst = new List<List<String>>();

            if (files == null || files.Length == 0)
            {
                throw new Exception("boom");
            }

            string filename = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "ExcelList_" + Guid.NewGuid() + ".xlsx"; ;

            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "files", "UploadedExcels", userID);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, filename);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }

            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    string cellValue = string.Empty;

                    List<List<string>> lstExcel = new List<List<string>>();
                    List<string> exelRow;
                    foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
                    {
                        int rowNumber = 0;
                        OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);

                        while (reader.Read())
                        {
                            if (reader.ElementType == typeof(Row))
                            {
                                reader.ReadFirstChild();

                                // int cellNumber = 1;
                                exelRow = new List<string>();
                                do
                                {
                                    if (reader.ElementType == typeof(Cell))
                                    {
                                        Cell c = (Cell)reader.LoadCurrentElement();

                                        if (c.DataType != null && c.DataType == CellValues.SharedString)
                                        {
                                            SharedStringItem ssi = workbookPart.SharedStringTablePart
                                                .SharedStringTable
                                                .Elements<SharedStringItem>()
                                                .ElementAt(Int32.Parse(c.CellValue.InnerText));

                                            cellValue = ssi.Text.Text;
                                        }
                                        else
                                        {
                                            cellValue = c.CellValue.InnerText;
                                        }

                                        exelRow.Add(cellValue);
                                    }
                                }
                                while (reader.ReadNextSibling());
                                lstExcel.Add(exelRow);
                                rowNumber++;
                            }
                        }
                    }

                    //await _dbContext.Stores.AddRangeAsync(storesFromExcel);
                    //await _dbContext.SaveChangesAsync();                   
                }
            }
            catch (Exception)
            {
                //file.Delete();
                throw new Exception("booooom on import");
            }
            finally
            {
                //file.Delete();
            }

            return lst;
        }
    }
}
