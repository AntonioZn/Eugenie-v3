﻿namespace Eugenie.Clients.Common.Helpers
{
    using System;

    using Eugenie.Common.Constants;

    public static class Validator
    {
        public static string ValidateServerName(string argument)
        {
            var name = argument?.Trim() ?? "";
            if (name.Length < ValidationConstants.ServerNameMinLength)
            {
                return $"Името трябва да бъде по - дълго от {ValidationConstants.ServerNameMinLength} символа";
            }

            if (name.Length > ValidationConstants.ServerNameMaxLength)
            {
                return $"Името трябва да бъде по - кратко от {ValidationConstants.ServerNameMaxLength} символа";
            }

            return null;
        }

        public static string ValidateProductName(string argument)
        {
            var name = argument?.Trim() ?? "";
            if (name.Length < ValidationConstants.ProductNameMinLength)
            {
                return $"Името трябва да бъде поне {ValidationConstants.ProductNameMinLength} символа";
            }

            if (name.Length > ValidationConstants.ProductNameMaxLength)
            {
                return $"Името трябва да бъде по - кратко от {ValidationConstants.ProductNameMaxLength} символа";
            }

            return null;
        }

        public static string ValidateUsername(string argument)
        {
            var name = argument?.Trim() ?? "";
            if (name.Length < ValidationConstants.UsernameMinLength)
            {
                return $"Името трябва да бъде по - дълго от {ValidationConstants.UsernameMinLength} символа";
            }

            if (name.Length > ValidationConstants.UsernameMaxLength)
            {
                return $"Името трябва да бъде по - кратко от {ValidationConstants.UsernameMaxLength} символа";
            }

            return null;
        }

        public static string ValidateAddress(string address)
        {
            try
            {
                new Uri(address);

                return null;
            }
            catch
            {
                return "Невалиден адрес";
            }
        }

        public static string ValidatePassword(string argument)
        {
            var password = argument?.Trim() ?? "";
            if (password.Length < ValidationConstants.PasswordMinLength)
            {
                return $"Паролата трябва да бъде по - дълга от {ValidationConstants.PasswordMinLength} символа";
            }

            if (password.Length > ValidationConstants.PasswordMaxLength)
            {
                return $"Паролата трябва да бъде по - кратка от {ValidationConstants.PasswordMaxLength} символа";
            }

            return null;
        }
    }
}