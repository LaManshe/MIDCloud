namespace MIDCloud.GlobalInterfaces.Responses
{
    public interface IAuthenticate
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
