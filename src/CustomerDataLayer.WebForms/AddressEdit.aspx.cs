using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using System;
using System.Collections.Generic;

namespace CustomerDataLayer.WebForms
{
    public partial class AddressEdit : System.Web.UI.Page
    {
        private readonly AddressRepository _addressRepository;
        protected Address Address;
        protected List<string> Countries = new List<string>
        {
            "United States",
            "Canada"
        };

        public AddressEdit()
        {
            _addressRepository = new AddressRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string addressIdString = Request.QueryString["id"];
            int.TryParse(addressIdString, out int addressId);

            Address = _addressRepository.Read(addressId);

            if (Address == null)
            {
                Response.Redirect("CustomerList.aspx");
            }

            if (IsPostBack)
            {
                return;
            }

            foreach (var country in Countries)
            {
                this.country.Items.Add(country);
            }

            this.addressId.Text = Address.AddressId.ToString();
            customerId.Text = Address.CustomerId.ToString();
            addressType.SelectedValue = Address.AddressType.ToString();
            addressLine.Text = Address.AddressLine;
            addressLine2.Text = Address.AddressLine2;
            city.Text = Address.City;
            postalCode.Text = Address.PostalCode;
            state.Text = Address.State;
            country.SelectedValue = Address.Country;
        }

        protected void OnClickSave(object sender, EventArgs e)
        {
            Enum.TryParse(this.addressType.SelectedValue, out AddressType addressType);
            var modifiedAddress = new Address
            {
                AddressId = Address.AddressId,
                CustomerId = Address.CustomerId,
                AddressLine = addressLine.Text,
                AddressLine2 = addressLine2.Text,
                AddressType = addressType,
                City = city.Text,
                PostalCode = postalCode.Text,
                State = state.Text,
                Country = country.SelectedValue,
            };
            _addressRepository.Update(modifiedAddress);

            Response.Redirect($"CustomerEdit.aspx?id={Address.CustomerId}");
        }

        protected void OnClickDelete(object sender, EventArgs e)
        {
            _addressRepository.Delete(Address.CustomerId);

            Response.Redirect($"CustomerEdit.aspx?id={Address.CustomerId}");
        }
    }
}
