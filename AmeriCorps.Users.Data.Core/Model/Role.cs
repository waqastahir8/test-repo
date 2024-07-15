namespace AmeriCorps.Users.Data.Core;


public sealed class Role : Entity
{
    /**
     * Role name in the format of NAME1_NAME2
     */
    public required string RoleName { get; set; }
    /**
* Human redeable name for display purposes
*/
    public required string FucntionalName { get; set; }
    /**
     * Optional summary of what the role means 
     */
    public string Description { get; set; } = string.Empty;

    public List<User> Users { get; set; } = new List<User>();

}

