using EventsManager.Data_Access_Layer;
using EventsManagerModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasswordManager;
using System;
using System.Collections.Generic;

namespace EventsManager.Controllers
{
	[ApiController]
	[Route("[Controller]/[Action]")]
	public class AdminController(
		LibraryUnitOfWork libraryUnitOfWork,
		ILogger<AdminController> logger) : ControllerBase
	{
		[HttpPost]
		public ActionResult<bool> Register([FromBody] User user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				logger.LogInformation("Registering user with username: {UserName}", user.UserName);

				if (string.IsNullOrWhiteSpace(user.TempPassword))
					return BadRequest("Password is required");

				user.UserPassword = new Password(user.TempPassword);
				user.CreationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

				bool result = libraryUnitOfWork.UserRepository.Insert(user);
				
				if (!result)
					return StatusCode(500, "Failed to register user");

				return Ok(true);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Register failed for username: {UserName}", user.UserName);
				return StatusCode(500, "An error occurred while registering the user");
			}
		}

		[HttpPost]
		public ActionResult<bool> Update([FromBody] User user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				logger.LogInformation("Updating user {UserId}", user.UserId);
				
				bool result = libraryUnitOfWork.UserRepository.UpdateInfo(user);
				
				if (!result)
					return NotFound($"User with ID {user.UserId} not found or update failed");

				return Ok(true);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Update failed for user {UserId}", user.UserId);
				return StatusCode(500, "An error occurred while updating the user");
			}
		}

		[HttpGet]
		public ActionResult<List<User>> GetUsers()
		{
			try
			{
				logger.LogInformation("Fetching all users");

				List<User> users = libraryUnitOfWork.UserRepository.ReadAll();

				foreach (User user in users)
				{
					user.UserType =
						libraryUnitOfWork.UserRepository
							.UserTypeByUserId(user.UserId.ToString());
				}

				return Ok(users);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetUsers failed");
				return StatusCode(500, "An error occurred while fetching users");
			}
		}

		[HttpGet]
		public ActionResult<User> UserDetails(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid user ID");

			try
			{
				logger.LogInformation("Fetching user details for {UserId}", id);
				
				User? user = libraryUnitOfWork.UserRepository.Read(id);
				
				if (user == null)
					return NotFound($"User with ID {id} not found");

				return Ok(user);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "UserDetails failed for {UserId}", id);
				return StatusCode(500, "An error occurred while fetching user details");
			}
		}

		[HttpGet]
		public ActionResult<List<Order>> GetOrders()
		{
			try
			{
				logger.LogInformation("Fetching all orders");

				List<Order> orders = libraryUnitOfWork.OrderRepository.ReadAll();

				foreach (Order order in orders)
				{
					order.ChosenMenu =
						libraryUnitOfWork.MenuRepository.Read(order.MenuId);

					order.ChosenHall =
						libraryUnitOfWork.HallRepository.Read(order.HallId);

					order.ChosenUser =
						libraryUnitOfWork.UserRepository.Read(order.UserId);

					order.EventType =
						libraryUnitOfWork.EventTypeRepository.Read(order.EventTypeId);
				}

				return Ok(orders);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetOrders failed");
				return StatusCode(500, "An error occurred while fetching orders");
			}
		}

		[HttpGet]
		public ActionResult<List<Menu>> GetMenus()
		{
			try
			{
				logger.LogInformation("Fetching menus");

				List<Menu> menus = libraryUnitOfWork.MenuRepository.ReadAll();

				foreach (Menu menu in menus)
				{
					menu.ChosenHall =
						libraryUnitOfWork.HallRepository.Read(menu.HallId);
				}

				return Ok(menus);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetMenus failed");
				return StatusCode(500, "An error occurred while fetching menus");
			}
		}

		[HttpGet]
		public ActionResult<List<Food>> GetFoods()
		{
			try
			{
				logger.LogInformation("Fetching foods");
				
				List<Food> foods = libraryUnitOfWork.FoodRepository.ReadAll();
				
				return Ok(foods);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetFoods failed");
				return StatusCode(500, "An error occurred while fetching foods");
			}
		}

		[HttpPost]
		public ActionResult<bool> NewOrder([FromBody] Order order)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				logger.LogInformation("Creating new order for user {UserId}", order.UserId);
				
				bool result = libraryUnitOfWork.OrderRepository.Insert(order);
				
				if (!result)
					return StatusCode(500, "Failed to create order");

				return Ok(true);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "NewOrder failed for user {UserId}", order.UserId);
				return StatusCode(500, "An error occurred while creating the order");
			}
		}
	}
}
