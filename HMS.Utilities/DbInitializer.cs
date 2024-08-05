using HMS.DB;
using HMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count()>0)
                {
                    _context.Database.Migrate();
                }

            }
            catch (Exception)
            {

                throw;
            }
            if (!_roleManager.RoleExistsAsync(HMS_Roles.HMS_Admin).GetAwaiter().GetResult()) 
            {
                _roleManager.CreateAsync(new IdentityRole(HMS_Roles.HMS_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(HMS_Roles.HMS_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(HMS_Roles.HMS_Doctor)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName ="Ahmad",
                    Email ="cssoft9@gmail.com"
                },"Admin@1234#").GetAwaiter().GetResult();
                var appUser = _context.ApplicationUsers.FirstOrDefault(a=>a.Email=="cssoft9@gmail.com");
                if (appUser!=null)
                {
                    _userManager.AddToRoleAsync(appUser, HMS_Roles.HMS_Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}
