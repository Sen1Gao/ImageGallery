namespace BasicArgs
{
    public class SigninInfoArgs
    {
        public SigninType SigninType { get; set; } = SigninType.None;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
