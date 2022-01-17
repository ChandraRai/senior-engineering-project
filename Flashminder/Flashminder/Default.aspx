﻿<%@ Page Title="Sign in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Flashminder.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-offset-4 col-md-4">
                <h3>Sign In!</h3>
                <asp:Label ID="LabelField" runat="server">Please enter username & password here.</asp:Label>
                <br />
                <br />
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title"><i class="fa fa-refresh fa-spin"></i></h2>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <label class="contorl-label" for="UserNameTextBox">Username</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="UserNameTextBox" Placeholder="Username" required="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="UserPassword">Password</label>
                            <asp:TextBox runat="server" type="password" class="form-control" ID="Password" Placeholder="Password" />
                        </div>
                        <div>
                            <asp:Button runat="server" ID="Send" CssClass="btn btn-primary" UseSubmitBehavior="false" CausesValidation="false" Text="Send" />
                            <asp:Button runat="server" ID="CancelButton" CssClass="btn btn-warning" UseSubmitBehavior="false" CausesValidation="false" Text="Cancel" />
                        </div>
                        <div class="label">
                            <a href="Signup.aspx">Register account?</a>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
