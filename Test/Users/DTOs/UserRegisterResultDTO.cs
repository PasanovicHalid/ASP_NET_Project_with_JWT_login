﻿namespace TestAPI.Users.DTOs
{
    public class UserRegisterResultDTO
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
