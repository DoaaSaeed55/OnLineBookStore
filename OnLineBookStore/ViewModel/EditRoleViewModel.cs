using System.ComponentModel.DataAnnotations;

namespace OnLineBookStore.ViewModel
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Display(Name ="Role")]
        [Required(ErrorMessage = "Role Name Is Required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}
