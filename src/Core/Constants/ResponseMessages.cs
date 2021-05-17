using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Constants
{
    public class ResponseMessages
    {
        public const string OperationSuccessful = "Operation successful!";
        public const string OperationFailed = "Operation Failed";

        public const string SQlTransactionNotInitialized = "Oops! An error occurred while processing your request. If this persists after three(3) trials, kindly contact your administrator.";
        public const string InvalidElasticTableName = "Could not locate entity for search. Kindly contact your administrator";
        public const string NoRecordFound = "No record was found";
        public const string GeneralError = "Oops! An error occurred while processing your request. If this persists after three(3) trials, kindly contact your administrator.";
        public const string SQlException = "Oops! A database error occurred while processing your request. If this persists after three(3) trials, kindly contact your administrator.";
        public const string AuditLogObjectEmpty = "Oops! A error occurred while preparing a trail for your request. If this persists after three(3) trials, kindly contact your administrator.";
    }
}
