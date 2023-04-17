using System.ComponentModel.DataAnnotations;

namespace AuditTest.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}