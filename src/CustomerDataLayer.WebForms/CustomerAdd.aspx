<%@ Page Title="Add Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAdd.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
        <asp:Label runat="server">First Name</asp:Label>
        <asp:TextBox ID="firstName" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ControlToValidate="firstName" ValidationExpression="^[a-zA-Z]{0,50}$" ErrorMessage="First name must consist of 0 to 50 latin characters" Display="Dynamic" runat="server"></asp:RegularExpressionValidator>
    </div>
    <div class="form-group">
        <asp:Label runat="server">Last Name</asp:Label>
        <asp:TextBox ID="lastName" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ControlToValidate="lastName" ValidationExpression="^[a-zA-Z]{1,50}$" ErrorMessage="Last name must consist of 1 to 50 latin characters" Display="Dynamic" runat="server"></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ControlToValidate="lastName" ErrorMessage="Last name is required" Display="Dynamic" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group">
        <asp:Label runat="server">Phone Number</asp:Label>
        <asp:TextBox ID="phoneNumber" CssClass="form-control" MaxLength="12" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ControlToValidate="phoneNumber" ValidationExpression="^(\+1|1)?([2-9]\d\d[2-9]\d{6})$" ErrorMessage="Incorrect phone number" Display="Dynamic" runat="server"></asp:RegularExpressionValidator>
    </div>
    <div class="form-group">
        <asp:Label runat="server">Email</asp:Label>
        <asp:TextBox ID="email" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ControlToValidate="email" ValidationExpression="^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$" ErrorMessage="Incorrect email" Display="Dynamic" runat="server"></asp:RegularExpressionValidator>
    </div>
    <asp:Button CssClass="btn btn-primary" Text="Add" OnClick="OnClickAdd" runat="server"></asp:Button>
</asp:Content>
