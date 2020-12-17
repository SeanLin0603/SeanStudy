namespace ZXing
{
    [System.Flags]
    public enum Status
    {
        NoError = 0,
        NotFound,
        FormatError,
        ChecksumError,
    };
}
