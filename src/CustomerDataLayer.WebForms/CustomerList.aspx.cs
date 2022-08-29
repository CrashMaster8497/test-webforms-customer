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
        const int MaxRecords = 3;

        private readonly CustomerRepository _customerRepository = new CustomerRepository();
        protected int _page;
        protected List<Customer> _customerList;

        public CustomerList()
        {
            _customerRepository = new CustomerRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageString = Request.QueryString["page"];
            _page = string.IsNullOrEmpty(pageString) ? 1 : int.Parse(pageString);

            int offset = (_page - 1) * MaxRecords;
            _customerList = _customerRepository.Read(offset, MaxRecords);

            int maxPage = (_customerRepository.Count() + MaxRecords - 1) / MaxRecords;

            if (_page == 1)
            {
                ButtonPrev.Enabled = false;
            }
            if (_page == maxPage)
            {
                ButtonNext.Enabled = false;
            }
        }

        protected void OnClickAdd(object sender, EventArgs e)
        {
            Response.Redirect("CustomerAdd.aspx");
        }

        protected void OnClickPrevPage(object sender, EventArgs e)
        {
            Response.Redirect($"CustomerList.aspx?page={_page - 1}");
        }

        protected void OnClickNextPage(object sender, EventArgs e)
        {
            Response.Redirect($"CustomerList.aspx?page={_page + 1}");
        }
    }
}
