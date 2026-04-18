namespace SafeBox.Domain.Entities
{
    /// <summary>
    /// Represents a permission type in the SafeBox system.
    /// Maps to Permission table (permission_id IDENTITY(200,7)).
    /// Values: VIEW, DOWNLOAD, EDIT, DELETE
    /// </summary>
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}
