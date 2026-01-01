using Microsoft.Extensions.Logging;

namespace EventsManager.Data_Access_Layer
{
	public class LibraryUnitOfWork
	{
		private readonly DbContext dbContext;
		private readonly ILoggerFactory loggerFactory;

		private EventTypeRepository eventTypeRepository;
		private FoodRepository foodRepository;
		private FoodTypeRepository foodTypeRepository;
		private HallRepository hallRepository;
		private MenuRepository menuRepository;
		private OrderRepository orderRepository;
		private RatingRepository ratingRepository;
		private RatingImageRepository ratingImageRepository;
		private UserRepository userRepository;
		private UserTypeRepository userTypeRepository;

		public LibraryUnitOfWork(
			DbContext dbContext,
			ILoggerFactory loggerFactory)
		{
			this.dbContext = dbContext;
			this.loggerFactory = loggerFactory;
		}

		public EventTypeRepository EventTypeRepository =>
			eventTypeRepository ??= new EventTypeRepository(
				dbContext,
				loggerFactory.CreateLogger<EventTypeRepository>());

		public FoodRepository FoodRepository =>
			foodRepository ??= new FoodRepository(
				dbContext,
				loggerFactory.CreateLogger<FoodRepository>());

		public FoodTypeRepository FoodTypeRepository =>
			foodTypeRepository ??= new FoodTypeRepository(
				dbContext,
				loggerFactory.CreateLogger<FoodTypeRepository>());

		public HallRepository HallRepository =>
			hallRepository ??= new HallRepository(
				dbContext,
				loggerFactory.CreateLogger<HallRepository>());

		public MenuRepository MenuRepository =>
			menuRepository ??= new MenuRepository(
				dbContext,
				loggerFactory.CreateLogger<MenuRepository>());

		public OrderRepository OrderRepository =>
			orderRepository ??= new OrderRepository(
				dbContext,
				loggerFactory.CreateLogger<OrderRepository>());

		public RatingRepository RatingRepository =>
			ratingRepository ??= new RatingRepository(
				dbContext,
				loggerFactory.CreateLogger<RatingRepository>());

		public RatingImageRepository RatingImageRepository =>
			ratingImageRepository ??= new RatingImageRepository(
				dbContext,
				loggerFactory.CreateLogger<RatingImageRepository>());

		public UserRepository UserRepository =>
			userRepository ??= new UserRepository(
				dbContext,
				loggerFactory.CreateLogger<UserRepository>());

		public UserTypeRepository UserTypeRepository =>
			userTypeRepository ??= new UserTypeRepository(
				dbContext,
				loggerFactory.CreateLogger<UserTypeRepository>());
	}
}
