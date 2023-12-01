﻿using System;
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
    }
}