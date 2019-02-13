using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{

    // example of FindInvalidEmails
    private class EmailResult
    {
        public SqlInt32 CustomerId;
        public SqlString EmailAdress;

        public EmailResult(SqlInt32 customerId, SqlString emailAdress)
        {
            CustomerId = customerId;
            EmailAdress = emailAdress;
        }
    }

    public static bool ValidateEmail(SqlString emailAddress)
    {
        if (emailAddress.IsNull)
            return false;

        if (!emailAddress.Value.EndsWith("@adventure-works.com"))
            return false;

        // Validate the address. Put any more rules here.  
        return true;
    }

    [SqlFunction(
        DataAccess = DataAccessKind.Read,
        FillRowMethodName = "FindInvalidEmails_FillRow",
        TableDefinition = "CustomerId int, EmailAddress nvarchar(4000)")]
    public static IEnumerable FindInvalidEmails(SqlDateTime modifiedSince)
    {
        ArrayList resultCollection = new ArrayList();

        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            using (SqlCommand selectEmails = new SqlCommand(
                "SELECT " +
                "[CustomerID], [EmailAddress] " +
                "FROM [AdventureWorksLT2008].[SalesLT].[Customer] " +
                "WHERE [ModifiedDate] >= @modifiedSince",
                connection))
            {
                SqlParameter modifiedSinceParam = selectEmails.Parameters.Add(
                    "@modifiedSince",
                    SqlDbType.DateTime);
                modifiedSinceParam.Value = modifiedSince;

                using (SqlDataReader emailsReader = selectEmails.ExecuteReader())
                {
                    while (emailsReader.Read())
                    {
                        SqlString emailAddress = emailsReader.GetSqlString(1);
                        if (ValidateEmail(emailAddress))
                        {
                            resultCollection.Add(new EmailResult(
                                emailsReader.GetSqlInt32(0),
                                emailAddress));
                        }
                    }
                }
            }
        }

        return resultCollection;
    }
    public static void FindInvalidEmails_FillRow(
           object emailResultObj,
           out SqlInt32 customerId,
           out SqlString emailAdress)
    {
        EmailResult emailResult = (EmailResult)emailResultObj;

        customerId = emailResult.CustomerId;
        emailAdress = emailResult.EmailAdress;
    }
    // fin of FindInvalidEmails
}
