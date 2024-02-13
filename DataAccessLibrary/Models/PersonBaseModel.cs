namespace DataAccessLibrary.Models
{
	public class PersonBaseModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public int? EmployerId { get; set; }
	}
}
