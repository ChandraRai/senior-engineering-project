﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="CreateCategory.aspx.cs" Inherits="Flashminder.CreateCategory" %>

<!DOCTYPE html>
<link rel="stylesheet" href="Content/bootstrap.css"/>

<html>
    <head>
        <title>Create Category</title>
    </head>
    <script type="text/javascript" src="Scripts/jquery-2.2.3.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
    <script type="text/javascript">

		function ShowMessage(msg, type) {
			$("#MessagePanel").html('<div class="message_pnl"><a href="#" class ="close" data-dismiss="alert" aria-label="close">&times;</a><span>' + msg + '</span></div>')
			$(".message_pnl").addClass("alert alert-" + type + " alert-dismissable");
			$(".message_pnl").attr("role", "alert");
			$(".message_pnl").show();
		}

		function ValidateForm() {
			var ret = true;
			if (!$(".categoryName_txtbx").val()) {
				ShowMessage("Category name must be present", "warning");
				ret = false;
			}

			if ($(".categoryName_txtbx").val() && $(".categoryName_txtbx").val().length > 100) {
				ShowMessage("Category name can only be 100 characters", "warning");
				ret = false;
			}
			if ($(".categoryDesc_txtbx").val() && $(".categoryDesc_txtbx").val().length > 500) {
				ShowMessage("Category description can only be 500 characters", "warning");
				ret = false;
			}

			return ret;
		}

	</script>
    <body>
        <div class="container">
                <div class="row">                    
                    <div class="col-md-offset-2  col-md-6">
                    <form runat="server">
                    <div id="MessagePanel" style="padding-bottom:25px">
                        <asp:Panel ID="message_pnl" CssClass="message_pnl" runat="server" >
                            <a href="#" class ="close hidden" data-dismiss="alert" aria-label="close">&times;</a>
                            <asp:Label ID="message_lbl" CssClass="message_lbl" runat="server" />
                        </asp:Panel>
                        <asp:Label ID="success_lbl" runat="server" Visible ="false"> Successfully created category.</asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="categoryName_lbl" CssClass="categoryName_lbl" runat="server">Category Name</asp:Label>
                        <asp:TextBox ID="categoryName_txtbx" CssClass="categoryName_txtbx" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Label ID="categoryDesc_lbl" CssClass="categoryDesc_lbl" runat="server">Category Description</asp:Label>
                        <asp:TextBox ID="categoryDesc_txtbx"  CssClass="categoryDesc_txtbx" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div>
                        <br />
                        <asp:Button ID="create_btn" runat="server" Text="Create Category" OnClick="CreateCategoryDB" OnClientClick="return ValidateForm()"/>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </body>
</html>