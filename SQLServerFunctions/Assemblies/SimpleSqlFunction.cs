using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    /* for test purpose     */
  public const double VAT_TAX = .020;
    /// <summary>
    /// Simple example of function (for testing purpose)
    /// </summary>
    /// <param name="originalAmount"></param>
    /// <returns></returns>
  [Microsoft.SqlServer.Server.SqlFunction]
  public static SqlDouble AddVAT(SqlDouble originalAmount)
  {
      SqlDouble taxAmount = originalAmount * VAT_TAX;

      return originalAmount + taxAmount;
      //        return new SqlString (string.Empty);
  }

  [Microsoft.SqlServer.Server.SqlFunction]
  public static SqlDouble RemVAT(SqlDouble amount)
  {
      SqlDouble originalAmount = amount / (1 + VAT_TAX);

      return originalAmount;
  }

  [Microsoft.SqlServer.Server.SqlFunction]
  public static SqlDouble GetVAT()
  {
      return VAT_TAX;
  }
  
}
