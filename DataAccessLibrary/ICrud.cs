using System.Collections.Generic;

using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
	public interface ICrud
	{
		void CreateAddress(AddressModel address);
		void CreateEmployer(EmployerBaseModel employer);
		void CreateEmployerAddress(EmployerAddressBaseModel employerAddress);
		void CreatePerson(PersonBaseModel person);
		void CreatePersonAddress(PersonAddressBaseModel personAddress);
		void DeleteAddress(AddressModel address);
		void DeleteEmployer(EmployerBaseModel employer);
		void DeleteEmployerAddress(EmployerAddressBaseModel employerAddress);
		void DeletePerson(PersonBaseModel person);
		void DeletePersonAddress(PersonAddressBaseModel personAddress);
		AddressModel RetrieveAddressById(int id);
		List<AddressModel> RetrieveAllAddresses();
		List<EmployerAddressBaseModel> RetrieveAllEmployerAddresses();
		List<EmployerBaseModel> RetrieveAllEmployers();
		List<PersonBaseModel> RetrieveAllPeople();
		List<PersonAddressBaseModel> RetrieveAllPersonAddresses();
		List<EmployerAddressBaseModel> RetrieveEmployerAddressByAddressId(int addressId);
		List<EmployerAddressBaseModel> RetrieveEmployerAddressByEmployerId(int employerId);
		EmployerAddressBaseModel RetrieveEmployerAddressById(int id);
		EmployerBaseModel RetrieveEmployerById(int id);
		List<PersonBaseModel> RetrievePeopleByEmployerId(int employerId);
		List<PersonAddressBaseModel> RetrievePersonAddressByAddressId(int addressId);
		PersonAddressBaseModel RetrievePersonAddressById(int id);
		List<PersonAddressBaseModel> RetrievePersonAddressByPersonId(int personId);
		PersonBaseModel RetrievePersonById(int id);
		void UpdateAddress(AddressModel address);
		void UpdateEmployer(EmployerBaseModel employer);
		void UpdateEmployerAddress(EmployerAddressBaseModel employerAddress);
		void UpdatePerson(PersonBaseModel person);
		void UpdatePersonAddress(PersonAddressBaseModel personAddress);
	}
}
