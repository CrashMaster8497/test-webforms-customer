<%@ Page Title="Edit Address" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddressEdit.aspx.cs" Inherits="CustomerDataLayer.WebForms.AddressEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h4 style="margin-top: 25px"><b>Edit Address</b></h4>

    <div class="form-group">
        <asp:Label Text="Address ID" runat="server"></asp:Label>
        <asp:TextBox ID="addressId" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Label Text="Customer ID" runat="server"></asp:Label>
        <asp:TextBox ID="customerId" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
    </div>

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

    <div class="d-flex flex-row">
        <asp:Button CssClass="btn btn-primary" Text="Save" OnClick="OnClickSave" ValidationGroup="AddressGroup" runat="server"></asp:Button>
        <asp:Button CssClass="btn btn-danger" Text="Delete" OnClick="OnClickDelete" ValidationGroup="NoGroup" runat="server"></asp:Button>
    </div>

</asp:Content>
