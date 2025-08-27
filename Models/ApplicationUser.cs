using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocoRealt.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        // Дополнительные свойства профиля
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        // Можно добавить поле для аватара (ссылка на изображение)
        [MaxLength(300)]
        public string? AvatarUrl { get; set; }

        // Пример: доп.телефон
        [Phone]
        public string? SecondaryPhone { get; set; }

        // Флаг активации профиля, если нужно
        public bool IsActive { get; set; } = true;

        // ... Добавляйте свои поля по необходимости
    }
}
