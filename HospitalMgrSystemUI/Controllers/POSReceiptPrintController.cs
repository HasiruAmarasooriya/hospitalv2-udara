using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.JSInterop;

namespace HospitalMgrSystemUI.Controllers
{
    public class POSReceiptPrintController : Controller
    {
        private readonly ILogger<POSReceiptPrintController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;
        private readonly IJSRuntime _jsRuntime;



        public POSReceiptPrintController(ILogger<POSReceiptPrintController> logger, IWebHostEnvironment webHostEnvironment, IJSRuntime jsRuntime)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _jsRuntime = jsRuntime;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> PrintReceptAsync()
        //{
        //    var dt = new DataTable();
        //    var dt2 = new DataTable();
        //    using var report = new LocalReport();
        //    dt = GetItemList();
        //    dt2 = GetDetailsReceipt();

        //    report.DataSources.Add(new ReportDataSource("DataSet1", dt));
        //    report.DataSources.Add(new ReportDataSource("DataSet2", dt2));
        //    report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcReport.rdlc";

        //    // Call the PrintAsync method with the injected IJSRuntime
        //    await PrintAsync(_jsRuntime);

        //    return View("Index"); // Return to your view
        //}

        //public IActionResult PrintRecept()
        //{
        //    var dt = new DataTable();
        //    var dt2 = new DataTable();
        //    using var report = new LocalReport();
        //    try
        //    {
        //        dt = GetItemList();
        //        dt2 = GetDetailsReceipt();

        //        report.DataSources.Add(new ReportDataSource("DataSet1", dt));
        //        report.DataSources.Add(new ReportDataSource("DataSet2", dt));
        //        report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcReport.rdlc";
        //        PrintToPrinter(report);

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while preparing the report.");
        //        // Handle the error, log it, and return an error response.
        //        return Json(new { success = false, errorMessage = ex+"Error printing receipt. Please try again later." });
        //    }
        //}

        //public IActionResult PrintRecept()
        //{
        //    var dt = new DataTable();
        //    var dt2 = new DataTable();
        //    using var report = new LocalReport();
        //    try
        //    {
        //        dt = GetItemList();
        //        dt2 = GetDetailsReceipt();

        //        report.DataSources.Add(new ReportDataSource("DataSet1", dt));
        //        report.DataSources.Add(new ReportDataSource("DataSet2", dt2));
        //        report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcReport.rdlc";

        //        // Render the report to an image (EMF)
        //        var imageBytes = Export(report);

        //        // Create a PDF document with paper size matching the image's dimensions
        //        var image = Image.GetInstance(imageBytes);
        //        var pdfDocument = new Document(new Rectangle(image.Width, image.Height), 0, 0, 0, 0); // No margins
        //        var memoryStream = new MemoryStream();
        //        var pdfWriter = PdfWriter.GetInstance(pdfDocument, memoryStream);
        //        pdfDocument.Open();

        //        // Add the image to the PDF document
        //        pdfDocument.Add(image);

        //        // Close the PDF document
        //        pdfDocument.Close();

        //        // Set the response headers to prompt the user to download the PDF
        //        Response.Headers.Add("Content-Disposition", "attachment; filename=receipt.pdf");
        //        Response.Headers.Add("Content-Type", "application/pdf");

        //        // Return the PDF data to the client
        //        return File(memoryStream.ToArray(), "application/pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while preparing the report.");
        //        // Handle the error, log it, and return an error response.
        //        return Json(new { success = false, errorMessage = "Error generating PDF receipt. Please try again later." });
        //    }
        //}

