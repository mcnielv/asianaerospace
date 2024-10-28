using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MNV.Infrastructure.Converter
{
    public class HtmlToPdf
    {
        public string Convert( string html, string location, string pdfName = "" )
        {
            string retValue = string.Empty;
            try
            {
                if ( string.IsNullOrEmpty( pdfName ) )
                    pdfName = "GenericPDF";

                string path = location + string.Format( "{0}.pdf", pdfName );
                if ( File.Exists( path ) )
                    File.Delete( path );

                // Step 1: Creating System.IO.FileStream object
                using ( FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None ) )
                // Step 2: Creating iTextSharp.text.Document object
                using ( Document doc = new Document() )
                // Step 3: Creating iTextSharp.text.pdf.PdfWriter object
                // It helps to write the Document to the Specified FileStream
                using ( PdfWriter writer = PdfWriter.GetInstance( doc, fs ) )
                {
                    // Step 4: Openning the Document
                    doc.Open();

                    iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                    iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker( doc );
                    string newhtml = "";
                    newhtml = html.Replace( System.Environment.NewLine, "" );
                    hw.Parse( new StringReader( newhtml ) );
                    //document.Close();

                    //// Step 5: Adding a paragraph
                    //// NOTE: When we want to insert text, then we've to do it through creating paragraph
                    //doc.Add( new Paragraph( html ) );

                    // Step 6: Closing the Document
                    doc.Close();
                }

            }
            catch ( DocumentException de )
            {
                retValue = string.Format( "Error: {0}", de.Message );
            }
            catch ( IOException ioe )
            {
                retValue = string.Format( "Error: {0}", ioe.Message );
            }
            return retValue;
        }
    }
}
