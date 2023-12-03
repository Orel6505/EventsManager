using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class ModelFactory
    {
        FoodModelCreator foodModelCreator;
        HallModelCreator hallModelCreator;
        MenuModelCreator menuModelCreator;
        MessageModelCreator messageModelCreator;
        OrderModelCreator orderModelCreator;
        RatingModelCreator ratingModelCreator;
        RatingImageModelCreator ratingImageModelCreator;
        UserModelCreator userModelCreator;
        UserTypeModelCreator userTypeModelCreator;

        public FoodModelCreator FoodModelCreator 
        {
            get 
            { 
                if(foodModelCreator == null) 
                {
                    this.foodModelCreator = new FoodModelCreator();
                }
                return foodModelCreator;
            }
        }

        public HallModelCreator HallModelCreator
        {
            get
            {
                if (hallModelCreator == null)
                {
                    this.hallModelCreator = new HallModelCreator();
                }
                return hallModelCreator;
            }
        }

        public MenuModelCreator MenuModelCreator
        {
            get
            {
                if (menuModelCreator == null)
                {
                    this.menuModelCreator = new MenuModelCreator();
                }
                return menuModelCreator;
            }
        }

        public MessageModelCreator MessageModelCreator
        {
            get
            {
                if (messageModelCreator == null)
                {
                    this.messageModelCreator = new MessageModelCreator();
                }
                return messageModelCreator;
            }
        }

        public OrderModelCreator OrderModelCreator
        {
            get
            {
                if (orderModelCreator == null)
                {
                    this.orderModelCreator = new OrderModelCreator();
                }
                return orderModelCreator;
            }
        }

        public RatingModelCreator RatingModelCreator
        {
            get
            {
                if (ratingModelCreator == null)
                {
                    this.ratingModelCreator = new RatingModelCreator();
                }
                return ratingModelCreator;
            }
        }

        public RatingImageModelCreator RatingImageModelCreator
        {
            get
            {
                if (ratingImageModelCreator == null)
                {
                    this.ratingImageModelCreator = new RatingImageModelCreator();
                }
                return ratingImageModelCreator;
            }
        }

        public UserModelCreator UserModelCreator
        {
            get
            {
                if (userModelCreator == null)
                {
                    this.userModelCreator = new UserModelCreator();
                }
                return userModelCreator;
            }
        }

        public UserTypeModelCreator UserTypeModelCreator
        {
            get
            {
                if (userTypeModelCreator == null)
                {
                    this.userTypeModelCreator = new UserTypeModelCreator();
                }
                return userTypeModelCreator;
            }
        }
    }
}