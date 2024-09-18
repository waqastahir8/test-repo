namespace AmeriCorps.Users.Models;



public class ProjectRoleRequestModel
{
    /**
     * Role name in the format of NAME1_NAME2
     */
    public string RoleName { get; set; } = string.Empty;
    /**
* Human redeable name for display purposes
*/
    public string FunctionalName { get; set; } = string.Empty;


}