<%@ Page Title="Add Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAdd.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h4 style="margin-top: 25px"><b>New Customer</b></h4>

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

    <asp:Button CssClass="btn btn-primary" Text="Add" OnClick="OnClickAdd" runat="server"></asp:Button>

</asp:Content>
