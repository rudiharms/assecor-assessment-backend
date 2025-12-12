namespace Assecor.Api.Domain.Common;

public static class Errors
{
    public static readonly Error InvalidAddressZipCode = new(Codes.InvalidAddressZipCodeCode, "The address zip code is invalid");
    public static readonly Error InvalidAddressCity = new(Codes.InvalidAddressCityCode, "The address city is invalid");
    public static readonly Error AddressIsMissing = new(Codes.AddressIsMissingCode, "The address  is missing");
    public static readonly Error InvalidColor = new(Codes.InvalidColorCode, "The color is invalid");

    public static Error PersonNotFound(string message)
    {
        return new Error(Codes.PersonNotFoundCode, $"Person could not be found: {message}");
    }

    public static Error FileNotFound(string filePath)
    {
        return new Error(Codes.FileNotFoundCode, $"CSV file not found at path: {filePath}");
    }

    public static Error CsvLoadingFailed(string message)
    {
        return new Error(Codes.CsvLoadingFailedCode, $"CSV file loading failed with message: {message}");
    }

    public static Error CsvParsingFailed(string message)
    {
        return new Error(Codes.CsvParsingFailedCode, $"CSV file parsing failed with message: {message}");
    }

    public static Error PersonQueryFailed(string message)
    {
        return new Error(Codes.PersonQueryFailedCode, $"Query failed with message: {message}");
    }

    public static class Codes
    {
        public const string InvalidAddressZipCodeCode = nameof(InvalidAddressZipCodeCode);
        public const string InvalidAddressCityCode = nameof(InvalidAddressCityCode);
        public const string InvalidColorCode = nameof(InvalidColorCode);
        public const string FileNotFoundCode = nameof(FileNotFoundCode);
        public const string CsvLoadingFailedCode = nameof(CsvLoadingFailedCode);
        public const string CsvParsingFailedCode = nameof(CsvParsingFailedCode);
        public const string AddressIsMissingCode = nameof(AddressIsMissingCode);
        public const string PersonNotFoundCode = nameof(PersonNotFoundCode);
        public const string PersonQueryFailedCode = nameof(PersonQueryFailedCode);
    }
}
