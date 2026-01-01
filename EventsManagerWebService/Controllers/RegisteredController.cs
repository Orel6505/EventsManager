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
	public class RegisteredController(
		LibraryUnitOfWork libraryUnitOfWork,
		ILogger<RegisteredController> logger) : ControllerBase
	{
		[HttpPost("[Controller]/Register")]
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

		[HttpPost("[Controller]/UpdateUser")]
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
				logger.LogError(ex, "UpdateUser failed {UserId}", user.UserId);
				return StatusCode(500, "An error occurred while updating the user");
			}
		}

		[HttpPost("[Controller]/UpdatePassword")]
		public ActionResult<bool> UpdatePassword([FromBody] User user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				logger.LogInformation("Updating password for {UserId}", user.UserId);

				if (string.IsNullOrWhiteSpace(user.TempPassword))
					return BadRequest("Password is required");

				user.UserPassword = new Password(user.TempPassword);
				
				bool result = libraryUnitOfWork.UserRepository.UpdatePassword(user);
				
				if (!result)
					return NotFound($"User with ID {user.UserId} not found or update failed");

				return Ok(true);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "UpdatePassword failed {UserId}", user.UserId);
				return StatusCode(500, "An error occurred while updating the password");
			}
		}

		[HttpGet]
		public ActionResult<bool> CheckPassword(string userId, string password)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
				return BadRequest("UserId and password are required");

			try
			{
				logger.LogInformation("Checking password for {UserId}", userId);

				Password? storedPassword =
					libraryUnitOfWork.UserRepository.GetPasswordByUserId(userId);

				if (storedPassword == null)
					return NotFound("User not found");

				bool isValid = storedPassword.IsSamePassword(password);
				
				return Ok(isValid);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "CheckPassword failed {UserId}", userId);
				return StatusCode(500, "An error occurred while checking the password");
			}
		}

		[HttpGet]
		public ActionResult<bool> IsAvailableUserName(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName))
				return BadRequest("Username is required");

			try
			{
				logger.LogInformation("Checking username availability {UserName}", userName);
				
				bool isAvailable = !libraryUnitOfWork.UserRepository.IsAvailableUserName(userName);
				
				return Ok(isAvailable);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "IsAvailableUserName failed {UserName}", userName);
				return StatusCode(500, "An error occurred while checking username availability");
			}
		}

		[HttpGet]
		public ActionResult<OrderListVIewModel> GetOrders(int userId)
		{
			if (userId <= 0)
				return BadRequest("Invalid user ID");

			try
			{
				logger.LogInformation("Fetching orders for user {UserId}", userId);

				OrderListVIewModel model = new OrderListVIewModel
				{
					Orders = libraryUnitOfWork.OrderRepository.ReadByUserId(userId),
					Halls = libraryUnitOfWork.HallRepository.ReadAll()
				};

				foreach (Order order in model.Orders)
				{
					order.ChosenMenu =
						libraryUnitOfWork.MenuRepository.Read(order.MenuId);

					order.EventType =
						libraryUnitOfWork.EventTypeRepository.Read(order.EventTypeId);
				}

				return Ok(model);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "GetOrders failed for user {UserId}", userId);
				return StatusCode(500, "An error occurred while fetching orders");
			}
		}

		[HttpGet]
		public ActionResult<User> UserDetails(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid user ID");

			try
			{
				logger.LogInformation("Fetching user details {UserId}", id);
				
				User? user = libraryUnitOfWork.UserRepository.Read(id);
				
				if (user == null)
					return NotFound($"User with ID {id} not found");

				return Ok(user);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "UserDetails failed {UserId}", id);
				return StatusCode(500, "An error occurred while fetching user details");
			}
		}

		[HttpPost]
		public ActionResult<bool> NewRate([FromBody] Rating rating)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				logger.LogInformation("Creating new rating for hall {HallId} by user {UserId}", 
					rating.HallId, rating.UserId);

				if (!libraryUnitOfWork.RatingRepository.Insert(rating))
					return StatusCode(500, "Failed to create rating");

				if (rating.RatingImagesLocation?.Count > 0)
				{
					foreach (string file in rating.RatingImagesLocation)
					{
						int ratingId =
							libraryUnitOfWork.RatingRepository
								.ReadByRatingDate(rating.RatingDate);

						if (ratingId == 0)
						{
							logger.LogError("Invalid RatingId after inserting rating");
							return StatusCode(500, "Failed to retrieve rating ID");
						}

						libraryUnitOfWork.RatingImageRepository.Insert(
							new RatingImage
							{
								ImageLocation = file,
								RatingId = ratingId
							});
					}
				}

				return Ok(true);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "NewRate failed for hall {HallId}", rating.HallId);
				return StatusCode(500, "An error occurred while creating the rating");
			}
		}

		[HttpGet]
		public ActionResult<List<EventType>> HallAvailability(string eventDate, string hallId)
		{
			if (string.IsNullOrWhiteSpace(eventDate) || string.IsNullOrWhiteSpace(hallId))
				return BadRequest("EventDate and HallId are required");

			try
			{
				logger.LogInformation(
					"Checking hall availability {HallId} for {EventDate}",
					hallId,
					eventDate);

				List<EventType> availableEventTypes = libraryUnitOfWork
					.EventTypeRepository
					.HallAvailability(eventDate, hallId);

				return Ok(availableEventTypes);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "HallAvailability failed {HallId} for {EventDate}", hallId, eventDate);
				return StatusCode(500, "An error occurred while checking hall availability");
			}
		}

		[HttpGet]
		public ActionResult<NewOrderViewModel> OrderMenus(string hallId)
		{
			if (string.IsNullOrWhiteSpace(hallId))
				return BadRequest("HallId is required");

			try
			{
				logger.LogInformation("Fetching order menus for hall {HallId}", hallId);

				Hall? hall = libraryUnitOfWork.HallRepository.Read(hallId);
				
				if (hall == null)
					return NotFound($"Hall with ID {hallId} not found");

				NewOrderViewModel model = new NewOrderViewModel
				{
					Menus = libraryUnitOfWork.MenuRepository.GetAllByHallId(hallId),
					OrderHall = hall
				};

				return Ok(model);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "OrderMenus failed {HallId}", hallId);
				return StatusCode(500, "An error occurred while fetching order menus");
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
