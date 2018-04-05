using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MicCRM.Data;
using MicCRM.Data.Entities;
using MicCRM.Helpers;
using MicCRM.Models.ApplicantViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicCRM.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FileController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult UploadExcel()
        {
            var lessons = _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Technology);

            ApplicantExcelUploadViewModel model = new ApplicantExcelUploadViewModel()
            {
                Lessons = Utilities.GetSelectListItem(lessons)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(ApplicantExcelUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ExcelFile == null || model.ExcelFile.Length == 0)
                    return Content("file not selected");

                string fileName = model.ExcelFile.FileName;

                string filePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot", "files", "UploadedExcels",
                            fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ExcelFile.CopyToAsync(stream);
                }

                List<Applicant> applicantList = new List<Applicant>();
                try
                {
                    using (SpreadsheetDocument spreadsheetDocument =
                        SpreadsheetDocument.Open(filePath, false))
                    {
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                        string cellValue = string.Empty;

                        foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
                        {
                            int rowNumber = 0;

                            OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);

                            while (reader.Read())
                            {
                                int columnNumber = 0;
                                if (reader.ElementType == typeof(Row))
                                {
                                    reader.ReadFirstChild();
                                    Applicant applicant = new Applicant()
                                    {
                                        Lesson = _dbContext.Lessons.Find(model.LessonId),
                                        Date = DateTime.Now
                                    };
                                    do
                                    {
                                        if (rowNumber == 0)
                                        {
                                            continue;
                                        }

                                        if (reader.ElementType == typeof(Cell))
                                        {
                                            Cell c = (Cell)reader.LoadCurrentElement();
                                            if (c.DataType != null &&
                                                c.DataType == CellValues.SharedString)
                                            {
                                                SharedStringItem ssi = workbookPart.SharedStringTablePart
                                                    .SharedStringTable
                                                    .Elements<SharedStringItem>()
                                                    .ElementAt(Int32.Parse(c.CellValue.InnerText));
                                                var s = ssi.Text;
                                                cellValue = ssi.Text.Text;
                                            }
                                            else
                                            {
                                                cellValue = c.CellValue.InnerText;
                                            }

                                            switch (columnNumber)
                                            {
                                                case 0:
                                                    applicant.FirstName = cellValue;
                                                    break;
                                                case 1:
                                                    applicant.LastName = cellValue;
                                                    break;
                                                case 2:
                                                    applicant.Email = cellValue;
                                                    break;
                                                case 3:
                                                    applicant.Phone1 = cellValue.RemoveWhiteSpace();
                                                    break;
                                                case 4:
                                                    if (cellValue != "x")
                                                        applicant.Phone2 = cellValue.RemoveWhiteSpace();
                                                    break;
                                                case 7:
                                                    applicant.Date = cellValue.UNIXTimeToDateTime();
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        columnNumber++;
                                    } while (reader.ReadNextSibling());

                                    if (rowNumber != 0)
                                        applicantList.Add(applicant);

                                    rowNumber++;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {

                }
                _dbContext.Applicants.AddRange(applicantList);
                _dbContext.SaveChanges();
                return RedirectToAction("AllApplicants", "Applicant");
            }

            return RedirectToAction("UploadExcel");
        }
    }
}