namespace myProject;

public class TenBIs
{
    public int Id { get; set; }
    public required String Name { get; set; }
    public bool Ismilky { get; set; }
    public int UserId { get; set; }  // ← NEW: Track which user owns this item
}