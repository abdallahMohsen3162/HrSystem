using AutoMapper;
using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.dto.Users;
using DataLayer.Entities;
using DataLayer.Validation;
using DataLayer.Validation.Fluent_validation;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HrSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]


    public class UsersController : Controller
    {
        private readonly IAccountsService _accountsService;
        private readonly IMapper _mapper;
        private readonly IRolesService rolesService;
        private readonly ApplicationDbContext context;
        public UsersController(IAccountsService accountsService, IMapper mapper, ApplicationDbContext context)
        {
            _accountsService = accountsService;
            _mapper = mapper;
            this.context = context;
        }




        [HttpGet("allUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Department.Show)]
        public async Task<IActionResult> getAllUsers()
        {
            try
            {
                List<UserViewModel> users = await _accountsService.GetAllUsersAsync();
                    
                return Ok(users);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Department.Add)]
        [HttpPost]
        public async Task<IActionResult> creatUser([FromForm] CreateUserViewModel user)
        {
            try
            {

                CreateUserValidation fv = new CreateUserValidation(context);
                var res = fv.Validate(user);
                if (!res.IsValid)
                {
                    return BadRequest(res.Errors);
                }
                await _accountsService.CreateUserAsync(user);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //delete
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Department.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            try
            {

                var result = await _accountsService.DeleteUserAsync(id);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("User deleted successfully.");

            }
            catch (Exception ex)
            {
                    
                return BadRequest("An error occurred while deleting the user.");

            }

        }





        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Department.Edit)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromForm] EditUserDto user)
        {
            try
            {
                if (user.Password != user.ConfirmPassword)
                {
                    return BadRequest("Password and Confirm Password do not match.");
                }
                //EditUserDto user
                EditUserViewModel editUserViewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password,
                    ConfirmPassword = user.ConfirmPassword,
                    SelectedRole = user.SelectedRole,
                    FullName = user.FullName
                };

                var result = await _accountsService.UpdateUserAsync(editUserViewModel);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while updating the user.");
            }
        }



    }
}
