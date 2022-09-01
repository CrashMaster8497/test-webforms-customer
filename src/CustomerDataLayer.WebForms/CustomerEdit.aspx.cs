using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CustomerDataLayer.WebForms
{
    public partial class CustomerEdit : System.Web.UI.Page
    {
        private readonly CustomerRepository _customerRepository;
        private readonly AddressRepository _addressRepository;
        private readonly NoteRepository _noteRepository;
        protected Customer Customer;
        protected List<Address> Addresses;
        protected List<Address> ShippingAddresses;
        protected List<Address> BillingAddresses;
        protected List<Note> Notes;
        protected List<string> Countries = new List<string>
        {
            "United States",
            "Canada"
        };

        public CustomerEdit()
        {
            _customerRepository = new CustomerRepository();
            _addressRepository = new AddressRepository();
            _noteRepository = new NoteRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string customerIdString = Request.QueryString["id"];
            int.TryParse(customerIdString, out int customerId);

            Customer = _customerRepository.Read(customerId);

            if (Customer == null)
            {
                Response.Redirect("CustomerList.aspx");
            }

            Addresses = _addressRepository.ReadByCustomerId(customerId, 0, 1000);
            ShippingAddresses = Addresses.Where(address => address.AddressType == AddressType.Shipping).ToList();
            BillingAddresses = Addresses.Where(address => address.AddressType == AddressType.Billing).ToList();

            ShippingAddressesRepeater.DataSource = ShippingAddresses;
            ShippingAddressesRepeater.DataBind();
            BillingAddressesRepeater.DataSource = BillingAddresses;
            BillingAddressesRepeater.DataBind();

            Notes = _noteRepository.ReadByCustomerId(customerId, 0, 1000);

            NotesRepeater.DataSource = Notes;
            NotesRepeater.DataBind();

            if (IsPostBack)
            {
                return;
            }

            this.customerId.Text = Customer.CustomerId.ToString();
            firstName.Text = Customer.FirstName;
            lastName.Text = Customer.LastName;
            phoneNumber.Text = Customer.PhoneNumber;
            email.Text = Customer.Email;

            foreach (var country in Countries)
            {
                this.country.Items.Add(country);
            }
        }

        protected void OnClickSave(object sender, EventArgs e)
        {
            var modifiedCustomer = new Customer
            {
                CustomerId = Customer.CustomerId,
                FirstName = firstName.Text,
                LastName = lastName.Text,
                PhoneNumber = phoneNumber.Text,
                Email = email.Text,
                TotalPurchasesAmount = Customer.TotalPurchasesAmount
            };
            _customerRepository.Update(modifiedCustomer);

            Response.Redirect("CustomerList.aspx");
        }

        protected void OnClickDelete(object sender, EventArgs e)
        {
            _customerRepository.Delete(Customer.CustomerId);

            Response.Redirect("CustomerList.aspx");
        }

        protected void OnClickAddAddress(object sender, EventArgs e)
        {
            Enum.TryParse(this.addressType.SelectedValue, out AddressType addressType);
            var address = new Address
            {
                CustomerId = Customer.CustomerId,
                AddressLine = addressLine.Text,
                AddressLine2 = addressLine2.Text,
                AddressType = addressType,
                City = city.Text,
                PostalCode = postalCode.Text,
                State = state.Text,
                Country = country.SelectedValue
            };
            _addressRepository.Create(address);

            Response.Redirect($"CustomerEdit.aspx?id={Customer.CustomerId}");
        }

        protected void OnClickDeleteAllShippingAddresses(object sender, EventArgs e)
        {
            foreach (var address in ShippingAddresses)
            {
                _addressRepository.Delete(address.AddressId);
            }

            Response.Redirect($"CustomerEdit.aspx?id={Customer.CustomerId}");
        }

        protected void OnClickDeleteAllBillingAddresses(object sender, EventArgs e)
        {
            foreach (var address in BillingAddresses)
            {
                _addressRepository.Delete(address.AddressId);
            }

            Response.Redirect($"CustomerEdit.aspx?id={Customer.CustomerId}");
        }

        protected void OnClickDeleteAddress(object sender, EventArgs e)
        {
            int.TryParse(((LinkButton)sender).CommandArgument, out int addressId);
            _addressRepository.Delete(addressId);

            Response.Redirect($"CustomerEdit.aspx?id={Customer.CustomerId}");
        }

        protected void OnClickAddNote(object sender, EventArgs e)
        {
            var note = new Note
            {
                CustomerId = Customer.CustomerId,
                Text = text.Text
            };
            _noteRepository.Create(note);

            Response.Redirect($"CustomerEdit.aspx?id={Customer.CustomerId}");
        }

        protected void OnClickDeleteNote(object sender, EventArgs e)
        {
            int.TryParse(((LinkButton)sender).CommandArgument, out int noteId);
            _noteRepository.Delete(noteId);

            Response.Redirect($"CustomerEdit.aspx?id={Customer.CustomerId}");
        }
    }
}
