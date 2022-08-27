<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                <a href="#">Edit</a>
            </td>
        </tr>
        <%
        }
        %>
    </table>
</asp:Content>
