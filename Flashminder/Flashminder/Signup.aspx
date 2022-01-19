<%@ Page Title="Sign up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="Flashminder.Signup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-offset-4 col-md-4">
                <h3>Sign Up!</h3>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title">All fields are required.</h2>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <label class="contorl-label" for="form-group-input">Username</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtUsername" Placeholder="Username" required="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="contorl-label" for="form-group-input">Email</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" Placeholder="Email" required="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="contorl-label" for="form-group-input">Password</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword" Placeholder="Password" required="true" TextMode="Password"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label class="contorl-label" for="form-group-input">Confirm</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirm" Placeholder="Confirm" required="true" TextMode="Password"></asp:TextBox>
                        </div>
                        <div class="text-left">
                            <asp:Button runat="server" ID="RegisterButton" CssClass="btn btn-primary" Text="Register" />
                            <asp:Button runat="server" ID="CancelButton" CssClass="btn btn-warning" Text="Cancel" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
