<%@ Page Title="Edit Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
        <asp:Label runat="server">ID</asp:Label>
        <asp:TextBox ID="customerId" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
    </div>
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
    <div class="d-flex flex-row">
        <asp:Button CssClass="btn btn-primary" Text="Save" OnClick="OnClickSave" runat="server"></asp:Button>
        <asp:Button CssClass="btn btn-danger" Text="Delete" OnClick="OnClickDelete" runat="server"></asp:Button>
    </div>
</asp:Content>