        public IActionResult PrintRecept()
        {
            var dt = new DataTable();
            var dt2 = new DataTable();
            using var report = new LocalReport();
            try
            {
                dt = GetItemList();
                dt2 = GetDetailsReceipt();

                report.DataSources.Add(new ReportDataSource("DataSet1", dt));
                report.DataSources.Add(new ReportDataSource("DataSet2", dt2));
                report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcReport.rdlc";

                // Render the report to an image (EMF)
                var imageBytes = Export(report);

                // Convert the image to a base64 data URL
                var imageBase64 = Convert.ToBase64String(imageBytes);

                // Return the image as a JSON response
                return Json(new { success = true, imageBase64 = imageBase64 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while preparing the report.");
                // Handle the error, log it, and return an error response.
                return Json(new { success = false, errorMessage = "Error generating image. Please try again later." });
            }
        }



        private float GetImageWidth(byte[] imageBytes)
        {
            using (var imageStream = new MemoryStream(imageBytes))
            using (var image = new System.Drawing.Bitmap(imageStream))
            {
                return image.Width;
            }
        }

        private float GetImageHeight(byte[] imageBytes)
        {
            using (var imageStream = new MemoryStream(imageBytes))
            using (var image = new System.Drawing.Bitmap(imageStream))
            {
                return image.Height;
            }
        }


        //public void PrintRecept()
        //{
        //    var dt = new DataTable();
        //    var dt2 = new DataTable();
        //    using var report = new LocalReport();
        //    dt = GetItemList();
        //    dt2 = GetDetailsReceipt();

        //    report.DataSources.Add(new ReportDataSource("DataSet1", dt));
        //    report.DataSources.Add(new ReportDataSource("DataSet2", dt2));
        //    report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcReport.rdlc";
        //    PrintToPrinter(report);
        //}

        public static byte[] Export(LocalReport report)
        {
            try
            {
                string deviceInfo =
    @"<DeviceInfo>
        <OutputFormat>PNG</OutputFormat>
        <PageWidth>3.5in</PageWidth>
        <PageHeight>10in</PageHeight>
        <MarginTop>0in</MarginTop>
        <MarginLeft>0in</MarginLeft>
        <MarginRight>0in</MarginRight>
        <MarginBottom>0in</MarginBottom>
    </DeviceInfo>";


                Warning[] warnings;
                m_streams = new List<Stream>();
                report.Render("Image", deviceInfo, CreateStream, out warnings);

                // Create a memory stream to collect the rendered image data
                using (MemoryStream imageStream = new MemoryStream())
                {
                    foreach (Stream stream in m_streams)
                    {
                        stream.Position = 0;
                        stream.CopyTo(imageStream);
                        stream.Close();
                    }

                    // Return the image data as a byte array
                    return imageStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //public static void Export(LocalReport report, bool print = true)
        //{
        //    try
        //    {
        //        string deviceInfo =
        //             @"<DeviceInfo>
        //        <OutputFormat>EMF</OutputFormat>
        //        <PageWidth>3.5in</PageWidth>
        //        <PageHeight>10in</PageHeight>
        //        <MarginTop>0in</MarginTop>
        //        <MarginLeft>0in</MarginLeft>
        //        <MarginRight>0in</MarginRight>
        //        <MarginBottom>0in</MarginBottom>
        //    </DeviceInfo>";
        //        Warning[] warnings;
        //        m_streams = new List<Stream>();
        //        report.Render("Image", deviceInfo, CreateStream, out warnings);
        //        foreach (Stream stream in m_streams)
        //            stream.Position = 0;

        //        if (print)
        //        {
        //            Print();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}
        //public static void Export(LocalReport report, bool print = true)
        //{
        //    string deviceInfo =
        //     @"<DeviceInfo>
        //        <OutputFormat>EMF</OutputFormat>
        //        <PageWidth>3.5in</PageWidth>
        //        <PageHeight>10in</PageHeight>
        //        <MarginTop>0in</MarginTop>
        //        <MarginLeft>0in</MarginLeft>
        //        <MarginRight>0in</MarginRight>
        //        <MarginBottom>0in</MarginBottom>
        //    </DeviceInfo>";
        //    Warning[] warnings;
        //    m_streams = new List<Stream>();
        //    report.Render("Image", deviceInfo, CreateStream, out warnings);
        //    foreach (Stream stream in m_streams)
        //        stream.Position = 0;

        //    if (print)
        //    {
        //        Print();
        //    }
        //}

        //public static void Print()
        //{
        //    try
        //    {
        //        if (m_streams == null || m_streams.Count == 0)
        //        {

        //            Console.WriteLine("Error: no stream to print.");
        //            throw new Exception("Error: no stream to print.");
        //        }

        //        PrintDocument printDoc = new PrintDocument();
        //        if (!printDoc.PrinterSettings.IsValid)
        //        {
        //            Console.WriteLine("Available.");
        //            throw new Exception("Error: cannot find the default printer.");
        //        }
        //        else
        //        {
        //            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //            m_currentPageIndex = 0;
        //            printDoc.Print();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void Print()
        //{
        //    if (m_streams == null || m_streams.Count == 0)
        //    {

        //        Console.WriteLine("Error: no stream to print.");
        //        throw new Exception("Error: no stream to print.");
        //    }

        //    PrintDocument printDoc = new PrintDocument();
        //    if (!printDoc.PrinterSettings.IsValid)
        //    {
        //        Console.WriteLine("Available.");
        //        throw new Exception("Error: cannot find the default printer.");
        //    }
        //    else
        //    {
        //        printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //        m_currentPageIndex = 0;
        //        printDoc.Print();
        //    }
        //}

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        //public static void PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    Metafile pageImage = new
        //       Metafile(m_streams[m_currentPageIndex]);

        //    // Adjust rectangular area with printer margins.
        //    Rectangle adjustedRect = new Rectangle(
        //        ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
        //        ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
        //        ev.PageBounds.Width,
        //        ev.PageBounds.Height);

        //    // Draw a white background for the report
        //    ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

        //    // Draw the report content
        //    ev.Graphics.DrawImage(pageImage, adjustedRect);

        //    // Prepare for the next page. Make sure we haven't hit the end.
        //    m_currentPageIndex++;
        //    ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        //}

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        public DataTable GetItemList()
        {
            var dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ItemQty");
            dt.Columns.Add("ItemPrice");

            DataRow row;
            for (int i = 1; i < 3; i++)
            {
                row = dt.NewRow();
                row["ItemID"] = i;
                row["ItemName"] = "DASUN " + i.ToString();
                row["ItemQty"] = 1;
                row["ItemPrice"] = 100 * i;

                dt.Rows.Add(row);
            }
            return dt;
        }

        public DataTable GetDetailsReceipt()
        {
            var dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("drName");
            dt.Columns.Add("billNo");
            dt.Columns.Add("date");

            DataRow row;

                row = dt.NewRow();
                row["name"] = "kasun";
                row["drName"] = "kasun";
                row["billNo"] = "KJH";
                row["date"] = "2023/10/01";

            dt.Rows.Add(row);

            return dt;
        }

        public static void PrintToPrinter(LocalReport report)
        {
            Export(report);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public static async Task PrintAsync(IJSRuntime jsRuntime)
        //{
        //    if (m_streams == null || m_streams.Count == 0)
        //    {
        //        await jsRuntime.InvokeVoidAsync("console.log", "Error: no stream to print.");
        //        throw new Exception("Error: no stream to print.");
        //    }

        //    PrintDocument printDoc = new PrintDocument();
        //    if (!printDoc.PrinterSettings.IsValid)
        //    {
        //        await jsRuntime.InvokeVoidAsync("console.log", "Error: cannot find the default printer.");
        //        throw new Exception("Error: cannot find the default printer.");
        //    }
        //    else
        //    {
        //        printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //        m_currentPageIndex = 0;
        //        printDoc.Print();
        //    }
        //}

    }
}
