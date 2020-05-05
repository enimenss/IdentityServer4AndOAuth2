﻿using System;
using System.Collections.Generic;

namespace CrewCloudRepository.Models
{
    public partial class JobApplication
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RestaurantId { get; set; }
        public DateTime JobPostId { get; set; }
        public int Price { get; set; }
        public string CoverLetter { get; set; }
        public string Video { get; set; }
    }
}
