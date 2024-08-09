using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Data;
public interface IRoleRepository
{
    Task<Role?> GetAsync(int id);

    Task<T> SaveAsync<T>(T entity) where T : Entity;

    Task<Role?> UpdateRoleAsync(Role entity);

    Task<bool> DeleteAsync<T>(int id) where T : Entity;



}