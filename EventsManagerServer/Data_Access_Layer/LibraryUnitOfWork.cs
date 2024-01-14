﻿using System;
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

        DbContext dbContext;
        public LibraryUnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public MessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                {
                    this.messageRepository = new MessageRepository(dbContext);
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