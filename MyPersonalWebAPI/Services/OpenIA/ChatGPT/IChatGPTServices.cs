namespace MyPersonalWebAPI.Services.OpenIA.ChatGPT
{
    public interface IChatGPTServices
    {
        Task<string> Execute(string textUser, string user);
    }
}
