using System.Text.Json;

public class UserDataService
{
    private readonly string _filePath =
        Path.Combine(AppContext.BaseDirectory, "user_data.json");

    private Dictionary<long, UserData> _users = new();

    public UserDataService()
    {
        Load();
    }

    private void Load()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            string json = File.ReadAllText(_filePath);

            _users =
                JsonSerializer.Deserialize<Dictionary<long, UserData>>(json)
                ?? new Dictionary<long, UserData>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения JSON: {ex.Message}");
            _users = new Dictionary<long, UserData>();
        }
    }

    public UserData GetUser(long userId)
    {
        if (!_users.TryGetValue(userId, out var user))
        {
            user = new UserData();
            _users[userId] = user;
        }

        return user;
    }

    public void Save()
    {
        string json = JsonSerializer.Serialize(
            _users,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(_filePath, json);
    }
}