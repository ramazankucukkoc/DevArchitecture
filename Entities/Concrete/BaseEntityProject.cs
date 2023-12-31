﻿using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class BaseEntityProject : IEntity
    {
        public int Id { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastUpdatedUserId { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
    }
}