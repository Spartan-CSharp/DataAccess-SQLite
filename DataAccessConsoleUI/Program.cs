using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DataAccessLibrary;
using DataAccessLibrary.Models;

using Microsoft.Extensions.Configuration;

namespace DataAccessConsoleUI
{
	internal static class Program
	{
		private static IConfiguration _configuration;
		private static IDataLogic _data;

		private static void Main(string[] args)
		{
			InitializeConfiguration();
			InitializeDatabaseConnection();
			ProgramIntro();
			ProgramLoop();
			ProgramOutro();
		}

		private static void InitializeConfiguration()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			_configuration = builder.Build();
		}

		private static void InitializeDatabaseConnection()
		{
			_data = new DataLogic(_configuration, DBTYPES.SQLServer);
		}

		private static void ProgramIntro()
		{
			Console.WriteLine();
			Console.WriteLine("Welcome to Relational Database CRUD!");
			Console.WriteLine("by Pierre Plourde");
			Console.WriteLine();
		}

		private static void ProgramLoop()
		{
			char action;
			char recordtype;

			do
			{
				action = GetActionSelection();
				switch ( action )
				{
					case 'c':
						recordtype = GetRecordTypeSelection();
						switch ( recordtype )
						{
							case 'p':
								PersonModel person = CreateNewPerson();
								DisplayPerson(person);
								break;
							case 'a':
								AddressModel address = CreateNewAddress();
								DisplayAddress(address);
								break;
							case 'e':
								EmployerModel employer = CreateNewEmployer();
								DisplayEmployer(employer);
								break;
							case 'x':
								break;
							default:
								break;
						}

						break;
					case 'r':
						recordtype = GetRecordTypeSelection();
						switch ( recordtype )
						{
							case 'p':
								PersonModel person = RetrievePerson();
								if ( person != null )
								{
									DisplayPerson(person);
								}

								break;
							case 'a':
								AddressModel address = RetrieveAddress();
								if ( address != null )
								{
									DisplayAddress(address);
								}

								break;
							case 'e':
								EmployerModel employer = RetrieveEmployer();
								if ( employer != null )
								{
									DisplayEmployer(employer);
								}

								break;
							case 'x':
								break;
							default:
								break;
						}

						break;
					case 'u':
						recordtype = GetRecordTypeSelection();
						switch ( recordtype )
						{
							case 'p':
								PersonModel person = RetrievePerson();
								if ( person != null )
								{
									person.UpdatePerson();
									DisplayPerson(person);
								}

								break;
							case 'a':
								AddressModel address = RetrieveAddress();
								if ( address != null )
								{
									address.UpdateAddress();
									DisplayAddress(address);
								}

								break;
							case 'e':
								EmployerModel employer = RetrieveEmployer();
								if ( employer != null )
								{
									employer.UpdateEmployer();
									DisplayEmployer(employer);
								}

								break;
							case 'x':
								break;
							default:
								break;
						}

						break;
					case 'd':
						recordtype = GetRecordTypeSelection();
						switch ( recordtype )
						{
							case 'p':
								DeletePerson();
								break;
							case 'a':
								DeleteAddress();
								break;
							case 'e':
								DeleteEmployer();
								break;
							case 'x':
								break;
							default:
								break;
						}

						break;
					case 'x':
						recordtype = 'x';
						break;
					default:
						recordtype = 'z';
						break;
				}
			} while ( action != 'x' && recordtype != 'x' );
		}

		private static char GetActionSelection()
		{
			char output;

			do
			{
				Console.Write("What action do you want to perform (C = Create/R = Retrieve/U = Update/D = Delete/X = Exit): ");
				string input = Console.ReadLine().ToLower();

				output = input.Length > 0 ? input[0] : 'z';

				if ( output != 'c' && output != 'r' && output != 'u' && output != 'd' && output != 'x' )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
					Console.WriteLine();
				}
			} while ( output != 'c' && output != 'r' && output != 'u' && output != 'd' && output != 'x' );

