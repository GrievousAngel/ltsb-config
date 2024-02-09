namespace ConfigAdmin;

public sealed class AppSettings
{
    public const string SectionKey = "AppSettings";

    public string ConfigFilePath { get; set; }
}