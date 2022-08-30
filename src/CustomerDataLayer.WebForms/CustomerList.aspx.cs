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
        private const int MaxRecords = 3;

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
            int.TryParse(pageString, out _page);

            int maxPage = Math.Max((_customerRepository.Count() + MaxRecords - 1) / MaxRecords, 1);

            if (_page < 1)
            {
                Response.Redirect("CustomerList.aspx?page=1");
            }
            if (_page > maxPage)
            {
                Response.Redirect($"CustomerList.aspx?page={maxPage}");
            }

            int offset = (_page - 1) * MaxRecords;
            _customerList = _customerRepository.Read(offset, MaxRecords);

            if (_page <= 1)
            {
                ButtonPrev.Enabled = false;
            }
            if (_page >= maxPage)
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
