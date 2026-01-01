using EventsManager.Data_Access_Layer;
using EventsManagerModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventsManager.Controllers
{
	[ApiController]
	[Route("[Controller]/[Action]")]
	public class VisitorController(
		LibraryUnitOfWork libraryUnitOfWork,
		ILogger<VisitorController> logger) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<List<Food>>> GetFoods()
		{
			try
			{
				logger.LogInformation("Fetching all foods");
				
				List<Food> foods = await libraryUnitOfWork.FoodRepository.ReadAllAsync();
				
				return Ok(foods);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetFoods failed");
				return StatusCode(500, "An error occurred while fetching foods");
			}
		}

		[HttpGet]
		public async Task<ActionResult<Food>> GetFoodById(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid food ID");

			try
			{
				logger.LogInformation("Fetching food {FoodId}", id);
				
				Food? food = await libraryUnitOfWork.FoodRepository.ReadAsync(id);
				
				if (food == null)
					return NotFound($"Food with ID {id} not found");

				return Ok(food);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetFoodById failed {FoodId}", id);
				return StatusCode(500, "An error occurred while fetching food");
			}
		}

		[HttpGet]
		public async Task<ActionResult<List<Hall>>> GetHalls()
		{
			try
			{
				logger.LogInformation("Fetching halls");
				
				List<Hall> halls = await libraryUnitOfWork.HallRepository.ReadAllAsync();
				
				return Ok(halls);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetHalls failed");
				return StatusCode(500, "An error occurred while fetching halls");
			}
		}

		[HttpGet]
		public async Task<ActionResult<Hall>> GetHallById(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid hall ID");

			try
			{
				logger.LogInformation("Fetching hall {HallId}", id);

				Hall? hall = await libraryUnitOfWork.HallRepository.ReadAsync(id);
				
				if (hall == null)
					return NotFound($"Hall with ID {id} not found");

				hall.Ratings =
					libraryUnitOfWork.RatingRepository
						.ReadRatingsByHallIdId(hall.HallId);

				foreach (Rating rating in hall.Ratings)
				{
					rating.RatingImages =
						libraryUnitOfWork.RatingImageRepository
							.ReadByRatingId(rating.RatingId);

					rating.UserRating =
						libraryUnitOfWork.UserRepository
							.Read(rating.UserId);
				}

				return Ok(hall);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetHallById failed {HallId}", id);
				return StatusCode(500, "An error occurred while fetching hall");
			}
		}

		[HttpGet]
		public ActionResult<MenuListVIewModel> GetMenus()
		{
			try
			{
				logger.LogInformation("Fetching menus (visitor)");

				MenuListVIewModel model = new MenuListVIewModel
				{
					Menus = libraryUnitOfWork.MenuRepository.ReadAll(),
					Foods = libraryUnitOfWork.FoodRepository.ReadAll(),
					FoodTypes = libraryUnitOfWork.FoodTypeRepository.ReadAll(),
					Halls = libraryUnitOfWork.HallRepository.ReadAll()
				};

				foreach (Menu menu in model.Menus)
				{
					menu.FoodIds =
						libraryUnitOfWork.MenuRepository
							.GetFoodIdsBy(menu.MenuId);

					menu.FoodTypeIds =
						libraryUnitOfWork.MenuRepository
							.GetFoodTypeIdsBy(menu.MenuId);
				}

				return Ok(model);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetMenus failed");
				return StatusCode(500, "An error occurred while fetching menus");
			}
		}

		[HttpGet]
		public ActionResult<Menu> GetMenuById(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid menu ID");

			try
			{
				logger.LogInformation("Fetching menu {MenuId}", id);

				Menu? menu = libraryUnitOfWork.MenuRepository.Read(id);
				
				if (menu == null)
					return NotFound($"Menu with ID {id} not found");

				menu.Foods =
					libraryUnitOfWork.FoodRepository
						.GetFoodsByMenuId(menu.MenuId);

				return Ok(menu);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetMenuById failed {MenuId}", id);
				return StatusCode(500, "An error occurred while fetching menu");
			}
		}

		[HttpGet]
		public ActionResult<User> CheckLogin(string userName, string password)
		{
			if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
				return BadRequest("Username and password are required");

			try
			{
				logger.LogInformation("Login attempt for {UserName}", userName);

				User? user =
					libraryUnitOfWork.UserRepository
						.GetLoginByUserName(userName);

				if (user == null)
				{
					logger.LogWarning("User not found: {UserName}", userName);
					return Unauthorized("Invalid username or password");
				}

				if (!user.UserPassword.IsSamePassword(password))
				{
					logger.LogWarning("Invalid password for user: {UserName}", userName);
					return Unauthorized("Invalid username or password");
				}

				User? authenticatedUser = libraryUnitOfWork.UserRepository
					.GetUser2FAByUserName(userName);

				if (authenticatedUser == null)
				{
					logger.LogError("Failed to retrieve user info after authentication: {UserName}", userName);
					return StatusCode(500, "Authentication succeeded but failed to retrieve user information");
				}

				return Ok(authenticatedUser);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "CheckLogin failed for {UserName}", userName);
				return StatusCode(500, "An error occurred during login");
			}
		}

		[HttpGet]
		public ActionResult<UserType> UserTypeByUserId(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
				return BadRequest("UserId is required");

			try
			{
				logger.LogInformation("Fetching user type for {UserId}", userId);
				
				UserType? userType = libraryUnitOfWork.UserRepository.UserTypeByUserId(userId);
				
				if (userType == null)
					return NotFound($"User type for user {userId} not found");

				return Ok(userType);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "UserTypeByUserId failed for {UserId}", userId);
				return StatusCode(500, "An error occurred while fetching user type");
			}
		}
	}
}
