using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWeb_Razor.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		[DisplayName("Category Name")]
		[MaxLength(30)]
		public string Name { get; set; }
		[DisplayName("Display Order")]
		[Range(1, 100)]
		[Required]
		public int DisplayOrder { get; set; }
	}
}
