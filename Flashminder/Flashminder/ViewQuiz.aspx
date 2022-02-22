﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewQuiz.aspx.cs" Inherits="Flashminder.ViewQuiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.very-easy-btn {
			background-color:lightgreen
		}
		.easy-btn {
			background-color:lawngreen
		}
		.moderate-btn {
			background-color:yellow
		}
		.hard-btn {
			background-color:orange
		}
		.very-hard-btn {
			background-color:red
		}
	</style>
<script type="text/javascript" src="Scripts/jquery-2.2.3.js"></script>
<script type="text/javascript">
	$(document).ready(function () {
		$('#Flashcard').load('ViewFlashcard.aspx?FlashcardID=' + $('.flashcardID').text());

	});
	$('.flashcardID').change(function () {
			$('#Flashcard').load('ViewFlashcard.aspx?FlashcardID=' + $('.flashcardID').text());
	});

	if (typeof (Storage) !== "undefined") {
		function updateVeryEasyStatistics() {
			if (sessionStorage.veryEasy) {
				sessionStorage.veryEasy = Number(sessionStorage.veryEasy) + 1;
			}
			else {
				sessionStorage.veryEasy = 1;
			}
			updateCategories();

		}

		function updateEasyStatistics() {
			if (sessionStorage.easy) {
				sessionStorage.easy = Number(sessionStorage.easy) + 1;
			}
			else {
				sessionStorage.easy = 1;
			}
			updateCategories();

		}

		function updateModerateStatistics() {
			if (sessionStorage.moderate) {
				sessionStorage.moderate = Number(sessionStorage.moderate) + 1;
			}
			else {
				sessionStorage.moderate = 1;
			}
			updateCategories();

		}

		function updateHardStatistics() {
			if (sessionStorage.hard) {
				sessionStorage.hard = Number(sessionStorage.hard) + 1;
			}
			else {
				sessionStorage.hard = 1;
			}
			updateCategories();

		}

		function updateVeryHardStatistics() {
			if (sessionStorage.veryHard) {
				sessionStorage.veryHard = Number(sessionStorage.veryHard) + 1;
			}
			else {
				sessionStorage.veryHard = 1;
			}
			updateCategories();

		}

		function updateCategories() {
			
			if (sessionStorage.categories) {
				var categories = JSON.parse(sessionStorage.categories);
				if (categories[$('.categoryName').text()]) {
					categories[$('.categoryName').text()] = Number(categories[$('.categoryName').text()]) +1;
				}
				else {
					categories[$('.categoryName').text()] = 1;
				}
				sessionStorage.categories = JSON.stringify(categories);
			}
			else {
				var category = "{\"" + $('.categoryName').text() + "\": 1}";
				sessionStorage.categories = category;
			}
		}

	} else {
		// unable to track statistics
	}
</script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="container">
		<div>
		Category: 	
		<asp:Label ID="category" CssClass="categoryName" runat="server" Text=""></asp:Label>
		</div>
		<div>
			Multiplier:
			<asp:DropDownList ID="multiplier_dropdown" runat="server" OnSelectedIndexChanged="DropdownChanged" AutoPostBack="true">
				<asp:ListItem Text="0.10" Value="0.10"></asp:ListItem>
				<asp:ListItem Text="0.25" Value="0.25"></asp:ListItem>
				<asp:ListItem Text="0.50" Value="0.5"></asp:ListItem>
				<asp:ListItem Text="1" Value ="1" Selected="True"></asp:ListItem>
				<asp:ListItem Text="2" Value="2"></asp:ListItem>
				<asp:ListItem Text="3" Value="3"></asp:ListItem>
				<asp:ListItem Text="4" Value="4"></asp:ListItem>
			</asp:DropDownList>
		</div>

		<asp:Label ID="currentFlashcardId_label" CssClass="flashcardID hidden" runat="server" Visible="True"></asp:Label>
		<div id="Flashcard"></div>
		<div>
			<div>
				<asp:Button runat="server" ID="very_easy_btn" CssClass="very-easy-btn" Text="Very Easy" OnClick="ButtonPressed" OnClientClick="updateVeryEasyStatistics()"/>
			</div>
			<div>
				<asp:Button runat="server" ID="easy_btn" CssClass="easy-btn" Text="Easy" OnClick="ButtonPressed" OnClientClick="updateEasyStatistics()"/>
			</div>
			<div>
				<asp:Button runat="server" ID="moderate_btn" CssClass="moderate-btn" Text="Moderate" OnClick="ButtonPressed" OnClientClick="updateModerateStatistics()"/>
			</div>
			<div>
				<asp:Button runat="server" ID="hard_btn" CssClass="hard-btn" Text="Hard" OnClick="ButtonPressed" OnClientClick="updateHardStatistics()"/>
			</div>
			<div>
				<asp:Button runat="server" ID="very_hard_btn" CssClass="very-hard-btn" Text="Very Hard" OnClick="ButtonPressed" OnClientClick="updateVeryHardStatistics()"/>
			</div>
		</div>
	</div>
</asp:Content>
