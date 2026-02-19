public class PostPermission
{
    public required string name { get; set;}
    public required string codename{ get; set;}
}

public class CreateRolePermission
{
    public required int role_id { get; set;}
    public required int permission_id { get; set;}
}

public class RolePermissionResponse
{
    public int id { get; set; }
    public int role_id { get; set; }
    public int permission_id { get; set; }
    public string? role_name { get; set; }
    public string? permission_name { get; set; }
    public string? permission_codename { get; set; }
}

public class RoleWithPermissionsDto
{
    public int role_id { get; set; }
    public string role_name { get; set; } = string.Empty;
    public List<PermissionInfoDto> permissions { get; set; } = new();
}

public class PermissionInfoDto
{
    public int permission_id { get; set; }
    public string permission_name { get; set; } = string.Empty;
    public string permission_codename { get; set; } = string.Empty;
}