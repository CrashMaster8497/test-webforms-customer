<%@ Page Title="Edit Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h4 style="margin-top: 25px"><b>Edit Customer Info</b></h4>

    <div class="form-group">
        <asp:Label Text="ID" runat="server"></asp:Label>
        <asp:TextBox ID="customerId" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="First Name" runat="server"></asp:Label>
        <asp:TextBox ID="firstName" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator
            ControlToValidate="firstName"
            ValidationGroup="CustomerGroup"
            ValidationExpression="^[a-zA-Z]{0,50}$"
            ErrorMessage="Incorrect First Name"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Last Name" runat="server"></asp:Label>
        <asp:TextBox ID="lastName" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator
            ControlToValidate="lastName"
            ValidationGroup="CustomerGroup"
            ErrorMessage="Last name is required"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator
            ControlToValidate="lastName"
            ValidationGroup="CustomerGroup"
            ValidationExpression="^[a-zA-Z]{1,50}$"
            ErrorMessage="Incorrect Last Name"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Phone Number" runat="server"></asp:Label>
        <asp:TextBox ID="phoneNumber" CssClass="form-control" MaxLength="12" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator
            ControlToValidate="phoneNumber"
            ValidationGroup="CustomerGroup"
            ValidationExpression="^(\+1|1)?([2-9]\d\d[2-9]\d{6})$"
            ErrorMessage="Incorrect Phone Number"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Email" runat="server"></asp:Label>
        <asp:TextBox ID="email" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator
            ControlToValidate="email"
            ValidationGroup="CustomerGroup"
            ValidationExpression="^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"
            ErrorMessage="Incorrect Email"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="d-flex flex-row">
        <asp:Button CssClass="btn btn-primary" Text="Save" OnClick="OnClickSave" ValidationGroup="CustomerGroup" runat="server"></asp:Button>
        <asp:Button CssClass="btn btn-danger" Text="Delete" OnClick="OnClickDelete" ValidationGroup="NoGroup" runat="server"></asp:Button>
    </div>

    <h4 style="margin-top: 50px"><b>Shipping Addresses</b></h4>

    <table class="table">
        <tr>
            <td>Address Line</td>
            <td>Address Line 2</td>
            <td>City</td>
            <td>Postal Code</td>
            <td>State</td>
            <td>Country</td>
            <td>
                <asp:Button CssClass="btn btn-danger" Text="Delete All" OnClick="OnClickDeleteAllShippingAddresses" ValidationGroup="NoGroup" runat="server" />
            </td>
        </tr>
        <asp:Repeater ID="ShippingAddressesRepeater" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "AddressLine") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "AddressLine2") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "City") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "PostalCode") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "State") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Country") %>
                    </td>
                    <td>
                        <a href='AddressEdit.aspx?id=<%# DataBinder.Eval(Container.DataItem, "AddressId") %>'>
                            <span class="glyphicon glyphicon-pencil"></span>
                            Edit
                        </a>
                        |
                        <asp:LinkButton OnClick="OnClickDeleteAddress" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AddressId") %>' ValidationGroup="NoGroup" runat="server">
                            <span class="glyphicon glyphicon-remove"></span>
                            Delete
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <h4 style="margin-top: 50px"><b>Billing Addresses</b></h4>

    <table class="table">
        <tr>
            <td>Address Line</td>
            <td>Address Line 2</td>
            <td>City</td>
            <td>Postal Code</td>
            <td>State</td>
            <td>Country</td>
            <td>
                <asp:Button CssClass="btn btn-danger" Text="Delete All" OnClick="OnClickDeleteAllBillingAddresses" ValidationGroup="NoGroup" runat="server" />
            </td>
        </tr>
        <asp:Repeater ID="BillingAddressesRepeater" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "AddressLine") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "AddressLine2") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "City") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "PostalCode") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "State") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Country") %>
                    </td>
                    <td>
                        <a href='AddressEdit.aspx?id=<%# DataBinder.Eval(Container.DataItem, "AddressId") %>'>
                            <span class="glyphicon glyphicon-pencil"></span>
                            Edit
                        </a>
                        |
                        <asp:LinkButton OnClick="OnClickDeleteAddress" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AddressId") %>' ValidationGroup="NoGroup" runat="server">
                            <span class="glyphicon glyphicon-remove"></span>
                            Delete
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <h4 style="margin-top: 50px"><b>Add New Address</b></h4>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Address Type" runat="server"></asp:Label>
        <asp:DropDownList ID="addressType" CssClass="form-control" runat="server">
            <asp:ListItem>Shipping</asp:ListItem>
            <asp:ListItem>Billing</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Address Line" runat="server"></asp:Label>
        <asp:TextBox ID="addressLine" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator
            ControlToValidate="addressLine"
            ValidationGroup="AddressGroup"
            ErrorMessage="Address Line is required"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator
            ControlToValidate="addressLine"
            ValidationGroup="AddressGroup"
            ValidationExpression="^[\x20-\x7E]{1,100}$"
            ErrorMessage="Incorrect format"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Address Line 2" runat="server"></asp:Label>
        <asp:TextBox ID="addressLine2" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator
            ControlToValidate="addressLine2"
            ValidationGroup="AddressGroup"
            ValidationExpression="^[\x20-\x7E]{0,100}$"
            ErrorMessage="Incorrect format"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="City" runat="server"></asp:Label>
        <asp:TextBox ID="city" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator
            ControlToValidate="city"
            ValidationGroup="AddressGroup"
            ErrorMessage="City is required"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator
            ControlToValidate="city"
            ValidationGroup="AddressGroup"
            ValidationExpression="^[\x20-\x7E]{1,50}$"
            ErrorMessage="Incorrect format"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Postal Code" runat="server"></asp:Label>
        <asp:TextBox ID="postalCode" CssClass="form-control" MaxLength="6" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator
            ControlToValidate="postalCode"
            ValidationGroup="AddressGroup"
            ErrorMessage="Postal Code is required"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator
            ControlToValidate="postalCode"
            ValidationGroup="AddressGroup"
            ValidationExpression="^[a-zA-Z0-9]{1,6}$"
            ErrorMessage="Incorrect format"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="State" runat="server"></asp:Label>
        <asp:TextBox ID="state" CssClass="form-control" MaxLength="20" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator
            ControlToValidate="state"
            ValidationGroup="AddressGroup"
            ErrorMessage="State is required"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator
            ControlToValidate="state"
            ValidationGroup="AddressGroup"
            ValidationExpression="^[a-zA-Z]{1,20}$"
            ErrorMessage="Incorrect format"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RegularExpressionValidator>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Country" runat="server"></asp:Label>
        <asp:DropDownList ID="country" CssClass="form-control" runat="server"></asp:DropDownList>
    </div>

    <asp:Button CssClass="btn btn-primary" Text="Add" OnClick="OnClickAddAddress" ValidationGroup="AddressGroup" runat="server"></asp:Button>

</asp:Content>
