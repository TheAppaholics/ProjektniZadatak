﻿namespace TeamRegistrationApi.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string Email { get; set; }
    }
}
