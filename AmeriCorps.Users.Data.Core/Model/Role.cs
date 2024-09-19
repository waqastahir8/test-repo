﻿﻿namespace AmeriCorps.Users.Data.Core;


public sealed class Role : Entity
{
    /**
     * Role name in the format of NAME1_NAME2
     */
    public string RoleName { get; set; } = string.Empty;
    /**
* Human redeable name for display purposes
*/
    public string FunctionalName { get; set; } = string.Empty;
    /**
     * Optional summary of what the role means 
     */
    public string Description { get; set; } = string.Empty;


    public string RoleType { get; set; } = string.Empty;


}