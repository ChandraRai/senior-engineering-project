<%@ Page Title="Logout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Flashminder.Logout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="Scripts/jquery-2.2.3.js"></script>
    	<script type="text/javascript">
			$(document).ready(function () {
				sessionStorage.clear();
			});
		</script>
	<center><h1>Logged out.</h1></center>
</asp:Content>
