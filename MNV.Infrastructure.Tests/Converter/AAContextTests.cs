using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MNV.Infrastructure.Converter;
using System.IO;

namespace MNV.Infrastructure.Tests
{
    public class TestData
    {
        public string asa { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    [TestClass()]
    public class AAContextTests
    {
       
        [TestMethod()]
        public void ExportToFileTest()
        {
            var list = new List<TestData>();
            string expected = string.Empty;
            for(var x=0; x<=5;x++ )
            {               
                string name = Guid.NewGuid().ToString().Replace( "-", "" ).Substring( 10 );
                TestData data = new TestData()
                {
                    asa = "asa" + x.ToString(),
                    Name = name.Substring( 5 ),
                    Description = string.Format( "{0},{1}", name.Substring( 6 ), name.Substring( 4 ) )
                };
                list.Add( data );
            }
            CsvExport<TestData> csv = new CsvExport<TestData>( list );
            expected = csv.Export( true );

            string path = @"c:\ftp\test.csv";
            if (File.Exists(path))
                File.Delete( path );

            if (!File.Exists(path))
            {
                //TextWriter tw = new StreamWriter( path );
                //tw.WriteLine( expected );
                using ( StreamWriter sw = new StreamWriter( new FileStream( path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite ) ) )
                {
                    sw.WriteLine( expected );
                }
            }
            File.OpenText( path );

            Assert.AreNotEqual(string.Empty, expected);
        }
    }
}