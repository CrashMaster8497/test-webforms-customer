using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerDataLayer.WebForms
{
    public partial class CustomerEdit : System.Web.UI.Page
    {
        private readonly CustomerRepository _customerRepository = new CustomerRepository();
        protected Customer customer;

        public CustomerEdit()
        {
            _customerRepository = new CustomerRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string customerIdString = Request.QueryString["id"];

            if (string.IsNullOrEmpty(customerIdString))
            {
                Response.Redirect("CustomerList.aspx");
            }

            customer = _customerRepository.Read(int.Parse(customerIdString));

            if (IsPostBack)
            {
                return;
            }

            customerId.Text = customer.CustomerId.ToString();
            firstName.Text = customer.FirstName;
            lastName.Text = customer.LastName;
            phoneNumber.Text = customer.PhoneNumber;
            email.Text = customer.Email;
        }

        protected void OnClickSave(object sender, EventArgs e)
        {
            var modifiedCustomer = new Customer
            {
                CustomerId = customer.CustomerId,
                FirstName = firstName.Text,
                LastName = lastName.Text,
                PhoneNumber = phoneNumber.Text,
                Email = email.Text,
                TotalPurchasesAmount = customer.TotalPurchasesAmount
            };
            _customerRepository.Update(modifiedCustomer);

            Response.Redirect("CustomerList.aspx");
        }

        protected void OnClickDelete(object sender, EventArgs e)
        {
            _customerRepository.Delete(customer.CustomerId);

            Response.Redirect("CustomerList.aspx");
        }
    }
}
