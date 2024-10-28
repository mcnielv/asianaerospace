using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Converter
{
    //public class Csv
    //{
    //    public string Export( string header, string contents )
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        #region Header
    //        sb.Append( header );
    //        sb.Remove( sb.Length - 1, 1 ).AppendLine();
    //        #endregion

    //        #region Contents
    //        sb.Append( MakeValueCsvFriendly( contents ) ).Append( "," );
    //        sb.Remove( sb.Length - 1, 1 ).AppendLine();
    //        #endregion
    //        //add value for each property.


    //        return sb.ToString();
    //    }
    //    //get the csv value for field.
    //    public string MakeValueCsvFriendly( object value )
    //    {
    //        if ( value == null )
    //            return "";
    //        if ( value is Nullable && ( (INullable)value ).IsNull )
    //            return "";

    //        if ( value is DateTime )
    //        {
    //            if ( ( (DateTime)value ).TimeOfDay.TotalSeconds == 0 )
    //                return ( (DateTime)value ).ToString( "yyyy-MM-dd" );
    //            return ( (DateTime)value ).ToString( "yyyy-MM-dd HH:mm:ss" );
    //        }
    //        string output = value.ToString();

    //        if ( output.Contains( "," ) || output.Contains( "\"" ) )
    //            output = '"' + output.Replace( "\"", "\"\"" ) + '"';

    //        return output;

    //    }
    //}

    public class CsvExport<T> where T : class
    {
        public List<T> Objects;
        public CsvExport()
        {
        }
        public CsvExport( List<T> objects )
        {
            Objects = objects;
        }

        public string Export()
        {
            return Export( true );
        }

        public string Export( bool includeHeaderLine )
        {

            StringBuilder sb = new StringBuilder();
            //Get properties using reflection.
            IList<PropertyInfo> propertyInfos = typeof( T ).GetProperties();

            if ( includeHeaderLine )
            {
                //add header line.
                foreach ( PropertyInfo propertyInfo in propertyInfos )
                {
                    sb.Append( propertyInfo.Name ).Append( "," );
                }
                sb.Remove( sb.Length - 1, 1 ).AppendLine();
            }

            //add value for each property.
            foreach ( T obj in Objects )
            {
                foreach ( PropertyInfo propertyInfo in propertyInfos )
                {
                    sb.Append( MakeValueCsvFriendly( propertyInfo.GetValue( obj, null ) ) ).Append( "," );
                }
                sb.Remove( sb.Length - 1, 1 ).AppendLine();
            }

            return sb.ToString();
        }

        public string Export(string header, string contents)
        {
            StringBuilder sb = new StringBuilder();
            #region Header
            sb.Append( header );
            sb.Remove( sb.Length - 1, 1 ).AppendLine();
            #endregion

            #region Contents
            sb.Append( MakeValueCsvFriendly( contents ) ).Append( "," );
            sb.Remove( sb.Length - 1, 1 ).AppendLine();
            #endregion
            //add value for each property.


            return sb.ToString();
        }

        //export to a file.
        public void ExportToFile( string path )
        {
            File.WriteAllText( path, Export() );
        }

        //export as binary data.
        public byte[] ExportToBytes()
        {
            return Encoding.UTF8.GetBytes( Export() );
        }

        //get the csv value for field.
        private string MakeValueCsvFriendly( object value )
        {
            if ( value == null )
                return "";
            if ( value is Nullable && ( (INullable)value ).IsNull )
                return "";

            if ( value is DateTime )
            {
                if ( ( (DateTime)value ).TimeOfDay.TotalSeconds == 0 )
                    return ( (DateTime)value ).ToString( "yyyy-MM-dd" );
                return ( (DateTime)value ).ToString( "yyyy-MM-dd HH:mm:ss" );
            }
            string output = value.ToString();

            if ( output.Contains( "," ) || output.Contains( "\"" ) )
                output = '"' + output.Replace( "\"", "\"\"" ) + '"';

            return output;

        }
    }
}
