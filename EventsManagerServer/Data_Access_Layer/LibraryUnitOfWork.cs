using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class LibraryUnitOfWork
    {
        FoodRepository foodRepository;
        HallRepository hallRepository;
        MenuRepository menuRepository;
        MessageRepository messageRepository;
        OrderRepository orderRepository;
        RatingRepository ratingRepository;
        RatingImageRepository ratingImageRepository;
        UserRepository userRepository;
        UserTypeRepository userTypeRepository;

        public FoodRepository FoodRepository
        {
            get
            {
                if (foodRepository == null)
                {
                    this.foodRepository = new FoodRepository();
                }
                return foodRepository;
            }
        }

        public HallRepository HallRepository
        {
            get
            {
                if (hallRepository == null)
                {
                    this.hallRepository = new HallRepository();
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
                    this.menuRepository = new MenuRepository();
                }
                return menuRepository;
            }
        }

        public MessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                {
                    this.messageRepository = new MessageRepository();
                }
                return messageRepository;
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    this.orderRepository = new OrderRepository();
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
                    this.ratingRepository = new RatingRepository();
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
                    this.ratingImageRepository = new RatingImageRepository();
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
                    this.userRepository = new UserRepository();
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
                    this.userTypeRepository = new UserTypeRepository();
                }
                return userTypeRepository;
            }
        }
    }
}