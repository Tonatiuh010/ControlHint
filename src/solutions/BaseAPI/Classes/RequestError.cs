namespace UttClassroom.Classes
{
    public class RequestError
    {
        public string? Info { get; set; }
        public Exception? Exception { get; set; }

        public string FormatError
        {
            get
            {
                string format = $"Error -> {Info}";

                if (Exception != null)
                {
                    format = $"{format} - Details {Exception.Message}. \n Source {Exception.Source}. \n On {Exception.StackTrace}";
                }

                return format;
            }
        }

        public static string FormatErrors(List<RequestError> errors)
        {
            string result = string.Empty;


            if (errors != null && errors.Count > 0)
            {
                foreach (var err in errors)
                {
                    result += $" * {err.FormatError} ";
                }
            }

            return result;
        }
    }
}
