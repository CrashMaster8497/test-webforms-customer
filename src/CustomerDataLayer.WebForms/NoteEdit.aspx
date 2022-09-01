<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NoteEdit.aspx.cs" Inherits="CustomerDataLayer.WebForms.NoteEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h4 style="margin-top: 25px"><b>Edit Note</b></h4>

    <div class="form-group">
        <asp:Label Text="Note ID" runat="server"></asp:Label>
        <asp:TextBox ID="noteId" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Label Text="Customer ID" runat="server"></asp:Label>
        <asp:TextBox ID="customerId" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Label CssClass="control-label" Text="Text" runat="server"></asp:Label>
        <asp:TextBox ID="text" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator
            ControlToValidate="text"
            ValidationGroup="NoteGroup"
            ErrorMessage="Text is required"
            SetFocusOnError="true"
            ForeColor="Red"
            Display="Dynamic"
            runat="server">
        </asp:RequiredFieldValidator>
    </div>

    <div class="d-flex flex-row">
        <asp:Button CssClass="btn btn-primary" Text="Save" OnClick="OnClickSave" ValidationGroup="NoteGroup" runat="server"></asp:Button>
        <asp:Button CssClass="btn btn-danger" Text="Delete" OnClick="OnClickDelete" ValidationGroup="NoGroup" runat="server"></asp:Button>
    </div>

</asp:Content>
