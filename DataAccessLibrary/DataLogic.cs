using System.Collections.Generic;

using DataAccessLibrary.Models;
using DataAccessLibrary.SQLDataAccess;
using DataAccessLibrary.SQLiteDataAccess;

using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary
{
	public class DataLogic : IDataLogic
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;
		private readonly ICrud _crud;

		public DataLogic(IConfiguration configuration, DBTYPES dbType)
		{
			_configuration = configuration;
			switch ( dbType )
			{
				case DBTYPES.SQLServer:
					_connectionString = _configuration.GetConnectionString("SQLServer");
					_crud = new SqlCrud(_connectionString);
					break;
				case DBTYPES.SQLite:
					_connectionString = _configuration.GetConnectionString("SQLite");
					_crud = new SQLiteCrud(_connectionString);
					break;
				default:
					_connectionString = _configuration.GetConnectionString("SQLServer");
					_crud = new SqlCrud(_connectionString);
					break;
			}
		}

		public void SaveNewPerson(PersonModel person)
		{
			PersonBaseModel basePerson = new PersonBaseModel
			{
				FirstName = person.FirstName,
				LastName = person.LastName,
				IsActive = person.IsActive
			};

			if ( person.Employer == null )
			{
				basePerson.EmployerId = null;
			}
			else if ( person.Employer.Id == 0 )
			{
				SaveNewEmployer(person.Employer);
				basePerson.EmployerId = person.Employer.Id;
			}
			else
			{
				basePerson.EmployerId = person.Employer.Id;
			}

			_crud.CreatePerson(basePerson);
			person.Id = basePerson.Id;

			foreach ( AddressModel address in person.Addresses )
			{
				if ( address.Id == 0 )
				{
					SaveNewAddress(address);
				}

				PersonAddressBaseModel baseAddress = new PersonAddressBaseModel
				{
					PersonId = person.Id,
					AddressId = address.Id
				};

				_crud.CreatePersonAddress(baseAddress);
			}
		}

		public void SaveNewEmployer(EmployerModel employer)
		{
			EmployerBaseModel baseEmployer = new EmployerBaseModel
			{
				CompanyName = employer.CompanyName
			};

			_crud.CreateEmployer(baseEmployer);
			employer.Id = baseEmployer.Id;

			foreach ( AddressModel address in employer.Addresses )
			{
				if ( address.Id == 0 )
				{
					SaveNewAddress(address);
				}

				EmployerAddressBaseModel baseAddress = new EmployerAddressBaseModel
				{
					EmployerId = employer.Id,
					AddressId = address.Id
				};

				_crud.CreateEmployerAddress(baseAddress);
			}
		}

		public void SaveNewAddress(AddressModel address)
		{
			_crud.CreateAddress(address);
		}

		public List<PersonModel> GetAllPeople()
		{
			List<PersonModel> output = new List<PersonModel>();

			List<PersonBaseModel> basePeople = _crud.RetrieveAllPeople();

			foreach ( PersonBaseModel basePerson in basePeople )
			{
				PersonModel person = new PersonModel
				{
					Id = basePerson.Id,
					FirstName = basePerson.FirstName,
					LastName = basePerson.LastName,
					IsActive = basePerson.IsActive
				};

				if ( basePerson.EmployerId != null && basePerson.EmployerId != 0 )
				{
					EmployerModel employer = GetEmployerById((int)basePerson.EmployerId);
					person.Employer = employer;
				}

				List<PersonAddressBaseModel> personAddresses = _crud.RetrievePersonAddressByPersonId(basePerson.Id);
				foreach ( PersonAddressBaseModel personAddress in personAddresses )
				{
					AddressModel address = _crud.RetrieveAddressById(personAddress.AddressId);
					person.Addresses.Add(address);
				}

				output.Add(person);
			}

			return output;
		}

		public List<EmployerModel> GetAllEmployers()
		{
			List<EmployerModel> output = new List<EmployerModel>();

			List<EmployerBaseModel> baseEmployers = _crud.RetrieveAllEmployers();

			foreach ( EmployerBaseModel baseEmployer in baseEmployers )
			{
				EmployerModel employer = new EmployerModel
				{
					Id = baseEmployer.Id,
					CompanyName = baseEmployer.CompanyName
				};

				List<EmployerAddressBaseModel> employerAddresses = _crud.RetrieveEmployerAddressByEmployerId(baseEmployer.Id);
				foreach ( EmployerAddressBaseModel employerAddress in employerAddresses )
				{
					AddressModel address = _crud.RetrieveAddressById(employerAddress.AddressId);
					employer.Addresses.Add(address);
				}

				output.Add(employer);
			}

			return output;
		}

		public List<AddressModel> GetAllAddresses()
		{
			List<AddressModel> output = _crud.RetrieveAllAddresses();
			return output;
		}

		public PersonModel GetPersonById(int personId)
		{
			PersonBaseModel basePerson = _crud.RetrievePersonById(personId);
			PersonModel output = new PersonModel
			{
				Id = basePerson.Id,
				FirstName = basePerson.FirstName,
				LastName = basePerson.LastName,
				IsActive = basePerson.IsActive
			};

			if ( basePerson.EmployerId != null && basePerson.EmployerId != 0 )
			{
				EmployerModel employer = GetEmployerById((int)basePerson.EmployerId);
				output.Employer = employer;
			}

			List<PersonAddressBaseModel> personAddresses = _crud.RetrievePersonAddressByPersonId(basePerson.Id);
			foreach ( PersonAddressBaseModel personAddress in personAddresses )
			{
				AddressModel address = _crud.RetrieveAddressById(personAddress.AddressId);
				output.Addresses.Add(address);
			}

			return output;
		}

		public EmployerModel GetEmployerById(int employerId)
		{
			EmployerBaseModel baseEmployer = _crud.RetrieveEmployerById(employerId);
			EmployerModel output = new EmployerModel
			{
				Id = baseEmployer.Id,
				CompanyName = baseEmployer.CompanyName
			};

			List<EmployerAddressBaseModel> employerAddresses = _crud.RetrieveEmployerAddressByEmployerId(baseEmployer.Id);
			foreach ( EmployerAddressBaseModel employerAddress in employerAddresses )
			{
				AddressModel address = _crud.RetrieveAddressById(employerAddress.AddressId);
				output.Addresses.Add(address);
			}

			return output;
		}

		public AddressModel GetAddressById(int addressId)
		{
			AddressModel output = _crud.RetrieveAddressById(addressId);
			return output;
		}

		public void UpdatePerson(PersonModel person)
		{
			PersonBaseModel basePerson = new PersonBaseModel
			{
				Id = person.Id,
				FirstName = person.FirstName,
				LastName = person.LastName,
				IsActive = person.IsActive
			};

			if ( person.Employer == null )
			{
				basePerson.EmployerId = null;
			}
			else if ( person.Employer.Id == 0 )
			{
				SaveNewEmployer(person.Employer);
				basePerson.EmployerId = person.Employer.Id;
			}
			else
			{
				basePerson.EmployerId = person.Employer.Id;
			}

			_crud.UpdatePerson(basePerson);

			List<PersonAddressBaseModel> linkedAddresses = _crud.RetrievePersonAddressByPersonId(person.Id);

			foreach ( AddressModel address in person.Addresses )
			{
				if ( address.Id == 0 )
				{
					SaveNewAddress(address);
					PersonAddressBaseModel baseAddress = new PersonAddressBaseModel
					{
						PersonId = person.Id,
						AddressId = address.Id
					};

					_crud.CreatePersonAddress(baseAddress);
				}
				else
				{
					PersonAddressBaseModel found = linkedAddresses.Find(x => x.AddressId == address.Id);
					if ( found == null )
					{
						PersonAddressBaseModel baseAddress = new PersonAddressBaseModel
						{
							PersonId = person.Id,
							AddressId = address.Id
						};

						_crud.CreatePersonAddress(baseAddress);
					}
					else
					{
						_ = linkedAddresses.Remove(found);
					}
				}
			}

			foreach ( PersonAddressBaseModel baseAddress in linkedAddresses )
			{
				_crud.DeletePersonAddress(baseAddress);
			}
		}

		public void UpdateEmployer(EmployerModel employer)
		{
			EmployerBaseModel baseEmployer = new EmployerBaseModel
			{
				Id = employer.Id,
				CompanyName = employer.CompanyName
			};

			_crud.UpdateEmployer(baseEmployer);

			List<EmployerAddressBaseModel> linkedAddresses = _crud.RetrieveEmployerAddressByEmployerId(employer.Id);

			foreach ( AddressModel address in employer.Addresses )
			{
				if ( address.Id == 0 )
				{
					SaveNewAddress(address);
					EmployerAddressBaseModel baseAddress = new EmployerAddressBaseModel
					{
						EmployerId = employer.Id,
						AddressId = address.Id
					};

					_crud.CreateEmployerAddress(baseAddress);
				}
				else
				{
					EmployerAddressBaseModel found = linkedAddresses.Find(x => x.AddressId == address.Id);
					if ( found == null )
					{
						EmployerAddressBaseModel baseAddress = new EmployerAddressBaseModel
						{
							EmployerId = employer.Id,
							AddressId = address.Id
						};

						_crud.CreateEmployerAddress(baseAddress);
					}
					else
					{
						_ = linkedAddresses.Remove(found);
					}
				}
			}

			foreach ( EmployerAddressBaseModel baseAddress in linkedAddresses )
			{
				_crud.DeleteEmployerAddress(baseAddress);
			}
		}

		public void UpdateAddress(AddressModel address)
		{
			_crud.UpdateAddress(address);
		}

		public void DeletePerson(PersonModel person)
		{
			List<PersonAddressBaseModel> linkedAddresses = _crud.RetrievePersonAddressByPersonId(person.Id);
			foreach ( PersonAddressBaseModel address in linkedAddresses )
			{
				_crud.DeletePersonAddress(address);
			}

			PersonBaseModel basePerson = new PersonBaseModel
			{
				Id = person.Id,
				FirstName = person.FirstName,
				LastName = person.LastName,
				IsActive = person.IsActive
			};

			if ( person.Employer == null )
			{
				basePerson.EmployerId = null;
			}
			else if ( person.Employer.Id == 0 )
			{
				SaveNewEmployer(person.Employer);
				basePerson.EmployerId = person.Employer.Id;
			}
			else
			{
				basePerson.EmployerId = person.Employer.Id;
			}

			_crud.DeletePerson(basePerson);
		}

		public void DeleteEmployer(EmployerModel employer)
		{
			List<EmployerAddressBaseModel> linkedAddresses = _crud.RetrieveEmployerAddressByEmployerId(employer.Id);
			foreach ( EmployerAddressBaseModel address in linkedAddresses )
			{
				_crud.DeleteEmployerAddress(address);
			}

			EmployerBaseModel baseEmployer = new EmployerBaseModel
			{
				Id = employer.Id,
				CompanyName = employer.CompanyName
			};

			_crud.DeleteEmployer(baseEmployer);
		}

		public void DeleteAddress(AddressModel address)
		{
			List<PersonAddressBaseModel> peopleAddresses = _crud.RetrievePersonAddressByAddressId(address.Id);
			foreach ( PersonAddressBaseModel personAddress in peopleAddresses )
			{
				_crud.DeletePersonAddress(personAddress);
			}

			List<EmployerAddressBaseModel> employerAddresses = _crud.RetrieveEmployerAddressByAddressId(address.Id);
			foreach ( EmployerAddressBaseModel employerAddress in employerAddresses )
			{
				_crud.DeleteEmployerAddress(employerAddress);
			}

			_crud.DeleteAddress(address);
		}
	}
}