			return output;
		}

		private static char GetRecordTypeSelection()
		{
			char output;

			do
			{
				Console.Write("On which record type do you want to perform that action (P = Person/A = Address/E = Employer/X = Exit): ");
				string input = Console.ReadLine().ToLower();

				output = input.Length > 0 ? input[0] : 'z';

				if ( output != 'p' && output != 'a' && output != 'e' && output != 'x' )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
					Console.WriteLine();
				}
			} while ( output != 'p' && output != 'a' && output != 'e' && output != 'x' );

			return output;
		}

		private static PersonModel CreateNewPerson()
		{
			PersonModel output = new PersonModel();

			Console.WriteLine("Please enter all values; empty values are not allowed");
			do
			{
				Console.Write("First Name: ");
				output.FirstName = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.FirstName) );
			do
			{
				Console.Write("Last Name: ");
				output.LastName = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.LastName) );
			string isactive;

			Console.Write("Is Active (True/False): ");
			isactive = Console.ReadLine().ToLower();

			output.IsActive = isactive.Length != 0 && isactive[0] == 't';

			Console.WriteLine();
			bool selectionvalid;
			int selection;
			do
			{
				Console.Write("Please select: 1 = No address, 2 = Create a new address, 3 = Select an existing address from the list: ");
				selectionvalid = int.TryParse(Console.ReadLine(), out selection);
				if ( !selectionvalid || selection < 1 || selection > 3 )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
				}
			} while ( !selectionvalid || selection < 1 || selection > 3 );

			if ( selection != 1 )
			{
				bool done = false;
				do
				{
					if ( selection == 2 )
					{
						AddressModel address = CreateNewAddress();
						output.Addresses.Add(address);
					}

					if ( selection == 3 )
					{
						AddressModel address = RetrieveAddress();
						AddressModel checkifexistingaddress = output.Addresses.Where(x => x.Id == address.Id).FirstOrDefault();
						if ( checkifexistingaddress == null )
						{
							output.Addresses.Add(address);
						}
						else
						{
							Console.WriteLine("That address is already associated with this person");
						}
					}

					do
					{
						Console.Write("Please select: 1 = No more addresses, 2 = Create another new address, 3 = Add another address from the list of existing addresses: ");
						selectionvalid = int.TryParse(Console.ReadLine(), out selection);
						if ( !selectionvalid || selection < 1 || selection > 3 )
						{
							Console.WriteLine("That is not a valid selection.  Please try again.");
						}
					} while ( !selectionvalid || selection < 1 || selection > 3 );
					if ( selection == 1 )
					{
						done = true;
					}
				} while ( !done );
			}

			Console.WriteLine();
			do
			{
				Console.Write("Please select: 1 = No employer, 2 = Create a new employer, 3 = Select an existing employer from the list: ");
				selectionvalid = int.TryParse(Console.ReadLine(), out selection);
				if ( !selectionvalid || selection < 1 || selection > 3 )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
				}
			} while ( !selectionvalid || selection < 1 || selection > 3 );

			if ( selection == 1 )
			{
				output.Employer = null;
			}

			if ( selection == 2 )
			{
				EmployerModel employer = CreateNewEmployer();
				output.Employer = employer;
			}

			if ( selection == 3 )
			{
				EmployerModel employer = RetrieveEmployer();
				output.Employer = employer;
			}

			_data.SaveNewPerson(output);

			return output;
		}

		private static AddressModel CreateNewAddress()
		{
			AddressModel output = new AddressModel();

			Console.WriteLine("Please enter all values; empty values are not allowed");
			do
			{
				Console.Write("Street Address: ");
				output.StreetAddress = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.StreetAddress) );
			do
			{
				Console.Write("City: ");
				output.City = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.City) );
			do
			{
				Console.Write("State: ");
				output.State = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.State) );
			do
			{
				Console.Write("Zip Code: ");
				output.ZipCode = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.ZipCode) );
			Console.WriteLine();

			_data.SaveNewAddress(output);

			return output;
		}

		private static EmployerModel CreateNewEmployer()
		{
			EmployerModel output = new EmployerModel();

			Console.WriteLine("Please enter all values; empty values are not allowed");
			do
			{
				Console.Write("Company Name: ");
				output.CompanyName = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.CompanyName) );

			Console.WriteLine();
			bool selectionvalid;
			int selection;
			do
			{
				Console.Write("Please select: 1 = No address, 2 = Create a new address, 3 = Select an existing address from the list: ");
				selectionvalid = int.TryParse(Console.ReadLine(), out selection);
				if ( !selectionvalid || selection < 1 || selection > 3 )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
				}
			} while ( !selectionvalid || selection < 1 || selection > 3 );

			if ( selection != 1 )
			{
				bool done = false;
				do
				{
					if ( selection == 2 )
					{
						AddressModel address = CreateNewAddress();
						output.Addresses.Add(address);
					}

					if ( selection == 3 )
					{
						AddressModel address = RetrieveAddress();
						AddressModel checkifexistingaddress = output.Addresses.Where(x => x.Id == address.Id).FirstOrDefault();
						if ( checkifexistingaddress == null )
						{
							output.Addresses.Add(address);
						}
						else
						{
							Console.WriteLine("That address is already associated with this employer");
						}
					}

					do
					{
						Console.Write("Please select: 1 = No more addresses, 2 = Create another new address, 3 = Add another address from the list of existing addresses: ");
						selectionvalid = int.TryParse(Console.ReadLine(), out selection);
						if ( !selectionvalid || selection < 1 || selection > 3 )
						{
							Console.WriteLine("That is not a valid selection.  Please try again.");
						}
					} while ( !selectionvalid || selection < 1 || selection > 3 );

					if ( selection == 1 )
					{
						done = true;
					}
				} while ( !done );
			}

			_data.SaveNewEmployer(output);

			return output;
		}

		private static PersonModel RetrievePerson()
		{
			bool selectionvalid = false;
			int personid = 0;
			PersonModel output = new PersonModel();

			List<PersonModel> people = _data.GetAllPeople();

			if ( people.Count > 0 )
			{
				do
				{
					Console.WriteLine("Please select the person from the following list using the number:");
					foreach ( PersonModel person in people )
					{
						Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}");
					}

					selectionvalid = int.TryParse(Console.ReadLine(), out personid);

					if ( !selectionvalid || personid == 0 || people.Where(x => x.Id == personid).FirstOrDefault() == null )
					{
						Console.WriteLine("That is not a valid selection.");
					}
					else
					{
						output = people.Where(x => x.Id == personid).FirstOrDefault();
					}
				} while ( !selectionvalid || personid == 0 || people.Where(x => x.Id == personid).FirstOrDefault() == null );
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("-- No people in list --");
				Console.WriteLine();
				output = null;
			}

			return output;
		}

		private static AddressModel RetrieveAddress()
		{
			bool selectionvalid = false;
			int addressid = 0;
			AddressModel output = new AddressModel();

			List<AddressModel> addresses = _data.GetAllAddresses();

			if ( addresses.Count > 0 )
			{
				do
				{
					Console.WriteLine("Please select the address from the following list using the number:");
					foreach ( AddressModel address in addresses )
					{
						Console.WriteLine($"{address.Id}: {address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
					}

					selectionvalid = int.TryParse(Console.ReadLine(), out addressid);

					if ( !selectionvalid || addressid == 0 || addresses.Where(x => x.Id == addressid).FirstOrDefault() == null )
					{
						Console.WriteLine("That is not a valid selection.");
					}
					else
					{
						output = addresses.Where(x => x.Id == addressid).FirstOrDefault();
					}
				} while ( !selectionvalid || addressid == 0 || addresses.Where(x => x.Id == addressid).FirstOrDefault() == null );
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("-- No addresses in list --");
				Console.WriteLine();
				output = null;
			}

			return output;
		}

		private static EmployerModel RetrieveEmployer()
		{
			bool selectionvalid = false;
			int employerid = 0;
			EmployerModel output = new EmployerModel();

			List<EmployerModel> employers = _data.GetAllEmployers();

			if ( employers.Count > 0 )
			{
				do
				{
					Console.WriteLine("Please select the employer from the following list using the number:");
					foreach ( EmployerModel employer in employers )
					{
						Console.WriteLine($"{employer.Id}: {employer.CompanyName}");
					}

					selectionvalid = int.TryParse(Console.ReadLine(), out employerid);

					if ( !selectionvalid || employerid == 0 || employers.Where(x => x.Id == employerid).FirstOrDefault() == null )
					{
						Console.WriteLine("That is not a valid selection.");
					}
					else
					{
						output = employers.Where(x => x.Id == employerid).FirstOrDefault();
					}
				} while ( !selectionvalid || employerid == 0 || employers.Where(x => x.Id == employerid).FirstOrDefault() == null );
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("-- No employers in list --");
				Console.WriteLine();
				output = null;
			}

			return output;
		}

		private static void UpdatePerson(this PersonModel person)
		{
			Console.WriteLine($"Current First Name: {person.FirstName}");
			Console.Write("Enter new First Name (press enter on blank line for no change): ");
			string input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				person.FirstName = input;
			}

			Console.WriteLine($"Current Last Name: {person.FirstName}");
			Console.Write("Enter new Last Name (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				person.LastName = input;
			}

			Console.WriteLine($"Current Is Active: {person.IsActive}");
			Console.Write("Enter new Is Active (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				person.IsActive = input.Length != 0 && input[0] == 't';
			}

			if ( person.Addresses.Count == 0 )
			{
				Console.Write("This person has no addresses.");
			}
			else
			{
				Console.WriteLine("Addresses for the Person:");
				foreach ( AddressModel address in person.Addresses )
				{
					Console.WriteLine($"{address.Id}: {address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
				}
			}

			Console.WriteLine();
			bool done = false;
			bool selectionvalid;
			int selection;
			do
			{
				do
				{
					Console.Write("Please select: 1 = Remove an address, 2 = Add an address. 3 = Change an address, 4 = No changes to addresses: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection < 1 || selection > 4 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection < 1 || selection > 4 );

				bool addressvalid;
				int addressselect;
				if ( selection == 1 )
				{
					if ( person.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to remove using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = person.Addresses.Where(x => x.Id == addressselect).FirstOrDefault();
						if ( addressvalid && address != null )
						{
							_ = person.Addresses.Remove(address);
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to remove.");
					}
				}
				else if ( selection == 2 )
				{
					do
					{
						Console.Write("Please select: 1 = Create a new address, 2 = Select an existing address from the list: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						if ( !addressvalid || addressselect < 1 || addressselect > 2 )
						{
							Console.WriteLine("That is not a valid selection.  Please try again.");
						}
					} while ( !addressvalid || addressselect < 1 || addressselect > 2 );

					if ( addressselect == 1 )
					{
						AddressModel address = CreateNewAddress();
						person.Addresses.Add(address);
					}

					if ( addressselect == 2 )
					{
						AddressModel address = RetrieveAddress();
						AddressModel checkifexistingaddress = person.Addresses.Where(x => x.Id == address.Id).FirstOrDefault();
						if ( checkifexistingaddress == null )
						{
							person.Addresses.Add(address);
						}
						else
						{
							Console.WriteLine("That address is already associated with this person");
						}
					}
				}
				else if ( selection == 3 )
				{
					if ( person.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to change using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = person.Addresses.Where(x => x.Id == addressselect).FirstOrDefault();
						if ( addressvalid && address != null )
						{
							address.UpdateAddress();
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to change.");
					}
				}
				else
				{
					done = true;
				}
			} while ( !done );

			if ( person.Employer == null )
			{
				Console.WriteLine("This person has no employer.");
				do
				{
					Console.Write("Please select: 1 = Add an employer. 5 = No change to employer: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection != 1 || selection != 5 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection != 1 || selection != 5 );
			}
			else
			{
				Console.WriteLine("Employer for the Person:");
				Console.WriteLine($"{person.Employer.Id}: {person.Employer.CompanyName}");
				do
				{
					Console.Write("Please select: 2 = Replace the employer. 3 = Change the employer. 4 = Remove the employer. 5 = No change to employer: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection < 2 || selection > 5 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection < 1 || selection > 4 );
			}

			if ( selection == 2 || selection == 4 )
			{
				person.Employer = null;
			}

			if ( selection == 1 || selection == 2 )
			{
				bool employervalid;
				int employerselect;
				do
				{
					Console.Write("Please select: 1 = Create a new employer, 2 = Select an existing employer from the list: ");
					employervalid = int.TryParse(Console.ReadLine(), out employerselect);
					if ( !employervalid || employerselect < 1 || employerselect > 2 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !employervalid || employerselect < 1 || employerselect > 2 );

				if ( employerselect == 1 )
				{
					EmployerModel employer = CreateNewEmployer();
					person.Employer = employer;
				}

				if ( employerselect == 3 )
				{
					EmployerModel employer = RetrieveEmployer();
					person.Employer = employer;
				}
			}

			if ( selection == 3 )
			{
				person.Employer.UpdateEmployer();
			}

			_data.UpdatePerson(person);
		}

		private static void UpdateAddress(this AddressModel address)
		{
			Console.WriteLine($"Current Street Address: {address.StreetAddress}");
			Console.Write("Enter new Street Address (press enter on blank line for no change): ");
			string input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.StreetAddress = input;
			}

			Console.WriteLine($"Current City: {address.City}");
			Console.Write("Enter new City (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.City = input;
			}

			Console.WriteLine($"Current State: {address.State}");
			Console.Write("Enter new State (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.State = input;
			}

			Console.WriteLine($"Current Zip Code: {address.ZipCode}");
			Console.Write("Enter new Zip Code (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.ZipCode = input;
			}

			_data.UpdateAddress(address);
		}

		private static void UpdateEmployer(this EmployerModel employer)
		{
			Console.WriteLine($"Current Company Name: {employer.CompanyName}");
			Console.Write("Enter new Company Name (press enter on blank line for no change): ");
			string input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				employer.CompanyName = input;
			}

			if ( employer.Addresses.Count == 0 )
			{
				Console.WriteLine("This employer has no addresses.");
			}
			else
			{
				Console.WriteLine("Addresses for the Employer:");
				foreach ( AddressModel address in employer.Addresses )
				{
					Console.WriteLine($"{address.Id}: {address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
				}
			}

			Console.WriteLine();
			bool done = false;
			do
			{
				bool selectionvalid;
				int selection;
				do
				{
					Console.Write("Please select: 1 = Remove an address, 2 = Add an address. 3 = Change an address, 4 = No changes to addresses: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection < 1 || selection > 4 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection < 1 || selection > 4 );

				bool addressvalid;
				int addressselect;
				if ( selection == 1 )
				{
					if ( employer.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to remove using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = employer.Addresses.Where(x => x.Id == addressselect).FirstOrDefault();
						if ( addressvalid && address != null )
						{
							_ = employer.Addresses.Remove(address);
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to remove.");
					}
				}
				else if ( selection == 2 )
				{
					do
					{
						Console.Write("Please select: 1 = Create a new address, 2 = Select an existing address from the list: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						if ( !addressvalid || addressselect < 1 || addressselect > 2 )
						{
							Console.WriteLine("That is not a valid selection.  Please try again.");
						}
					} while ( !addressvalid || addressselect < 1 || addressselect > 2 );

					if ( addressselect == 1 )
					{
						AddressModel address = CreateNewAddress();
						employer.Addresses.Add(address);
					}

					if ( addressselect == 2 )
					{
						AddressModel address = RetrieveAddress();
						AddressModel checkifexistingaddress = employer.Addresses.Where(x => x.Id == address.Id).FirstOrDefault();
						if ( checkifexistingaddress == null )
						{
							employer.Addresses.Add(address);
						}
						else
						{
							Console.WriteLine("That address is already associated with this employer");
						}
					}
				}
				else if ( selection == 3 )
				{
					if ( employer.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to change using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = employer.Addresses.Where(x => x.Id == addressselect).FirstOrDefault();
						if ( addressvalid && address != null )
						{
							address.UpdateAddress();
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to change.");
					}
				}
				else
				{
					done = true;
				}
			} while ( !done );

			_data.UpdateEmployer(employer);
		}

		private static void DeletePerson()
		{
			PersonModel output = RetrievePerson();

			_data.DeletePerson(output);
		}

		private static void DeleteAddress()
		{
			AddressModel output = RetrieveAddress();

			_data.DeleteAddress(output);
		}

		private static void DeleteEmployer()
		{
			EmployerModel output = RetrieveEmployer();

			_data.DeleteEmployer(output);
		}

		private static void DisplayPerson(PersonModel person)
		{
			Console.WriteLine();
			Console.WriteLine("-- Person --");
			Console.WriteLine();
			Console.WriteLine($"First Name: {person.FirstName}");
			Console.WriteLine($"Last Name: {person.LastName}");
			Console.WriteLine($"Is Active: {person.IsActive}");
			if ( person.Addresses.Count > 0 )
			{
				Console.WriteLine();
				Console.WriteLine("Addresses for the Person:");
				foreach ( AddressModel address in person.Addresses )
				{
					Console.WriteLine($"{address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
				}
			}

			if ( person.Employer != null )
			{
				Console.WriteLine();
				Console.WriteLine($"Employer: {person.Employer.CompanyName}");
				if ( person.Employer.Addresses.Count > 0 )
				{
					Console.WriteLine();
					Console.WriteLine("Addresses for the Employer:");
					foreach ( AddressModel address in person.Employer.Addresses )
					{
						Console.WriteLine($"{address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
					}
				}
			}

			Console.WriteLine();
		}

		private static void DisplayAddress(AddressModel address)
		{
			Console.WriteLine();
			Console.WriteLine("-- Address --");
			Console.WriteLine();
			Console.WriteLine($"Street Address: {address.StreetAddress}");
			Console.WriteLine($"City: {address.City}");
			Console.WriteLine($"State: {address.State}");
			Console.WriteLine($"Zip Code: {address.ZipCode}");
			Console.WriteLine();
		}

		private static void DisplayEmployer(EmployerModel employer)
		{
			Console.WriteLine();
			Console.WriteLine("-- Employer --");
			Console.WriteLine();
			Console.WriteLine($"Company Name: {employer.CompanyName}");
			if ( employer.Addresses.Count > 0 )
			{
				Console.WriteLine();
				Console.WriteLine("Addresses for the Employer:");
				foreach ( AddressModel address in employer.Addresses )
				{
					Console.WriteLine($"{address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
				}
			}

			Console.WriteLine();
		}

		private static void ProgramOutro()
		{
			Console.WriteLine();
			Console.WriteLine("Thank you for using Relational Database CRUD!");
			Console.WriteLine();
		}
	}
}
