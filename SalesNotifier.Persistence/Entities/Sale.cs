namespace SalesNotifier.Persistence.Entities;

public class Sale
{
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string MaleAssortment { get; set; } = string.Empty;

    public string FemaleAssortment { get; set; } = string.Empty;

    public string InstagramPage { get; set; } = string.Empty;
}