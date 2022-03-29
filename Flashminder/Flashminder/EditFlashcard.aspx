<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditFlashcard.aspx.cs" Inherits="Flashminder.EditFlashcard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">		    
	<!-- Inline CSS section -->
	<style>
		.image {
			height: 150px; 
			width: 290px;
		}

        .back_txtbx, .front_txtbx {
            width: 100%;
            padding: 12px 20px;
            margin: 8px 0;
            display: inline-block;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
	</style>
	<script type="text/javascript" src="Scripts/jquery-2.2.3.js"></script>
	<script type="text/javascript">
		function OpenPopup() {

			var newWindow = window.open("CreateCategory.aspx", "List", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=yes, width=500,height=300,left=430,top=100");
			newWindow.onbeforeunload = function () {
				window.location.href = window.location.href;
			}

			return false;
		}

		function ValidateForm() {
			var ret = true;
			if (!$(".front_txtbx").val() && !$(".front_upload").val()) {
				ShowMessage("Front of the card needs text or an image", "warning");
				ret = false;
			}
			if ((!$(".back_txtbx").val() && !$(".back_upload").val())) {
				ShowMessage("Back of the card needs text or an image", "warning");
				ret = false;
			}

			if ($(".front_txtbx").val() && $(".front_txtbx").val().length > 500) {
				ShowMessage("Text can only be 500 characters", "warning");
				ret = false;
			}
			if ($(".back_txtbx").val() && $(".back_txtbx").val().length > 500) {
				ShowMessage("Text can only be 500 characters", "warning");
				ret = false;
			}

			return ret;
		}

		function ShowMessage(msg, type) {
			$("#MessagePanel").html('<div class="message_pnl"><a href="#" class ="close" data-dismiss="alert" aria-label="close">&times;</a><span>' + msg + '</span></div>')
			$(".message_pnl").addClass("alert alert-" + type + " alert-dismissable");
			$(".message_pnl").attr("role", "alert");
			$(".message_pnl").show();
		}


		//https://stackoverflow.com/questions/28214458/how-to-preview-uploaded-image-in-file-upload-control-before-save-using-asp-net-o
		function readURLFront(input) {
			if (input.files && input.files[0]) {
				var reader = new FileReader();

				reader.onload = function (e) {
					$('#front_img').css('display', 'block');
					$('#front_img').attr('src', e.target.result);
				}

				reader.readAsDataURL(input.files[0]);
			}
		}

		function readURLBack(input) {
			if (input.files && input.files[0]) {
				var reader = new FileReader();

				reader.onload = function (e) {
					$('#back_img').css('display', 'block');
					$('#back_img').attr('src', e.target.result);
				}

				reader.readAsDataURL(input.files[0]);
			}
		}

		$(".front_upload").change(function () {
			readURLFront(this);
		});

		$(".back_upload").change(function () {
			readURLBack(this);
		});


	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container">
        <div class="row">
            <div class="col-md-offset-2  col-md-8">
                <div id="MessagePanel" style="padding-bottom:25px">
                    <asp:Panel ID="message_pnl" CssClass="message_pnl" runat="server" >
                        <a href="#" class ="close hidden" data-dismiss="alert" aria-label="close">&times;</a>
                        <asp:Label ID="message_lbl" CssClass="message_lbl" runat="server" />
                    </asp:Panel>
                </div>
                <div class="well well-lg">
                    <asp:Label ID="category_lbl" runat="server">Category: </asp:Label>
                    <asp:DropDownList ID="category_dropdownlist" CssClass="btn btn-default dropdown-toggle" runat="server" DataTextField="CategoryName" DataValueField="Id" AppendDataBoundItems="true" ></asp:DropDownList>
                    <button type="button" ID="createCategory_btn" class="btn btn-warning" OnClick="OpenPopup()"><i class="fa fa-plus" aria-hidden="true"> Create Category</i></button>
                </div>
                <div class="panel panel-default">
					<div class="panel-body">
                        <h4 class="text-left">Front</h4>
						<asp:Panel ID="front_img_change_pnl" runat="server" Visible="false">
							<asp:FileUpload id="front_upload" CssClass="front_upload btn btn-default" runat="server" OnChange="readURLFront(this)"/>
							<img id="front_img" src="//:0" style="height: 150px; width: 290px" class="img-responsive" onerror="this.style.display='none'"/>
						</asp:Panel>
						<asp:Button id="front_img_change_btn" runat="server" Text="Change Image" OnClick="ShowChangeFrontPanel"/>
						<asp:Button id="remove_front_img_btn" runat="server" Text="Remove Image" OnClick="RemoveFrontImage" OnClientClick="return confirm('Are you sure you want to delete the image? This cannot be undone')"/>
						<p>Current Front Image:</p>
						<asp:Image id="current_front_img" CssClass="image" runat="server" onerror="this.style.display='none'" />
						<br />
						<asp:TextBox ID="front_txtbx" CssClass="front_txtbx" runat="server" TextMode="MultiLine" placeholder="Enter text here..."></asp:TextBox>
					</div>
                </div>
                <div class="panel panel-default">
					<div class="panel-body">
						<h4 class="text-left">Back</h4>
						<asp:Panel ID="back_img_change_pnl" runat="server" Visible="false">
							<asp:FileUpload id="back_upload" CssClass="back_upload btn btn-default" runat="server" OnChange="readURLBack(this)"/>
							<img id="back_img" src="//:0" style="height: 150px; width: 290px" class="img-responsive" onerror="this.style.display='none'"/>
						</asp:Panel>
						<asp:Button id="back_img_change_btn" runat="server" Text="Change Image" OnClick="ShowChangeBackPanel"/>
						<asp:Button id="remove_back_img_btn" runat="server" Text="Remove Image" OnClick="RemoveBackImage" OnClientClick="return confirm('Are you sure you want to delete the image? This cannot be undone')"/>
						<p>Current Back Image:</p>
						<asp:Image id="current_back_img" CssClass="image" runat="server" onerror="this.style.display='none'"/>
						<br />
						<asp:TextBox ID="back_txtbx" CssClass="back_txtbx" runat="server" TextMode="MultiLine" placeholder="Enter text here..."></asp:TextBox>
					</div>
                </div>
                <asp:Button ID="update_btn" runat="server" Text="Update Flashcard" OnClientClick="return ValidateForm()" OnClick="UpdateFlashcard" UseSubmitBehavior="true" />
            </div>
        </div>
    </div>
</asp:Content>
