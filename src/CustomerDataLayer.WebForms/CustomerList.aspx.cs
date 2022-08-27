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
    public partial class CustomerList : System.Web.UI.Page
    {
        private readonly CustomerRepository _customerRepository = new CustomerRepository();
        protected List<Customer> _customerList;

        public CustomerList()
        {
            _customerRepository = new CustomerRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _customerList = _customerRepository.ReadAll();
        }

        protected void OnClickAdd(object sender, EventArgs e)
        {
            Response.Redirect("CustomerAdd.aspx");
        }
    }
}
