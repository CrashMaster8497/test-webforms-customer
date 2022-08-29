<%@ Page Title="Add Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAdd.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label runat="server">First Name</asp:Label>
        <asp:TextBox ID="firstName" MaxLength="50" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label runat="server">Last Name</asp:Label>
        <asp:TextBox ID="lastName" MaxLength="50" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="lastName" ErrorMessage="Last name is required" Display="Dynamic" runat="server"/>
    </div>
    <div>
        <asp:Label runat="server">Phone Number</asp:Label>
        <asp:TextBox ID="phoneNumber" MaxLength="12" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ControlToValidate="phoneNumber" ErrorMessage="Incorrect phone number" Display="Dynamic" ValidationExpression="^(\+1|1)?([2-9]\d\d[2-9]\d{6})$" runat="server"/>
    </div>
    <div>
        <asp:Label runat="server">Email</asp:Label>
        <asp:TextBox ID="email" MaxLength="100" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ControlToValidate="email" ErrorMessage="Incorrect email" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$" runat="server"/>
    </div>
    <asp:Button CssClass="btn btn-primary" Text="Add" onclick="OnClickAdd" runat="server"></asp:Button>
</asp:Content>
