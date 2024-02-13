using System.Collections.Generic;
using System.Linq;

using Dapper;

using DataAccessLibrary.Models;

namespace DataAccessLibrary.SQLDataAccess
{
	public class SqlCrud : ICrud
	{
		private readonly string _connectionString;

		public SqlCrud(string connectionString)
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
			string sqlstatement = "INSERT INTO [dbo].[People] ([FirstName], [LastName], [IsActive], [EmployerId]) VALUES (@FirstName, @LastName, @IsActive, @EmployerId);";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT [Id] FROM [dbo].[People] WHERE [FirstName] = @FirstName AND [LastName] = @LastName AND [IsActive] = @IsActive AND [EmployerId] = @EmployerId;";
			person.Id = SqlDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreateEmployer(EmployerBaseModel employer)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@CompanyName", employer.CompanyName);
			string sqlstatement = "INSERT INTO [dbo].[Employers] ([CompanyName]) VALUES (@CompanyName);";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT [Id] FROM [dbo].[Employers] WHERE [CompanyName] = @CompanyName;";
			employer.Id = SqlDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreateAddress(AddressModel address)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@StreetAddress", address.StreetAddress);
			parameters.Add("@City", address.City);
			parameters.Add("@State", address.State);
			parameters.Add("@ZipCode", address.ZipCode);
			string sqlstatement = "INSERT INTO [dbo].[Addresses] ([StreetAddress], [City], [State], [ZipCode]) VALUES (@StreetAddress, @City, @State, @ZipCode);";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT [Id] FROM [dbo].[Addresses] WHERE [StreetAddress] = @StreetAddress AND [City] = @City AND [State] = @State AND [ZipCode] = @ZipCode;";
			address.Id = SqlDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreatePersonAddress(PersonAddressBaseModel personAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PersonId", personAddress.PersonId);
			parameters.Add("@AddressId", personAddress.AddressId);
			string sqlstatement = "INSERT INTO [dbo].[PersonAddresses] ([PersonId], [AddressId]) VALUES (@PersonId, @AddressId);";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT [Id] FROM [dbo].[PersonAddresses] WHERE [PersonId] = @PersonId AND [AddressId] = @AddressId;";
			personAddress.Id = SqlDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public void CreateEmployerAddress(EmployerAddressBaseModel employerAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@EmployerId", employerAddress.EmployerId);
			parameters.Add("@AddressId", employerAddress.AddressId);
			string sqlstatement = "INSERT INTO [dbo].[EmployerAddresses] ([EmployerId], [AddressId]) VALUES (@EmployerId, @AddressId);";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
			sqlstatement = "SELECT [Id] FROM [dbo].[EmployerAddresses] WHERE [EmployerId] = @EmployerId AND [AddressId] = @AddressId;";
			employerAddress.Id = SqlDataAccess.ReadData<int, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
		}

		public List<PersonBaseModel> RetrieveAllPeople()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT [Id], [FirstName], [LastName], [IsActive], [EmployerId] FROM [dbo].[People];";
			List<PersonBaseModel> output = SqlDataAccess.ReadData<PersonBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public PersonBaseModel RetrievePersonById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT [Id], [FirstName], [LastName], [IsActive], [EmployerId] FROM [dbo].[People] WHERE Id = @Id;";
			PersonBaseModel output = SqlDataAccess.ReadData<PersonBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<PersonBaseModel> RetrievePeopleByEmployerId(int employerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@EmployerId", employerId);
			string sqlstatement = "SELECT [Id], [FirstName], [LastName], [IsActive], [EmployerId] FROM [dbo].[People] WHERE EmployerId = @EmployerId;";
			List<PersonBaseModel> output = SqlDataAccess.ReadData<PersonBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<EmployerBaseModel> RetrieveAllEmployers()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT [Id], [CompanyName] FROM [dbo].[Employers];";
			List<EmployerBaseModel> output = SqlDataAccess.ReadData<EmployerBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public EmployerBaseModel RetrieveEmployerById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT [Id], [CompanyName] FROM [dbo].[Employers] WHERE Id = @Id;";
			EmployerBaseModel output = SqlDataAccess.ReadData<EmployerBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<AddressModel> RetrieveAllAddresses()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT [Id], [StreetAddress], [City], [State], [ZipCode] FROM [dbo].[Addresses];";
			List<AddressModel> output = SqlDataAccess.ReadData<AddressModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public AddressModel RetrieveAddressById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT [Id], [StreetAddress], [City], [State], [ZipCode] FROM [dbo].[Addresses] WHERE Id = @Id;";
			AddressModel output = SqlDataAccess.ReadData<AddressModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<PersonAddressBaseModel> RetrieveAllPersonAddresses()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT [Id], [PersonId], [AddressId] FROM [dbo].[PersonAddresses];";
			List<PersonAddressBaseModel> output = SqlDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public PersonAddressBaseModel RetrievePersonAddressById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT [Id], [PersonId], [AddressId] FROM [dbo].[PersonAddresses] WHERE Id = @Id;";
			PersonAddressBaseModel output = SqlDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<PersonAddressBaseModel> RetrievePersonAddressByPersonId(int personId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PersonId", personId);
			string sqlstatement = "SELECT [Id], [PersonId], [AddressId] FROM [dbo].[PersonAddresses] WHERE PersonId = @PersonId;";
			List<PersonAddressBaseModel> output = SqlDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<PersonAddressBaseModel> RetrievePersonAddressByAddressId(int addressId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AddressId", addressId);
			string sqlstatement = "SELECT [Id], [PersonId], [AddressId] FROM [dbo].[PersonAddresses] WHERE AddressId = @AddressId;";
			List<PersonAddressBaseModel> output = SqlDataAccess.ReadData<PersonAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<EmployerAddressBaseModel> RetrieveAllEmployerAddresses()
		{
			DynamicParameters parameters = new DynamicParameters();
			string sqlstatement = "SELECT [Id], [EmployerId], [AddressId] FROM [dbo].[EmployerAddresses];";
			List<EmployerAddressBaseModel> output = SqlDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public EmployerAddressBaseModel RetrieveEmployerAddressById(int id)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", id);
			string sqlstatement = "SELECT [Id], [EmployerId], [AddressId] FROM [dbo].[EmployerAddresses] WHERE Id = @Id;";
			EmployerAddressBaseModel output = SqlDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString).First();
			return output;
		}

		public List<EmployerAddressBaseModel> RetrieveEmployerAddressByEmployerId(int employerId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@EmployerId", employerId);
			string sqlstatement = "SELECT [Id], [EmployerId], [AddressId] FROM [dbo].[EmployerAddresses] WHERE EmployerId = @EmployerId;";
			List<EmployerAddressBaseModel> output = SqlDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
			return output;
		}

		public List<EmployerAddressBaseModel> RetrieveEmployerAddressByAddressId(int addressId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@AddressId", addressId);
			string sqlstatement = "SELECT [Id], [EmployerId], [AddressId] FROM [dbo].[EmployerAddresses] WHERE AddressId = @AddressId;";
			List<EmployerAddressBaseModel> output = SqlDataAccess.ReadData<EmployerAddressBaseModel, DynamicParameters>(sqlstatement, parameters, _connectionString);
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
			string sqlstatement = "UPDATE [dbo].[People] SET [FirstName] = @FirstName, [LastName] = @LastName, [IsActive] = @IsActive, [EmployerId] = @EmployerId WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdateEmployer(EmployerBaseModel employer)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employer.Id);
			parameters.Add("@CompanyName", employer.CompanyName);
			string sqlstatement = "UPDATE [dbo].[Employers] SET [CompanyName] = @CompanyName WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdateAddress(AddressModel address)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", address.Id);
			parameters.Add("@StreetAddress", address.StreetAddress);
			parameters.Add("@City", address.City);
			parameters.Add("@State", address.State);
			parameters.Add("@ZipCode", address.ZipCode);
			string sqlstatement = "UPDATE [dbo].[Addresses] SET [StreetAddress] = @StreetAddress, [City] = @City, [State] = @State, [ZipCode] = @ZipCode WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdatePersonAddress(PersonAddressBaseModel personAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", personAddress.Id);
			parameters.Add("@PersonId", personAddress.PersonId);
			parameters.Add("@AddressId", personAddress.AddressId);
			string sqlstatement = "UPDATE [dbo].[PersonAddresses] SET [PersonId] = @PersonId, [AddresId] = @AddressId WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void UpdateEmployerAddress(EmployerAddressBaseModel employerAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employerAddress.Id);
			parameters.Add("@EmployerId", employerAddress.EmployerId);
			parameters.Add("@AddressId", employerAddress.AddressId);
			string sqlstatement = "UPDATE [dbo].[EmployerAddresses] SET [EmployerId] = @EmployerId, [AddresId] = @AddressId WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeletePerson(PersonBaseModel person)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", person.Id);
			string sqlstatement = "DELETE FROM [dbo].[People] WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeleteEmployer(EmployerBaseModel employer)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employer.Id);
			string sqlstatement = "DELETE FROM [dbo].[Employers] WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeleteAddress(AddressModel address)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", address.Id);
			string sqlstatement = "DELETE FROM [dbo].[Addresses] WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeletePersonAddress(PersonAddressBaseModel personAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", personAddress.Id);
			string sqlstatement = "DELETE FROM [dbo].[PersonAddresses] WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}

		public void DeleteEmployerAddress(EmployerAddressBaseModel employerAddress)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Id", employerAddress.Id);
			string sqlstatement = "DELETE FROM [dbo].[EmployerAddresses] WHERE [Id] = @Id;";
			SqlDataAccess.WriteData(sqlstatement, parameters, _connectionString);
		}
	}
}
