<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex flex-row">
        <asp:Button ID="ButtonPrev" CssClass="btn btn-secondary" Text="Prev" onclick="OnClickPrevPage" runat="server"></asp:Button>
        <asp:Button ID="ButtonNext" CssClass="btn btn-secondary" Text="Next" onclick="OnClickNextPage" runat="server"></asp:Button>
    </div>
    <table class="table">
        <%
        foreach (var customer in _customerList)
        {
        %>
        <tr>
            <td>
                <% =customer.CustomerId %>
            </td>
            <td>
                <% =customer.FirstName %>
            </td>
            <td>
                <% =customer.LastName %>
            </td>
            <td>
                <a href="CustomerEdit.aspx?id=<% =customer.CustomerId %>">Edit</a>
            </td>
        </tr>
        <%
        }
        %>
    </table>
    <asp:Button CssClass="btn btn-primary" Text="Add new customer" onclick="OnClickAdd" runat="server"></asp:Button>
</asp:Content>
