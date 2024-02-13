using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
	public class PersonModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public EmployerModel? Employer { get; set; }
		public List<AddressModel> Addresses { get; set; } = new List<AddressModel>();
	}
}
