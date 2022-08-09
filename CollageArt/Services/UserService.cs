using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CollageArt.Services
{
    public class UserService
    {
        
        private UserManager<IdentityUser> UserManager { get; }
        private RoleManager<IdentityRole> RoleManager { get; }
        private AuthenticationStateProvider AuthenticationState { get; }

        public UserService(UserManager<IdentityUser> userManager, AuthenticationStateProvider authenticationState, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            AuthenticationState = authenticationState;
            RoleManager = roleManager;
        }

        public async Task GiveAdmin()
        {
            await RoleManager.CreateAsync(new IdentityRole("admin"));
            var state = await AuthenticationState.GetAuthenticationStateAsync();
            var user = await UserManager.GetUserAsync(state.User);
            await UserManager.AddToRoleAsync(user, "admin");
        }

    }
}