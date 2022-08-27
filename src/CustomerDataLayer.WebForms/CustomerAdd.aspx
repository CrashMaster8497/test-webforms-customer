<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAdd.aspx.cs" Inherits="CustomerDataLayer.WebForms.CustomerAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label runat="server">First Name</asp:Label>
        <asp:TextBox ID="firstName" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label runat="server">Last Name</asp:Label>
        <asp:TextBox ID="lastName" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label runat="server">Phone Number</asp:Label>
        <asp:TextBox ID="phoneNumber" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label runat="server">Email</asp:Label>
        <asp:TextBox ID="email" runat="server"></asp:TextBox>
    </div>
    <asp:Button CssClass="btn btn-primary" Text="Add" onclick="OnClickAdd" runat="server"></asp:Button>
</asp:Content>
