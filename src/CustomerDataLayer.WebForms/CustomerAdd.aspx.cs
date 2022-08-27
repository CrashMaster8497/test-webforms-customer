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
    public partial class CustomerAdd : System.Web.UI.Page
    {
        private readonly CustomerRepository _customerRepository = new CustomerRepository();

        public CustomerAdd()
        {
            _customerRepository = new CustomerRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClickAdd(object sender, EventArgs e)
        {
            var customer = new Customer
            {
                FirstName = firstName.Text,
                LastName = lastName.Text,
                PhoneNumber = phoneNumber.Text,
                Email = email.Text
            };
            _customerRepository.Create(customer);

            Response.Redirect("CustomerList.aspx");
        }
    }
}
