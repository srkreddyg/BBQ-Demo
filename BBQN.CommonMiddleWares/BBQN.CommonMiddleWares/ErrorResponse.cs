namespace BBQN.CommonMiddleWares
{
    internal class ErrorResponse
    {
        public bool Success { get; set; }
        public string? Message { get; internal set; }
    }
}