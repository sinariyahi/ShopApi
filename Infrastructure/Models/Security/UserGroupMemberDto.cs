﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Security
{
    public class UserGroupMemberDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid UserGroupId { get; set; }


    }
}
