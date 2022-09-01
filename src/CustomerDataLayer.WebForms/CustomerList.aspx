<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h4 style="margin-top: 25px"><b>Customers</b></h4>

    <div class="btn-group" role="group">
        <asp:Button ID="ButtonPrev" CssClass="btn btn-default" Text="Prev" OnClick="OnClickPrevPage" runat="server"></asp:Button>
        <asp:Button ID="ButtonNext" CssClass="btn btn-default" Text="Next" OnClick="OnClickNextPage" runat="server"></asp:Button>
    </div>

    <table class="table">
        <tr>
            <td>ID</td>
            <td>First Name</td>
            <td>Last Name</td>
            <td></td>
            <td></td>
        </tr>
        <asp:Repeater ID="CustomersRepeater" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "CustomerId") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "FirstName") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "LastName") %></td>
                    <td>
                        <a href='CustomerEdit.aspx?id=<%# DataBinder.Eval(Container.DataItem, "CustomerId") %>'>
                            <span class="glyphicon glyphicon-pencil"></span>
                            Edit
                        </a>
                        |
                        <asp:LinkButton OnClick="OnClickDeleteCustomer" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CustomerId") %>' ValidationGroup="NoGroup" runat="server">
                            <span class="glyphicon glyphicon-remove"></span>
                            Delete
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <asp:Button CssClass="btn btn-primary" Text="Create Customer" OnClick="OnClickAdd" runat="server"></asp:Button>

</asp:Content>
