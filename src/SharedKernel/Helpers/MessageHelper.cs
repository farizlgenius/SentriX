using System;

namespace SharedKernel.Helpers;

public static class MessageHelper
{
      public static class Common
      {
            public static string UserIdEmpty = "OperatorId must not be empty.";
            public static string UsernameEmpty = "Username must not be empty.";
            public static string PasswordEmpty = "Password must not be empty.";
            public static string NameEmpty = "Name must not empty.";
            public static string FirstnameEmpty = "Firstname must not empty.";
            public static string LastnameEmpty = "Lastname must not empty.";
            public static string EmailEmpty = "Email must not empty.";
            public static string DuplicatedName = "Found duplicate name.";
            public static string DuplicatedUsername = "Found duplicate username.";
            public static string DuplicatedUserId = "Found duplicate operatorid.";
            public static string RecordNotFound = "Record not found.";
            public static string QueryIdInvalid = "Query id invalid.";
            public static string CompanyInvalid = "Company invalid.";
            public static string CompanyNotFound = "Company not found.";
            public static string PositionInvalid = "Position invalid.";
            public static string DepartmentInvalid = "Department invalid.";
            
            public static string PasswordLenEmpty = "Passowrd length must more than zero";
      }
      public static class Role
      {
            public static string RoleInvalid = "Role invalid.";
            public static string RoleNotFound = "Role not found.";
      }
      public static class Auth
      {
            public const string LoginSuccess = "Login successful.";
            public const string LogoutSuccess = "Logout successful.";
            public const string InvalidCredentials = "Invalid username or password.";
            public const string UserNotFound = "User not found.";
            public const string UsernameCannotBeEmpty = "Username cannot be empty.";
            public const string PasswordCannotBeEmpty = "Password cannot be empty.";
            public const string RefreshTokenNotFound = "Refresh token not found.";
            public const string RefreshExpired = "Refresh token expired.";
            public const string RefreshTokenInvalid = "Refresh token invalid.";
            public const string GetMeSuccess = "Get Me Successful.";
            public const string RefreshTokenSuccess = "Refresh token successful.";
      }

      public static class DB
      {
            public static string SaveRecordUnsuccessful = "Save record unsuccessful.";
            public static string RecordNotFound = "Record not found.";
            public static string DeleteRecordUnsuccessful = "Delete record unsuccessful.";
            public static string UpdateRecordUnsuccessful = "Update record unsuccessful.";
            public static string QueryIdInvalid = "Query id invalid.";
            public static string DeleteRelateRecordUnsuccessful = "Delete old related record unsuccessful.";
            public static string CreateReferenceRecordUnsuccessful = "Create new related record unsuccessful.";
      }

      public static class Location
      {
            public static string LocationNotFound = "Location not found.";
            public static string LocationInvalid = "Location invalid.";
            public static string CountryInvalid = "Country invalid.";
      }

}
