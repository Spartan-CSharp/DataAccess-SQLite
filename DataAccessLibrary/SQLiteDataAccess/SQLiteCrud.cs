using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dapper;

using DataAccessLibrary.Models;

namespace DataAccessLibrary.SQLiteDataAccess
{
	public class SQLiteCrud : ICrud
	{
		private readonly string _connectionString;

		public SQLiteCrud(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void CreatePerson(PersonBaseModel person)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@FirstName", person.FirstName);
			parameters.Add("@LastName", person.LastName);
			parameters.Add("@IsActive", person.IsActive);
			parameters.Add("@EmployerId", person.EmployerId);
			string sqlstatement = "INSERT INTO People (FirstName, LastName, IsActive, EmployerId) VALUES (@FirstName, @LastName, @IsActive, @EmployerId);";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT Id FROM People WHERE FirstName = @FirstName AND LastName = @LastName AND IsActive = @IsActive AND EmployerId = @EmployerId;";
			person.Id = SQLiteDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreateEmployer(EmployerBaseModel employer)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@CompanyName", employer.CompanyName);
			string sqlstatement = "INSERT INTO Employers (CompanyName) VALUES (@CompanyName);";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT Id FROM Employers WHERE CompanyName = @CompanyName;";
			employer.Id = SQLiteDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreateAddress(AddressModel address)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@StreetAddress", address.StreetAddress);
			parameters.Add("@City", address.City);
			parameters.Add("@State", address.State);
			parameters.Add("@ZipCode", address.ZipCode);
			string sqlstatement = "INSERT INTO Addresses (StreetAddress, City, State, ZipCode) VALUES (@StreetAddress, @City, @State, @ZipCode);";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT Id FROM Addresses WHERE StreetAddress = @StreetAddress AND City = @City AND State = @State AND ZipCode = @ZipCode;";
			address.Id = SQLiteDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreatePersonAddress(PersonAddressBaseModel personAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PersonId", personAddress.PersonId);
			parameters.Add("@AddressId", personAddress.AddressId);
			string sqlstatement = "INSERT INTO PersonAddresses (PersonId, AddressId) VALUES (@PersonId, @AddressId);";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT Id FROM PersonAddresses WHERE PersonId = @PersonId AND AddressId = @AddressId;";
			personAddress.Id = SQLiteDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreateEmployerAddress(EmployerAddressBaseModel employerAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@EmployerId", employerAddress.EmployerId);
			parameters.Add("@AddressId", employerAddress.AddressId);
			string sqlstatement = "INSERT INTO EmployerAddresses (EmployerId, AddressId) VALUES (@EmployerId, @AddressId);";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT Id FROM EmployerAddresses WHERE EmployerId = @EmployerId AND AddressId = @AddressId;";
			employerAddress.Id = SQLiteDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public List<PersonBaseModel> RetrieveAllPeople()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT Id, FirstName, LastName, IsActive, EmployerId FROM People;";
			List<PersonBaseModel> output = SQLiteDataAccess.ReadData<PersonBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public PersonBaseModel RetrievePersonById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT Id, FirstName, LastName, IsActive, EmployerId FROM People WHERE Id = @Id;";
			PersonBaseModel output = SQLiteDataAccess.ReadData<PersonBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<PersonBaseModel> RetrievePeopleByEmployerId(int employerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@EmployerId", employerId);
			string sqlstatement = "SELECT Id, FirstName, LastName, IsActive, EmployerId FROM People WHERE EmployerId = @EmployerId;";
			List<PersonBaseModel> output = SQLiteDataAccess.ReadData<PersonBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<EmployerBaseModel> RetrieveAllEmployers()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT Id, CompanyName FROM Employers;";
			List<EmployerBaseModel> output = SQLiteDataAccess.ReadData<EmployerBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public EmployerBaseModel RetrieveEmployerById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT Id, CompanyName FROM Employers WHERE Id = @Id;";
			EmployerBaseModel output = SQLiteDataAccess.ReadData<EmployerBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<AddressModel> RetrieveAllAddresses()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT Id, StreetAddress, City, State, ZipCode FROM Addresses;";
			List<AddressModel> output = SQLiteDataAccess.ReadData<AddressModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public AddressModel RetrieveAddressById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT Id, StreetAddress, City, State, ZipCode FROM Addresses WHERE Id = @Id;";
			AddressModel output = SQLiteDataAccess.ReadData<AddressModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<PersonAddressBaseModel> RetrieveAllPersonAddresses()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT Id, PersonId, AddressId FROM PersonAddresses;";
			List<PersonAddressBaseModel> output = SQLiteDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public PersonAddressBaseModel RetrievePersonAddressById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT Id, PersonId, AddressId FROM PersonAddresses WHERE Id = @Id;";
			PersonAddressBaseModel output = SQLiteDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<PersonAddressBaseModel> RetrievePersonAddressByPersonId(int personId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PersonId", personId);
			string sqlstatement = "SELECT Id, PersonId, AddressId FROM PersonAddresses WHERE PersonId = @PersonId;";
			List<PersonAddressBaseModel> output = SQLiteDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<PersonAddressBaseModel> RetrievePersonAddressByAddressId(int addressId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AddressId", addressId);
			string sqlstatement = "SELECT Id, PersonId, AddressId FROM PersonAddresses WHERE AddressId = @AddressId;";
			List<PersonAddressBaseModel> output = SQLiteDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<EmployerAddressBaseModel> RetrieveAllEmployerAddresses()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT Id, EmployerId, AddressId FROM EmployerAddresses;";
			List<EmployerAddressBaseModel> output = SQLiteDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public EmployerAddressBaseModel RetrieveEmployerAddressById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT Id, EmployerId, AddressId FROM EmployerAddresses WHERE Id = @Id;";
			EmployerAddressBaseModel output = SQLiteDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<EmployerAddressBaseModel> RetrieveEmployerAddressByEmployerId(int employerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@EmployerId", employerId);
			string sqlstatement = "SELECT Id, EmployerId, AddressId FROM EmployerAddresses WHERE EmployerId = @EmployerId;";
			List<EmployerAddressBaseModel> output = SQLiteDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<EmployerAddressBaseModel> RetrieveEmployerAddressByAddressId(int addressId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AddressId", addressId);
			string sqlstatement = "SELECT Id, EmployerId, AddressId FROM EmployerAddresses WHERE AddressId = @AddressId;";
			List<EmployerAddressBaseModel> output = SQLiteDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public void UpdatePerson(PersonBaseModel person)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", person.Id);
			parameters.Add("@FirstName", person.FirstName);
			parameters.Add("@LastName", person.LastName);
			parameters.Add("@IsActive", person.IsActive);
			parameters.Add("@EmployerId", person.EmployerId);
			string sqlstatement = "UPDATE People SET FirstName = @FirstName, LastName = @LastName, IsActive = @IsActive, EmployerId = @EmployerId WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdateEmployer(EmployerBaseModel employer)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employer.Id);
			parameters.Add("@CompanyName", employer.CompanyName);
			string sqlstatement = "UPDATE Employers SET CompanyName = @CompanyName WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdateAddress(AddressModel address)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", address.Id);
			parameters.Add("@StreetAddress", address.StreetAddress);
			parameters.Add("@City", address.City);
			parameters.Add("@State", address.State);
			parameters.Add("@ZipCode", address.ZipCode);
			string sqlstatement = "UPDATE Addresses SET StreetAddress = @StreetAddress, City = @City, State = @State, ZipCode = @ZipCode WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdatePersonAddress(PersonAddressBaseModel personAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", personAddress.Id);
			parameters.Add("@PersonId", personAddress.PersonId);
			parameters.Add("@AddressId", personAddress.AddressId);
			string sqlstatement = "UPDATE PersonAddresses SET PersonId = @PersonId, AddresId = @AddressId WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdateEmployerAddress(EmployerAddressBaseModel employerAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employerAddress.Id);
			parameters.Add("@EmployerId", employerAddress.EmployerId);
			parameters.Add("@AddressId", employerAddress.AddressId);
			string sqlstatement = "UPDATE EmployerAddresses SET EmployerId = @EmployerId, AddresId = @AddressId WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeletePerson(PersonBaseModel person)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", person.Id);
			string sqlstatement = "DELETE FROM People WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeleteEmployer(EmployerBaseModel employer)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employer.Id);
			string sqlstatement = "DELETE FROM Employers WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeleteAddress(AddressModel address)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", address.Id);
			string sqlstatement = "DELETE FROM Addresses WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeletePersonAddress(PersonAddressBaseModel personAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", personAddress.Id);
			string sqlstatement = "DELETE FROM PersonAddresses WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeleteEmployerAddress(EmployerAddressBaseModel employerAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employerAddress.Id);
			string sqlstatement = "DELETE FROM EmployerAddresses WHERE Id = @Id;";
			SQLiteDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}
	}
}
