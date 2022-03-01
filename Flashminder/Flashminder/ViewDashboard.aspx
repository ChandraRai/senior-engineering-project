<%@ Page Title="Dashboard Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDashboard.aspx.cs" Inherits="Flashminder.ViewDashboard" %>

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
                    responsive: false
                }
            });
        });
	</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
    <div class="container">
        <div class="row">
            <div class="col-md-8">
                <div class="jumbotron">
                    <h2 runat="server">Learning Graph</h2>
                    <hr />
                    <div class="text-right">
                        <asp:Label ID="category_lbl" runat="server">Category: </asp:Label>
                        <asp:DropDownList ID="category_dropdownlist" runat="server" DataTextField="CategoryName" DataValueField="Id" AppendDataBoundItems="true">
                            <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="graph">
                        <canvas id="statisticsChart" width="400" height="400"></canvas>
                    </div>
                </div>
                <div class="text-left">
                    <asp:Button ID="CreateFlashcards" runat="server" CssClass="btn btn-info" Text="Create Flashcards" OnClick="RedirectToCreateFlashcards" />
                    <asp:Button ID="ViewFlashcards" CssClass="btn btn-info" runat="server" Text="View Flashcards" OnClick="RedirectToViewFlashcards" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="panel-title"><h4>Questions Done Per Category</h4></div>
                    </div>
                    <div class="panel-body">
                        <div id="statistics"></div>
                    </div>
                </div>
                <div>
                    <asp:Button ID="StartQuiz" runat="server" CssClass="btn btn-primary" Text="Start Quiz" OnClick="RedirectToStartQuiz" />
                    <asp:Button ID="QuizSettings" runat="server" Text="Quiz Settings" CssClass="btn btn-warning" OnClick="RedirectToQuizSettings" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
