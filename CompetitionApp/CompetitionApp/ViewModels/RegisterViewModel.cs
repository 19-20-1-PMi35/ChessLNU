using System;
using System.ComponentModel.DataAnnotations;


namespace CompetitionApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логін")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Дата народження")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Університет")]
        public string University { get; set; }

        [Required]
        [Display(Name = "Факультет")]
        public string Faculty { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити пароль")]
        public string PasswordConfirm { get; set; }
    }
}
