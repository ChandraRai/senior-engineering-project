<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDashboard.aspx.cs" Inherits="Flashminder.ViewDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="Scripts/jquery-2.2.3.js"></script>
	<script type="text/javascript" src="Scripts/chart.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			veryEasy = sessionStorage["veryEasy"] ? sessionStorage["veryEasy"] : 0;
			easy = sessionStorage["easy"] ? sessionStorage["easy"] : 0;
			moderate = sessionStorage["moderate"] ? sessionStorage["moderate"] : 0;
			hard = sessionStorage["hard"] ? sessionStorage["hard"] : 0;
			veryHard = sessionStorage["veryHard"] ? sessionStorage["veryHard"] : 0;

			categoriesHTML = "";
			if (sessionStorage["categories"]) {
				categories = JSON.parse(sessionStorage["categories"]);
				for (var key in categories) {
					value = categories[key];
					categoriesHTML += "<p>" + key + ":" + value + "<p>";
				}
			}



			$("#statistics").html("<p>Very Easy: " + veryEasy + "</p>" +
				"<p>Easy: " + easy + "</p>" +
				"<p>Moderate: " + moderate + "</p>" +
				"<p>Hard: " + hard + "</p>" +
				"<p>Very Hard: " + veryHard + "</p>" +
				"<br/> Questions done per Category:"+
				categoriesHTML
			);

			const context = document.getElementById('statisticsChart').getContext('2d');
			const chart = new Chart(context, {
				type: 'bar',
				data: {
					labels: ['Very Easy', 'Easy', 'Moderate', 'Hard', 'Very Hard'],
					datasets: [{
						label: "Question Difficulty",
						data: [veryEasy, easy, moderate, hard, veryHard],
						backgroundColor: [
							'rgba(170, 241, 66, 0.8)',
							'rgba(85, 247, 34, 0.8)',
							'rgba(245, 247, 34, 0.8)',
							'rgba(247, 174, 34, 0.8)',
							'rgba(255, 30, 0, 0.8)'
						],
					}]
				},
				options: {
					responsive:false
				}
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container">
		<div id ="statistics"></div>
		<div id ="graph">
			<canvas id="statisticsChart" width="300" height="300"></canvas>
		</div>

		<div>
			<asp:Button id="CreateFlashcards" runat="server" Text="Create Flashcards" OnClick="RedirectToCreateFlashcards"/>
			<asp:Button id="ViewFlashcards" runat="server" Text="View Flashcards" OnClick="RedirectToViewFlashcards"/>
			<div>
				<asp:Label ID="category_lbl" runat="server">Category: </asp:Label>
				<asp:DropDownList ID="category_dropdownlist" runat="server" DataTextField="CategoryName" DataValueField="Id" AppendDataBoundItems="true">
					<asp:ListItem  Selected="True" Text="All" Value="All"></asp:ListItem>
				</asp:DropDownList>
				<asp:Button id="StartQuiz" runat="server" Text="Start Quiz" OnClick="RedirectToStartQuiz"/>
			</div>
			<asp:Button id="QuizSettings" runat="server" Text="Quiz Settings" OnClick="RedirectToQuizSettings"/>
		</div>
	</div>		
</asp:Content>
