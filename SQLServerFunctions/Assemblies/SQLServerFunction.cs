using Excel.Lib;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;

/// <summary>
/// CLR User-Defined Functions in C# inside SQL Server
/// Same fashion than
/// http://westclintech.com/SQL-Server-Financial-Functions/SQL-Server-XIRR-function
/// 
/// https://docs.microsoft.com/en-us/sql/relational-databases/clr-integration-database-objects-user-defined-functions/clr-user-defined-functions?view=sql-server-2017
/// </summary>
public partial class UserDefinedFunctions
{
    private static List<double> GetDoubleDataList(SqlConnection conn, String tableName, String columnName)
    {

        List<Double> c1 = new List<double>(30);

        SqlCommand cmd = new SqlCommand(String.Format("select {0} from {1}", columnName, tableName),conn);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                c1.Add(reader.GetDouble(0));
            }
        }
        return c1;
    }

    private static List<DateTime> GetDateTimeDataList(SqlConnection conn, String tableName, String columnName)
    {
        List<DateTime> c2 = new List<DateTime>(30);
        SqlCommand cmd = new SqlCommand(String.Format("select {0} from {1}", columnName, tableName), conn);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                c2.Add(reader.GetDateTime(0));
            }
        }
        return c2;
    }



    /// <summary>
    /// calculate the Internal Rate return given 2 lists of (value,date) for cashflows
    /// </summary>
    /// <param name="cashflows">Name of a table containing the value of cashflows</param>
    /// <param name="cashflowsColumn">Column Name of the value inside cashflows table</param>
    /// <param name="dates">Name of a table containing the dates of each given cashflows (could be the same that cashFlows)</param>
    /// <param name="datesColumn">Column Name of the dates inside the dates table</param>
    /// <param name="estimate">could be NULL</param>
    /// <returns></returns>
    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = false)]   
    public static SqlDouble XIRR(SqlString cashflows, SqlString cashflowsColumn, SqlString dates, SqlString datesColumn)
    {
        List<double> c1;
        List<DateTime> c2;
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            c1 = GetDoubleDataList(conn, cashflows.Value, cashflowsColumn.Value);
            c2 = GetDateTimeDataList(conn, dates.Value, datesColumn.Value);
        }

        return new XIRR(c1, c2).Calculate();

    }


    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = false)]   
    public static SqlDouble XIRR_detail(SqlString cashflows, SqlString cashflowsColumn, SqlString dates, SqlString datesColumn, SqlDouble estimate, SqlInt32 maxFloatingPoints, SqlInt32 maxNbDecimalDigits)
    {
        double precision; int decimals;

        if (maxFloatingPoints == SqlInt32.Null)
        {
            precision = 0.00001;
        }
        else
        {
            precision = Math.Pow(10, -maxFloatingPoints.Value);
        }

        if (maxNbDecimalDigits == SqlDouble.Null)
        {
            decimals = 10;
        }
        else
        {
            decimals = maxNbDecimalDigits.Value;
        }

        List<double> c1;
        List<DateTime> c2;
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            c1 = GetDoubleDataList(conn, cashflows.Value, cashflowsColumn.Value);
            c2 = GetDateTimeDataList(conn, dates.Value, datesColumn.Value);

        }
        if (estimate == SqlDouble.Null)
        {
            return new XIRR(c1, c2).Calculate();
        }
        else
        {
            return new XIRR(c1, c2, estimate.Value).Calculate(precision, decimals);
        }


    }

}
