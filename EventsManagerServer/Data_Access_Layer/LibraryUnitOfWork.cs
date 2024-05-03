using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class LibraryUnitOfWork
    {
        EventTypeRepository eventTypeRepository;
        FoodRepository foodRepository;
        FoodTypeRepository foodTypeRepository;
        HallRepository hallRepository;
        MenuRepository menuRepository;
        OrderRepository orderRepository;
        RatingRepository ratingRepository;
        RatingImageRepository ratingImageRepository;
        UserRepository userRepository;
        UserTypeRepository userTypeRepository;

        DbContext dbContext;
        public LibraryUnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public EventTypeRepository EventTypeRepository
        {
            get
            {
                if (eventTypeRepository == null)
                {
                    this.eventTypeRepository = new EventTypeRepository(dbContext);
                }
                return eventTypeRepository;
            }
        }

        public FoodRepository FoodRepository
        {
            get
            {
                if (foodRepository == null)
                {
                    this.foodRepository = new FoodRepository(dbContext);
                }
                return foodRepository;
            }
        }

        public FoodTypeRepository FoodTypeRepository
        {
            get
            {
                if (foodTypeRepository == null)
                {
                    this.foodTypeRepository = new FoodTypeRepository(dbContext);
                }
                return foodTypeRepository;
            }
        }

        public HallRepository HallRepository
        {
            get
            {
                if (hallRepository == null)
                {
                    this.hallRepository = new HallRepository(dbContext);
                }
                return hallRepository;
            }
        }

        public MenuRepository MenuRepository
        {
            get
            {
                if (menuRepository == null)
                {
                    this.menuRepository = new MenuRepository(dbContext);
                }
                return menuRepository;
            }
        }

        
        public OrderRepository OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(dbContext);
                }
                return orderRepository;
            }
        }

        public RatingRepository RatingRepository
        {
            get
            {
                if (ratingRepository == null)
                {
                    this.ratingRepository = new RatingRepository(dbContext);
                }
                return ratingRepository;
            }
        }

        public RatingImageRepository RatingImageRepository
        {
            get
            {
                if (ratingImageRepository == null)
                {
                    this.ratingImageRepository = new RatingImageRepository(dbContext);
                }
                return ratingImageRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    this.userRepository = new UserRepository(dbContext);
                }
                return userRepository;
            }
        }

        public UserTypeRepository UserTypeRepository
        {
            get
            {
                if (userTypeRepository == null)
                {
                    this.userTypeRepository = new UserTypeRepository(dbContext);
                }
                return userTypeRepository;
            }
        }
    }
}