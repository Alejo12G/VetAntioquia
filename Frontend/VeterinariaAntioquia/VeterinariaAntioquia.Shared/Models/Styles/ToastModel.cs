namespace VeterinariaAntioquia.Shared.Models.Styles;

public enum ToastType
{
    Success,
    Info,
    Warning,
    Error
}

public class ToastModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    public ToastType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}